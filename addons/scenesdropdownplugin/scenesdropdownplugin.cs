#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Extensions.Godot;

[Tool]
public partial class scenesdropdownplugin : EditorPlugin
{
	private string sceneDirectory = "Scenes";
	private List<string> scenePaths;
	private MenuButton pluginMenuBtn;
	private OptionButton optionButton;
	private PopupMenu pluginsMenu;

	public override void _EnterTree()
	{

		pluginMenuBtn = new MenuButton();
		pluginsMenu = pluginMenuBtn.GetPopup();

		
		pluginMenuBtn.Text = "Scene Dropdown";
		pluginMenuBtn.TooltipText = "Select Scene";
			
		FillButtonList();

		pluginsMenu.IndexPressed += IndexPressed;
		pluginMenuBtn.AboutToPopup += FillButtonList;
			
		AddControlToContainer(CustomControlContainer.Toolbar, pluginMenuBtn);
	}

	public override void _ExitTree()
	{
		RemoveControlFromContainer(CustomControlContainer.Toolbar, pluginMenuBtn);
		pluginMenuBtn.QueueFree();
	}
	private void FillButtonList()
	{
		pluginsMenu.Clear();
		scenePaths = FindScenes(sceneDirectory);
		foreach (string scenePath in scenePaths)
		{
			pluginsMenu.AddItem(scenePath);
		}
	}

	private List<string> FindScenes(string directory)
	{
		List<string> scenePaths = new List<string>();

		// Get a list of all files in the directory
		string[] files = Directory.GetFiles(directory);

		foreach (string file in files)
		{
			//file = file.Replace("res:\")
			// Check if the file has a ".tscn" extension (Godot scene file)
			if (file.EndsWith(".tscn"))
			{
				// Convert the file path to a resource path
				string resourcePath = ProjectSettings.GlobalizePath(file);
				scenePaths.Add(resourcePath);
			}
		}

		return scenePaths;
	}
	
	private void IndexPressed(long index)
	{
		if (scenePaths != null && scenePaths.Count > 0)
		{
			var editorInterface = GetEditorInterface();
			
			var openScenes = editorInterface.GetOpenScenes();
			
			var targetScene = $"res://{scenePaths[(int)index]}";
			targetScene = targetScene.Replace("\\","/");
			editorInterface.SelectFile(targetScene);
			foreach (string openScene in openScenes)
			{
				if (targetScene == openScene)
				{
					editorInterface.ReloadSceneFromPath(targetScene);
					return;
				}
			}
			editorInterface.OpenSceneFromPath(targetScene);
		}
	}
	
	
}
#endif

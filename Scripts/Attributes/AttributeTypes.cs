using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace Legion.Attributes;

[Tool]
public partial class AttributeTypes : Node
{
	[Export] public Resource[] attributes = Array.Empty<Resource>();
	public static Dictionary<int, BaseAttribute> Map { get; } = new Dictionary<int, BaseAttribute>();
	
	public override void _Ready()
	{
		if(Engine.IsEditorHint()) return;
		
		Map.Clear();
		foreach (var resource in attributes)
		{
			var baseAttribute = (BaseAttribute)resource;
			Map.Add(baseAttribute, baseAttribute);
		}
		base._Ready();
	}
	
	private IEnumerable<string> FindAttributes(string directory)
	{
		List<string> scenePaths = new List<string>();

		// Get a list of all files in the directory
		string[] files = Directory.GetFiles(directory);

		return files;
		foreach (string file in files)
		{
			GD.Print(file);
			//file = file.Replace("res:\")
			// Check if the file has a ".tscn" extension (Godot scene file)
			if (file.EndsWith(".tres"))
			{
				// Convert the file path to a resource path
				string resourcePath = ProjectSettings.GlobalizePath(file);
				scenePaths.Add(resourcePath);
			}
		}

		return scenePaths;
	}
	
}
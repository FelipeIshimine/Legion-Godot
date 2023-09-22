#if TOOLS
using Godot;
using System;
using System.Diagnostics;
using Legion.Combat.Core;

[Tool]
public partial class ActionSystemHelper : EditorPlugin
{
	public override void _EnterTree()
	{
		GetTree().NodeAdded += OnNodeAdded;
		GD.Print($"{this} EnterTree");
		// Initialization of the plugin goes here.
	}

	public override void _ExitTree()
	{
		GD.Print($"{this} ExitTree");
		GetTree().NodeAdded -= OnNodeAdded;
		// Clean-up of the plugin goes here.
	}

	private void OnNodeAdded(Node node)
	{
		//GD.Print($"{node._ImportPath}: {node.GetType().Name}");
		/*if (node is SkillSettings skillSettings)
		{
			GD.Print($"{node._ImportPath}: {node.GetType().Name}<<<<<<<<<<<<<<<<");
			var parent = skillSettings.GetParent();
			if(parent is ActiveSkill activeSkill)
				activeSkill.FindSettings();
		}*/
	}
}
#endif

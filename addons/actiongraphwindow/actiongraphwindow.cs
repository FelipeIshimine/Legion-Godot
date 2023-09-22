#if TOOLS
using Godot;
using System;
using System.Diagnostics;
using Legion.Combat.Core;

[Tool]
public partial class actiongraphwindow : EditorPlugin
{
	private Control dockedScene;

	private CombatActionGraph combatActionGraph;
	
	public override void _EnterTree()
	{
		dockedScene = ResourceLoader.Load<PackedScene>("res://addons/ScenesDropdownPlugin/ActionsGraph.tscn").Instantiate() as Control;
		combatActionGraph = dockedScene as CombatActionGraph;
		CombatActionSystem.OnExecutionTreeUpdate += TreeUpdated;
		AddControlToDock(DockSlot.LeftUl, dockedScene);
		// Initialization of the plugin goes here.
	}

	public override void _ExitTree()
	{
		RemoveControlFromDocks(dockedScene);
		dockedScene.Free();
		CombatActionSystem.OnExecutionTreeUpdate -= TreeUpdated;
	}

	private void TreeUpdated(CombatActionSystem obj)
	{
		combatActionGraph.Clear();
		foreach (var pair in obj.ExecutionTree)
		{
			combatActionGraph.Add($"{pair.Value.GetInstanceId()}:{pair.Value.Name}", $"{pair.Value.GetInstanceId()}:{pair.Value.Name}");
		}
		GD.Print($">>>>>>>>>> {combatActionGraph.Nodes.Count}");
	}
}
#endif

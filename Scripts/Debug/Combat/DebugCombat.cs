using Godot;
using Legion.Combat.Core;

namespace Legion.Debug.Combat;

public partial class DebugCombat : Node
{
	[Export] private CombatActionGraph graph;
	[Export] private CombatActionSystem actionSystem;
	[Export] private string openDebugId = "Open Debug";


	public override void _Ready()
	{
		base._Ready();
		graph.Initialize(actionSystem);
	}

	public override void _Input(InputEvent input)
	{
		if (input.IsActionPressed(openDebugId))
		{
			graph.Visible = !graph.Visible;
		}
	}
}
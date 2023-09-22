using System.Collections.Generic;
using Fractural.Tasks;
using Godot;
using Legion.Character;
using Legion.Scripts.GameSystems;
using LegionCombat.Extensions;

namespace Legion.Combat.Turn;

public partial class TurnsSystem : GameSystem
{
	private CombatSystemsContainer combatSystemsContainer;

	private List<CharacterUnit> units = new List<CharacterUnit>();
	private List<CharacterUnit> turnOrder = new List<CharacterUnit>();

	private GDTaskCompletionSource waitForInput;
	protected override void OnInitialize()
	{
		combatSystemsContainer = GetParent<CombatSystemsContainer>();

		units.Clear();
		foreach (var unit in combatSystemsContainer.Configuration.AllUnits())
		{
			units.Add(unit);
		}
	}
	protected override void OnTerminate()
	{
	}
	
	public async GDTask StartLoop()
	{
		int n = 0;
		while (true)
		{
			turnOrder.AddRange(units);
			turnOrder.Shuffle();

			GD.Print($"Round {++n} START");
			while (turnOrder.Count > 0)
			{
				var unit = turnOrder.TakeAt(0);
				GD.Print($"Turn {unit} START");
				await unit.Controller.TakeTurn(unit, combatSystemsContainer);
				GD.Print($"Turn {unit} END");

				GD.Print($"Turn {unit} Waiting for player input");
				await (waitForInput = new GDTaskCompletionSource()).Task;
			}
			GD.Print($"Round {n} END");
		}
	}

	public override void _Input(InputEvent input)
	{
		if (waitForInput != null && input.IsActionPressed("ui_select"))
		{
			waitForInput.TrySetResult();
		}
	}
}
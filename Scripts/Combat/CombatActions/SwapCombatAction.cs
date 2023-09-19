using System.Threading;
using Fractural.Tasks;
using Godot;
using Legion.Character;
using Legion.Combat.Core;
using Legion.Combat.Formation;

namespace Legion.Combat.CombatActions;

public class SwapCombatAction : CombatAction<SwapCombatAction, SwapCombatAction.Settings>
{
	public class Settings : GodotObject
	{
		public float Speed = 10;
		public Curve Curve;
	}

	protected override GDTask PerformFlow(CancellationToken cancellationToken) => GDTask.CompletedTask;

	protected override GDTask PrepareFlow(CancellationToken cancellationToken)
	{
		var formationSystem = GetSystem<FormationSystem>();
		
		var coorA = Targets[0];
		var coorB = Targets[1];

		CharacterUnit unit = formationSystem[coorA].Unit;

		var sourceTile = formationSystem[coorA];
		var targetTile = formationSystem[coorB];
		
		if(sourceTile)
		
	}
}
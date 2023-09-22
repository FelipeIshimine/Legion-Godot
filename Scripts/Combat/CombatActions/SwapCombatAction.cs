using System.Collections.Generic;
using System.Threading;
using Fractural.Tasks;
using Godot;
using Legion.Character;
using Legion.Combat.Core;
using Legion.Combat.Formation;
using Time = Legion.GameStates.Time;

namespace Legion.Combat.CombatActions;

public partial class SwapCombatAction : CombatAction<SwapCombatAction, SwapCombatActionSettings>
{
	private FormationSystem formationSystem;
	protected override GDTask PerformFlow(CancellationToken cancellationToken) => GDTask.CompletedTask;

	protected override async GDTask PrepareFlow(CancellationToken token)
	{
		formationSystem = GetSystem<FormationSystem>();
		
		var sourceCoordinate = Targets[0];
		var targetCoordinate = Targets[1];

		FormationTile sourceTile = formationSystem[sourceCoordinate];
		FormationTile targetTile = formationSystem[targetCoordinate];

		List<GDTask> tasks = new List<GDTask>();

		if (!sourceTile.IsEmpty)
		{
			tasks.Add(UnitTransition(sourceTile.Unit, targetCoordinate, token));	
		}
		if (!targetTile.IsEmpty)
		{
			tasks.Add(UnitTransition(targetTile.Unit, sourceCoordinate, token));	
		}

		await GDTask.WhenAll(tasks);
	}

	private async GDTask UnitTransition(CharacterUnit unit, Vector3I coordinate, CancellationToken token)
	{
		var targetTile = formationSystem[coordinate];

		var targetPosition = targetTile.GlobalPosition;

		var speed = MySettings.Speed;

		var startDistance = (targetPosition - unit.GlobalPosition).LengthSquared();
		
		do
		{
			float t = (targetPosition - unit.GlobalPosition).LengthSquared() / startDistance;
			var delta = speed * Time.Delta * MySettings.Curve.Sample(t);
			unit.GlobalPosition = unit.GlobalPosition.MoveToward(targetPosition, delta);
			await GDTask.NextFrame(PlayerLoopTiming.Process, token);
		} while (unit.GlobalPosition != targetPosition);
		
		unit.SetTile(formationSystem.Tiles[coordinate]);
	}
}
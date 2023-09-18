using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fractural.Tasks;
using Legion.Character;
using Legion.GameSystems;
using Legion.Scripts.Combat;
using Legion.Scripts.Combat.Formation;

public partial class CombatSystemsContainer : GameSystemsContainer
{
	public async GDTask CombatFlow(CombatConfiguration configuration, CancellationToken cancellationToken)
	{
		GD.Print("AAA");
		var formationSystem = GetSystem<FormationSystem>();

		foreach (var unit in configuration.Units())
		{
			AddChild(unit);
		}
		
		LoadTeam(configuration.LeftTeam, formationSystem.LeftFormation);
		LoadTeam(configuration.RightTeam, formationSystem.RightFormation);

		foreach (CharacterUnit unit in configuration.RightTeam)
		{
			unit.Flip();
		}
		
		GD.Print("BBB");

		while (!cancellationToken.IsCancellationRequested)
		{
			await GDTask.NextFrame(cancellationToken);
		}
	}

	private static void LoadTeam(CharacterUnit[] team, Formation formation)
	{
		for (var index = 0; index < team.Length; index++)
		{
			CharacterUnit unit = team[index];
			unit.GlobalPosition = formation[index].GlobalPosition;
		}
	}
}
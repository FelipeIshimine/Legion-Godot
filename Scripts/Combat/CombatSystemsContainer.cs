using System.Threading;
using Fractural.Tasks;
using Godot;
using Legion.Character;
using Legion.Combat.Turn;
using Legion.GameSystems;

namespace Legion.Combat;

public partial class CombatSystemsContainer : GameSystemsContainer
{
	public CombatConfiguration Configuration { get; private set; }

	public async GDTask CombatFlow(CombatConfiguration combatConfig, CancellationToken cancellationToken)
	{
		Configuration = combatConfig;
		GD.Print("AAA");
		var formationSystem = GetSystem<Formation.FormationSystem>();

		foreach (var unit in combatConfig.AllUnits())
		{
			AddChild(unit);
		}
		
		LoadTeam(combatConfig.LeftTeam, formationSystem.LeftFormation);
		LoadTeam(combatConfig.RightTeam, formationSystem.RightFormation);

		foreach (CharacterUnit unit in combatConfig.RightTeam)
		{
			unit.Flip();
		}
		
		GD.Print("BBB");

		Initialize();

		await GetSystem<TurnsSystem>().StartLoop();
	}


	private static void LoadTeam(CharacterUnit[] team, Formation.Formation formation)
	{
		for (var index = 0; index < team.Length; index++)
		{
			CharacterUnit unit = team[index];
			unit.GlobalPosition = formation[index].GlobalPosition;
			unit.SetTile(formation[index]);
		}
	}
}
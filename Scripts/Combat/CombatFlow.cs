using System.Collections.Generic;
using System.Threading;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;
using Legion.Character;
using Legion.Scripts.Combat;

namespace Legion.Combat;

public partial class CombatFlow : GameStates.GameFlowState<CombatFlow, CombatFlow.Result>
{
	[Export] private string dummyCharacterPath = "res://Characters/dummy_character.tscn";

	public CombatFlow(Node parent) : base(parent)
	{
	}

	protected override async GDTask<Result> 
		OnFlow(CancellationToken cancellationToken)
	{
		//LoadScene
		var combatScene = ResourceLoader.Load<PackedScene>(GameScenes.Instance.Combat).Instantiate();
		Root.AddChild(combatScene);
		
		List<Character.CharacterUnit> allies = new List<Character.CharacterUnit>();
		List<Character.CharacterUnit> enemies = new List<Character.CharacterUnit>();

		var packedScene = ResourceLoader.Load<PackedScene>(dummyCharacterPath);

		GD.Print($"Packed scene is null: {(packedScene ==null)}    {packedScene.ResourceName} {packedScene.ResourcePath} CanInstantiate:{packedScene.CanInstantiate()}");

		for (int i = 0; i < 4; i++)
		{
			allies.Add(packedScene.Instantiate<CharacterUnit>());
			enemies.Add(packedScene.Instantiate<CharacterUnit>());
		}
		
		CombatConfiguration combatConfiguration = new CombatConfiguration(allies.ToArray(),enemies.ToArray());
		
		await combatScene.FindNode<CombatSystemsContainer>().CombatFlow(combatConfiguration,cancellationToken);

		//ReleaseScene
		Root.RemoveChild(combatScene);
		combatScene.QueueFree();

		return Result.Win;
	}

	public enum Result
	{
		Win,
		Lose,
		Retreat
	}
}
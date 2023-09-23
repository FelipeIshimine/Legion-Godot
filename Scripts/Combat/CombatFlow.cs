using System.Collections.Generic;
using System.Threading;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;
using Legion.Character;

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
		var scenePack = ResourceLoader.Load<PackedScene>(GameScenes.Instance.Combat);

		GD.Print($"scenePack.CanInstantiate():{scenePack.CanInstantiate()}");

		var packed = ResourceLoader.Load<PackedScene>(GameScenes.Instance.Combat);

		var combatScene = packed.Instantiate();
		
		Root.AddChild(combatScene);
		
		List<Character.CharacterUnit> allies = new List<Character.CharacterUnit>();
		List<Character.CharacterUnit> enemies = new List<Character.CharacterUnit>();

		var characterPack = ResourceLoader.Load<PackedScene>(dummyCharacterPath);

		GD.Print($"Packed scene is null: {(characterPack ==null)}    {characterPack.ResourceName} {characterPack.ResourcePath} CanInstantiate:{characterPack.CanInstantiate()}");

		for (int i = 0; i < 4; i++)
		{
			allies.Add(characterPack.Instantiate<CharacterUnit>());
			enemies.Add(characterPack.Instantiate<CharacterUnit>());
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
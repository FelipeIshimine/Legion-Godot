using System.Collections.Generic;
using Godot;

namespace Legion.Combat;

public class CombatConfiguration
{
	[Export] public Character.CharacterUnit[] LeftTeam = new Character.CharacterUnit[4];
	[Export] public Character.CharacterUnit[] RightTeam = new Character.CharacterUnit[4];

	public CombatConfiguration()
	{
	}

	public CombatConfiguration(Character.CharacterUnit[] leftTeam, Character.CharacterUnit[] rightTeam)
	{
		this.LeftTeam = leftTeam;
		this.RightTeam = rightTeam;
	}

	public IEnumerable<Character.CharacterUnit> AllUnits()
	{
		foreach (Character.CharacterUnit unit in LeftTeam)
		{
			yield return unit;
		}
		
		foreach (Character.CharacterUnit unit in RightTeam)
		{
			yield return unit;
		}
	}
}
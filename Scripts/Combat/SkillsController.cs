using Godot;
using System;
using System.Collections.Generic;
using Legion.Combat.Core;

[GlobalClass]
public partial class SkillsController : Node
{
	[Export] public ActiveSkill[] ActiveSkills { get; private set; }
	[Export] public PassiveSkill[] PassiveSkill { get; private set; }
	
	public override void _Ready()
	{
		List<ActiveSkill> activeSkills = new List<ActiveSkill>();
		List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
		foreach (Node child in GetChildren())
		{
			if (child is ActiveSkill activeSkill)
			{
				activeSkills.Add(activeSkill);
			}
			else if (child is PassiveSkill passiveSkill)
			{
				passiveSkills.Add(passiveSkill);
			}
		}

		ActiveSkills = activeSkills.ToArray();
		PassiveSkill = passiveSkills.ToArray();
	}
	
	
}

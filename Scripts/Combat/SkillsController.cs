using System.Collections.Generic;
using Godot;
using Legion.Combat.Core;

namespace Legion.Combat;

[GlobalClass]
public partial class SkillsController : Node
{
	[ExportCategory("Debug")] public ActiveSkill[] ActiveSkills { get; private set; }
	[ExportCategory("Debug")] public PassiveSkill[] PassiveSkill { get; private set; }
	
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
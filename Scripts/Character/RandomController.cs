using System;
using System.Collections.Generic;
using System.Threading;
using Fractural.Tasks;
using Godot;
using Legion.Combat;
using Legion.Combat.Core;
using LegionCombat.Extensions;

namespace Legion.Character;

[GlobalClass]
public partial class RandomController : UnitController
{
	public override async GDTask TakeTurn(CharacterUnit unit, CombatSystemsContainer combatContainer)
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		var combatActionSystem = combatContainer.GetSystem<CombatActionSystem>();

		
		GD.Print(unit);
		GD.Print(unit.Skills);
		GD.Print(unit.Skills.ActiveSkills);
		if (unit.Skills.ActiveSkills.Length == 0)
		{
			throw new Exception("no skills found");
		}
		
		ActiveSkill skill = unit.Skills.ActiveSkills[rng.RandiRange(0, unit.Skills.ActiveSkills.Length - 1)];

		var targets = new List<Vector3I>(skill.FilterTargets(combatContainer));

		var a = targets.TakeAt(rng.RandiRange(0, targets.Count - 1));
		var b = targets.TakeAt(rng.RandiRange(0, targets.Count - 1));
		
		await combatActionSystem.Execute(skill.CreateAction(unit,combatContainer, a, b), null, CancellationToken.None);
	}
}
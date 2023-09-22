using Godot;
using Legion.Combat.CombatActions;
using Legion.Combat.Core;

namespace Legion.Combat.CombatSkills;

[GlobalClass]
public partial class SwapSkill : ActiveSkill<SwapCombatAction,SwapCombatActionSettings>
{
	[Export] public override SwapCombatActionSettings SkillSettings { get; set; }
}
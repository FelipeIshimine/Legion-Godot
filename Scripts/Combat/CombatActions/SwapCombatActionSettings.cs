using Godot;
using Legion.Combat.Core;

namespace Legion.Combat.CombatActions;

[GlobalClass]
public partial class SwapCombatActionSettings : SkillSettings
{
	[Export] public float Speed = 10;
	[Export] public Curve Curve = new Curve();
}
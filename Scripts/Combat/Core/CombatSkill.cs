using Godot;

namespace Legion.Combat.Core;

public partial class CombatSkill : Node
{
	[Export] private string displayName;
	public string DisplayName => string.IsNullOrEmpty(displayName) ? Name : displayName;
}


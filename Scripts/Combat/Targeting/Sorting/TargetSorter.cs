using Godot;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Sorting;

public abstract partial class TargetSorter : Resource
{
	public abstract int Compare(CharacterUnit source, FormationTile x, FormationTile y);
}
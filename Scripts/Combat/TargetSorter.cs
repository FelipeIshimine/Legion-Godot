using Godot;
using Legion.Attributes.Derived;
using Legion.Character;
using Legion.Combat.Formation;
using Legion.GameStates;

namespace Legion.Combat;

[System.Serializable]
public abstract partial class TargetSorter : Resource
{
	public abstract int Compare(CharacterUnit source, FormationTile x, FormationTile y);
}

[System.Serializable]
public partial class Random : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y) => GameFlowMachine.Rng.RandiRange(-1, 2);
}

[System.Serializable]
public partial class LowestHealth : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		return x.Unit.CurrentHealth.CompareTo(y.Unit.CurrentHealth);
	}
}

[System.Serializable]
public partial class HighestHealth : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		return -x.Unit.CurrentHealth.CompareTo(y.Unit.CurrentHealth);
	}
}

[System.Serializable]
public partial class LowestMissingHealth : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;

		return (x.Unit.Attributes.Get<MaxHealth>() - x.Unit.CurrentHealth).CompareTo(
			y.Unit.Attributes.Get<MaxHealth>() - y.Unit.CurrentHealth);
	}
}

[System.Serializable]
public partial class HighestMissingHealth : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		return -(x.Unit.Attributes.Get<MaxHealth>() - x.Unit.CurrentHealth).CompareTo(
			y.Unit.Attributes.Get<MaxHealth>() - y.Unit.CurrentHealth);
	}
}

[System.Serializable]
public partial class Ally : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		int value = source.WorldCoordinate.Z == x.Unit.WorldCoordinate.Z?-1:0;
		value += source.WorldCoordinate.Z == y.Unit.WorldCoordinate.Z?1:0;
		return value;
	}
}
[System.Serializable]
public partial class Enemy : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		int value = source.WorldCoordinate.Z != x.Unit.WorldCoordinate.Z?-1:0;
		value += source.WorldCoordinate.Z != y.Unit.WorldCoordinate.Z?1:0;
		return value;
	}
}

[System.Serializable]
public partial class Self : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		int value = source == x.Unit?-1:0;
		value += source == y.Unit?1:0;
		return value;
	}
}

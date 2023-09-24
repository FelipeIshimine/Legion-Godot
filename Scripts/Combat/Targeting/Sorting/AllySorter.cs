using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Sorting;

[System.Serializable]
public partial class AllySorter : TargetSorter
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
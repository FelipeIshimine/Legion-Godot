using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Sorting;

[System.Serializable]
public partial class HighestHealthSorter : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		return -x.Unit.CurrentHealth.CompareTo(y.Unit.CurrentHealth);
	}
}
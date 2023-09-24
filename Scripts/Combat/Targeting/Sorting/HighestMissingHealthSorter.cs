using Legion.Attributes.Derived;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Sorting;

[System.Serializable]
public partial class HighestMissingHealthSorter : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y)
	{
		if (x.Unit == null) return 1;
		if (y.Unit == null) return -1;
		return -(x.Unit.Attributes.Get<HealthAttribute>() - x.Unit.CurrentHealth).CompareTo(
			y.Unit.Attributes.Get<HealthAttribute>() - y.Unit.CurrentHealth);
	}
}
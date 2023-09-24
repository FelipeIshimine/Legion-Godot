using Legion.Character;
using Legion.Combat.Formation;
using Legion.GameStates;

namespace Legion.Combat.Targeting.Sorting;

[System.Serializable]
public partial class RandomSorter : TargetSorter
{
	public override int Compare(CharacterUnit source, FormationTile x, FormationTile y) => GameFlowMachine.Rng.RandiRange(-1, 2);
}
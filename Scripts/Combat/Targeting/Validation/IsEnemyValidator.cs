using Godot;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Validation;

public partial class IsEnemyValidator : BaseTargetValidator
{
	[Export] private Mode mode;

	public override bool Validate(CharacterUnit source, FormationTile target)
	{
		bool result;
		if (mode == Mode.Unit)
			result= target.Unit != null && target.WorldCoordinate.Z != source.WorldCoordinate.Z;
		else
			result= target.WorldCoordinate.Z != source.WorldCoordinate.Z;
		return result;
	}

	public enum Mode
	{
		Unit,
		Coordinate
	}
}
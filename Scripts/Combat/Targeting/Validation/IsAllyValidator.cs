using Godot;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Validation;

public partial class IsAllyValidator : BaseTargetValidator
{
	[Export] private Mode mode;

	public override bool Validate(CharacterUnit source, FormationTile target)
	{
		if (mode == Mode.Unit)
			return target.Unit != null && target.WorldCoordinate.Z == source.WorldCoordinate.Z;
		return target.WorldCoordinate.Z == source.WorldCoordinate.Z;
	}

	public enum Mode
	{
		Unit,
		Coordinate
	}
}
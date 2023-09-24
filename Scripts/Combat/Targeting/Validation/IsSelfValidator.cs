using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Validation;

public partial class IsSelfValidator : BaseTargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target)
	{
		return source == target.Unit;
	}
}
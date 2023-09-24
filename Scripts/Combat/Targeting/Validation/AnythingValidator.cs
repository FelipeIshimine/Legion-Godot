using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Validation;

public partial class AnythingValidator : BaseTargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target) => true;
}
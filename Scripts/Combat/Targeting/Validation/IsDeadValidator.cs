using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Validation;

public partial class IsDeadValidator : BaseTargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target) => target.Unit != null && target.Unit.CurrentHealth <= 0;
}
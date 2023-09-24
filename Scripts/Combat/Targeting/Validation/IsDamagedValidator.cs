using Legion.Attributes.Derived;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Validation;

public partial class IsDamagedValidator : BaseTargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target)
	{
		return target.Unit != null && target.Unit.Attributes.Get<HealthAttribute>() > target.Unit.CurrentHealth;
	}
}
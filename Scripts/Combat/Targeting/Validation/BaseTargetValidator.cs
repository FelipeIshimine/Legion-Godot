using Godot;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Targeting.Validation;

public abstract partial class BaseTargetValidator : Resource
{
	public abstract bool Validate(CharacterUnit source, FormationTile target);
}
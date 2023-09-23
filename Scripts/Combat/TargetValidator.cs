using Godot;
using Legion.Attributes.Derived;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat;

[System.Serializable]
public abstract partial class TargetValidator : Resource
{
	public abstract bool Validate(CharacterUnit source, FormationTile target);
}


[System.Serializable]
public partial class IsAlly : TargetValidator
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

[System.Serializable]
public partial class IsEnemy : TargetValidator
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
	
[System.Serializable]
public partial class IsSelf : TargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target)
	{
		return source == target.Unit;
	}
}
	
[System.Serializable]
public partial class IsDamaged : TargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target)
	{
		return target.Unit != null && target.Unit.Attributes.Get<MaxHealth>() > target.Unit.CurrentHealth;
	}
}
	
[System.Serializable]
public partial class IsFullHealth : TargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target)
	{
		return target.Unit != null && target.Unit.Attributes.Get<MaxHealth>() <= target.Unit.CurrentHealth;
	}
}
	
[System.Serializable]
public partial class IsAlive : TargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target) => target.Unit != null && target.Unit.CurrentHealth > 0;
}
[System.Serializable]
public partial class IsDead : TargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target) => target.Unit != null && target.Unit.CurrentHealth <= 0;
}

[System.Serializable]
public partial class All : TargetValidator
{
	public override bool Validate(CharacterUnit source, FormationTile target) => true;
}
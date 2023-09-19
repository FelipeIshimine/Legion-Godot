using Godot;

namespace Legion.Combat.Core;

public partial class CombatSkill : Node
{
	public string DisplayName;
}


public abstract partial class ActiveSkill<T> : CombatSkill where T : CombatAction<T>
{
	public T CreateAction()
	{
		T combatAction = OnCreateSkill();
		combatAction.DisplayName = DisplayName;
		return combatAction;
	}
	protected abstract T OnCreateSkill();
}

public abstract partial class ActiveSkill<T,TB> : ActiveSkill<T> where T : CombatAction<T,TB>
{
}

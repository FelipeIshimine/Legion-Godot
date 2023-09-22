using System.Linq;
using Extensions;
using Extensions.Godot;
using Godot;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat.Core;

public abstract partial class ActiveSkill : CombatSkill
{
	public abstract void FindSettings();
	public abstract CombatAction CreateAction(CharacterUnit characterUnit,
	                                          CombatSystemsContainer combatSystemsContainer,
	                                          params Vector3I[] targets);

	public Vector3I[] FilterTargets(CombatSystemsContainer combatContainer)
	{
		var formationSystem = combatContainer.GetSystem<FormationSystem>();
		return formationSystem.Tiles.Keys.ToArray();
	}
}

public abstract partial class ActiveSkill<T> : ActiveSkill where T : CombatAction<T>
{
	public override CombatAction CreateAction(CharacterUnit characterUnit,
	                                          CombatSystemsContainer combatSystemsContainer,
	                                          params Vector3I[] targets)
	{
		T combatAction = OnCreateAction();
		combatAction.Source = characterUnit;
		combatAction.SystemsContainer = combatSystemsContainer;
		combatAction.DisplayName = DisplayName;
		combatAction.Targets = targets;
		return combatAction;
	}
	protected abstract T OnCreateAction();
}

[Tool]
public abstract partial class ActiveSkill<T,TB> : ActiveSkill<T> where T : CombatAction<T,TB>, new() where TB : SkillSettings, new()
{
	public abstract TB SkillSettings { get; set; }

	protected override T OnCreateAction()
	{
		var combatAction = new T();
		combatAction.MySettings = SkillSettings;
		return combatAction;
	}

	public override void _Ready()
	{
		FindSettings();
	}

	public override void FindSettings()
	{
		/*if (SkillSettings == null)
		{
			SkillSettings = this.FindNode<SkillSettings>();
			/*if (SkillSettings == null)
			{
				SkillSettings = new TB();
				AddChild(SkillSettings,true);
			}#1#
		}	*/	
	}
}

public partial class SkillSettings : Resource
{
	
}
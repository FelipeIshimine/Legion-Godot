using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fractural.Tasks;
using Godot;
using Legion.Character;
using Legion.Scripts.GameSystems;

namespace Legion.Combat.Core;

public abstract partial class CombatAction : Node
{
	public CharacterUnit Source;
	public CombatSystemsContainer SystemsContainer;
	public Vector3I[] Targets;
	public string DisplayName;

	protected T GetSystem<T>() where T : GameSystem => SystemsContainer.GetSystem<T>();
	
	public async GDTask Perform(CancellationToken cancellationToken)
	{
		await PerformFlow(cancellationToken);
	}

	protected abstract GDTask PerformFlow(CancellationToken cancellationToken);

	public async Task Prepare(CancellationToken cancellationToken)
	{
		await PrepareFlow(cancellationToken);
	}
	
	protected abstract GDTask PrepareFlow(CancellationToken cancellationToken);

	
}


public class PrepareNotification
{
	public readonly CombatAction Value;
	public List<GDTask> Reactions = new List<GDTask>();

	public PrepareNotification(CombatAction value)
	{
		Value = value;
	}

	public bool WasCanceled { get; private set; }
	public void Cancel()
	{
		WasCanceled = true;
	}
}

public class PrepareNotification<T> : PrepareNotification where T : CombatAction<T>
{
	public T Action => (T)Value;
	public PrepareNotification(T value) : base(value)
	{
	}
}

public class PerformNotification
{
	public readonly CombatAction Value;
	public List<GDTask> Reactions = new List<GDTask>();

	public PerformNotification(CombatAction value)
	{
		Value = value;
	}
}

public class PerformNotification<T> : PerformNotification where T : CombatAction<T>
{
	public T Action => (T)Value;
	public PerformNotification(T value) : base(value)
	{
	}
}



public abstract partial class CombatAction<T> : CombatAction where T : CombatAction<T>
{
	public static event Action<PrepareNotification<T>> OnPrepare;
	public static event Action<PerformNotification> OnPerform;
	
	public PrepareNotification<T> NotificatePrepare()
	{
		PrepareNotification<T> notification = new PrepareNotification<T>((T)this);
		OnPrepare?.Invoke(notification);
		return notification;
	}
	
	public PerformNotification NotificatePerform()
	{
		PerformNotification<T> notification = new PerformNotification<T>((T)this);
		OnPerform?.Invoke(notification);
		return notification;
	}
}

public abstract partial class CombatAction<T, TB> : CombatAction<T> where T : CombatAction<T, TB>
{
	public TB MySettings;
}
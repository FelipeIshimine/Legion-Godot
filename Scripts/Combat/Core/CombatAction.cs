using System;
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

	private static int idCount;

	public readonly int ID = idCount++;

	protected T GetSystem<T>() where T : GameSystem => SystemsContainer.GetSystem<T>();
	
	public async GDTask Perform(CancellationToken cancellationToken)
	{
		await PerformFlow(cancellationToken);
	}

	protected abstract GDTask PerformFlow(CancellationToken cancellationToken);

	public async Task Prepare(CancellationToken cancellationToken)
	{
		GD.Print($"Prepare:{this} START");
		await PrepareFlow(cancellationToken);
		GD.Print($"Prepare:{this} END");
	}
	
	protected abstract GDTask PrepareFlow(CancellationToken token);

	public abstract PrepareNotification EmitPrepareNotification();
	public abstract PerformNotification EmitPerformNotification();
}



public abstract partial class CombatAction<T> : CombatAction where T : CombatAction<T>
{
	public static event Action<PrepareNotification<T>> OnPrepare;
	public static event Action<PerformNotification> OnPerform;
	
	public override PrepareNotification EmitPrepareNotification()
	{
		PrepareNotification<T> notification = new PrepareNotification<T>((T)this);
		OnPrepare?.Invoke(notification);
		return notification;
	}
	
	public override PerformNotification EmitPerformNotification()
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
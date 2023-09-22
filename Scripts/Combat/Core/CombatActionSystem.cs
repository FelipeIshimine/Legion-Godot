using System;
using System.Threading;
using System.Threading.Tasks;
using Fractural.Tasks;
using Godot;
using Godot.Collections;
using Legion.Scripts.GameSystems;

namespace Legion.Combat.Core;

public partial class CombatActionSystem : GameSystem
{
	public static event Action<CombatActionSystem> OnExecutionTreeUpdate; 
	
	[Export] private Dictionary<CombatAction, CombatAction> executionTree = new Dictionary<CombatAction, CombatAction>();

	public Dictionary<CombatAction, CombatAction> ExecutionTree => executionTree;

	private CancellationTokenSource cts;
	
	protected override void OnInitialize()
	{
		cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
	}

	protected override void OnTerminate()
	{
		cts.Cancel();
	}

	public async GDTask Execute(CombatAction combatAction, CombatAction parent, CancellationToken token)
	{
		if (parent == null)
		{
			GD.Print($"Sequence Start {combatAction}");
		}
		executionTree.Add(combatAction,parent);
		OnExecutionTreeUpdate?.Invoke(this);
		
		token = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token).Token;
		bool canPerform = await Prepare(combatAction, token);

		if (canPerform)
		{
			await Perform(combatAction, token);
		}
		
		executionTree.Remove(combatAction);
		OnExecutionTreeUpdate?.Invoke(this);

		if (parent == null)
		{
			GD.Print($"Sequence End {combatAction}");
		}
	}

	private async Task Perform(CombatAction combatAction, CancellationToken token)
	{
		GD.Print($"Perform:{combatAction}");

		PerformNotification notification = combatAction.EmitPerformNotification();

		await combatAction.Perform(token);
		
		foreach (var reaction in notification.Reactions)
		{
			await reaction;
		}
	}

	private async GDTask<bool> Prepare(CombatAction combatAction, CancellationToken cancellationToken)
	{
		GD.Print($"Prepare:{combatAction}");
		await combatAction.Prepare(cancellationToken);
		PrepareNotification notification = combatAction.EmitPrepareNotification();

		foreach (var reaction in notification.Reactions)
		{
			await reaction;
		}
		return !notification.WasCanceled;
	}
}

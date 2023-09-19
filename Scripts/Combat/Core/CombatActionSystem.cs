using System.Threading;
using System.Threading.Tasks;
using Fractural.Tasks;
using Godot;
using Godot.Collections;
using Legion.Scripts.GameSystems;

namespace Legion.Combat.Core;

public partial class CombatActionSystem : GameSystem
{
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

	public async GDTask Execute<T>(T combatAction, CombatAction parent, CancellationToken token) where T : CombatAction<T>
	{
		if (parent == null)
		{
			GD.Print($"Sequence Start {combatAction}");
		}
		executionTree.Add(combatAction,parent);
		
		token = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token).Token;
		bool canPerform = await Prepare(combatAction, token);

		if (canPerform)
		{
			await Perform(combatAction, token);
		}
		
		executionTree.Remove(combatAction);

		if (parent == null)
		{
			GD.Print($"Sequence End {combatAction}");
		}
	}

	private async Task Perform<T>(T combatAction, CancellationToken token) where T : CombatAction<T>
	{
		PerformNotification notification = combatAction.NotificatePerform();

		await combatAction.Perform(token);
		
		foreach (var reaction in notification.Reactions)
		{
			await reaction;
		}
	}

	private async GDTask<bool> Prepare<T>(T combatAction, CancellationToken cancellationToken) where T : CombatAction<T>
	{
		await combatAction.Prepare(cancellationToken);
		PrepareNotification notification = combatAction.NotificatePrepare();

		foreach (var reaction in notification.Reactions)
		{
			await reaction;
		}
		return !notification.WasCanceled;
	}
}

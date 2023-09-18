using System.Threading;
using Fractural.Tasks;
using Godot;

namespace Legion.GameStates;

public abstract partial class GameFlowState<T, TResult> : BaseGameFlowState<T> where T : GameFlowState<T, TResult>
{
	protected GameFlowState(Node parent) : base(parent)
    {
    }

	public async GDTask<TResult> Flow(CancellationToken cancellationToken)
    {
        GD.Print($"{typeof(T).Name} START");
        var result = await OnFlow(cancellationToken);
        GetParent().RemoveChild(this);
        QueueFree();
        GD.Print($"{typeof(T).Name} END Result:{result}");
        return result;
    }

	protected abstract GDTask<TResult> OnFlow(CancellationToken cancellationToken);
}

public abstract partial class GameFlowState<T> : BaseGameFlowState<T> where T : GameFlowState<T>
{
	protected GameFlowState(Node parent) : base(parent)
    {
    }

	public async GDTask Flow(CancellationToken cancellationToken)
    {
        GD.Print($"{typeof(T).Name} START");
        await OnFlow(cancellationToken);
        GetParent().RemoveChild(this);
        QueueFree();
        GD.Print($"{typeof(T).Name} END");
    }

	protected abstract GDTask OnFlow(CancellationToken cancellationToken);
}

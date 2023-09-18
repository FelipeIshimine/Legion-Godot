using System.Threading;
using Fractural.Tasks;
using Godot;

namespace Legion.GameFlow;

public abstract partial class FlowState<T, TResult> : BasePhaseFlow<T> where T : FlowState<T, TResult>
{
    public async GDTask<TResult> Flow(CancellationToken cancellationToken)
    {
        GD.Print($"{typeof(T).Name} START");
        var result = await OnFlow(cancellationToken);
        GD.Print($"{typeof(T).Name} END Result:{result}");
        return result;
    }

    protected abstract GDTask<TResult> OnFlow(CancellationToken cancellationToken);
}

public abstract partial class FlowState<T> : BasePhaseFlow<T> where T : FlowState<T>
{
    public async GDTask Flow()
    {
        GD.Print($"{typeof(T).Name} START");
        await OnFlow();
        GD.Print($"{typeof(T).Name} END");
    }

    protected abstract GDTask OnFlow();
}

public abstract partial class BasePhaseFlow<T> : Node where T : BasePhaseFlow<T>
{
    private static GameScenes gameScenes;
    protected static GameScenes GameScenes => gameScenes ??=GD.Load<GameScenes>("res://Scenes/GameScenes.tres");

    protected DataManager Data => DataManager.Instance;
    protected Node Root => GetTree().Root;
    
    public static  T Instance { get; set; }

    public override void _EnterTree()
    {
        base._EnterTree();
        Instance = this as T;
    }

    public override void _ExitTree()
    {
        Instance = null;
        base._ExitTree();
    }
    
}
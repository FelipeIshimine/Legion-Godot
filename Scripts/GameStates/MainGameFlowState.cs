using System.Threading;
using System.Threading.Tasks;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;

namespace Legion.GameFlow;

public partial class MainGameFlowState : FlowState<MainGameFlowState, MainGameFlowState.Result>
{
    [Export] private Button quitBtn; 
    [Export] private Button restartBtn; 
    [Export] private Button menuBtn; 
    [Export] private Button testCombatBtn; 
    
    public enum Result
    {
        Quit = 0,
        Restart = 1,
        Menu = 2,
    }

    protected override async GDTask<Result> OnFlow(CancellationToken cancellationToken)
    {
        CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var resultIndex = await new[] { quitBtn, restartBtn, menuBtn, testCombatBtn}.WaitForButtonUp(cancellationTokenSource.Token);
        cancellationTokenSource.Cancel();

        if (resultIndex < 3)
        {
            return (Result)resultIndex;
        }

        await TestCombatFlow(cancellationToken); 
        
        return Result.Menu;
    }

    private async GDTask TestCombatFlow(CancellationToken cancellationToken)
    {
        ResourceLoader.Load<PackedScene>()
    }
}

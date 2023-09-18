using System.Threading;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;

namespace Legion.GameFlow;

public partial class MenuFlowState : FlowState<MenuFlowState,MenuFlowState.Result>
{
    [Export] public Button continueBtn;
    [Export] public Button newGameBtn;
    [Export] public Button quitBtn;
    
    public enum Result
    {
        Quit = 0,
        NewGame = 1,
        Restart = 2,
        Menu = 3
    }

    public void ShowContinueButton(bool value) => continueBtn.Visible = value;
    
    protected override async GDTask<Result> OnFlow(CancellationToken cancellationToken)
    {
        CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var result = await new Button[] { quitBtn, newGameBtn, continueBtn }.WaitForButtonDown(cts.Token);
        cts.Cancel();
        return (Result)result;
    }

}
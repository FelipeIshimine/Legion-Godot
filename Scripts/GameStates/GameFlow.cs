using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Fractural.Tasks;
using Godot;
using Legion.GameFlow;

namespace Legion.GameStates;

public partial class GameFlow : FlowState<GameFlow>
{

    private CancellationToken destroyToken;
 

    public override async void _Ready()
    {
        destroyToken = this.GetCancellationTokenOnDestroy();
        await GDTask.NextFrame(destroyToken);
        await Flow();
    }

    protected override async GDTask OnFlow()
    {
        Debug.Print("Flow.Start");
        GD.Print("GameScenes Resource loaded");
        
    MainMenu:
        Debug.Print("Flow.MainMenu");
        
        var menuResult = await PlayMainMenuFlow(destroyToken);

        if (menuResult == MenuFlowState.Result.Quit) goto Quit;

        bool newGame = menuResult == MenuFlowState.Result.NewGame;
        
    Gameplay:
        Debug.Print("Flow.Gameplay");
        
        var result = await PlayMainGameFlow(newGame, destroyToken);

        switch (result)
        {
            case MainGameFlowState.Result.Quit:
                goto Quit;
            case MainGameFlowState.Result.Restart:
                newGame = true;
                goto Gameplay;
            case MainGameFlowState.Result.Menu:
                goto MainMenu;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    Quit:
        
        Debug.Print("Flow.End");
        GetTree().Quit();
    }

    private async GDTask<MainGameFlowState.Result> PlayMainGameFlow(bool newGame, CancellationToken cancellationToken)
    {
        if (!newGame)
        {
            GD.Print("Continue. Loading...");
            await GDTask.Delay(500, DelayType.DeltaTime, PlayerLoopTiming.Process, cancellationToken);
        }
        
        var scene = ResourceLoader.Load<PackedScene>(GameScenes.MainGame).Instantiate();
        Root.AddChild(scene);
        
        var result = await MainGameFlowState.Instance.Flow(destroyToken);

        Root.RemoveChild(scene);
        scene.QueueFree();

        if (!newGame)
        {
            GD.Print("Restart. Unloading...");
            await GDTask.Delay(500, DelayType.DeltaTime, PlayerLoopTiming.Process, cancellationToken);
        }
        

        return result;
    }

    private async Task<MenuFlowState.Result> PlayMainMenuFlow(CancellationToken cancellationToken)
    {
        var scene = ResourceLoader.Load<PackedScene>(GameScenes.MainMenu).Instantiate();

        Root.AddChild(scene);
        
        
        MenuFlowState.Instance.ShowContinueButton(Data.HasSavedGame());
        MenuFlowState.Result result = await MenuFlowState.Instance.Flow(cancellationToken);

        Root.RemoveChild(scene);
        scene.QueueFree();

        return result;
    }
}
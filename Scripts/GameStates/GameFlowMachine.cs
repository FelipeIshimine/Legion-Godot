using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;
using Legion.MainMenu;

namespace Legion.GameStates;

public partial class GameFlowMachine : Node
{
	public override async void _Ready()
    {
        var destroyToken = this.GetCancellationTokenOnDestroy();
        await GDTask.NextFrame(destroyToken);

        try
        {
	        await Flow(destroyToken);
        }
        catch (Exception e)
        {
	        GD.PrintErr(e);
	        
	        GetTree().Quit(-1);
	        throw;
        }
    }

	protected async GDTask Flow(CancellationToken cancellationToken)
    {
        Debug.Print("Flow.Start");
        
    MainMenu:
        var menuResult = await MainMenuFlow(cancellationToken);
        
        if (menuResult == MainMenuCanvas.Result.Quit) goto Quit;

        bool newGame = menuResult == MainMenuCanvas.Result.NewGame;
        
    Gameplay:

        var result = await new MainGameFlowState(this, newGame).Flow(cancellationToken);

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


	private async Task<MainMenuCanvas.Result> MainMenuFlow(CancellationToken cancellationToken)
    {
	    var canvasScene = ResourceLoader.Load<PackedScene>(GameScenes.Instance.MainMenu).Instantiate();
	    AddChild(canvasScene);

	    MainMenuCanvas mainMenuCanvas = canvasScene.FindNode<MainMenuCanvas>();

	    mainMenuCanvas.ShowContinueButton(DataManager.Instance.HasSavedGame());
	    MainMenuCanvas.Result menuResult = await mainMenuCanvas.Flow(cancellationToken);
	    
	    RemoveChild(canvasScene);
	    canvasScene.QueueFree();

        return menuResult;
    }
}
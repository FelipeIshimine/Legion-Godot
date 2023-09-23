using System.Threading;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;
using CombatFlow = Legion.Combat.CombatFlow;

namespace Legion.GameStates;

public partial class MainGameFlowState : GameFlowState<MainGameFlowState, MainGameFlowState.Result>
{
	public readonly bool NewGame;

	public MainGameFlowState(Node parent, bool newGame) : base(parent)
	{
		NewGame = newGame;
	}

	protected override async GDTask<Result> OnFlow(CancellationToken cancellationToken)
    {
	    if (!NewGame)
	    {
		    GD.Print("Continue. Loading...");
		    await GDTask.Delay(500, DelayType.DeltaTime, PlayerLoopTiming.Process, cancellationToken);
	    }
        
	    //Main Game Scene Load
	    var mainGameScene = ResourceLoader.Load<PackedScene>(GameScenes.Instance.MainGame).Instantiate();
	    Root.AddChild(mainGameScene);
	    mainGameScene.TryFindNodeOfType<MainGameCanvas>(out MainGameCanvas mainGameCanvas);
	    //Canvas 
	    var canvasResult = await mainGameCanvas.Flow(cancellationToken);
	    
	    //Unload Scene
        Root.RemoveChild(mainGameScene);
        mainGameScene.QueueFree();

        //Next Flow
        if (canvasResult == MainGameCanvas.Result.Combat)
        {
	        await TestCombatFlow(cancellationToken);
	        return Result.Menu;
        }

        return (Result)canvasResult;
    }

	private async GDTask TestCombatFlow(CancellationToken cancellationToken)
    {
	    CombatFlow.Result result = await new CombatFlow(this).Flow(cancellationToken);
	    
	    GD.Print($"Combat Result: {result}");
	    
    }

	public enum Result
    {
        Quit = 0,
        Restart = 1,
        Menu = 2,
    }
}
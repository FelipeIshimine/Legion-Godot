using System.Threading;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;
using CombatFlow = Legion.Combat.CombatFlow;

namespace Legion.GameStates;

public partial class MainGameCanvas : Node
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
		Combat = 3
	}

	public async GDTask<Result> Flow(CancellationToken cancellationToken)
	{
		CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		var resultIndex = await new[] { quitBtn, restartBtn, menuBtn, testCombatBtn}.WaitForButtonUp(cancellationTokenSource.Token);
		cancellationTokenSource.Cancel();
		return (Result)resultIndex;
	}

	
}
using System.Threading;
using Extensions.Godot;
using Fractural.Tasks;
using Godot;

namespace Legion.MainMenu;

public partial class MainMenuCanvas : Node
{
	[Export] private Button continueBtn;
	[Export] private Button newGameBtn;
	[Export] private Button quitBtn;

	public enum Result
	{
		Quit = 0,
		NewGame = 1,
		Restart = 2,
	}
	
	public void ShowContinueButton(bool value) => continueBtn.Visible = value;

	public async GDTask<Result> Flow(CancellationToken cancellationToken)
	{
		CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		var result = await new Button[] { quitBtn, newGameBtn, continueBtn }.WaitForButtonDown(cts.Token);
		cts.Cancel();
		return (Result)result;
	}
}
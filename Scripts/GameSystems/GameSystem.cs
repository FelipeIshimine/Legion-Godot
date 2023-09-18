using Godot;

namespace Legion.Scripts.GameSystems;

public abstract partial class GameSystem : Node
{
	internal void Initialize() => OnInitialize();
	internal void Terminate() => OnTerminate();
	
	protected abstract void OnInitialize();
	protected abstract void OnTerminate();
}
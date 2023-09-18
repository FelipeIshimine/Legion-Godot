using System.Threading;
using Fractural.Tasks;
using Godot;

namespace Legion.GameStates;

public abstract partial class BaseGameFlowState : Node
{
	protected Node Root => GetTree().Root;
	protected DataManager Data => DataManager.Instance;

	protected BaseGameFlowState(Node parent)
	{
		parent.AddChild(this);
		Name = GetType().Name;
	}

	public override void _Ready()
	{
		Name = GetType().Name;
	}
}

public abstract partial class BaseGameFlowState<T> : BaseGameFlowState where T : BaseGameFlowState<T>
{
	protected BaseGameFlowState(Node parent) : base(parent)
	{
	}
}
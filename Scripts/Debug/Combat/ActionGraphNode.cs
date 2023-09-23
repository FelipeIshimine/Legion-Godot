using Godot;

namespace Legion.Debug.Combat;

public partial class ActionGraphNode : GraphNode
{

	[Export] private Label label;
	public override void _Ready()
	{
		SetSlot(0,true, 0, Colors.Azure, true, 0, Colors.Bisque);
		this.ResizeRequest += OnResizeRequest;
		this.CloseRequest += OnCloseRequested;
	}

	private void OnCloseRequested()
	{
		QueueFree();
	}

	public void OnResizeRequest(Vector2 nSize)
	{
		this.Size = nSize;
	}

	public override void _Input(InputEvent input)
	{
		if (input.IsActionPressed("ToggleNodeSlots"))
		{
			bool left = IsSlotEnabledLeft(0);
			bool right = IsSlotEnabledRight(0);
			GD.Print($"Left:{left} Right:{right}");
			SetSlot(
				0,
				!IsSlotEnabledLeft(0),
				0,
				Colors.Azure,
				!IsSlotEnabledRight(0),
				0,
				Colors.Bisque);
		}
	}
	public void SetSource(string s) => label.Text = s;
}
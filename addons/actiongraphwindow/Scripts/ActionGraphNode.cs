using Godot;
using System;

public partial class SimpleGraphNode : GraphNode
{
	public override void _Ready()
	{
		this.ResizeRequest += OnResizeRequest;
		this.CloseRequest += OnCloseRequested;
		
		SetSlot(0,true, 0, Colors.Azure, true, 0, Colors.Bisque);
	}

	private void OnCloseRequested()
	{
		QueueFree();
	}

	public void OnResizeRequest(Vector2 nSize)
	{
		this.Size = nSize;
	}
}

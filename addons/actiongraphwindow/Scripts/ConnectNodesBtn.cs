using Godot;
using System;

public partial class ConnectBtn : HBoxContainer
{
	public event Action<string, string> OnConnectionRequest;
	[Export] private LineEdit fromTxt;
	[Export] private LineEdit toTxt;
	[Export] private Button btn;

	public override void _Ready()
	{
		btn.ButtonDown += WhenButtonDown;
		fromTxt.TextSubmitted += WhenTextSubmitted;
		toTxt.TextSubmitted += WhenTextSubmitted;
	}

	private void WhenTextSubmitted(string newtext) => WhenButtonDown();

	private void WhenButtonDown()
	{
		OnConnectionRequest?.Invoke(fromTxt.Text, toTxt.Text);
	}
}

#if TOOLS
using Godot;
using System;

[Tool]
public partial class quickreloadproject : EditorPlugin
{
	public override void _EnterTree()
	{
	}

	public override void _ExitTree()
	{
	}


	public override void _Input(InputEvent input)
	{
		if (input is InputEventKey key && key.Pressed && key.CtrlPressed && key.ShiftPressed && key.Keycode == Key.F5)
		{
			GetEditorInterface().RestartEditor(true);
		}
	}
}
#endif

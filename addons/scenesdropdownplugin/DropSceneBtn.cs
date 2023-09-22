using System;
using Godot;

namespace Legion.ScenesDropdownPlugin;

public partial class DropSceneBtn : Control
{
	public event Action<int> OnItemSelected;  
	[Export] private OptionButton optionButton;

	
	public void SetOptions(string[] names)
	{
		foreach (string name in names)
		{
			optionButton.AddItem(name);
		}
	}

	public override void _Ready()
	{
		optionButton.ItemSelected += ItemSelected;

		for (int i = 0; i < 8; i++)
		{
			optionButton.AddItem(i.ToString());
		}
	}

	private void ItemSelected(long index) => OnItemSelected?.Invoke((int)index);
}
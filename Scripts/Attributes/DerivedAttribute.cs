using System.Collections.Generic;
using Godot;

namespace Legion.Attributes;

[GlobalClass]
public abstract partial class DerivedAttribute : BaseAttribute
{
	[Export] private int baseValue;

	public override int Calculate(Dictionary<int, int> dictionary)
	{
		int value = baseValue;
		return value;
	}
}
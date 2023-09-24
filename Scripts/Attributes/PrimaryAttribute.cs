using System.Collections.Generic;
using Godot;

namespace Legion.Attributes;

[GlobalClass]
public abstract partial class PrimaryAttribute : BaseAttribute
{
	public override int Calculate(Dictionary<int, int> dictionary) => dictionary.TryGetValue(this, out var value) ? value : 0;
}
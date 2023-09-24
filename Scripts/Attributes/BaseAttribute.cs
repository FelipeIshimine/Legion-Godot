using System.Collections.Generic;
using Godot;

namespace Legion.Attributes;

[GlobalClass]
public abstract partial class BaseAttribute : Resource
{
	[Export] public Texture2D Icon { get; set; }
	[Export] public string Description { get; set; }
	public abstract int Key { get; }
	public abstract int Calculate(Dictionary<int, int> dictionary);

	public static implicit operator int(BaseAttribute attribute) => attribute.Key;

	public abstract void Initialize();

}

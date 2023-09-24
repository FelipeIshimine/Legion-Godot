using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace Legion.Attributes;

public partial class AttributesSetup : Node
{
	[Export] public PrimaryAttribute[] MainAttributes = Array.Empty<PrimaryAttribute>();
	[Export] public DerivedAttribute[] DerivedAttributes = Array.Empty<DerivedAttribute>();
	public static Dictionary<int, BaseAttribute> Map { get; } = new Dictionary<int, BaseAttribute>();
	
	public override void _Ready()
	{
		Map.Clear();
		foreach (var resource in MainAttributes)
		{
			var baseAttribute = (BaseAttribute)resource;
			baseAttribute.Initialize();
			Map.Add(baseAttribute, baseAttribute);
		}
		foreach (var resource in DerivedAttributes)
		{
			var baseAttribute = (BaseAttribute)resource;
			baseAttribute.Initialize();
			Map.Add(baseAttribute, baseAttribute);
		}
		base._Ready();
	}
	
}
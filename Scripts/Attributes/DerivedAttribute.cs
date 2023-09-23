using System;
using System.Collections.Generic;
using Godot;

namespace Legion.Attributes;

public abstract partial class DerivedAttribute : BaseAttribute
{
	[Export] private int baseValue;
	//[Export] private AttributePercentage[] sourceAttributes = Array.Empty<AttributePercentage>();

	public override int Calculate(Dictionary<int, int> dictionary)
	{
		int value = baseValue;
		/*
		foreach (var pair in sourceAttributes)
		{
			value += Mathf.FloorToInt(pair.Attribute.Calculate(dictionary) * pair.Percentage);
		}
		*/
		return value;
	}

}

public abstract partial class DerivedAttribute<T> : DerivedAttribute where T : DerivedAttribute<T>
{
	private static readonly int HashCode = typeof(T).GetHashCode();
	public override int GetHashCode() => HashCode;
	public override int Key => HashCode;
}
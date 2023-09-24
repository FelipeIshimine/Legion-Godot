using System;
using System.Collections.Generic;
using Godot;
using Legion.Attributes.Modifiers;
using LegionCombat.Extensions;

namespace Legion.Attributes;

public partial class AttributesNode : Node
{
	public event Action<BaseAttribute> OnAttributeUpdate;

	private readonly Dictionary<int,int> cache = new Dictionary<int,int>();

	private readonly Dictionary<PrimaryAttribute, List<AttributeModifier.MainAttributeModifier>>
			mainModifiers = new ();

	private readonly Dictionary<DerivedAttribute, List<AttributeModifier.DerivedAttributeModifier>> derivedModifiers = new();

	private readonly Dictionary<object, List<AttributeModifier.BaseAttributeModifier>> sourceToModifiers = new();

	[ExportGroup("Debug")] private bool isDirty = true;

	private int this[BaseAttribute attribute] => attribute.Calculate(GetCache());

	public void AddModifiers(IEnumerable<AttributeModifier.MainAttributeModifier> modifiers, object source)
		{
			foreach (var modifier in modifiers) AddModifier(modifier, source);
		}

	public void AddModifier(AttributeModifier.MainAttributeModifier modifier, object source)
		{
			isDirty = true;
			AddSource(modifier, source);
			if (!mainModifiers.TryGetValue(modifier.Primary, out var list))
				mainModifiers[modifier.Primary] = list = new List<AttributeModifier.MainAttributeModifier>();
			list.InsertSorted(modifier);
			OnAttributeUpdate?.Invoke(modifier.Attribute);
		}

	public void AddModifiers(IEnumerable<AttributeModifier.DerivedAttributeModifier> modifiers, object source)
		{
			foreach (var modifier in modifiers) AddModifier(modifier, source);
		}

	public void AddModifier(AttributeModifier.DerivedAttributeModifier modifier, object source)
		{
			isDirty = true;
			AddSource(modifier, source);
			if (!derivedModifiers.TryGetValue(modifier.Derived, out var list))
				derivedModifiers[modifier.Derived] = list = new List<AttributeModifier.DerivedAttributeModifier>();
			list.InsertSorted(modifier);
			OnAttributeUpdate?.Invoke(modifier.Attribute);
		}

	public int Get<T>() where T : BaseAttribute => this[AttributesSetup.Map[typeof(T).GetHashCode()]];

	private Dictionary<int,int> GetCache()
		{
			if (isDirty)
			{
				Recalculate();
			}
			return cache;
		}

	private void Recalculate()
		{
			isDirty = false;
			cache.Clear();

			foreach (var pair in mainModifiers)
			{
				foreach (var modifier in pair.Value)
				{
					cache.TryGetValue(modifier.Primary.Key, out var value);
					cache[modifier.Primary.Key] = modifier.Process(value);
				}
			}
			
			foreach (var pair in derivedModifiers)
			{
				foreach (var modifier in pair.Value)
				{
					cache.TryGetValue(modifier.Derived.Key, out var value);
					cache[modifier.Derived.Key] = modifier.Process(value);
				}
			}
			
			
		}

	private void AddSource(AttributeModifier.BaseAttributeModifier modifier, object source)
		{
			if (!sourceToModifiers.TryGetValue(source, out var list))
				sourceToModifiers[modifier] = list = new List<AttributeModifier.BaseAttributeModifier>();
			list.Add(modifier);
		}
}
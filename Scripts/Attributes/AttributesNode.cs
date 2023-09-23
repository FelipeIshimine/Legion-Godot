using System;
using System.Collections.Generic;
using Godot;
using Legion.Attributes.Modifiers;
using LegionCombat.Extensions;

namespace Legion.Attributes;

public partial class AttributesNode : Node
{
	public event Action<BaseAttribute> OnAttributeUpdate;

		private Dictionary<int,int> cache = new Dictionary<int,int>();

		private Dictionary<MainAttribute, List<AttributeModifier.MainAttributeModifier>>
			mainModifiers;

		private Dictionary<DerivedAttribute, System.Collections.Generic.List<AttributeModifier.DerivedAttributeModifier>> derivedModifiers =
			new Dictionary<DerivedAttribute, System.Collections.Generic.List<AttributeModifier.DerivedAttributeModifier>>();

		private Dictionary<object, System.Collections.Generic.List<AttributeModifier.BaseAttributeModifier>> sourceToModifiers =
			new Dictionary<object, System.Collections.Generic.List<AttributeModifier.BaseAttributeModifier>>();

		[ExportGroup("Debug")] private bool isDirty = true;

		private Dictionary<int,int> GetCache()
		{
			if (isDirty)
			{
				Recalculate();
			}
			return cache;
		}

		private int this[BaseAttribute attribute] => attribute.Calculate(GetCache());

		private void Recalculate()
		{
			isDirty = false;
			cache.Clear();

			foreach (var pair in mainModifiers)
			{
				foreach (var modifier in pair.Value)
				{
					cache.TryGetValue(modifier.Main.Key, out var value);
					cache[modifier.Main.Key] = modifier.Process(value);
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

		public void AddModifiers(IEnumerable<AttributeModifier.MainAttributeModifier> modifiers, object source)
		{
			foreach (var modifier in modifiers) AddModifier(modifier, source);
		}

		public void AddModifier(AttributeModifier.MainAttributeModifier modifier, object source)
		{
			isDirty = true;
			AddSource(modifier, source);
			if (!mainModifiers.TryGetValue(modifier.Main, out var list))
				mainModifiers[modifier.Main] = list = new List<AttributeModifier.MainAttributeModifier>();
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

		private void AddSource(AttributeModifier.BaseAttributeModifier modifier, object source)
		{
			if (!sourceToModifiers.TryGetValue(source, out var list))
				sourceToModifiers[modifier] = list = new List<AttributeModifier.BaseAttributeModifier>();
			list.Add(modifier);
		}

		public int Get<T>() where T : BaseAttribute => this[AttributeTypes.Map[typeof(T).GetHashCode()]];
}
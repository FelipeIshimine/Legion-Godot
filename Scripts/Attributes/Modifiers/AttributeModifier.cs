using System;
using System.Collections.Generic;
using Godot;

namespace Legion.Attributes.Modifiers;

public abstract partial class AttributeModifier : Resource
{
	[System.Serializable]
	public abstract class BaseAttributeModifier
	{
		public abstract BaseAttribute Attribute { get; }
		public abstract int Process(int oldValue);
	}

	[System.Serializable]
	public abstract partial class MainAttributeModifier : BaseAttributeModifier, IComparer<MainAttributeModifier>, IComparable<MainAttributeModifier>
	{
		[Export] protected MainAttribute mainAttribute;
		public MainAttribute Main => mainAttribute;
		public override BaseAttribute Attribute => mainAttribute;

		public int Compare(MainAttributeModifier x, MainAttributeModifier y) 
		{
			int result = 0;
			result += x is MainAttributeModifierFlat ? -1 : 0;
			result += y is MainAttributeModifierFlat ? 1 : 0;
			return result;
		}

		public int CompareTo(MainAttributeModifier other) => Compare(this, other);
	}

	[System.Serializable]
	public partial class MainAttributeModifierFlat : MainAttributeModifier
	{
		[Export] private int value;
		public override int Process(int oldValue) => oldValue + this.value;
		
		public MainAttributeModifierFlat(MainAttribute attribute, int value)
		{
			mainAttribute = attribute;
			this.value = value;
		}
	}

	[System.Serializable]
	public partial class MainAttributeModifierPercent : MainAttributeModifier
	{
		public float Percentage = 1;
		public override int Process(int oldValue) => Mathf.RoundToInt(oldValue * Percentage);	
		
		public MainAttributeModifierPercent(MainAttribute attribute, int percentage)
		{
			mainAttribute = attribute;
			this.Percentage = percentage;
		}
	}
	
	[System.Serializable]
	public abstract class DerivedAttributeModifier : BaseAttributeModifier, IComparer<DerivedAttributeModifier>, IComparable<DerivedAttributeModifier>
	{
		[Export] protected DerivedAttribute derivedAttribute;
		public DerivedAttribute Derived => derivedAttribute;
		public override BaseAttribute Attribute => derivedAttribute;


		public int Compare(DerivedAttributeModifier x, DerivedAttributeModifier y)
		{
			int result = 0;
			result += x is DerivedAttributeModifierFlat ? -1 : 0;
			result += y is DerivedAttributeModifierFlat ? 1 : 0;
			return result;
		}

		public int CompareTo(DerivedAttributeModifier other) => Compare(this, other);
	}

	[System.Serializable]
	public partial class DerivedAttributeModifierFlat : DerivedAttributeModifier
	{
		[Export] private int value;
		public DerivedAttributeModifierFlat() { }

		public DerivedAttributeModifierFlat(DerivedAttribute attribute, int value)
		{
			derivedAttribute = attribute;
			this.value = value;
		}
		public override int Process(int oldValue) => oldValue + this.value;
	}

	[System.Serializable]
	public partial class DerivedAttributeModifierPercent : DerivedAttributeModifier
	{
		public float Percentage = 1;
		public override int Process(int oldValue) => Mathf.RoundToInt(oldValue * Percentage);
		
		public DerivedAttributeModifierPercent(DerivedAttribute attribute, int percentage)
		{
			derivedAttribute = attribute;
			this.Percentage = percentage;
		}
	}
}
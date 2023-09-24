using System.Collections.Generic;
using Godot;

namespace Legion.Attributes;

public partial class AttributesSheet : Node
{
	[Export] public int Strength;
	[Export] public int Dexterity;
	[Export] public int Constitution;
	[Export] public int Wisdom;
	[Export] public int Intelligence;
	[Export] public int Charisma;
	
	public IEnumerable<(PrimaryAttribute Attribute, int Value)> GetValues()
	{
		yield return (MainAttributes.StrengthAttribute.Instance, Strength);
		yield return (MainAttributes.DexterityAttribute.Instance, Dexterity);
		yield return (MainAttributes.ConstitutionAttribute.Instance, Constitution);
		yield return (MainAttributes.WisdomAttribute.Instance, Wisdom);
		yield return (MainAttributes.IntelligenceAttribute.Instance, Intelligence);
		yield return (MainAttributes.CharismaAttribute.Instance, Charisma);
	}
}


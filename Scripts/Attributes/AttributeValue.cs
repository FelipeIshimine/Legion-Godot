using Godot;

namespace Legion.Attributes;


[GlobalClass]
public partial class AttributeValue : RefCounted
{
	[Export] public int Value;
	[Export] public PrimaryAttribute Attribute;
}
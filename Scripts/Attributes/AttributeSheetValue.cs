using Godot;

namespace Legion.Attributes;

[GlobalClass]
public partial class AttributeSheetValue : Node
{
	[Export] public PrimaryAttribute Attribute;
	[Export] public int Value;
}
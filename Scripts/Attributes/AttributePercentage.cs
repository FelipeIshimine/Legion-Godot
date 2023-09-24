using Godot;
using Legion.Attributes.MainAttributes;

namespace Legion.Attributes;

[GlobalClass]
public partial class AttributePercentage : Resource
{
	[Export] public PrimaryAttribute Attribute; 
	[Export] public float Percentage = 1;
}
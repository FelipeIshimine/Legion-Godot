using Godot;
using Legion.Attributes.MainAttributes;

namespace Legion.Attributes;

[GlobalClass]
public partial class AttributePercentage : Resource
{
	[Export] public MainAttribute Attribute; 
	[Export] public float Percentage = 1;
}
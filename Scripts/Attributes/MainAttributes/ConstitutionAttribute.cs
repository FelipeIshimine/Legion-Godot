using Godot;

namespace Legion.Attributes.MainAttributes;

[GlobalClass]
public partial class ConstitutionAttribute : PrimaryAttributeT<ConstitutionAttribute>
{
	[Export] private string value;
}
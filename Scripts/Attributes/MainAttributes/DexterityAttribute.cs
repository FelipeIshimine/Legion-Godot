using Godot;

namespace Legion.Attributes.MainAttributes;

[GlobalClass]
public partial class DexterityAttribute : PrimaryAttributeT<DexterityAttribute>
{
	[Export] private string value;

}
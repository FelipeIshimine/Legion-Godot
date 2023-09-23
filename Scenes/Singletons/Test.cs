using Godot;
using System;
using Legion.Attributes;

public partial class Test : Node
{
	[Export] public BaseAttribute BaseAttribute;
	[Export] public BaseAttribute[] BaseAttributes;
	
	[Export] public MainAttribute MainAttribute;
	[Export] public MainAttribute[] MainAttributes;
	
	[Export] public DerivedAttribute DerivedAttribute;
	[Export] public DerivedAttribute[] DerivedAttributes;
}

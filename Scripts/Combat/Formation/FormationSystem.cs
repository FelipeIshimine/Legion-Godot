using System;
using Godot;
using Godot.Collections;
using Legion.Scripts.Combat.Formation;

public partial class FormationSystem : Legion.Scripts.GameSystems.GameSystem
{
	[Export] public Formation LeftFormation {get; private set;}
	[Export] public Formation RightFormation {get; private set;}
	
	protected override void OnInitialize()
	{
	}

	protected override void OnTerminate()
	{
	}
	
	
	
}
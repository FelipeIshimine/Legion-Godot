using Godot;

namespace Legion.Scripts.Combat.Formation;

[GlobalClass,Tool]
public partial class FormationTile : Node3D
{
	private Vector2I coordinate;
	[Export] public Vector2I Coordinate
	{
		get => coordinate;
		private set
		{
			coordinate = value;
			Name = coordinate.ToString();
		}
	}
}
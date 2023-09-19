using Godot;
using Legion.Character;

namespace Legion.Combat.Formation;

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

	public CharacterUnit Unit { get; set; }
	public bool IsEmpty => Unit == null;
}
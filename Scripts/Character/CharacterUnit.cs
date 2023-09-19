using Godot;
using FormationTile = Legion.Combat.Formation.FormationTile;

namespace Legion.Character;

public partial class CharacterUnit : Node3D
{
	[Export] private Sprite3D sprite;

	public bool LookingRight => !sprite.FlipH;
	
	public FormationTile Tile { get; private set; }
	public Vector2I Coordinate => Tile.Coordinate;
	
	public void Flip() => sprite.FlipH = !sprite.FlipH;

	public void SetTile(FormationTile formationTile)
	{
		if (Tile != null)
		{
			Tile.Unit = null;
		}
		
		Tile = formationTile;

		if (Tile != null)
		{
			Tile.Unit = this;
		}
	}

	public void LookAt(Vector3 targetGlobalPosition)
	{
		switch (LookingRight)
		{
			case true when targetGlobalPosition.X  < GlobalPosition.X:
			case false when targetGlobalPosition.X > GlobalPosition.X:
				Flip();
				break;
		}
	}


}
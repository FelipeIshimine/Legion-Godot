using System.Collections;
using Godot;

namespace Legion.Combat.Formation;

public partial class Formation : Node3D
{
	[Export] private FormationTile[] tiles = new FormationTile[4];
	public FormationTile[] Tiles => tiles;
	public FormationTile this[int index] => tiles[index];

	public override void _Ready()
	{
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i].LocalCoordinate = new Vector2I(i%2,i/2);
		}
	}
}
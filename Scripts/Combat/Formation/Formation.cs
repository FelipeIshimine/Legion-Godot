using System.Collections;
using Godot;

namespace Legion.Combat.Formation;

public partial class Formation : Node3D
{
	[Export] private FormationTile[] tiles = new FormationTile[4];
	public FormationTile[] Tiles => tiles;
	public FormationTile this[int index] => tiles[index];
}
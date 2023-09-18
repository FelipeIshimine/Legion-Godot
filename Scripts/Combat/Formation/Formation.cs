using System.Collections.Generic;
using Godot;

namespace Legion.Scripts.Combat.Formation;

public partial class Formation : Node3D
{
	[Export] private FormationTile[] tiles = new FormationTile[4];
	public FormationTile this[int index] => tiles[index];
}
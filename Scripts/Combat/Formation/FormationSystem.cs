using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Legion.Combat.Formation;

public partial class FormationSystem : Legion.Scripts.GameSystems.GameSystem
{
	private Dictionary<Vector3I, FormationTile> tiles = new Dictionary<Vector3I, FormationTile>();
	[Export] public Formation LeftFormation {get; private set;}
	[Export] public Formation RightFormation {get; private set;}

	public Dictionary<Vector3I, FormationTile> Tiles => tiles;

	public FormationTile this[Vector3I coord] => Tiles[coord];

	protected override void OnInitialize()
	{
		foreach (var tile in LeftFormation.Tiles)
		{
			Tiles.Add(new Vector3I(tile.LocalCoordinate.X,tile.LocalCoordinate.Y,0), tile);
			tile.WorldCoordinate = new Vector3I(
				tile.LocalCoordinate.X,
				tile.LocalCoordinate.Y,
				0);
		}
		
		foreach (var tile in RightFormation.Tiles)
		{
			Tiles.Add(new Vector3I(tile.LocalCoordinate.X,tile.LocalCoordinate.Y,1), tile);
			tile.WorldCoordinate = new Vector3I(
				tile.LocalCoordinate.X,
				tile.LocalCoordinate.Y,
				1);
		}
	}

	protected override void OnTerminate()
	{
	}
}
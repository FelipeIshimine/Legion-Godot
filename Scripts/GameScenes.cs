using Godot;
using System;

public partial class GameScenes : Resource
{
    private static GameScenes instance;
    public static GameScenes Instance => instance ??=GD.Load<GameScenes>("res://Scenes/GameScenes.tres");

	[Export] public string Combat = "res://GamePhases/Combat.tscn";
    [Export] public string MainGame = "res://GamePhases/MainGame.tscn";
    [Export] public string Shop = "res://GamePhases/Shop.tscn";
    [Export] public string MainMenu = "res://GamePhases/MainMenu.tscn";
}

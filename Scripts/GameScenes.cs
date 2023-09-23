using Godot;
using System;

public partial class GameScenes : Resource
{
    private static GameScenes instance;
    public static GameScenes Instance => instance ??=GD.Load<GameScenes>("res://Scenes/GameScenes.tres");

	[Export] public string Combat = "res://Scenes/Combat.tscn";
    [Export] public string MainGame = "res://Scenes/MainGame.tscn";
    [Export] public string Shop = "res://Scenes/Shop.tscn";
    [Export] public string MainMenu = "res://Scenes/MainMenu.tscn";
}

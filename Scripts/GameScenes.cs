using Godot;
using System;

public partial class GameScenes : Resource
{
    [Export] public string Combat = "res://GamePhases/Combat.tscn";
    [Export] public string MainGame = "res://GamePhases/MainGame.tscn";
    [Export] public string Shop = "res://GamePhases/Shop.tscn";
    [Export] public string MainMenu = "res://GamePhases/MainMenu.tscn";
}

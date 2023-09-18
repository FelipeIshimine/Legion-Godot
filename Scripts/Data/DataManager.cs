using Godot;
using System;

public partial class DataManager : Node
{
    public static DataManager Instance { get; private set; }

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }

    public bool HasSavedGame() => new RandomNumberGenerator().RandiRange(0, 1) == 0;
}

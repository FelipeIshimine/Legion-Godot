using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Legion.GameSystems;

public partial class GameSystemsContainer : Node
{
    [Export] private GameSystem[] systems = new GameSystem[0]; 
    public override void _Ready()
    {
        systems = CollectSystems().ToArray();
    }

    IEnumerable<GameSystem> CollectSystems()
    {
        foreach (Node child in GetChildren())
        {
            if (child is GameSystem gameSystem)
                yield return gameSystem;
        }
    }

}
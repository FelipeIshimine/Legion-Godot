using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Godot;
using Godot;
using Legion.Scripts.GameSystems;

namespace Legion.GameSystems;

public partial class GameSystemsContainer : Node
{
	[Export] private Scripts.GameSystems.GameSystem[] systems = Array.Empty<GameSystem>();

	public void Initialize()
	{
		systems = this.FindNodesOfType<GameSystem>().ToArray();
		
		foreach (var gameSystem in systems)
		{
			GD.Print($"Initialize {gameSystem.Name}");
			gameSystem.Initialize();
		}
	}

	public void Terminate()
	{
		foreach (var gameSystem in systems)
		{
			gameSystem.Terminate();
		}
	}

	public override void _Ready()
    {
        systems = CollectSystems().ToArray();
    }

	public T GetSystem<T>() where T : Scripts.GameSystems.GameSystem => Array.Find(systems, x => x is T) as T;

	IEnumerable<Scripts.GameSystems.GameSystem> CollectSystems()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Scripts.GameSystems.GameSystem gameSystem)
                yield return gameSystem;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Legion.GameSystems;

public partial class GameSystemsContainer : Node
{
	[Export] private Scripts.GameSystems.GameSystem[] systems = new Scripts.GameSystems.GameSystem[0];

	public void Initialize()
	{
		foreach (var gameSystem in systems)
		{
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
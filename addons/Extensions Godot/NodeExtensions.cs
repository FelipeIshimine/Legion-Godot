using System.Collections.Generic;
using Godot;

namespace Extensions.Godot;

public static class NodeExtensions
{
	public static T FindNodeFromRoot<T>(this Node node) where T : Node
	{
		foreach (Node child in node.GetTree().Root.GetChildren())
		{
			if (child is T childT)
				return childT;
		}
		return default;
	}
	
	public static bool TryFindNodeOfType<T>(this Node node, out T result) where T : Node
	{
		if (node is T value)
		{
			result = value;
			return true;
		}
		foreach (Node child in node.GetChildren())
		{
			if (child.TryFindNodeOfType<T>(out result))
				return true;
		}
		result = null;
		return false;
	}
	public static IEnumerable<T> FindNodesOfType<T>(this Node node) where T : Node
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is T childT)
				yield return childT;
		}
	}
	
}
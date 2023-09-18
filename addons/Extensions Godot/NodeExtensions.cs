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
	
	public static T FindNode<T>(this Node node) where T : Node
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is T childT)
				return childT;
		}
		return default;
	}
	
}
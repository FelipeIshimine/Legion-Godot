using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Legion.Combat.Core;

namespace Legion.Debug.Combat;

public partial class CombatActionGraph : Control
{
	private readonly Dictionary<string, ActionGraphNode> idToNode = new Dictionary<string, ActionGraphNode>();
	private readonly Dictionary<string, List<string>> parentToChildren = new Dictionary<string, List<string>>();
	private readonly Dictionary<string, string> childToParent = new Dictionary<string, string>();

	[Export] public bool AutoCenterOnSelected = true;
	[Export] public float AutoCenterSpeed = 8;
	
	private CombatActionSystem combatActionSystem;
	[Export] private Button btn;
	[Export] private Button removeBtn;
	[Export] private GraphEdit graphEdit;
	[Export] private PackedScene nodeScn;
	[Export] private ConnectNodesBtn connectBtn;
	[Export] private Vector2 nodesSeparation;

	private HashSet<string> roots = new HashSet<string>();
	private List<ActionGraphNode> nodes = new List<ActionGraphNode>();

	public GraphNode SelectedNode { get; private set; }

	public List<ActionGraphNode> Nodes => nodes;

	public override async void _Ready()
	{
		base._Ready();
		btn.ButtonUp += WhenButtonUp;
		removeBtn.ButtonUp += RemoveNode;
		connectBtn.OnConnectionRequest += ConnectionRequested;
		
		graphEdit.DisconnectionRequest += WhenDisconnectionRequested;
		graphEdit.ConnectionRequest += WhenConnectionRequested;
		graphEdit.NodeSelected += SelectNode;
		graphEdit.NodeDeselected += DeselectNode;

		
		return;
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Seed = 0;
		for (int i = 0; i < 128; i++)
		{
			CreateRandomNode();
			//graphEdit.ScrollOffset = node.PositionOffset - GetScreenHaftSize();
			await Task.Delay(1000);
		}
	}

	public void Initialize(CombatActionSystem system)
	{
		if (combatActionSystem != null)
		{
			combatActionSystem.OnTreeUpdate -= UpdateTree;
		}
		combatActionSystem = system;
		if (combatActionSystem != null)
		{
			combatActionSystem.OnTreeUpdate += UpdateTree;
		}		
	}
	
	public void UpdateTree(CombatActionSystem combatActionSystem)
	{
		Clear();
		foreach (var pair in combatActionSystem.ExecutionTree)
		{
			if (pair.Value != null)
			{
				Add(CreateId(pair.Key),CreateId(pair.Value));
			}
			else
			{
				GetOrCreateNode(CreateId(pair.Key));
			}
		}
	}


	private static string CreateId(CombatAction combatAction)
	{
		GD.Print($"||||||||||||||||||||||||||||||||| {combatAction.DisplayName}");
		return $"{combatAction.GetInstanceId()}:{combatAction.DisplayName}";
	}

	private void CreateRandomNode()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		string newNodeName, parent;
		do
		{
			newNodeName = rng.Randi().ToString();
		} while (idToNode.ContainsKey(newNodeName));

		var node = GetOrCreateNode(newNodeName);

		if (Nodes.Count > 1)
		{
			do
			{
				parent = Nodes[rng.RandiRange(0, Nodes.Count - 1)].Name;
			} while (parent == newNodeName);

			Add(parent, newNodeName);
		}

		RefreshPositions();
		graphEdit.SetSelected(node);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (AutoCenterOnSelected && SelectedNode != null)
		{
			graphEdit.ScrollOffset=graphEdit.ScrollOffset.Lerp(SelectedNode.PositionOffset - GetScreenHaftSize(), (float)(delta * AutoCenterSpeed));
			//graphEdit.ScrollOffset =  graphEdit.ScrollOffset.MoveToward(
			//	SelectedNode.PositionOffset - GetScreenHaftSize(),
			//	(float)(delta * AutoCenterSpeed));
		}
	}

	public void Add(Dictionary<string, string> nChildToParentConnections)
	{
		foreach (var childToParentConnection in nChildToParentConnections)
		{
			Add(childToParentConnection.Value,childToParentConnection.Key);
		}
	}

	public void Clear()
	{
		graphEdit.ClearConnections();

		var nodes = new List<ActionGraphNode>(idToNode.Values);

		foreach (var node in nodes)
		{
			node.QueueFree();
		}

		idToNode.Clear();
		roots.Clear();
		parentToChildren.Clear();
		childToParent.Clear();
	}

	public void RemoveTree(string root)
	{
		Queue<string> next = new Queue<string>(16);
		next.Enqueue(root);

		while (next.Count > 0)
		{
			var nodeName = next.Dequeue();
			var node = idToNode[nodeName];

			if (parentToChildren.TryGetValue(nodeName, out var children))
			{
				foreach (string childName in children)
				{
					GD.Print($"From:{nodeName} To:{childName}");
					graphEdit.DisconnectNode(nodeName, 0, childName, 0);
					next.Enqueue(childName);
				}
				parentToChildren.Remove(nodeName);
			}

			if (childToParent.TryGetValue(nodeName, out var parentName))
			{
				graphEdit.DisconnectNode(parentName,0,nodeName,0);
			}
			RemoveNode(node);
		}
	}

	
	public void Add(string from, string to)
	{
		GD.Print($">>>>>>>>> From:{from} To:{to}");
		
		ActionGraphNode fromNode = GetOrCreateNode(from);
		fromNode.Title  = fromNode.Name = from;
		ActionGraphNode toNode = GetOrCreateNode(to);
		toNode.Title  = toNode.Name = to;

		graphEdit.ConnectNode(from, 0, to, 0);
		
		childToParent.Add(to,from);

		if (!parentToChildren.TryGetValue(from, out var children))
		{
			parentToChildren[from] = children = new List<string>();
		}
		children.Add(to);
	}

	public void RefreshPositions()
	{
		//int depth = CalculateDepth();
		CalculateRoots();
		Vector2 offset = Vector2.Zero;

		int index = 0;
		foreach (var root in roots)
		{
			var treeSize = UpdateTreePositions(root,offset);
			//GD.Print($"{index++}:{treeSize} ");
			offset.Y += treeSize.Y;
			//GD.Print($"Offset:{offset}");
		}
	}

	private void DeselectNode(Node node)
	{
		if (SelectedNode == node)
		{
			SelectedNode = null;
		}
	}

	private void SelectNode(Node node)
	{
		SelectedNode = node as GraphNode;
	}

	private void RemoveNode()
	{
		if (SelectedNode != null)
		{
			RemoveTree(SelectedNode.Name);
			RefreshPositions();
		}
	}

	private void RemoveNode(ActionGraphNode node)
	{
		Nodes.Remove(node);
		node.QueueFree();
	}

	private void ConnectionRequested(string arg1, string arg2)
	{
		Add(arg1,arg2);
		RefreshPositions();
	}


	private void WhenButtonUp()
	{
		RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
		var node = GetOrCreateNode(randomNumberGenerator.Randi().ToString());
		node.PositionOffset = GetScreenHaftSize();
	}

	private Vector2I GetScreenHaftSize() => (Vector2I)(Size / 2);

	private ActionGraphNode CreateNode()
	{
		GD.Print(nodeScn);
		var node = (ActionGraphNode)nodeScn.Instantiate();
		GD.Print(node);
		graphEdit.AddChild(node);
		Nodes.Add(node);
		return node;
	}

	private void WhenDisconnectionRequested(StringName from, long fromPort, StringName to, long toPort)
	{
		graphEdit.DisconnectNode(from, (int)fromPort, to, (int)toPort);
	}

	private void WhenConnectionRequested(StringName from, long fromPort, StringName to, long toPort)
	{
		graphEdit.ConnectNode(from, (int)fromPort, to, (int)toPort);
	}

	private ActionGraphNode GetOrCreateNode(string nodeName)
	{
		if (!idToNode.TryGetValue(nodeName, out var value))
		{
			idToNode[nodeName] = value = CreateNode();

			var split = nodeName.Split(":");
			value.Name = nodeName;
			value.SetSource(split[0]);
			value.Title = split[1];
		}
		return value;
	}

	private Vector2 UpdateTreePositions(string root, Vector2 offset)
	{
		Queue<string[]> next = new Queue<string[]>();
		next.Enqueue(new []{root});
		Vector2 size = Vector2.Zero;
		int depth = 0;
		while (next.Count > 0)
		{
			var nodeNames = next.Dequeue();
			List<string> nodesForNext = new List<string>();
			for (int i = 0; i < nodeNames.Length; i++)
			{
				var nodeName = nodeNames[i];
				var node = idToNode[nodeName];
				node.PositionOffset = offset + new Vector2(depth * nodesSeparation.X, i * nodesSeparation.Y);
				if (parentToChildren.TryGetValue(nodeName, out var children))
				{
					nodesForNext.AddRange(children);
				}
				size = new Vector2(Mathf.Max(size.X, node.PositionOffset.X -offset.X), Mathf.Max(size.Y, node.PositionOffset.Y -offset.Y));
			}

			if (nodesForNext.Count > 0)
			{
				next.Enqueue(nodesForNext.ToArray());
			}
			depth++;
		}
		return size + nodesSeparation;
	}

	private int CalculateDepth()
	{
		CalculateRoots();

		int depth = 0;
		foreach (string root in roots)
		{
			depth = Mathf.Max(depth, GetDepth(root));
		}

		return depth;
	}

	private int GetDepth(string node)
	{
		if (parentToChildren.TryGetValue(node, out var children))
		{
			int depth = 0;
			foreach (string child in children)
			{
				depth = Mathf.Max(depth, GetDepth(child));
			}
			return 1 +depth;
		}
		return 1;
	}

	private void CalculateRoots()
	{
		roots.Clear();
		foreach (var pair in childToParent)
		{
			if (!childToParent.ContainsKey(pair.Value))
			{
				roots.Add(pair.Value);
			}
		}
	}
}
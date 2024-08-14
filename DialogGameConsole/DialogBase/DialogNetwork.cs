using DialogGameConsole.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.DialogBase;

public class DialogNetwork : DialogBase<DialogNetwork>
{
    private readonly DialogNode _startNode;

    private readonly HashSet<DialogNode> _nodes = new();

    private readonly HashSet<DialogEdge> _edges = new();

    private readonly Dictionary<DialogNode, HashSet<DialogEdge>> _nodeConnections = new();

    private readonly Dictionary<DialogEdge, DialogNode> _edgeFromNodes = new();

    private readonly Dictionary<DialogEdge, DialogNode> _edgeToNodes = new();

    public DialogNetwork()
    {
        _startNode = DialogNode.NewNode(NodeType.Start);
        AddNode(_startNode);
    }

    public void AddNode(DialogNode node)
    {
        Assert.IsNotNull(node, nameof(node));
        if (_nodes.Contains(node)) throw new ArgumentException($"Node {node} already added to network {this}");
        _nodes.Add(node);
        _nodeConnections[node] = new();
    }

    public bool HasNode(DialogNode node)
    {
        return _nodes.Contains(node);
    }

    public void AddEdge(DialogEdge edge, DialogNode nodeFrom, DialogNode nodeTo)
    {
        Assert.IsNotNull(edge, nameof(edge));
        Assert.IsNotNull(nodeFrom, nameof(nodeFrom));
        Assert.IsNotNull(nodeTo, nameof(nodeTo));

        if (_edges.Contains(edge)) throw new ArgumentException($"Edge {edge} already added to network {this}");
        AssertNodeExists(nodeFrom);
        AssertNodeExists(nodeTo);

        _nodeConnections[nodeFrom].Add(edge);

        _edges.Add(edge);
        _edgeFromNodes[edge] = nodeFrom;
        _edgeToNodes[edge] = nodeTo;
    }

    public DialogNode GetStartNode()
    {
        return _startNode;
    }

    public HashSet<DialogEdge> GetEdges(DialogNode node)
    {
        AssertNodeExists(node);
        return _nodeConnections[node];
    }

    public DialogNode GetNodeFrom(DialogEdge edge)
    {
        AssertEdgeExists(edge);
        return _edgeFromNodes[edge];
    }

    public DialogNode GetNodeTo(DialogEdge edge)
    {
        AssertEdgeExists(edge);
        return _edgeToNodes[edge];
    }

    private void AssertNodeExists(DialogNode node)
    {
        Assert.IsNotNull(node, nameof(node));
        if (!_nodes.Contains(node))
            throw new ArgumentException($"Node {node} is not part of the network {this}");

    }

    private void AssertEdgeExists(DialogEdge edge)
    {
        Assert.IsNotNull(edge, nameof(edge));
        if (!_edges.Contains(edge))
            throw new ArgumentException($"Edge {edge} is not part of the network {this}");

    }

    public override string ToString()
    {
        return base.ToString() + "\n" +
            string.Join("\n", _nodes.Select(x => x.ToString())) + "\n" +
            string.Join("\n", _edges.Select(x => x.ToString() + " From " + GetNodeFrom(x).Id + " to " + GetNodeTo(x).Id));
    }
}
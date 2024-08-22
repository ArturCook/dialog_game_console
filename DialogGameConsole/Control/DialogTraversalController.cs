using DialogGameConsole.DialogBase;
using DialogGameConsole.Enums;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Text.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Control;

public class DialogTraversalController
{
    private readonly TimerController _timerController;

    private readonly DialogPanelController _dialogPanelController;

    private readonly OptionsController _optionsController;

    private readonly DebugController _debugController;

    public DialogTraversalController(TimerController timerController, DialogPanelController dialogPanelController,
        OptionsController optionsController, DebugController debugController)
    {
        _timerController = timerController;
        _dialogPanelController = dialogPanelController;
        _optionsController = optionsController;
        _debugController = debugController;
    }

    private IDialog _dialog;
    private Stack<(DialogEdge, DialogNode)> _history = new();
    private HashSet<DialogEdge> _currentEdges = new();
    private readonly Dictionary<DialogEdge, long> _timeout = new();

    public void AssignDialog(IDialog dialog)
    {
        _dialog = dialog;
        _history.Clear();
        _currentEdges.Clear();
        _timeout.Clear();
        var node = _dialog.Network.GetStartNode();

        _history.Push((null, node));
        ApplyNode(node);
        AddEdges(node);
    }

    public void UpdateTime(long dt)
    {
        foreach (var edge in _currentEdges)
        {
            var isActive = false;
            switch (edge.Type)
            {
                case EdgeType.Direct:
                    isActive = true;
                    break;
                case EdgeType.Timing:
                    _timeout[edge] -= dt;
                    isActive = _timeout[edge] <= 0;
                    break;
                case EdgeType.Probability:
                    isActive = false;//edge.Probability <= probability;
                    break;
                default:
                    break;
            }

            if (isActive)
            {
                TraverseEdge(edge);
                return;
            }
        }
    }

    public void TraverseEdge(DialogEdge edge)
    {
        _currentEdges.Remove(edge);
        foreach (var edge2 in _currentEdges)
        {
            if (edge2.Type == EdgeType.Option)
                _currentEdges.Remove(edge2);
        }

        _dialogPanelController.DisablePlayerTyping();
        _dialogPanelController.DisableNpcTyping();

        _debugController.AddMessage($"Traversing {edge}");

        var node = _dialog.Network.GetNodeTo(edge);
        _history.Push((edge, node));
        ApplyNode(node);
        AddEdges(node);
    }

    public void Rewind()
    {
        var (edge, node) = _history.Pop();

        _debugController.AddMessage($"Rewinding {edge}");

        UnapplyNode(node);
        RemoveEdges(node);

        var (_, prevNode) = _history.Peek();
        AddEdges(prevNode);

        if (node.Type != NodeType.Start && edge.Type == EdgeType.Direct)
            Rewind();
    }

    private void ApplyNode(DialogNode node)
    {
        _debugController.AddMessage($"Reached {node}");
        if (node.Type == NodeType.Text)
        {
            _timerController.ResetTimer();
            _dialogPanelController.AddDialog(node.Text.GetText(_dialog.Map), node.Text.Character);

        }
        else if (node.Type == NodeType.SetInfo)
        {
            _dialog.Map.SetValue(node.InfoValue);
            UpdateOptions();
        }
        else if (node.Type == NodeType.Start) {
            _debugController.AddMessage($"Dialog Started");
        }
        else if (node.Type == NodeType.Finish) {
            _debugController.AddMessage($"Dialog Finished");
        }
    }

    private void AddEdges(DialogNode node)
    {
        if (node.Type == NodeType.Finish) return;

        var edges = _dialog.Network.GetEdges(node);
        if (!edges.Any())
            throw new Exception($"No edges found for node {node}");

        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

    private void AddEdge(DialogEdge edge)
    {
        if (edge.IsTiming())
        {
            var delayProfile = edge.Text.GetDelayProfile(_dialog.Map).ToList();
            var timeout = delayProfile.Sum(p => p.Item1);
            _timeout[edge] = timeout;
            _dialogPanelController.SetTyping(edge.Text.Character, delayProfile);
        }

        if (_currentEdges.Contains(edge)) return;
        _currentEdges.Add(edge);

        if (edge.IsOption() && !_optionsController.HasOption(edge.Option)) {
            var newInstance = _dialog.NewOptionInstance(edge.Option, () => TraverseEdge(edge));
            _optionsController.AddOption(newInstance);
        }
    }

    private void UnapplyNode(DialogNode node)
    {
        _debugController.AddMessage($"Rewinding {node}");
        if (node.Type == NodeType.Text)
        {
            _timerController.ResetTimer();
            _dialogPanelController.Rewind();
        }
        else if (node.Type == NodeType.SetInfo)
        {
            _dialog.Map.Rewind();
            UpdateOptions();
        }
    }

    private void RemoveEdges(DialogNode node)
    {
        if (node.Type == NodeType.Finish) return;

        var edges = _dialog.Network.GetEdges(node);
        if (!edges.Any())
            throw new Exception($"No edges found for node {node}");

        foreach (var edge in edges)
        {
            RemoveEdge(edge);
        }
    }

    private void RemoveEdge(DialogEdge edge)
    {
        if (!_currentEdges.Contains(edge)) return;
        
        _currentEdges.Remove(edge);
        if (edge.IsTiming())
        {
            _timeout.Remove(edge);
        }
        else if (edge.IsOption())
            _optionsController.RemoveOption(edge.Option);
    }

    private void UpdateOptions() {
        _optionsController.UpdateOptions();

        foreach (var edge in _currentEdges) {
            if (edge.Type != EdgeType.Option)
                continue;
            if (_optionsController.IsObsolete(edge.Option))
                _currentEdges.Remove(edge);
        }
    }
}

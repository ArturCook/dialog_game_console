using DialogGameConsole.DialogBase;
using DialogGameConsole.Enums;
using DialogGameConsole.Infos;
using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Text.Enums;
using DialogGameConsole.Texts;
using DialogGameConsole.Util;
using System;

namespace DialogGameConsole.DialogBuilderBase;

public class DialogBuilder<T> where T : IDialog
{
    public T _dialog;
    public MentalMap _mentalMap => _dialog.Map;

    public DialogNetwork _network => _dialog.Network;

    public DialogNode CurrentNode;

    public DialogEdge CurrentEdge;

    public DialogBuilder(params dynamic[] arguments) { }

    public DialogBuilder(T dialog) :
        this(dialog, dialog.Network.GetStartNode(), DialogEdge.NewDirect()) { }

    public DialogBuilder<T> Clone(DialogNode node = null, DialogEdge edge = null) 
        => new(_dialog, node ?? CurrentNode, edge ?? CurrentEdge);

    public DialogBuilder<T> Clone(DialogEdge edge) => new(_dialog, CurrentNode, edge);

    private DialogBuilder(T dialog, DialogNode node, DialogEdge edge)
    {
        Assert.IsNotNull(dialog, nameof(dialog));
        Assert.IsNotNull(node, nameof(node));
        Assert.IsNotNull(edge, nameof(edge));

        _dialog = dialog;
        CurrentNode = node;
        CurrentEdge = edge;
    }

    public DialogBuilder<T> AddText(Character character, string text, Delay delay = Delay.Immediate)
        => AddText(new(character, text, delay));

    public DialogBuilder<T> AddText(TextGroupBuilder builder) {
        Assert.IsNotNull(builder, nameof(builder));
        var newTexts = builder.Build();
        var dialogBuilder = this;
        foreach (var newText in newTexts) {
            var edge = DialogEdge.NewDelay(newText);
            var node = DialogNode.NewTextNode(newText);
            dialogBuilder = dialogBuilder.Clone(edge).AddNode(node);
        }
        return dialogBuilder;
    }

    public DialogBuilder<T> GetOrAddNode(DialogNode node)
    {
        if (!_network.HasNode(node))
            return AddNode(node);

        _network.AddEdge(CurrentEdge, CurrentNode, node);
        return Clone(node, DialogEdge.NewDirect());
    }

    public DialogBuilder<T> AddNode(DialogNode node)
    {
        _network.AddNode(node);
        _network.AddEdge(CurrentEdge, CurrentNode, node);
        return Clone(node, DialogEdge.NewDirect());
    }

    public DialogBuilder<T> AddEmptyNode() {
        var node = DialogNode.NewNode(NodeType.Empty);
        return AddNode(node);
    }

    public DialogBuilder<T> SetVariable<K>(Info<K> info, K value) where K : struct
        => SetVariable(info.GetInfoValue(value));

    public DialogBuilder<T> SetVariable(IInfoValue infoValue)
    {
        var node = DialogNode.NewSetInfoNode(infoValue);
        return AddNode(node);
    }

    public DialogBuilderList<T> BranchOut()
    {
        return new DialogBuilderList<T>(Clone(DialogEdge.NewStop()));
    }

    public DialogBuilder<T> Values(params dynamic[] arguments) {
        return this;
    }

    public DialogBuilder<T> FinishDialog()
    {
        return AddNode(DialogNode.NewNode(NodeType.Finish));
    }


    /*




    public DialogNetworkBuilder<T> AddProbability(double probability)
    {
        Assert.IsWithin(probability, 0, 1, nameof(probability));
        var edge1 = DialogEdge.NewProbability(0, probability);
        var edge2 = DialogEdge.NewProbability(probability, 1);
        var stopEdge = DialogEdge.NewStop();

        var mainNode = GetCurrentNode(NBType.Main);
        var newBranches = _branchList
            .AddBranch(NBType.Prob_01, mainNode, edge1)
            .AddBranch(NBType.Prob_02, mainNode, edge2)
            .ReplaceBranch(NBType.Main, mainNode, stopEdge);

        return Clone(newBranches);
    }

    public DialogNetworkBuilder<T> AddTimeout(double delay, NBType type = NBType.Timeout_01)
    {
        Assert.IsPositive(delay, nameof(delay));
        if (!type.IsTimeout())
            throw new ArgumentException($"Type {type.GetText()} is not a valid timout type");
        return AddDelay(delay, false, DS.Undefined, type);
    }


    public DialogNetworkBuilder<T> Merge(DialogNetworkBuilder<T> otherBuilder)
    {
        var empty_node = DialogNode.NewNode(NodeType.Empty);
        otherBuilder.AddNode(empty_node);
        _network.AddEdge(GetCurrentEdge(), GetCurrentNode(), empty_node);
        var newEdge = DialogEdge.NewDirect();
        var branches = _branchList.ReplaceBranch(_branchType, empty_node, newEdge);
        return Clone(branches);
    }

    public DialogNetworkBuilder<T> MergeBranch(NBType type)
    {
        if (type == _branchType) throw new ArgumentException("Can't merge a branch with itself. Type: " + type);
        if (type == NBType.Main) throw new ArgumentException("Can't merge main branch.");
        if (type == NBType.Undefined) throw new ArgumentException("Can't merge undefined branch.");

        var emptyNode = DialogNode.NewNode(NodeType.Empty);
        _network.AddNode(emptyNode);
        _network.AddEdge(GetCurrentEdge(), GetCurrentNode(), emptyNode);

        var branchNode = _branchList.GetNode(type);
        var branchEdge = _branchList.GetEdge(type);
        _network.AddEdge(branchEdge, branchNode, emptyNode);

        var newEdge = DialogEdge.NewDirect();
        var newBranches = _branchList.RemoveBranch(type)
            .ReplaceBranch(_branchType, emptyNode, newEdge);

        return Clone(newBranches);
    }

    public DialogNetworkBuilder<T> MergeAllBranches()
    {
        var emptyNode = DialogNode.NewNode(NodeType.Empty);
        _network.AddNode(emptyNode);
        foreach (var (node, edge) in _branchList.GetBranches())
        {
            _network.AddEdge(edge, node, emptyNode);

        }
        var newEdge = DialogEdge.NewDirect();
        var branches = _branchList.ClearBranches();
        if (!branches.HasBranch(NBType.Main))
        {
            branches.AddBranch(NBType.Main, emptyNode, newEdge);
        }
        else
        {
            branches = branches.ReplaceBranch(NBType.Main, emptyNode, newEdge);
        }
        return Clone(branches, NBType.Main);
    }
    */
    public DialogNetwork Build()
    {
        return _network;
    }





}
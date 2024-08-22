using DialogGameConsole.DialogBase;
using DialogGameConsole.Enums;
using DialogGameConsole.Infos.Interfaces;
using DialogGameConsole.Options;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Util;
using LanguageExt;
using LanguageExt.ClassInstances;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.DialogBuilderBase;

public class DialogBuilderList<T> where T : IDialog
{
    private readonly DialogBuilder<T> _rootBuilder;

    private readonly Dictionary<BranchType, DialogBuilder<T>> _builders = new();

    public DialogBuilderList(DialogBuilder<T> builder)
    {
        Assert.IsNotNull(builder, nameof(builder));
        _rootBuilder = builder;
    }

    public DialogBuilderList<T> NewOption(BranchType type, OptionBuilder optionBuilder, Func<DialogBuilder<T>, DialogBuilder<T>> onBranch = null) {
        var newOption = optionBuilder.Build();
        return AddOption(type, newOption, onBranch);
    }

    public DialogBuilderList<T> NewOptionWithText(BranchType type, string optionText, Func<DialogBuilder<T>, DialogBuilder<T>> onBranch = null) {
        var newOption = OptionBuilder.New(optionText).Build();
        return AddOption(type, newOption, onBranch);
    }

    public DialogBuilderList<T> AddOption(BranchType type, DialogOption newOption, Func<DialogBuilder<T>, DialogBuilder<T>> onBranch = null) {
        if (type is not BranchType.Option_01 and not BranchType.Option_02 and not BranchType.Option_03)
            throw new ArgumentException("Invalid Option Type");

        var edge = DialogEdge.NewOption(newOption);
        var builder = _rootBuilder.Clone(edge).AddEmptyNode();
        var newBranch = AddBranch(type, builder);
        if (onBranch == null)
            return newBranch;

        return OnBranch(type, onBranch);
    }

    public DialogBuilderList<T> OnBranch(BranchType type, Func<DialogBuilder<T>, DialogBuilder<T>> onBranch = null)
    {
        AssertExists(type);
        if (onBranch == null) return this;

        _builders[type] = onBranch(_builders[type]);
        return this;
    }

    public DialogBuilder<T> SwitchTo(BranchType type) {
        AssertExists(type);
        return _builders[type];
    }

    public DialogBuilder<T> Merge()
    {
        if (!_builders.Any()) return _rootBuilder;
        
        var emptyNode = DialogNode.NewNode(NodeType.Empty);
        foreach(var (type, branch) in _builders)
        {
            _builders[type] = branch.GetOrAddNode(emptyNode);
        }
        
        return _builders.Values.First();
    }



    private DialogBuilderList<T> AddBranch(BranchType type, DialogBuilder<T> builder) {
        AssertNotExists(type);
        Assert.IsNotNull(builder, nameof(builder));
        _builders[type] = builder;
        return this;
    }

    private bool HasBranch(BranchType type)
    {
        return _builders.ContainsKey(type);
    }

    private void AssertExists(BranchType type)
    {
        Assert.IsNotDefault(type, nameof(type));
        if (!HasBranch(type))
            throw new ArgumentException($"Branch of type {type} is not present on {this}");
    }

    private void AssertNotExists(BranchType type)
    {
        Assert.IsNotDefault(type, nameof(type));
        if (HasBranch(type))
            throw new ArgumentException($"Branch of type {type} is already present on {this}");
    }
}
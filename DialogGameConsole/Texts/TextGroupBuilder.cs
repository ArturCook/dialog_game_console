using DialogGameConsole.Text.Enums;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Texts;
public class TextGroupBuilder
{
    private readonly List<DialogText> _texts = new();

    public TextGroupBuilder(params object[] args) {
        if (args is null || args.Length == 0)
            throw new ArgumentException("No sufficient arguments passed to builder");

        ParseObject(args);
    }

    private void ParseObject(object[] args) {
        foreach (var argGroup in args.BreakAt(arg => arg is Character)) {
            var character = (Character)argGroup[0];
            var subGroups = argGroup.Skip(1).BreakAt(arg => arg is string).ToList();

            var subGroupCount = subGroups.Count;
            var textArgsIndex = 0;
            while (textArgsIndex < subGroupCount) {
                var textArgs = new List<object>() { };
                var textArgsTry = new List<object>() { character };

                foreach (var (i, subGroup) in subGroups.WithIndex().Skip(textArgsIndex)) {
                    textArgsTry.AddRange(subGroup);
                    if (TextBuilder.CanParse(textArgsTry.ToArray())) {
                        textArgs.Clear();
                        textArgs.AddRange(textArgsTry);
                        textArgsIndex = i+1;
                    }
                }
                if (textArgs.Count == 0) {
                    throw new Exception("Failed to parse text arguments");
                }
                _texts.Add(new TextBuilder(textArgs.ToArray()).Build());
            }
        }
    }

    public List<DialogText> Build() => _texts;
}

using DialogGameConsole.DialogBase;
using System;

namespace DialogGameConsole.DialogBuilderBase;
public class DialogBuilderFunction<T> where T : IDialog
{
    private readonly Func<DialogBuilder<T>, DialogBuilder<T>> _function;

    public DialogBuilder<T> Apply(DialogBuilder<T> builder) {
        return _function(builder);
    }  

    public static DialogBuilderFunction<T> IdentityFunction = new DialogBuilderFunction<T>(x=>x);

    public DialogBuilderFunction(Func<DialogBuilder<T>, DialogBuilder<T>> function) {
        _function = function;
    }
}

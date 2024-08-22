using DialogGameConsole.DialogBuilderBase;
using DialogGameConsole.Enums;
using DialogGameConsole.Infos.Enums;
using DialogGameConsole.Options;
using DialogGameConsole.Options.Enums;
using DialogGameConsole.Text.Enums;

namespace DialogGameConsole.Dialogs.PetDialog;

public static class PetDialogBuilder
{
    public static PetDialog New() {
        return FullDialog();
    }
    private static PetDialog FullDialog() {
        var dialog = new PetDialog();
        var mm = dialog.Map;
        var su = dialog.SubjectMap.Subject;
        var builder = new DialogBuilder<PetDialog>(dialog);

 
        
        var knowDogName = mm.Generic.DogName.IsNot(DogName.Unknown);
        var dogName = mm.Generic.DogName;


        var petType = mm.Branching.PetType;
        var petTypeDog = petType.GetInfoValue(PetType.Dog);
        var petTypeCat = petType.GetInfoValue(PetType.Dog);
        var petTypeNone = petType.GetInfoValue(PetType.Dog);

        builder = builder
            .AddText(Character.Npc, "Hey!", Delay.Fast)
            .AddText(Character.Npc, "How's it going?", Delay.Fast)
            .AddText(Character.Npc, "Do you have any pets?? 🐱", Delay.Fast)
            .BranchOut()

            .NewOption(BranchType.Option_01, new("A Dog.", OptionDelay.Fast, OptionSpeed.Fast))
            .NewOption(BranchType.Option_02, new("None.", OptionDelay.Fast, OptionSpeed.Normal))
            .NewOption(BranchType.Option_03, new("A Cat!", OptionDelay.Slow, OptionSpeed.Slow))

            .OnBranch(BranchType.Option_01, new(branch => branch
                .SetVariable(petTypeDog)
                .AddText(Character.Player, "I have a Dog!", Delay.Fast)
                .AddText(Character.Player, "He's the bestest of the boys!", Delay.Fast)

            ))
            .OnBranch(BranchType.Option_02, new(branch => branch
                .SetVariable(petTypeNone)
                .AddText(Character.Player, "None")
                .AddText(Character.Player, "I Wish!", Delay.Slow)
            ))
            .OnBranch(BranchType.Option_03, new(branch => branch
                .SetVariable(petTypeCat)
                .AddText(Character.Player, "A Cat...", Delay.Fast)
                .AddText(Character.Player, "I didn't even want it at first", Delay.Slow)
                .AddText(Character.Player, "It just hopped in my house one day by accident", Delay.Normal)
                .AddText(Character.Npc, "What?", Delay.Fast)
            ))
            .Merge();

        var hasAPet = petType.Is(PetType.Dog).Or(PetType.Cat);
        builder = builder
            .AddText(new(Character.Npc, "Awwwnnn!", Delay.Fast, hasAPet))
            .AddText(new(Character.Npc, "So cute!", Delay.Fast, hasAPet));


        var petName1 = OptionBuilder.New("Buddy");
        var petName2 = OptionBuilder.New("Duke");
        var petName3 = OptionBuilder.New("Loki");

        builder = builder
            .AddText(new(Character.Npc, "Awwwnnn!", Delay.Fast, hasAPet))
            .AddText(new(Character.Npc, "What is it's name?", Delay.Fast, hasAPet))
            .BranchOut()
            .NewOption(BranchType.Option_01, petName1, new(branch => branch
                .AddText(Character.Player, "Buddy")
                .SetVariable(dogName.GetInfoValue(DogName.Buddy))
            ))
            .NewOption(BranchType.Option_02, petName2, new(branch => branch
                .AddText(Character.Player, "Duke")
                .SetVariable(dogName.GetInfoValue(DogName.Duke))
            ))
            .NewOption(BranchType.Option_03, petName3, new(branch => branch
                .AddText(Character.Player, "Loki")
                .SetVariable(dogName.GetInfoValue(DogName.Loki))
            ))
            .Merge();

        return dialog;

    }
}
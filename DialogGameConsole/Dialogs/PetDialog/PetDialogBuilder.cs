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

    private static PetDialog TestDialog() {

        var dialog = new PetDialog();
        var mm = dialog.Map;
        var su = dialog.SubjectMap.Subject;
        var builder = new DialogBuilder<PetDialog>(dialog);

        var trivialConversation = su.Is(SubjectStrength.Trivial);
        var talkingAboutPets = su.Is(PetDialogSubjects.Pets);
        var talkingAboutPetsNames = su.Is(PetDialogSubjects.Pets, PetDialogSubSubjects.PetName);
        var knowDogName = mm.Generic.DogName.IsNot(DogName.Unknown);
        var dogName = mm.Generic.DogName;


        var dogNameOption1 = OptionBuilder.New("Buddy");
        var dogNameOption2 = OptionBuilder.New("Duke");
        var dogNameOption3 = OptionBuilder.New("Loki");

        builder = builder
            .AddText(new(Character.Player, "Hi.", Delay.Fast))
            .AddText(Character.Npc, "Dog Name?", Delay.Normal)
            .BranchOut()
            .NewOption(BranchType.Option_01, dogNameOption1, new(branch => branch
                .AddText(Character.Player, "Buddy")
                .SetVariable(dogName.GetInfoValue(DogName.Buddy))
            ))
            .NewOption(BranchType.Option_02, dogNameOption2, new(branch => branch
                .AddText(Character.Player, "Duke")
                .SetVariable(dogName.GetInfoValue(DogName.Duke))
            ))
            .NewOption(BranchType.Option_03, dogNameOption3, new(branch => branch
                .AddText(Character.Player, "Loki")
                .SetVariable(dogName.GetInfoValue(DogName.Loki))
            ))

            //.AddOption(BranchType.Option_01, new(
            //    "Buddy is ok", dogName.Is(DogName.Buddy),
            //    "Loki is ok", dogName.Is(DogName.Loki),
            //    "My dog is ok"
            //), new(builder => builder.Values(
            //        Character.Player, "aaaa",
            //        Character.Npc, "bbbb")
            //))
            //.AddOption(BranchType.Option_02, new("My Dog's name is buddy",
            //    OptionStatus.Obsolete, knowDogName,
            //    OptionStatus.Hidden, talkingAboutPets.Not(), trivialConversation.Not(),
            //    "His Name is Buddy", talkingAboutPets,
            //    "Buddy", talkingAboutPetsNames), new(branch => branch
            //        .SetVariable(dogName.GetInfoValue(DogName.Loki))
            //        .AddText(Character.Player, "Loki")
            //))
            //.AddOption(BranchType.Option_03, new("My Dog's name is Loki",
            //    OptionStatus.Obsolete, knowDogName,
            //    OptionStatus.Hidden, talkingAboutPets.Not(), trivialConversation.Not(),
            //    "His Name is Loki", talkingAboutPets,
            //    "Loki", talkingAboutPetsNames))
            //.OnBranch(BranchType.Option_03, branch => branch
            //    .SetVariable(dogName.GetInfoValue(DogName.Loki))
            //    .AddText(Character.Player, "Loki")
            // )
            .Merge()
            .AddText(new(
                Character.Player, "ok, 3, 2, 1, go!", Delay.Normal,
                Character.Npc, "hi", Delay.Fast,
                Character.Player, "hi", Delay.Fast,
                Character.Npc, "hi", Delay.Fast,
                Character.Player, "hello", Delay.Normal,
                Character.Npc, "hello", Delay.Normal,
                Character.Player, "hello", Delay.Normal,
                Character.Npc, "helloooo", Delay.Slow,
                Character.Player, "helloooo", Delay.Slow,
                Character.Npc, "helloooo", Delay.Slow,
                Character.Player, "helloooo", Delay.Slow,
                Character.Npc, "thats a regular sized text on fast", Delay.Fast,
                Character.Player, "this one is another regular sized text on normal", Delay.Normal,
                Character.Npc, "this one is also regular, but it's slow", Delay.Slow,
                Character.Player, "now thats a very big text that should be very long, but im typing it very fast because im hyped", Delay.Fast,
                Character.Npc, "this one is a regular text, it should be very big, so it takes a while to type, but eventually it gets done", Delay.Normal,
                Character.Player, "now for this one it takes a while, i need to thing a lot about what im typing and type and retype, so it should take a while", Delay.Slow,
                Character.Npc, "cool", Delay.Fast
             ))



            .AddText(Character.Npc, "bye")
            .FinishDialog();
        return dialog;
    }

    private static PetDialog FullDialog() {
        var dialog = new PetDialog();
        var mm = dialog.Map;
        var su = dialog.SubjectMap.Subject;
        var builder = new DialogBuilder<PetDialog>(dialog);

        var petType = mm.Branching.PetType;

        builder = builder
            .AddText(new(Character.Npc, Delay.Fast, "Hey!"))
            .BranchOut()

            .NewOption(BranchType.Option_01, new("Dog", OptionDelay.Fast, OptionSpeed.Normal))
            .NewOption(BranchType.Option_02, new("Cat", OptionDelay.Slow, OptionSpeed.Fast))
            .NewOption(BranchType.Option_03, new("None", OptionDelay.Slow, OptionSpeed.Slow))

            .OnBranch(BranchType.Option_01, new(branch => branch
                .AddText(Character.Player, "Dog!")
                .SetVariable(petType.GetInfoValue(PetType.Dog))
            ))
            .OnBranch(BranchType.Option_02, new(branch => branch
                .AddText(Character.Player, "Cat!")
                .SetVariable(petType.GetInfoValue(PetType.Cat))
            ))
            .OnBranch(BranchType.Option_03, new(branch => branch
                .AddText(Character.Player, "None")
                .SetVariable(petType.GetInfoValue(PetType.None))
            ))

            .Merge()
            .AddText(new(Character.Npc, 
                "What do you like about cats?", petType.Is(PetType.Cat),
                "What do you like about dogs?", petType.Is(PetType.Dog),
                "You're boring"
            ))
            .FinishDialog();

        return dialog;

    }
}
using DialogGameConsole.Infos;
using DialogGameConsole.Infos.Base;
using LanguageExt;
using LanguageExt.DataTypes.Serialisation;
using System;

namespace DialogGameConsole.Dialogs.PetDialog;

public class PetDialogMentalMap : MentalMap
{
    public class BranchingInfo : InfoGroup<BranchingInfo>
    {
        public readonly Info<YesOrNo> HasPet;
        public readonly Info<PetType> PetType;
    }

    public class GenericInfo : InfoGroup<GenericInfo>
    {
        public readonly Info<DogName> DogName;
    }

    public override BranchingInfo Branching { get; } = new();

    public override GenericInfo Generic { get; } = new();

}


public enum DogName { Unknown, Buddy, Duke, Loki }
public enum YesOrNo { Unknown, Yes, No, }
public enum PetType { Unknown, Cat, Dog, None }








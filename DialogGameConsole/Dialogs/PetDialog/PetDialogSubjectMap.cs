using DialogGameConsole.Infos;
using DialogGameConsole.Infos.Base;
using System;

namespace DialogGameConsole.Dialogs.PetDialog;

public class PetDialogSubjectMap : SubjectMap
{
    public class SubjectInfo : SubjectInfoGroup<PetDialogSubjects, PetDialogSubSubjects> { }

    public override SubjectInfo Subject { get; } = new();

}

public enum PetDialogSubjects
{
    Unkown,
    Pets,
    Dogs,
    Cats,
    Personal
}

public enum PetDialogSubSubjects
{
    Unkown,
    PetName,
}
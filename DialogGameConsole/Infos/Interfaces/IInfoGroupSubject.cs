using DialogGameConsole.Infos.Base;
using DialogGameConsole.Infos.Enums;

namespace DialogGameConsole.Infos.Interfaces;

public interface IInfoGroupSubject : IInfoGroup
{
    public IInfoValue GetSubjectValue();

    public IInfoValue GetSubSubjectValue();

    public SubjectStrength GetStrenghtValue();

    public void SetSubject(IInfoValue infoValue);

    public void SetSubSubject(IInfoValue infoValue);

    public void SetSubjectStrength(SubjectStrength value);
}
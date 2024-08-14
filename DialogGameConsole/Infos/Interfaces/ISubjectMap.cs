using DialogGameConsole.Infos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Infos.Interfaces;
public interface ISubjectMap
{
    public IInfoValue GetSubjectValue();

    public IInfoValue GetSubSubjectValue();

    public SubjectStrength GetStrenghtValue();

    public void SetSubject(IInfoValue infoValue);

    public void SetSubSubject(IInfoValue infoValue);

    public void SetSubjectStrength(SubjectStrength value);
}

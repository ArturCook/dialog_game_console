using DialogGameConsole.Infos.Enums;
using System;

namespace DialogGameConsole.Infos.Base;

public class SubjectInfoGroup<T, K> : InfoGroup<SubjectInfoGroup<T, K>>
    where T : struct where K : struct
{
    public readonly Info<T> Subject;
    public readonly Info<K> SubSubject;
    public readonly Info<SubjectStrength> SubjectStrenght;

    public InfoPredicate Is(T subject)
        => BuildPredicate(subject, null, null);

    public InfoPredicate Is(SubjectStrength strength)
        => BuildPredicate(null, null, strength);

    public InfoPredicate Is(T subject, K subSubject)
        => BuildPredicate(subject, subSubject, null);

    public InfoPredicate Is(T subject, SubjectStrength strength)
        => BuildPredicate(subject, null, strength);

    public InfoPredicate Is(T subject, K subSubject, SubjectStrength strength)
        => BuildPredicate(subject, subSubject, strength);

    public InfoPredicate IsNot(T subject)
        => BuildPredicate(subject, null, null).Not();

    public InfoPredicate IsNot(SubjectStrength strength)
        => BuildPredicate(null, null, strength).Not();

    public InfoPredicate IsNot(T subject, K subSubject)
        => BuildPredicate(subject, subSubject, null).Not();

    public InfoPredicate IsNot(T subject, SubjectStrength strength)
        => BuildPredicate(subject, null, strength).Not();

    public InfoPredicate IsNot(T subject, K subSubject, SubjectStrength strength)
        => BuildPredicate(subject, subSubject, strength).Not();

    private InfoPredicate BuildPredicate(T? subject, K? subSubject, SubjectStrength? strength) {
        return new InfoPredicate(BuildPredicateFunction(subject, subSubject, strength), 
            BuildPredicateText(subject, subSubject, strength));
    }

    private Predicate<InfoMap> BuildPredicateFunction(T? subject, K? subSubject, SubjectStrength? strength) {
        return (map) => (!subject.HasValue || map.GetValue(Subject).Equals(subject.Value))
            && (!subSubject.HasValue || map.GetValue(SubSubject).Equals(subSubject.Value))
            && (!strength.HasValue || map.GetValue(SubjectStrenght).Equals(strength.Value));
    }

    private string BuildPredicateText(T? subject, K? subSubject, SubjectStrength? strength) {
        var text = "";
        text += subject.HasValue ? "Subject=" + subject + "," : "";
        text += subSubject.HasValue ? "SubSubject=" + subSubject + "," : "";
        text += strength.HasValue ? "Strength=" + strength + "," : "";
        return text;
    }

    public void SetSubject(T value) => SetValue(Subject.GetInfoValue(value));
    public void SetSubSubject(K value) => SetValue(SubSubject.GetInfoValue(value));
    public void SetSubjectStrenght(SubjectStrength value) => SetValue(SubjectStrenght.GetInfoValue(value));
}
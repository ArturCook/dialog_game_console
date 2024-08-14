using DialogGameConsole.Util;

namespace DialogGameConsole.DialogBase
{
    public class DialogBase<T> where T : class
    {
        public int Id { get; set; } = Ids.GetId(typeof(T));

        public string Prefix => typeof(T).Name.Replace("Dialog", "") + " " + Ids.FormatId(Id, typeof(T));

        public override string ToString() => Prefix;
    }
}
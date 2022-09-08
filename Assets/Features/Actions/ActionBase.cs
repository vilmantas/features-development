namespace Features.Actions
{
    public class ActionBase
    {
        public readonly string Name;

        public readonly string Alias;
        
        public ActionBase(string name)
        {
            Name = name;
            Alias = name;
        }
        
        public ActionBase(string name, string alias)
        {
            Name = name;
            Alias = alias;
        }
    }
}
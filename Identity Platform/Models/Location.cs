namespace Identity.Platform.Models
{
    public class Location
    {
        public Location(string controller, string action, string name)
        {
            Controller = controller;
            Action = action;
            Name = name;
        }

        public Location(string controller, string action) : this(controller, action, controller)
        {
        }

        public string Controller { get; }

        public string Action { get; }

        public string Name { get; }
    }
}
namespace Identity.Platform.Models.ViewModels
{
    public class NavLocation
    {
        public NavLocation(bool isActive, Location location)
        {
            IsActive = isActive;
            Location = location;
        }

        public bool IsActive { get; }

        public Location Location { get; }
    }
}
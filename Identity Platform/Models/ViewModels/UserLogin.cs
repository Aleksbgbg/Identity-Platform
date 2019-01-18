namespace Identity.Platform.Models.ViewModels
{
    public class UserLogin
    {
        public UserLogin(AppUser user, bool isLoggedIn)
        {
            User = user;
            IsLoggedIn = isLoggedIn;
        }

        public AppUser User { get; }

        public bool IsLoggedIn { get; }
    }
}
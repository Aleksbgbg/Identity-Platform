namespace Identity.Platform.Models.ViewModels
{
    public class UserLogin
    {
        public UserLogin(AppUser user, bool isAuthenticatedUser)
        {
            User = user;
            IsAuthenticatedUser = isAuthenticatedUser;
        }

        public AppUser User { get; }

        public bool IsAuthenticatedUser { get; }
    }
}
namespace Identity.Platform.Models.ViewModels
{
    using System.Collections.Generic;

    public class UserInfo
    {
        public UserInfo(string id, string userName, string email, IEnumerable<string> roles)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Roles = roles;
        }

        public string Id { get; }

        public string UserName { get; }

        public string Email { get; }

        public IEnumerable<string> Roles { get; }
    }
}
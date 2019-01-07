namespace Identity.Platform.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Identity.Platform.Models.ViewModels;

    using Microsoft.AspNetCore.Identity;

    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserInfoRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserInfo[]> RetrieveUserInfosAsync()
        {
            AppUser[] appUsers = _userManager.Users.ToArray();
            UserInfo[] userInfos = new UserInfo[appUsers.Length];

            for (int userIndex = 0; userIndex < appUsers.Length; ++userIndex)
            {
                AppUser appUser = appUsers[userIndex];
                userInfos[userIndex] = new UserInfo(appUser.Id, appUser.UserName, appUser.Email, await _userManager.GetRolesAsync(appUser));
            }

            return userInfos;
        }
    }
}
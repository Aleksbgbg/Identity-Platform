namespace Identity.Platform.Models.Repositories
{
    using System.Threading.Tasks;

    using Identity.Platform.Models.ViewModels;

    public interface IUserInfoRepository
    {
        Task<UserInfo[]> RetrieveUserInfosAsync();
    }
}
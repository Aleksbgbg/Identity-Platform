namespace Identity.Platform.Controllers
{
    using System.Threading.Tasks;

    using Identity.Platform.Models.Repositories;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User, Admin")]
    public class UsersController : Controller
    {
        private readonly IUserInfoRepository _userInfoRepository;

        public UsersController(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public async Task<ViewResult> Index()
        {
            return View(await _userInfoRepository.RetrieveUserInfosAsync());
        }
    }
}
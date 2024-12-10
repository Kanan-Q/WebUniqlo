using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUniqlo.Enums;
using WebUniqlo.Helper;

namespace WebUniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =nameof(Roles.Admin))]
    //[Authorize(Roles =RoleConstants.Product)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

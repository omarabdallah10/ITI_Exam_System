using BLL.Repository;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemPL.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

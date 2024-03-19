using BLL.IRepository;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamSystemPL.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;
       
        public AccountController(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
        }
        public IActionResult Login()
        {
          
            return View();
        }
        #region Login
        [HttpPost]
   
        public async Task<IActionResult> Login(UserLoginModelView userLogin)
        {
            if (userLogin != null)
            {
                if (ModelState.IsValid)
                {
                    var user = accountRepository.GetUserAuth(userLogin);
                    if (user != null)
                    {

                        var claimPrincipal= accountRepository.AddUserAuthentication(user);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);

                        if (user.Role == "admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (user.Role == "student")
                        {
                            return RedirectToAction("Index", "Student");
                        }
                        else if (user.Role == "instructor")
                        {
                            return RedirectToAction("Index", "Instructor");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Invalid UserName or Password");
                    }
                }

            }

            return View(userLogin);
        } 
        #endregion

       
        [HttpGet]
        #region Register
        public IActionResult Register()
        {
            ViewBag.Depts = accountRepository.GetDepartments();
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterModelView userRegister)
        {
            if (ModelState.IsValid)
            {
                if (userRegister!=null)
                {
                    accountRepository.AddUser(userRegister);

                    //Display Success Message
                    TempData["Success"] = "User Registered Successfully";
                    UserLoginModelView userLogin = new UserLoginModelView
                    {
                        UserName = userRegister.UserName,
                        Password = userRegister.Password
                    };
                    return RedirectToAction("Login", userLogin);
                }
                else
                {
                    return View(userRegister);
                }
            }
            return View(userRegister);
        }
        #endregion
        public async Task<IActionResult>Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

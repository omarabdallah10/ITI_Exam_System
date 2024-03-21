using BLL.IRepository;
using BLL.ViewModels;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Repository
{
    
    public class AccountRepository : IAccountRepository
    {
        private readonly ITIContext context;
        public AccountRepository(ITIContext _context)
        {
            context = _context;
        }

     
        public ClaimsPrincipal AddUserAuthentication(User userLogin)
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();    
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            Claim userName = new Claim(ClaimTypes.Name, userLogin.Username);

            //Claim surName = new Claim(ClaimTypes.Surname, userLogin.Fname+" "+userLogin.Lname);
            Claim role = new Claim(ClaimTypes.Role, userLogin.Role);
            claimsIdentity.AddClaim(userName);
            //claimsIdentity.AddClaim(surName);
            claimsIdentity.AddClaim(role);
            claimsPrincipal.AddIdentity(claimsIdentity);
            return claimsPrincipal;
        }

        public User GetUserAuth(UserLoginModelView userLogin)
        {
            return context.Users.FirstOrDefault(u => u.Username == userLogin.UserName && u.Password == userLogin.Password);
        }
        public void AddUser(UserRegisterModelView userRegister)
        {
            context.Users.Add(new User
            {
                Username = userRegister.UserName,
                Fname = userRegister.Fname,
                Lname = userRegister.Lname,
                Password = userRegister.Password,
                Role = "student",
                Student = new Student
                {
                   DeptId = userRegister.DeptId
                }
            });

            context.SaveChanges();

        }

        public User? GetUserByName(string userName)
        {
            User u = context.Users.Include(u => u.Instructor).FirstOrDefault(u => u.Username == userName);

            if (u == null)
                return null;
            return u;
        }

        public List<Department> GetDepartments()
        {
            return context.Departments.ToList();
        }


        //public async Task SignOutUser()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //}
    }
}

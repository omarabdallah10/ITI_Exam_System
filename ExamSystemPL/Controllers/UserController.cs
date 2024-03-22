using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;
using BLL.IRepository;
using BLL.Repository;

namespace ExamSystemPL.Controllers
{
    public class UserController : Controller
    {
        IUserRepository userRepository;

        public UserController(IUserRepository _userRepository)
        {
			userRepository = _userRepository;
		}


        public IActionResult Index()
        {
			var model = userRepository.GetAllUsers();
			return View(model);

            /*var admins = userRepository.GetUserByRole("Admin");
            var students = userRepository.GetUserByRole("Student");
            var instructors = userRepository.GetUserByRole("Instructor");

            ViewBag.Admins = admins;
            ViewBag.Students = students;
            ViewBag.Instructors = instructors;

            return View();*/
        }

        public IActionResult Create()
        {
            /*send the instructor table and departments table and student table as view bags*/
            var depts = userRepository.GetAllDeprtments();
            ViewBag.Depts = depts;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var userModel = userRepository.GetUserByUsername(user.Username);
                if (userModel != null)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }
                userRepository.AddUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = userRepository.GetUserById(id.Value);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var userModel = userRepository.GetUserByUsername(user.Username);
                if (userModel != null)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }
                userRepository.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = userRepository.GetUserById(id.Value);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }


        /*public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = userRepository.GetUserById(id.Value);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        public IActionResult confirmDelete(int? id)
        {
            var userModel = userRepository.GetUserById(id.Value);
            userRepository.DeleteUser(userModel);
            return RedirectToAction("Index");
        }*/
       


    }
}

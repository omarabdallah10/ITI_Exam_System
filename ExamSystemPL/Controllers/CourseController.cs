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
using Microsoft.AspNetCore.Authorization;

namespace ExamSystemPL.Controllers
{
    /*[Authorize(Roles = "Admin")]*/
    public class CourseController : Controller
    {
        ICourseRepository courseRepository;

        public CourseController(ICourseRepository _courseRepository)
        {
            courseRepository = _courseRepository;
        }

        // GET: Course
        public IActionResult Index()
        {
            var model = courseRepository.GetAllCourses();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course crs)
        {
            if (ModelState.IsValid)
            {
                var crsModel = courseRepository.GetCourseByName(crs.CrsName);
                if (crsModel != null)
                {
                    ModelState.AddModelError("CrsName", "Course Name already exists");
                    return View(crs);
                }
                courseRepository.AddCourse(crs);
                return RedirectToAction("Index");
            }
            return View(crs);
        }

        public IActionResult Edit(int? id)
        {
			if (id == null)
            {
				return NotFound();
			}

			var crsModel = courseRepository.GetCourseById(id.Value);
			if (crsModel == null)
            {
				return NotFound();
			}
			return View(crsModel);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course crs)
        {
			if(ModelState.IsValid)
            {
/*				var oldCrs = courseRepository.GetCourseById(crs.CrsId);*/
				var crsModel = courseRepository.GetCourseByName(crs.CrsName);
                if (crsModel != null)
                {
					ModelState.AddModelError("CrsName", "Course Name already exists");
					return View(crs);
				}
/*
                if (oldCrs.CrsName == crs.CrsName)
                {
                    return RedirectToAction("Index");
                }
*/
                courseRepository.UpdateCourse(crs);
                return RedirectToAction("Index");
			}
            return View(crs);
		}


        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crsModel = courseRepository.GetCourseById(id.Value);
            if (crsModel == null)
            {
                return NotFound();
            }

            return View(crsModel);
        }

        public IActionResult Delete(int? id)
        {
			if (id == null)
            {
				return NotFound();
			}

			var crsModel = courseRepository.GetCourseById(id.Value);
			if (crsModel == null)
            {
				return NotFound();
			}

			return View(crsModel);
		}

        public IActionResult confirmDelete(int? id)
        {
            var crsModel = courseRepository.GetCourseById(id.Value);
            courseRepository.DeleteCourse(crsModel);
            return RedirectToAction("Index");
        }


    }
}

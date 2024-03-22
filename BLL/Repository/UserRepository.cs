using BLL.IRepository;
using DAL.Data;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
	public class UserRepository : IUserRepository
	{
		ITIContext db;
		public UserRepository(ITIContext _db)
		{
			db = _db;
		}

		public List<Department> GetAllDeprtments()
		{
            return db.Departments.ToList();
        }

		public List<User> GetAllUsers()
		{

			return db.Users.OrderBy(u => u.Role).ToList();
		}

		public User GetUserByRole(string role)
		{
            return db.Users.FirstOrDefault(u => u.Role == role);
        }

        public User GetUserByUsername(string username)
        {
            return db.Users.FirstOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
		{

			/*if(user.Role == "Admin")
			{
                user.Instructor = null;
                user.Student = null;
            }
            else if (user.Role == "Instructor")
			{
                user.Student = null;
            }
            else if (user.Role == "Student")
			{
                user.Instructor = null;
            }
			db.Users.Add(user);
			db.SaveChanges();*/

			//add user to the database and save changes
			//1-if he is an admin then add him to the user table
			//2-if he is an instructor then add him to the instructor table besides the user table
			//3-if he is a student then add him to the student table besides the user table
			if (user.Role == "Admin")
			{
                db.Users.Add(user);
                db.SaveChanges();
            }
            else if (user.Role == "Instructor")
			{
				user.Student = null;
                db.Users.Add(user);
				/*db.SaveChanges();*/
				Instructor instructor = new Instructor();
                instructor.InsId = user.UId;
                db.Instructors.Add(instructor);
                db.SaveChanges();
            }
            else if (user.Role == "Student")
			{
				user.Instructor = null;
				db.Users.Add(user);
                db.SaveChanges();
            }
		}

		public void DeleteUser(User user)
		{
			db.Users.Remove(user);
			db.SaveChanges();
		}


		public User GetUserById(int id)
		{
			return db.Users.FirstOrDefault(u => u.UId == id);
		}



		public void UpdateUser(User user)
		{
			db.Users.Update(user);
			db.SaveChanges();
		}
    }
}

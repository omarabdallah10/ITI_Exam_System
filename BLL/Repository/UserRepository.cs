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
		public List<User> GetAllUsers()
		{

			return db.Users.OrderBy(u => u.Role).ToList();
		}
		public void AddUser(User user)
		{
			throw new NotImplementedException();
		}

		public void DeleteUser(int id)
		{
			throw new NotImplementedException();
		}


		public User GetUserById(int id)
		{
			throw new NotImplementedException();
		}

		public User GetUserByUsername(string username)
		{
			throw new NotImplementedException();
		}

		public void UpdateUser(User user)
		{
			throw new NotImplementedException();
		}
	}
}

using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{

	public interface IUserRepository
	{
		public List<User> GetAllUsers();
		public User GetUserById(int id);
		public User GetUserByUsername(string username);
		public User GetUserByRole(string role);

        public List<Department> GetAllDeprtments();
		public void AddUser(User user);
		public void UpdateUser(User user);
		public void DeleteUser(User user);
	}
}

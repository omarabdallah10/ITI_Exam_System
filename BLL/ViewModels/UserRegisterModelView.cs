using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class UserRegisterModelView
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        [Required]
        public string? Fname { get; set; }

        [StringLength(50)]
        [Required]
        public string? Lname { get; set; }


        [DataType(DataType.Password)]
        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        public int DeptId { get; set; }
    }
}

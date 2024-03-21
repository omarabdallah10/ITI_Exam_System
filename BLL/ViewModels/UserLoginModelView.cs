using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class UserLoginModelView
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}

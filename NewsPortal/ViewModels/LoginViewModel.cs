using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsPortal.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public LoginViewModel() { }

        public LoginViewModel(Login login)
        {
            Id = login.Id;
            UserName = login.UserName;
            Password = login.Password;
        }
    }
}
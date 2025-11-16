using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.User
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; } // Student / Instructor
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.User
{
    public class ProfileDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }

    }
}

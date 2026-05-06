using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Profile
{
    public class ProfileDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DOB { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public DateTime? JoiningDate { get; set; }

        public string? ProfileImagePath { get; set; }
    }
}

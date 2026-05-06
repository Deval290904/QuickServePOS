using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Profile
{
    public class UpdateProfileDto
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DOB { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }
    }
}

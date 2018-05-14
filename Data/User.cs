using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data
{

    public class User : BaseModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        public virtual List<Note> Notes { get; set; }
    }
}

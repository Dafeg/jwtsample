using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}

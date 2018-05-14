using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Journal : BaseModel
    {
        public string Action { get; set; }

        public bool Success { get; set; }

        public string UserName { get; set; }
    }
}

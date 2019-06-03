using System;
using System.Collections.Generic;

namespace EntityFrameworkConsole.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual Department Department { get; set; }
    }
}

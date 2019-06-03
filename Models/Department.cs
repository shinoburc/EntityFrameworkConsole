using System;
using System.Collections.Generic;

namespace EntityFrameworkConsole.Models
{
    public class Department
    {
        public long Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public virtual Department Parent { get; set; }
        public virtual ICollection<Department> Children { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

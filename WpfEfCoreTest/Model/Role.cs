using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class Role
    {
        public Role()
        {
            UserSies = new HashSet<UserSy>();
        }

        public int Id { get; set; }
        public string Role1 { get; set; }

        public virtual ICollection<UserSy> UserSies { get; set; }
    }
}

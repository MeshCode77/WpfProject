using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class SprDgm
    {
        public int Id { get; set; }
        public int? IdNameOborudKomplekt { get; set; }
        public string NameOborud { get; set; }
        public string Gold { get; set; }
        public string Silver { get; set; }
        public string Mpg { get; set; }

        public virtual Komplect IdNameOborudKomplektNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class Dgm
    {
        public int Id { get; set; }
        public int Idformular { get; set; }
        public string NumForm { get; set; }
        public string Gold { get; set; }
        public string Silver { get; set; }
        public string Mpg { get; set; }

        public virtual Formular IdformularNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class Spisanie
    {
        public int Id { get; set; }
        public int Idformular { get; set; }
        public string NumForm { get; set; }
        public string InvNum { get; set; }
        public string ZavodNum { get; set; }
        public string NumAktTechSost { get; set; }
        public DateTime DateSpisan { get; set; }
        public string Text { get; set; }

        public virtual Formular IdformularNavigation { get; set; }
    }
}

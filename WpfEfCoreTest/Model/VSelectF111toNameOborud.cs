using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class VSelectF111toNameOborud
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdnameOborud { get; set; }
        public string NameOborud { get; set; }
        public string KartNum { get; set; }
        public string NumForm { get; set; }
        public string InvNum { get; set; }
        public DateTime GtDate { get; set; }
        public DateTime? OutDate { get; set; }
        public bool? Remont { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class OtchetOborud
    {
        public int Id { get; set; }
        public string NameOborud { get; set; }
        public string NumForm { get; set; }
        public string InvNum { get; set; }
        public string ZavodNum { get; set; }
    }
}

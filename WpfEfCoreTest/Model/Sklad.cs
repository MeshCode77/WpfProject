using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class Sklad
    {
        public int Id { get; set; }
        public int Idformular { get; set; }
        public string NumForm { get; set; }
        public string InvNum { get; set; }
        public DateTime? DateSpisania { get; set; }
        public DateTime? DateToSklad { get; set; }
        public string Text { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace WpfEfCoreTest.Model
{
    public partial class OtchetFormular
    {
        public int Id { get; set; }
        public int Idf111 { get; set; }
        public string IdKomplect { get; set; }
        public string NumForm { get; set; }
        public string InvNum { get; set; }
        public string Model { get; set; }
        public string Count { get; set; }
        public string Serial { get; set; }
        public DateTime? DataTo { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }
    }
}

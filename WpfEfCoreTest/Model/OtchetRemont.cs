#nullable disable

using System;

namespace WpfEfCoreTest.Model
{
    public class OtchetRemont
    {
        public int Id { get; set; }
        public int Idf111 { get; set; }
        public string Podr { get; set; }
        public string User { get; set; }
        public string InvNum { get; set; }
        public string ZavodNum { get; set; }
        public string NumForm { get; set; }
        public string NameOborud { get; set; }

        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Title { get; set; }
        public string TitleComplected { get; set; }

        public virtual F111 Idf111Navigation { get; set; }
    }
}
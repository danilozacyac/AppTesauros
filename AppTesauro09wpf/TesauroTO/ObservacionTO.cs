using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TesauroTO
{
    public class ObservacionTO
    {
        public Int64 Id { get; set; }
        public int Tipo { get; set; }
        public String Texto { get; set; }
        public int UserId { get; set; }
        public String Usuario { get; set; }
        public DateTime Hora { get; set; }
        public int IdTema { get; set; }
    }
}

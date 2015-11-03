using System;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO
{
    public class VotoSimplificadoTO
    {
        private static VotosTO votoActual { get; set; }
        public String Clasificacion { get; set; }
        public String Complemento { get; set; }
        public String Consec { get; set; }
        public String ConsecIndx { get; set; }
        public String Epoca { get; set; }
        public String Fuente { get; set; }
        public String Id { get; set; }
        public String Pagina { get; set; }
        public String ParteT { get; set; }
        public String Procesado { get; set; }
        public String Promovente { get; set; }
        public String Rubro { get; set; }
        public String Sala { get; set; }
        public String Tesis { get; set; }
        public String TpoAsunto { get; set; }
        public String VolOrden { get; set; }
        public String Volumen { get; set; }
        public String Loc { get { return this.getLoc(); } }
        public int OrdenAsunto { get; set; }
        public int OrdenEmisor { get; set; }
        public VotoSimplificadoTO()
        {
        }

        /// <summary>
        /// Iniciar los campos de acuerdo con un voto determinado.
        /// </summary>
        /// <param name="original">El voto obtenido originalmente</param>
        public VotoSimplificadoTO(VotosTO original)
        {
            Clasificacion = original.Clasificacion;
            Complemento = original.Complemento;
            Consec = original.Consec;
            ConsecIndx = original.ConsecIndx;
            Epoca = original.Epoca;
            Fuente = original.Fuente;
            Id = original.Id;
            Pagina = original.Pagina;
            ParteT = original.ParteT;
            Procesado = original.Procesado;
            Promovente = original.Promovente;
            Rubro = original.Rubro;
            Sala = original.Sala;
            Tesis = original.Tesis;
            TpoAsunto = original.TpoAsunto;
            VolOrden = original.VolOrden;
            Volumen = original.Volumen;
            OrdenAsunto = original.OrdenAsunto;
            OrdenEmisor = original.OrdenEmisor;
        }

        public String getLoc()
        {
            return this.Epoca + "; " + this.Sala + "; " + this.Fuente
               + "; " + this.Volumen + "; Pág." + this.Pagina;
        }
    }
}
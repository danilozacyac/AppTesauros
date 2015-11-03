using System;
using System.Linq;

namespace AppTesauro09wpf.Listado
{
    public class ListadoCertificacion
    {
        private int ius;
        private int idTema;
        private String rubro;
        private String tema;

        public int Ius
        {
            get
            {
                return this.ius;
            }
            set
            {
                this.ius = value;
            }
        }

        public int IdTema
        {
            get
            {
                return this.idTema;
            }
            set
            {
                this.idTema = value;
            }
        }

        public string Rubro
        {
            get
            {
                return this.rubro;
            }
            set
            {
                this.rubro = value;
            }
        }

        public string Tema
        {
            get
            {
                return this.tema;
            }
            set
            {
                this.tema = value;
            }
        }
    }
}

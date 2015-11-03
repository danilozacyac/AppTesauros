using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppTesauro09wpf.Verificar
{
    public class DatosExcel
    {
        private string ius;
        private string rubro;
        private string tesis;
        private string localizacion;
        private string materia1;
        private string materia2;
        private string materia3;
        private string descripcion;
        public string Ius
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

        public string Tesis
        {
            get
            {
                return this.tesis;
            }
            set
            {
                this.tesis = value;
            }
        }

        public string Localizacion
        {
            get
            {
                return this.localizacion;
            }
            set
            {
                this.localizacion = value;
            }
        }

        public string Materia1
        {
            get
            {
                return this.materia1;
            }
            set
            {
                this.materia1 = value;
            }
        }

        public string Materia2
        {
            get
            {
                return this.materia2;
            }
            set
            {
                this.materia2 = value;
            }
        }

        public string Materia3
        {
            get
            {
                return this.materia3;
            }
            set
            {
                this.materia3 = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
            }
        }
    }
}

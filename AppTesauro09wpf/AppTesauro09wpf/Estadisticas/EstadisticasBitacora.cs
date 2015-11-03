using System;
using System.Linq;

namespace AppTesauro09wpf.Estadisticas
{
    public class EstadisticasBitacora
    {
        private int agregados;

        

        public int Agregados
        {
            get { return agregados; }
            set { agregados = value; }
        }

        private int modificados;

        public int Modificados
        {
            get { return modificados; }
            set { modificados = value; }
        }
        private int eliminados;

        public int Eliminados
        {
            get { return eliminados; }
            set { eliminados = value; }
        }
        private int importados;

        public int Importados
        {
            get { return importados; }
            set { importados = value; }
        }

        private int promedio;
        public int Promedio
        {
            get
            {
                return this.promedio;
            }
            set
            {
                this.promedio = value;
            }
        }

        private String elemento;
        public String Elemento
        {
            get
            {
                return this.elemento;
            }
            set
            {
                this.elemento = value;
            }
        }

    }


}

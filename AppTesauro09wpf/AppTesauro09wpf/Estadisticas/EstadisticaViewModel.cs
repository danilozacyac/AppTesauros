using System;
using System.Collections.Generic;
using System.Linq;

namespace AppTesauro09wpf.Estadisticas
{
    public class EstadisticaViewModel
    {
        private int idUsuario;
        private String usuario;
        
        private List<EstadisticasBitacora> estadisticas;
        private List<EstadisticasBitacora> totales;

        public int IdUsuario
        {
            get
            {
                return this.idUsuario;
            }
            set
            {
                this.idUsuario = value;
            }
        }

        public String Usuario
        {
            get
            {
                return this.usuario;
            }
            set
            {
                this.usuario = value;
            }
        }

        
        public List<EstadisticasBitacora> Estadisticas
        {
            get
            {
                return this.estadisticas;
            }
            set
            {
                this.estadisticas = value;
            }
        }

        public List<EstadisticasBitacora> Totales
        {
            get
            {
                return this.totales;
            }
            set
            {
                this.totales = value;
            }
        }
    }
}

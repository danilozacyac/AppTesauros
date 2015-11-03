
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{

    [DataContract]
    public class DirectorioCatalogosTO
    {

        ///El Id del Elemento 
        [DataMember]
        public int IdElemento { get { return this.getIdElemento(); } set { this.setIdElemento(value); } }

        private int idElemento;

        public int getIdElemento() { return idElemento; }
        public void setIdElemento(int idElemento) { this.idElemento = idElemento; }

        ///El Id del Elemento
        [DataMember]
        public int Orden { get { return this.getOrden(); } set { this.setOrden(value); } }

        private int orden;

        public int getOrden() { return orden; }
        public void setOrden(int orden) { this.orden = orden; }

        ///El nombre del Elemento
        [DataMember]
        public String NombreElemento { get { return this.getNombreElemento(); } set { this.setNombreElemento(value); } }

        private String Nombre;

        public String getNombreElemento() { return Nombre; }
        public void setNombreElemento(String Nombre) { this.Nombre = Nombre; }

        ///El nombre,  limpio del Elemento.
        [DataMember]
        public String NombreStrElemento { get { return this.getNombreStrElemento(); } set { this.setNombreStrElemento(value); } }

        private String NombreStr;

        public String getNombreStrElemento() { return NombreStr; }
        public void setNombreStrElemento(String NombreStr) { this.NombreStr = NombreStr; }

    }

}

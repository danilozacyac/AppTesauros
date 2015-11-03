
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{

    [DataContract]
    public class DirectorioAreasTO
    {

        [DataMember]
        public int IdArea { get { return this.getIdArea(); } set { this.setIdArea(value); } }

        private int idArea;

        public int getIdArea() { return idArea; }
        public void setIdArea(int idArea) { this.idArea = idArea; }

        /// El Tipo del Área.
        [DataMember]
        public int IdTipoOrgJ { get { return this.getIdTipoOrgJ(); } set { this.setIdTipoOrgJ(value); } }

        private int idTipoOrgJ;

        public int getIdTipoOrgJ() { return idTipoOrgJ; }
        public void setIdTipoOrgJ(int idTipoOrgJ) { this.idTipoOrgJ = idTipoOrgJ; }

        ///El nombre del Área
        [DataMember]
        public String NombreArea { get { return this.getNombreArea(); } set { this.setNombreArea(value); } }

        private String Nombre;

        public String getNombreArea() { return Nombre; }
        public void setNombreArea(String Nombre) { this.Nombre = Nombre; }

        ///El nombre,  limpio de la Area.
        [DataMember]
        public String NombreStrArea { get { return this.getNombreStrArea(); } set { this.setNombreStrArea(value); } }

        private String NombreStr;

        public String getNombreStrArea() { return NombreStr; }
        public void setNombreStrArea(String NombreStr) { this.NombreStr = NombreStr; }

        ///el Domicilio
        [DataMember]
        public String DomArea { get { return this.getDomArea(); } set { this.setDomArea(value); } }

        private String Domicilio;

        public String getDomArea() { return Domicilio; }
        public void setDomArea(String Domicilio) { this.Domicilio = Domicilio; }

        ///el Teléfono
        [DataMember]
        public String TelArea { get { return this.getTelArea(); } set { this.setTelArea(value); } }

        private String Telefono;

        public String getTelArea() { return Telefono; }
        public void setTelArea(String Telefono) { this.Telefono = Telefono; }

        ///La Extensión
        [DataMember]
        public String ExtArea { get { return this.getExtArea(); } set { this.setExtArea(value); } }

        private String Ext;

        public String getExtArea() { return Ext; }
        public void setExtArea(String Ext) { this.Ext = Ext; }

        ///Titulares
        [DataMember]
        public List<String> Titulares { get { return this.getTitulares(); } set { this.setTitulares(value); } }

        private List<String> lstTitulares;

        public List<String> getTitulares() { return lstTitulares; }
        public void setTitulares(List<String> lstTitulares) { this.lstTitulares = lstTitulares; }

    }

}

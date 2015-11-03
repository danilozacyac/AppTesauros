
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{

    [DataContract]
    public class DirectorioOrgJurTO
    {

        /// El identificador del Órgano Jurisdiccional.
        [DataMember]
        public int IdOrganoJur { get { return this.getIdOrganoJur(); } set { this.setIdOrganoJur(value); } }

        private int idOrganoJur;

        public int getIdOrganoJur() { return idOrganoJur; }
        public void setIdOrganoJur(int idOrganoJur) { this.idOrganoJur = idOrganoJur; }

        /// El Tipo del Órgano Jurisdiccional.
        [DataMember]
        public int IdTipoOrgJ { get { return this.getIdTipoOrgJ(); } set { this.setIdTipoOrgJ(value); } }

        private int idTipoOrgJ;

        public int getIdTipoOrgJ() { return idTipoOrgJ; }
        public void setIdTipoOrgJ(int idTipoOrgJ) { this.idTipoOrgJ = idTipoOrgJ; }

        /// Materia.
        [DataMember]
        public int IdMat { get { return this.getIdMat(); } set { this.setIdMat(value); } }

        private int idMat;

        public int getIdMat() { return idMat; }
        public void setIdMat(int idMat) { this.idMat = idMat; }

        /// Circuito.
        [DataMember]
        public int IdCto { get { return this.getIdCto(); } set { this.setIdCto(value); } }

        private int idCto;

        public int getIdCto() { return idCto; }
        public void setIdCto(int idCto) { this.idCto = idCto; }

        /// Ordinal.
        [DataMember]
        public int IdOrd { get { return this.getIdOrd(); } set { this.setIdOrd(value); } }

        private int idOrd;

        public int getIdOrd() { return idOrd; }
        public void setIdOrd(int idOrd) { this.idOrd = idOrd; }

        ///El nombre del Órgano Jurisdiccional
        [DataMember]
        public String NombreOrganoJur { get { return this.getNombreOrganoJur(); } set { this.setNombreOrganoJur(value); } }

        private String Nombre;

        public String getNombreOrganoJur() { return Nombre; }
        public void setNombreOrganoJur(String Nombre) { this.Nombre = Nombre; }

        ///El nombre,  limpio de la Órgano Jurisdiccional.
        [DataMember]
        public String NombreStrOrganoJur { get { return this.getNombreStrOrganoJur(); } set { this.setNombreStrOrganoJur(value); } }

        private String NombreStr;

        public String getNombreStrOrganoJur() { return NombreStr; }
        public void setNombreStrOrganoJur(String NombreStr) { this.NombreStr = NombreStr; }

        ///el Domicilio
        [DataMember]
        public String DomOrganoJur { get { return this.getDomOrganoJur(); } set { this.setDomOrganoJur(value); } }

        private String Domicilio;

        public String getDomOrganoJur() { return Domicilio; }
        public void setDomOrganoJur(String Domicilio) { this.Domicilio = Domicilio; }

        ///el Teléfono
        [DataMember]
        public String TelOrganoJur { get { return this.getTelOrganoJur(); } set { this.setTelOrganoJur(value); } }

        private String Telefono;

        public String getTelOrganoJur() { return Telefono; }
        public void setTelOrganoJur(String Telefono) { this.Telefono = Telefono; }

        ///La Extensión
        [DataMember]
        public String ExtOrganoJur { get { return this.getExtOrganoJur(); } set { this.setExtOrganoJur(value); } }

        private String Ext;

        public String getExtOrganoJur() { return Ext; }
        public void setExtOrganoJur(String Ext) { this.Ext = Ext; }

        ///Titulares
        [DataMember]
        public List<String> Titulares { get { return this.getTitulares(); } set { this.setTitulares(value); } }

        private List<String> lstTitulares;

        public List<String> getTitulares() { return lstTitulares; }
        public void setTitulares(List<String> lstTitulares) { this.lstTitulares = lstTitulares; }

        ///Titular, Para cuando es un sólo titular, como en areas admin. 
        [DataMember]
        public String TitularSolo { get { return this.getTitularSolo(); } set { this.setTitularSolo(value); } }

        private String titularSolo;

        public String getTitularSolo() { return titularSolo; }
        public void setTitularSolo(String titularSolo) { this.titularSolo = titularSolo; }

    }
}

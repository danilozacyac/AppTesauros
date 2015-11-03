
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{

    [DataContract]
    public class DirectorioPersonasTO
    {

        /// El identificador de la Persona.
        [DataMember]
        public int IdPersona { get { return this.getIdPersona(); } set { this.setIdPersona(value); } }

        private int idPersona;

        [DataMember]
        public int IdAdsc { get { return this.getIdAdsc(); } set { this.setIdAdsc(value); } }

        private int idAdsc;

        ///El nombre de la persona.
        [DataMember]
        public String NombrePersona { get { return this.getNombrePersona(); } set { this.setNombrePersona(value); } }

        private String Nombre;

        ///el Domicilio
        [DataMember]
        public String DomPersona { get { return this.getDomPersona(); } set { this.setDomPersona(value); } }

        private String Domicilio;

        ///el Teléfono
        [DataMember]
        public String TelPersona { get { return this.getTelPersona(); } set { this.setTelPersona(value); } }

        private String Telefono;

        ///La Extensión
        [DataMember]
        public String ExtPersona { get { return this.getExtPersona(); } set { this.setExtPersona(value); } }

        private String Ext;

        ///el Cargo
        [DataMember]
        public String CargoPersona { get { return this.getCargoPersona(); } set { this.setCargoPersona(value); } }

        private String Cargo;

        ///el Título
        [DataMember]
        public String TituloPersona { get { return this.getTituloPersona(); } set { this.setTituloPersona(value); } }

        private String Titulo;

        ///la Adscripcion, aplica para func admin
        [DataMember]
        public String AdscripcionPersona { get { return this.getAdscripcionPersona(); } set { this.setAdscripcionPersona(value); } }

        private String Adscripcion;

        //Para Jueces y Mag.

        [DataMember]
        public String IdPuesto { get { return this.getIdPuesto(); } set { this.setIdPuesto(value); } }

        private String idPuesto;

        ///El Apellido de la persona.
        [DataMember]
        public String ApellidosPersona { get { return this.getApellidosPersona(); } set { this.setApellidosPersona(value); } }

        private String apellido;

        ///El nombre,  limpio de la persona.
        [DataMember]
        public String NombreStrPersona { get { return this.getNombreStrPersona(); } set { this.setNombreStrPersona(value); } }

        private String NombreStr;

        ///Los apellidos,  limpios de la persona.
        [DataMember]
        public String ApellidosStrPersona { get { return this.getApellidosStrPersona(); } set { this.setApellidosStrPersona(value); } }

        private String ApellidosStr;

        ///Prefijo
        [DataMember]
        public String Prefijo { get { return this.getPrefijo(); } set { this.setPrefijo(value); } }

        private String prefijo;

        ///Posfijo
        [DataMember]
        public String Posfijo { get { return this.getPosfijo(); } set { this.setPosfijo(value); } }

        private String posfijo;

        public int getIdPersona() { return idPersona; }
        public void setIdPersona(int idPersona) { this.idPersona = idPersona; }

        public int getIdAdsc() { return idAdsc; }
        public void setIdAdsc(int idAdsc) { this.idAdsc = idAdsc; }

        public String getNombrePersona() { return Nombre; }
        public void setNombrePersona(String Nombre) { this.Nombre = Nombre; }

        public String getDomPersona() { return Domicilio; }
        public void setDomPersona(String Domicilio) { this.Domicilio = Domicilio; }

        public String getTelPersona() { return Telefono; }
        public void setTelPersona(String Telefono) { this.Telefono = Telefono; }

        public String getExtPersona() { return Ext; }
        public void setExtPersona(String Ext) { this.Ext = Ext; }

        public String getCargoPersona() { return Cargo; }
        public void setCargoPersona(String Cargo) { this.Cargo = Cargo; }

        public String getTituloPersona() { return Titulo; }
        public void setTituloPersona(String Titulo) { this.Titulo = Titulo; }

        public String getAdscripcionPersona() { return Adscripcion; }
        public void setAdscripcionPersona(String Adscripcion) { this.Adscripcion = Adscripcion; }

        public String getIdPuesto() { return idPuesto; }
        public void setIdPuesto(String idPuesto) { this.idPuesto = idPuesto; }

        public String getApellidosPersona() { return apellido; }
        public void setApellidosPersona(String apellido) { this.apellido = apellido; }

        public String getNombreStrPersona() { return NombreStr; }
        public void setNombreStrPersona(String NombreStr) { this.NombreStr = NombreStr; }

        public String getApellidosStrPersona() { return ApellidosStr; }
        public void setApellidosStrPersona(String ApellidosStr) { this.ApellidosStr = ApellidosStr; }

        public String getPrefijo() { return prefijo; }
        public void setPrefijo(String prefijo) { this.prefijo = prefijo; }

        public String getPosfijo() { return posfijo; }
        public void setPosfijo(String posfijo) { this.posfijo = posfijo; }

    }
}

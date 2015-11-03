
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{

    [DataContract]
    public class DirectorioMinistrosTO
    {

        #region Propiedades de la clase

        //private int nId;
        //private String strNombreCompleto;
        //private String strNombre;
        //private String strApp;
        //private String strPrefijo;
        //private String strPosfijo;
        //private String strDomicilio;
        //private String strTituloSemplanza;
        //private String strArchivo;
        //private String[] strTelefonos;
        //private int IdOrganoPertenece;
        #endregion

        /// El identificador de la Persona.
        [DataMember]
        public int IdPersona { get { return this.getIdPersona(); } set { this.setIdPersona(value); } }

        private int idPersona;

        //IdTitulo
        [DataMember]
        public int IdTitulo { get { return this.getIdTitulo(); } set { this.setIdTitulo(value); } }

        private int idTitulo;

        //IdPuesto
        [DataMember]
        public int IdPuesto { get { return this.getIdPuesto(); } set { this.setIdPuesto(value); } }

        private int idPuesto;

        //IdPonencia
        [DataMember]
        public int IdPonencia { get { return this.getIdPonencia(); } set { this.setIdPonencia(value); } }

        private int idPonencia;

        ///El nombre de la persona.
        [DataMember]
        public String NombrePersona { get { return this.getNombrePersona(); } set { this.setNombrePersona(value); } }

        private String Nombre;

        ///Los Apellidos de la persona.
        [DataMember]
        public String ApellidosPersona { get { return this.getApellidosPersona(); } set { this.setApellidosPersona(value); } }

        private String apellidos;

        ///Nombre completo
        [DataMember]
        public String NombreCompleto { get { return this.getNombreCompleto(); } set { this.setNombreCompleto(value); } }

        private String nombreCompleto;

        ///Prefijo
        [DataMember]
        public String Prefijo { get { return this.getPrefijo(); } set { this.setPrefijo(value); } }

        private String prefijo;

        ///Posfijo
        [DataMember]
        public String Posfijo { get { return this.getPosfijo(); } set { this.setPosfijo(value); } }

        private String posfijo;

        //Orden
        [DataMember]
        public int Orden { get { return this.getOrden(); } set { this.setOrden(value); } }

        private int orden;

        //Orden Sala
        [DataMember]
        public int OrdenSala { get { return this.getOrdenSala(); } set { this.setOrdenSala(value); } }

        private int ordenSala;

        //Sala
        [DataMember]
        public int Sala { get { return this.getSala(); } set { this.setSala(value); } }

        private int sala;

        ///Domicilio
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

        ///Titulo de la Semblanza
        [DataMember]
        public String TitSemblanza { get { return this.getTitSemblanza(); } set { this.setTitSemblanza(value); } }

        private String titSemblanza;

        ///Archivo Semblanza
        [DataMember]
        public String ArchivoSemblanza { get { return this.getArchivoSemblanza(); } set { this.setArchivoSemblanza(value); } }

        private String archivoSemblanza;

        public String getArchivoSemblanza() { return archivoSemblanza; }
        public void setArchivoSemblanza(String archivoSemblanza) { this.archivoSemblanza = archivoSemblanza; }

        ///el Título
        [DataMember]
        public String TituloPersona { get { return this.getTituloPersona(); } set { this.setTituloPersona(value); } }

        private String Titulo;

        ///el Cargo
        [DataMember]
        public String Cargo { get { return this.getCargo(); } set { this.setCargo(value); } }

        private String cargo;

        public int getIdPersona() { return idTitulo; }
        public void setIdPersona(int idPersona) { this.idPersona = idPersona; }

        public int getIdTitulo() { return idPersona; }
        public void setIdTitulo(int idTitulo) { this.idTitulo = idTitulo; }

        public int getIdPuesto() { return idPuesto; }
        public void setIdPuesto(int idPuesto) { this.idPuesto = idPuesto; }

        public int getIdPonencia() { return idPonencia; }
        public void setIdPonencia(int idPonencia) { this.idPonencia = idPonencia; }

        public String getNombrePersona() { return Nombre; }
        public void setNombrePersona(String Nombre) { this.Nombre = Nombre; }

        public String getApellidosPersona() { return apellidos; }
        public void setApellidosPersona(String apellidos) { this.apellidos = apellidos; }

        public String getNombreCompleto() { return nombreCompleto; }
        public void setNombreCompleto(String nombreCompleto) { this.nombreCompleto = nombreCompleto; }

        public String getPrefijo() { return prefijo; }
        public void setPrefijo(String prefijo) { this.prefijo = prefijo; }

        public String getPosfijo() { return posfijo; }
        public void setPosfijo(String posfijo) { this.posfijo = posfijo; }

        public int getOrden() { return orden; }
        public void setOrden(int orden) { this.orden = orden; }

        public int getOrdenSala() { return ordenSala; }
        public void setOrdenSala(int ordenSala) { this.ordenSala = ordenSala; }

        public int getSala() { return sala; }
        public void setSala(int sala) { this.sala = sala; }

        public String getTitSemblanza() { return titSemblanza; }
        public void setTitSemblanza(String titSemblanza) { this.titSemblanza = titSemblanza; }

        public String getDomPersona() { return Domicilio; }
        public void setDomPersona(String Domicilio) { this.Domicilio = Domicilio; }

        public String getTelPersona() { return Telefono; }
        public void setTelPersona(String Telefono) { this.Telefono = Telefono; }

        public String getExtPersona() { return Ext; }
        public void setExtPersona(String Ext) { this.Ext = Ext; }

        public String getCargo() { return cargo; }
        public void setCargo(String cargo) { this.cargo = cargo; }

        public String getTituloPersona() { return Titulo; }
        public void setTituloPersona(String Titulo) { this.Titulo = Titulo; }
    }
}

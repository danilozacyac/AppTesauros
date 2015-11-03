using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Es la clase que contiene los datos normales de una tesis.
    /// </summary>
    /// 
    [DataContract]
    public class TesisTO
    {
        public long ConsecIndxInt { get; set; }
        private String parte;
        [DataMember]
        public String Parte{get{return parte;} set{this.parte=value;}}
        private String consec;
        [DataMember]
        public String Consec{get{return consec;} set{this.consec=value;}}
        private String ius;
        [DataMember]
        public String Ius{get{return ius;} set{this.ius=value;}}
        private String rubro;
        [DataMember]
        public String Rubro{get{return rubro;} set{this.rubro=value;}}
        private String texto;
        [DataMember]
        public String Texto{get{return texto;} set{this.texto=value;}}
        private String precedentes;
        [DataMember]
        public String Precedentes {get{return precedentes;} set{this.precedentes=value;}}
        private String epoca;
        [DataMember]
        public String Epoca{get{return epoca;} set{this.epoca=value;}}
        private String sala;
        [DataMember]
        public String Sala{get{return sala;} set{this.sala=value;}}
        private String fuente;
        [DataMember]
        public String Fuente {get{return fuente;} set{this.fuente=value;}}
        private String volumen;
        [DataMember]
        public String Volumen{get{return volumen;} set{this.volumen=value;}}
        private String tesis;
        [DataMember]
        public String Tesis{get{return tesis;} set{this.tesis=value;}}
        private String pagina;
        [DataMember]
        public String Pagina{get{return pagina;} set{this.pagina=value;}}
        private String ta_tj;
        [DataMember]
        public String Ta_tj{get{return ta_tj;} set{this.ta_tj=value;}}
        private String materia1;
        [DataMember]
        public String Materia1{get{return materia1;} set{this.materia1=value;}}
        private String materia2;
        [DataMember]
        public String Materia2{get{return materia2;} set{this.materia2=value;}}
        private String materia3;
        [DataMember]
        public String Materia3{get{return materia3;} set{this.materia3=value;}}
        private String idGenealogia;
        [DataMember]
        public String IdGenealogia {get{return idGenealogia;} set{this.idGenealogia=value;}}
        private String volOrden;
        [DataMember]
        public String VolOrden{get{return volOrden;} set{this.volOrden=value;}}
        private String consecIndx;
        [DataMember]
        public String ConsecIndx{get{return consecIndx;} set{this.consecIndx=value;}}
        private String idTCC;
        [DataMember]
        public String IdTCC{get{return idTCC;} set{this.idTCC=value;}}
        private String infAnexos;
        [DataMember]
        public String InfAnexos{get{return infAnexos;} set{this.infAnexos=value;}}
        private String locAbr;
        [DataMember]
        public String LocAbr { get { return locAbr; } set { this.locAbr = value; } }
        private String numLetra;
        [DataMember]
        public String NumLetra { get { return numLetra; } set { this.numLetra = value; } }
        private String consecLetra;
        [DataMember]
        public String ConsecLetra { get { return consecLetra; } set { this.consecLetra = value; } }
        private String instancia;
        [DataMember]
        public String Instancia { get { return instancia; } set { this.instancia = value; } }
        private String consecInst;
        [DataMember]
        public String ConsecInst { get { return consecInst; } set { this.consecInst = value; } }
        private String tpoTesis;
        [DataMember]
        public String TpoTesis { get { return tpoTesis; } set { this.tpoTesis = value; } }
        private String descTpoTesis;
        [DataMember]
        public String DescTpoTesis { get { return descTpoTesis; } set { this.descTpoTesis = value; } }
        private String imageWeb;
        [DataMember]
        public String ImageWeb { get { return imageWeb; } set { this.imageWeb = value; } }
        private String imageOther;
        [DataMember]
        public String ImageOther { get { return imageOther; } set { this.imageOther = value; } }
        [DataMember]
        public long OrdenTesis { get; set; }
        [DataMember]
        public long OrdenRubro { get; set; }
        [DataMember]
        public long OrdenInstancia { get; set; }
        [DataMember]
        public int[] Ponentes { get; set; }
        [DataMember]
        public int[] TipoPonente { get; set; }
        [DataMember]
        public int[] TipoTesis { get; set; }
        [DataMember]
        public bool ExistenTemas { get; set; }
        [DataMember]
        public String VolumenPrefijo { get; set; }
        [DataMember]
        public int Vigencia { get; set; }
        [DataMember]
        public int Tomo { get; set; }
        [DataMember]
        public int Seccion { get; set; }
        [DataMember]
        public bool ExistenNG { get; set; }

        public String getParte()
        {
            return Parte;
        }

        public void setParte(String parte)
        {
            Parte = parte;
        }

        public String getConsec()
        {
            return Consec;
        }

        public void setConsec(String consec)
        {
            Consec = consec;
        }

        public String getIus()
        {
            return ius;
        }

        public void setIus(String ius)
        {
            this.ius = ius;
        }

        public String getRubro()
        {
            return Rubro;
        }

        public void setRubro(String rubro)
        {
            Rubro = rubro;
        }

        public String getTexto()
        {
            return Texto;
        }

        public void setTexto(String texto)
        {
            Texto = texto;
        }

        public String getPrecedentes()
        {
            return Precedentes;
        }

        public void setPrecedentes(String precedentes)
        {
            Precedentes = precedentes;
        }

        public String getEpoca()
        {
            return Epoca;
        }

        public void setEpoca(String epoca)
        {
            Epoca = epoca;
        }

        public String getSala()
        {
            return Sala;
        }

        public void setSala(String sala)
        {
            Sala = sala;
        }

        public String getFuente()
        {
            return Fuente;
        }

        public void setFuente(String fuente)
        {
            Fuente = fuente;
        }

        public String getVolumen()
        {
            return Volumen;
        }

        public void setVolumen(String volumen)
        {
            Volumen = volumen;
        }

        public String getTesis()
        {
            return Tesis;
        }

        public void setTesis(String tesis)
        {
            Tesis = tesis;
        }

        public String getPagina()
        {
            return Pagina;
        }

        public void setPagina(String pagina)
        {
            Pagina = pagina;
        }

        public String getTa_tj()
        {
            return ta_tj;
        }

        public void setTa_tj(String ta_tj)
        {
            this.ta_tj = ta_tj;
        }

        public String getMateria1()
        {
            return Materia1;
        }

        public void setMateria1(String materia1)
        {
            Materia1 = materia1;
        }

        public String getMateria2()
        {
            return Materia2;
        }

        public void setMateria2(String materia2)
        {
            Materia2 = materia2;
        }

        public String getMateria3()
        {
            return Materia3;
        }

        public void setMateria3(String materia3)
        {
            Materia3 = materia3;
        }

         public String getIdGenealogia()
        {
            return IdGenealogia;
        }

        public void setIdGenealogia(String idGenealogia)
        {
            IdGenealogia = idGenealogia;
        }

        public String getVolOrden()
        {
            return VolOrden;
        }

        public void setVolOrden(String volOrden)
        {
            VolOrden = volOrden;
        }

        public String getConsecIndx()
        {
            return ConsecIndx;
        }

        public void setConsecIndx(String consecIndx)
        {
            ConsecIndx = consecIndx;
        }

        public String getIdTCC()
        {
            return IdTCC;
        }

        public void setIdTcc(String idTcc)
        {
            IdTCC = idTcc;
        }
        public String getIdTcc()
        {
            return IdTCC;
        }

        public void setIdTCC(String idTCC)
        {
            IdTCC = idTCC;
        }

         public String getInfAnexos()
        {
            return InfAnexos;
        }

        public void setInfAnexos(String infAnexos)
        {
            InfAnexos = infAnexos;
        }

        public String getLocAbr()
        {
            return LocAbr;
        }

        public void setLocAbr(String locAbr)
        {
            LocAbr = locAbr;
        }

        public String getNumLetra()
        {
            return NumLetra;
        }

        public void setNumLetra(String numLetra)
        {
            NumLetra = numLetra;
        }

        public String getConsecLetra()
        {
            return ConsecLetra;
        }

        public void setConsecLetra(String consecLetra)
        {
            ConsecLetra = consecLetra;
        }

        public String getInstancia()
        {
            return Instancia;
        }

        public void setInstancia(String instancia)
        {
            Instancia = instancia;
        }

        public String getConsecInst()
        {
            return ConsecInst;
        }
        /// <summary>
        /// Responde si la tesis actual es jurisprudencia o no.
        /// </summary>
        /// <returns>True si la tesis es Jurisprudencia</returns>
        public Boolean getEsJuris()
        {
            bool resultado;
            resultado = (this.getTa_tj() != null) ? this.getTa_tj().Equals("1") : false;
            return resultado;
        }

        /// <summary>
        /// Obtiene el tag de la imagen Web.
        /// </summary>
        /// <returns>Un tag de HTML para poner una imagen de acuerdo al tipo de tesis</returns>
        public String getWebImageTag()
        {
            return "<IMG SRC = '" + this.getImageWeb() + "' ALT = '" + this.getDescTpoTesis() + "' WIDTH = '33px' HEIGHT = '33px'>";
        }
        public void setConsecInst(String consecInst)
        {
            ConsecInst = consecInst;
        }

        public String getTpoTesis()
        {
            return tpoTesis;
        }

        public void setTpoTesis(String tpoTesis)
        {
            this.tpoTesis = tpoTesis;
        }

        public String getDescTpoTesis()
        {
            return descTpoTesis;
        }

        public void setDescTpoTesis(String descTpoTesis)
        {
            this.descTpoTesis = descTpoTesis;
        }

        public String getImageWeb()
        {
            return imageWeb;
        }

        public void setImageWeb(String imageWeb)
        {
            this.imageWeb = imageWeb;
        }

        public String getImageOther()
        {
            return imageOther;
        }

        public void setImageOther(String imageOthers)
        {
            this.imageOther = imageOthers;
        }

    }
}
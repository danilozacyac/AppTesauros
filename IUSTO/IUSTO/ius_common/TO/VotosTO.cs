using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
[DataContract]
public class VotosTO {
	private String parteT;
    [DataMember]
    public String ParteT { get { return this.getParteT(); } set { this.setParteT(value); } }
	private String consec;
    [DataMember]
    public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
    private String id;
    [DataMember]
    public String Id { get { return this.getId(); } set { this.setId(value); } }
    private String tesis;
    [DataMember]
    public String Tesis { get { return this.getTesis(); } set { this.setTesis(value); } }
    private String sala;
    [DataMember]
    public String Sala { get { return this.getSala(); } set { this.setSala(value); } }
    private String epoca;
    [DataMember]
    public String Epoca { get { return this.getEpoca(); } set { this.setEpoca(value); } }
    private String volumen;
    [DataMember]
    public String Volumen { get { return this.getVolumen(); } set { this.setVolumen(value); } }
    private String fuente;
    [DataMember]
    public String Fuente { get { return this.getFuente(); } set { this.setFuente(value); } }
    private String pagina;
    [DataMember]
    public String Pagina { get { return this.getPagina(); } set { this.setPagina(value); } }
    private String rubro;
    [DataMember]
    public String Rubro { get { return this.getRubro(); } set { this.setRubro(value); } }
    private String volOrden;
    [DataMember]
    public String VolOrden { get { return this.getVolOrden(); } set { this.setVolOrden(value); } }
    private String consecIndx;
    [DataMember]
    public String ConsecIndx { get { return this.getConsecIndx(); } set { this.setConsecIndx(value); } }
    private String procesado;
    [DataMember]
    public String Procesado { get { return this.getProcesado(); } set { this.setProcesado(value); } }
    private String tpoAsunto;
    [DataMember]
    public String TpoAsunto { get { return this.getTpoAsunto(); } set { this.setTpoAsunto(value); } }
    private String promovente;
    [DataMember]
    public String Promovente { get { return this.getPromovente(); } set { this.setPromovente(value); } }
    private String clasificacion;
    [DataMember]
    public String Clasificacion { get { return this.getClasificacion(); } set { this.setClasificacion(value); } }
    private String complemento;
    [DataMember]
    public String Complemento{ get { return this.getComplemento(); } set { this.setComplemento(value); } }
    [DataMember]
    public int OrdenAsunto { get; set; }
    [DataMember]
    public int OrdenEmisor { get; set; }
    /**
         * @return the parteT
         */
	public String getParteT() {
		return parteT;
	}
	/**
	 * @param parteT the parteT to set
	 */
	public void setParteT(String parteT) {
		this.parteT = parteT;
	}
	/**
	 * @return the consec
	 */
	public String getConsec() {
		return consec;
	}
	/**
	 * @param consec the consec to set
	 */
	public void setConsec(String consec) {
		this.consec = consec;
	}
	/**
	 * @return the id
	 */
	public String getId() {
		return id;
	}
	/**
	 * @param id the id to set
	 */
	public void setId(String id) {
		this.id = id;
	}
	/**
	 * @return the tesis
	 */
	public String getTesis() {
		return tesis;
	}
	/**
	 * @param tesis the tesis to set
	 */
	public void setTesis(String tesis) {
		this.tesis = tesis;
	}
	/**
	 * @return the sala
	 */
	public String getSala() {
		return sala;
	}
	/**
	 * @param sala the sala to set
	 */
	public void setSala(String sala) {
		this.sala = sala;
	}
	/**
	 * @return the epoca
	 */
	public String getEpoca() {
		return epoca;
	}
	/**
	 * @param epoca the epoca to set
	 */
	public void setEpoca(String epoca) {
		this.epoca = epoca;
	}
	/**
	 * @return the volumen
	 */
	public String getVolumen() {
		return volumen;
	}
	/**
	 * @param volumen the volumen to set
	 */
	public void setVolumen(String volumen) {
		this.volumen = volumen;
	}
	/**
	 * @return the fuente
	 */
	public String getFuente() {
		return fuente;
	}
	/**
	 * @param fuente the fuente to set
	 */
	public void setFuente(String fuente) {
		this.fuente = fuente;
	}
	/**
	 * @return the pagina
	 */
	public String getPagina() {
		return pagina;
	}
	/**
	 * @param pagina the pagina to set
	 */
	public void setPagina(String pagina) {
		this.pagina = pagina;
	}
	/**
	 * @return the rubro
	 */
	public String getRubro() {
		return rubro;
	}
	/**
	 * @param rubro the rubro to set
	 */
	public void setRubro(String rubro) {
		this.rubro = rubro;
	}
	/**
	 * @return the volOrden
	 */
	public String getVolOrden() {
		return volOrden;
	}
	/**
	 * @param volOrden the volOrden to set
	 */
	public void setVolOrden(String volOrden) {
		this.volOrden = volOrden;
	}
	/**
	 * @return the consecIndx
	 */
	public String getConsecIndx() {
		return consecIndx;
	}
	/**
	 * @param consecIndx the consecIndx to set
	 */
	public void setConsecIndx(String consecIndx) {
		this.consecIndx = consecIndx;
	}
	/**
	 * @return the procesado
	 */
	public String getProcesado() {
		return procesado;
	}
	/**
	 * @param procesado the procesado to set
	 */
	public void setProcesado(String procesado) {
		this.procesado = procesado;
	}
	/**
	 * @return the tpoAsunto
	 */
	public String getTpoAsunto() {
		return tpoAsunto;
	}
	/**
	 * @param tpoAsunto the tpoAsunto to set
	 */
	public void setTpoAsunto(String tpoAsunto) {
		this.tpoAsunto = tpoAsunto;
	}
	/**
	 * @return the promovente
	 */
	public String getPromovente() {
		return promovente;
	}
	/**
	 * @param promovente the promovente to set
	 */
	public void setPromovente(String promovente) {
		this.promovente = promovente;
	}
	/**
	 * @return the clasificacion
	 */
	public String getClasificacion() {
		return clasificacion;
	}
	/**
	 * @param clasificacion the clasificacion to set
	 */
	public void setClasificacion(String clasificacion) {
		this.clasificacion = clasificacion;
	}
	/**
	 * @return the complemento
	 */
	public String getComplemento() {
		return complemento;
	}
	/**
	 * @param complemento the complemento to set
	 */
	public void setComplemento(String complemento) {
		this.complemento = complemento;
	}
	/**
	 * Genera la cadena de Localizacion
	 */
	public String getLoc(){
        if (getPagina().Trim().Equals("0"))
        {
            return this.getEpoca() + "; " + this.getSala() + "; " + this.getFuente()
               + "; " + this.getVolumen();
        }
        else
        {
            return this.getEpoca() + "; " + this.getSala() + "; " + this.getFuente()
               + "; " + this.getVolumen() + "; Pág. " + this.getPagina();
        }
	}

}
}

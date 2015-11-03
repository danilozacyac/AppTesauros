using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
 
namespace mx.gob.scjn.ius_common.TO
{
[DataContract]
public class RelDocumentoTesisTO {
	/// <summary>
    /// 	  Define la tesis de la cual se quiere la relación.
	/// </summary>
	 ///
	private String ius;
    [DataMember]
    public String Ius { get { return this.getIus(); } set { this.setIus(value); } }
	/// <summary>
	/// Define el documento con el cual está relacionado la tesis.
	/// </summary>
	private String id;
    [DataMember]
    public String Id { get { return this.getId(); } set { this.setId(value); } }
	/// <summary>
	/// Define cual es el tipo de documento con el cual se está relacionado.
	/// </summary>
    /// 
	private String tpoRel;
    [DataMember]
    public String TpoRel { get { return this.getTpoRel(); } set { this.setTpoRel(value); } }
    /**
     * Obtiene la tesis de la relación
     * @return El IUS de la tesis.
     */
	public String getIus() {
		return ius;
	}
	/**
	 * Establece cual es el IUS de la tesis en esta relación.
	 * @param ius el IUS de la tesis a relacionar.
	 */
	public void setIus(String ius) {
		this.ius = ius;
	}
	/**
	 * Obtiene el Id del documento que está relacionado con
	 * las tesis.
	 * @return El id del documento relacionado.
	 */
	public String getId() {
		return id;
	}
	/**
	 * Establece el identificador del 
	 * documento que está relacionado con la tesis.
	 * @param id El id del documento que se relaciona con una tesis.
	 */
	public void setId(String id) {
		this.id = id;
	}
	/**
	 * Define el tipo de documento sobre el cual se está relacionando.
	 * 2 Es una ejecutoria
	 * 3 es un Voto
	 * @return El tipo de relación
	 */
	public String getTpoRel() {
		return tpoRel;
	}
	/**
	 * Establece el tipo de documento sobre el cual se está relacionando.
	 * 2 Es una ejecutoria
	 * 3 es un Voto
	 * @param tpoRel El documento que se relaciona con la tesis
	 */
	public void setTpoRel(String tpoRel) {
		this.tpoRel = tpoRel;
	}
}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.TO
{
/**
 * Esta clase representa los parametros que serán mandados al 
 * DAO de iBatis para poder realizar las consultas de la BD
 * @author Carlos de Luna Sáenz
 * @version 1.0
 *
 */
public class EpocasTO {
	public int epoca;
	public List<EpocasSalasTO> epocasSalas;
	private int sala;
	private int actualizacion;
	private int tipoAct;
	private int procedencia;
	private int acuerdoEpoca;
	
	public EpocasTO(){
		
	}
	/**
	 * Representa la epoca a consultar, donde 5 representa a la 9a., 4 a la
	 * 8a. y así sucesivamente.
	 * @return el valor de la epoca a consultar.
	 */
	public int getEpoca() {
		return epoca;
	}
	/**
	 * Indica el valor de la época de acuerdo a los representados en la BD.
	 * @param epoca el valor que tendrá la epoca
	 * @see #getEpoca()
	 */
	public void setEpoca(int epocaParam) {
		this.epoca = epocaParam;
	}
	/**
	 * Obtiene la sala sobre la cual se hará la consulta.
	 * @return el número de sala o bien un valor específico para el TC.
	 */
	public int getSala() {
		return sala;
	}
	/**
	 * Define la sala sobre la que se hará la consulta para obtener las
	 * tesis de una determinada epoca en dicha sala.
	 * @param sala El valor de la sala sobre la cual se realizará la búsqueda
	 */
	public void setSala(int sala) {
		this.sala = sala;
	}
	/**
	 * @return the actualizacion
	 */
	public int getActualizacion() {
		return actualizacion;
	}
	/**
	 * @param actualizacion the actualizacion to set
	 */
	public void setActualizacion(int actualizacion) {
		this.actualizacion = actualizacion;
	}
	/**
	 * @return the tipoAct
	 */
	public int getTipoAct() {
		return tipoAct;
	}
	/**
	 * @param tipoAct the tipoAct to set
	 */
	public void setTipoAct(int tipoAct) {
		this.tipoAct = tipoAct;
	}
	/**
	 * @return the procedencia
	 */
	public int getProcedencia() {
		return procedencia;
	}
	/**
	 * @param procedencia the procedencia to set
	 */
	public void setProcedencia(int procedencia) {
		this.procedencia = procedencia;
	}
	/**
	 * @return the acuerdoEpoca
	 */
	public int getAcuerdoEpoca() {
		return acuerdoEpoca;
	}
	/**
	 * @param acuerdoEpoca the acuerdoEpoca to set
	 */
	public void setAcuerdoEpoca(int acuerdoEpoca) {
		this.acuerdoEpoca = acuerdoEpoca;
	}
	/**
	 * @return the epocas
	 */
	public List<EpocasSalasTO> getEpocasSalas() {
		return epocasSalas;
	}
	/**
	 * @param epocas the epocas to set
	 */
	public void setEpocasSalas(List<EpocasSalasTO> epocas) {
		this.epocasSalas = epocas;
	}
  }
}

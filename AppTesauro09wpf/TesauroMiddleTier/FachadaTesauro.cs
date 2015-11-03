using System;
using System.Collections.Generic;
using System.Linq;
using TesauroTO;
using mx.gob.scjn.ius_common.TO;

namespace TesauroMiddleTier
{
    public class FachadaTesauro : IFachadaTesauro
    {
        #region IFachadaTesauro Members

        //Metodos originales suceptibles de ser depreciados


        public List<TemaTO> GetTemas()
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.getTemas();
        }

        public List<TemaTO> GetTemas(String busqueda)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetTemas(busqueda);
        }
        //////


        /// <summary>
        /// Devuelve un listado solo con los temas de la materia o materias que le corresponden a cada usuario
        /// </summary>
        /// <param name="materias">Materias asignadas a este usuario</param>
        /// <returns></returns>
        public List<TemaTO> GetTemas(List<int> materias)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.getTemas(materias);
        }

        public List<TemaTO> GetTemas(String busqueda, List<int> materias)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetTemas(busqueda);
        }

        public List<SinonimoTO> GetSinonimos(int p, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetSinonimos(p,idMateria);
        }

        public List<SinonimoTO> GetRelacionesProximas(int p, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetRP(p,idMateria);
        }
        
        public TemaTO GetIA(int p)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetIA(p);
        }
        
        public int ActualizaPadre(TemaTO temaTO)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.ActualizaPadre(temaTO);
        }
        
        public void ActualizaTema(TemaTO datos)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.ActualizaTema(datos);
        }
        
        public int GeneraNuevoTema(TemaTO datos)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GeneraNuevoTema(datos);
        }

        public void ActualizaSinonimo(SinonimoTO sinonimoActual, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.actualizaSinonimo(sinonimoActual,idMateria);
        }

        public int GeneraNuevoSinonimo(SinonimoTO sinonimoActual, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GeneraNuevoSinonimo(sinonimoActual,idMateria);
        }

        public void EliminaSinonimo(SinonimoTO actual, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.EliminarSinonimo(actual,idMateria);
        }

        public List<ExpresionTO> GetExpresiones(int tema, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetExpresiones(tema,idMateria);
        }

        public void actualizaExpresion(ExpresionTO expresionActual, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.actualizaExpresion(expresionActual,idMateria);
        }
        
        public void EliminaExpresion(int id)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.EliminaExpresion(id);
        }

        public void InsertaExpresion(ExpresionTO expresionActual, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.InsertaExpresiones(expresionActual,idMateria);
        }

        public void InsertaIA(String ascendente, int idTema, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.InsertaIA(ascendente, idTema,idMateria);
        }
        
        public void actualizaIA(TemaTO datosIA)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.ActualizaIA(datosIA);
        }

        public void ActualizaMateria(TemaTO temaQueActualizar,int materia)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.ActualizaMateria(temaQueActualizar, materia);
        }

        public int CopiaTema(int id, int padre, int materia, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.copiaTema(id, padre, materia,idMateria);
        }
        
        
        
        public List<TemaTO> GetHijos(int padre)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetHijos(padre);
        }

        public bool VerifyExistence(String texto, int idpadre,int idMateria)
        {
            TemaBusinessTier dac = new TemaBusinessTier();
            return dac.VerifyExistence(texto, idpadre, idMateria);
        }

        public List<int> GetTesisPorTema(TemaTO tema)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetTesisPorTema(tema);
        }
        
        public String GetRuta(TemaTO tema)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.ObtenRuta(tema);
        }
        
        public void CopiaTema(List<TemaTO> temasCopiar, int idPadre, int idMateria)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.copiaTema(temasCopiar, idPadre, idMateria);
        }
        
        public void EstableceUsuario(int usuario)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.EstableceUsuario(usuario);
        }
        
        public List<StatusTO> GetStatus()
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.GetStatus();
        }
        
        public void InsertaObservacion(ObservacionTO obs)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.InsertaObservaciones(obs);
        }
        
        public List<ObservacionTO> ObtenObservaciones(int id)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            return temas.ObtenObservaciones(id);
        }
        
        /// <summary>
        /// Establece una nueva relación entre un tema y una tesis
        /// </summary>
        /// <param name="tema"></param>
        /// <param name="tesis"></param>
        public void NuevaRelacionTemaTesis(TemaTO tema, TesisTO tesis)
        {
            TemaBusinessTier temas = new TemaBusinessTier();
            temas.NuevaRelacionTemaTesis(tema, tesis);
        }
        
        /// <summary>
        /// Devuelve las tesis relacionadas con el tema seleccionado
        /// </summary>
        /// <param name="tema"></param>
        /// <returns></returns>
        public List<int> ObtenTesisRelacionadasPorTema(TemaTO tema)
        {
            TemaBusinessTier tesis = new TemaBusinessTier();
            return tesis.ObtenTesisRelacionadasPorTema(tema);
        }
        
        /// <summary>
        /// Elimina una relación existente entre tema y tesis
        /// </summary>
        /// <param name="tema"></param>
        /// <param name="tesis">Número de registro ius cuya relación con este tema será eliminada</param>
        public void RemueveRelacionTemaTesis(TemaTO tema, int tesis)
        {
            TemaBusinessTier tier = new TemaBusinessTier();
            tier.RemueveRelacionTemaTesis(tema, tesis);
        }
        
        /// <summary>
        /// Elimina todas las relaciones con números de registros IUS de un tema
        /// </summary>
        /// <param name="tema"></param>
        public void RemueveRelacionTemaTesis(TemaTO tema)
        {
            TemaBusinessTier dac = new TemaBusinessTier();
            dac.RemueveRelacionTemaTesis(tema);
        }

        

        public int VerificaExistenciaRelacion(String registroIus, int idMateria)
        {
            TemaBusinessTier dac = new TemaBusinessTier();
            return dac.VerificaExistenciaRelacion(registroIus, idMateria);
        }
        
        #endregion
        
        #region TematicoIUS
        
        public void SetTemaQueImporta(int idTema, int idMateria, int idTemaQueImporta)
        {
            TemaBusinessTier dac = new TemaBusinessTier();
            dac.SetTemaQueImporta(idTema, idMateria, idTemaQueImporta);
        }
    
        #endregion

        #region Bitacora

        public void SetBitacora(int idTema, int idSeccion, int idMovimiento, int idUsuario, String edoAnterior, String edoActual,int idMateria)
        {
            TemaBusinessTier dac = new TemaBusinessTier();
            dac.SetBitacora(idTema, idSeccion, idMovimiento, idUsuario, edoAnterior, edoActual, idMateria);
        }

        #endregion
    }
}
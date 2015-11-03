using System;
using System.Collections.Generic;
using System.Linq;
using TesauroTO;

namespace TesauroMiddleTier
{
    public interface IFachadaTesauro
    {
        List<TemaTO> GetTemas();

        //Probando
        List<TemaTO> GetTemas(List<int> materias);

        List<SinonimoTO> GetSinonimos(int p, int idMateria);

        List<SinonimoTO> GetRelacionesProximas(int p, int idMateria);

        TemaTO GetIA(int p);

        int ActualizaPadre(TemaTO temaTO);

        void ActualizaTema(TemaTO Datos);

        int GeneraNuevoTema(TemaTO Datos);

        void ActualizaSinonimo(SinonimoTO sinonimoActual, int idMateria);

        int GeneraNuevoSinonimo(SinonimoTO sinonimoActual, int idMateria);

        void EliminaSinonimo(SinonimoTO actual, int idMateria);

        List<ExpresionTO> GetExpresiones(int tema, int idMateria);

        void actualizaExpresion(ExpresionTO ExpresionActual, int idMateria);

        void InsertaExpresion(ExpresionTO ExpresionActual, int idMateria);

        void EliminaExpresion(int id);

        void InsertaIA(string InclusionAscendente, int IDTema, int idMateria);

        void actualizaIA(TemaTO DatosIA);

        /// <summary>
        /// Actualiza la materia del tema seleccionado
        /// </summary>
        /// <param name="temaQueActualizar">Tema al cual se hara la actulización</param>
        /// <param name="materia">Nueva materia</param>
        void ActualizaMateria(TemaTO temaQueActualizar,int materia);

        int CopiaTema(int id, int padre, int Materia, int idMateria);

        List<TemaTO> GetTemas(String busqueda);

        List<TemaTO> GetHijos(int p);

        String GetRuta(TemaTO Tema);

        void CopiaTema(List<TemaTO> TemasCopiar, int IDPadre, int IDMateria);

        void EstableceUsuario(int p);

        List<StatusTO> GetStatus();

        bool VerifyExistence(String texto, int idpadre,int idMateria);

        void InsertaObservacion(ObservacionTO obs);

        List<ObservacionTO> ObtenObservaciones(int p);

        void SetBitacora(int idTema, int idSeccion, int idMovimiento, int idUsuario, String edoAnterior, String edoActual,int idMateria);
    }
}

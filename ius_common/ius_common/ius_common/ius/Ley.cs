using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.DAO;

namespace mx.gob.scjn.ius_common.ius
{
public class Ley {
	private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.ius.Tesis");
	private IUSApplicationContext contexto;
	/**
	 * Inicia el objeto obteniendo los contextos necesarios.
	 */
	public Ley(){
		try{
		    this.contexto = new IUSApplicationContext();
		} catch (Exception e ) {
			log.Debug("Problemas al iniciar el contexto");
			log.Debug(e.StackTrace);
		}
	}
	/**
	 * Obtiene una ley en específico.
	 * @param ley La ley en la que se busca.
	 * @param articulo El artículo de la ley
	 * @return El documento conteniendo el artículo
	 */
	public DocumentoLeyTO getDocumentoLey(int ley, int articulo, int idRef, int TipoLey){
		DocumentoLeyTO doc = new DocumentoLeyTO();
		ArticulosTO parametros = new ArticulosTO();
		parametros.setIdLey(ley+"");
		parametros.setIdArt(articulo+"");
		parametros.setIdRef(idRef+"");
		LeyDAO daoLey =  (LeyDAO)contexto.getInitialContext().GetObject("LeyDAO");
        List<ArticulosTO> articulos = null;
        if (TipoLey == 0)
            articulos = daoLey.getArticulos(parametros);
        else
            articulos = daoLey.getArticulosEst(parametros);
		doc.setArticulo(articulos);
		doc.setLey(daoLey.getLeyPorId(ley));
		return doc;
	}

    public List<string> getArchivosLeyes(ArticulosTO articulo)
    {
        List<String> Resultado = new List<string>();
        LeyDAO daoLey = (LeyDAO)contexto.getInitialContext().GetObject("LeyDAO");
        Resultado = daoLey.getArchivos(articulo);
        return Resultado;
    }

    

    internal List<ArticulosTO> getArchivosLeyPorIUS(long tesis)
    {
        List<ArticulosTO> Resultado = new List<ArticulosTO>();
        LeyDAO daoLey = (LeyDAO)contexto.getInitialContext().GetObject("LeyDAO");
        Resultado = daoLey.getArticulosPorIUS(tesis);
        return Resultado;
    }
}
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using Common.Logging;
using xmlparsing;


namespace mx.gob.scjn.ius_common.context
{
   public class IUSApplicationContext {
      private readonly ILog log = LogManager.GetLogger(typeof(mx.gob.scjn.ius_common.context.IUSApplicationContext));
      private static IUSApplicationContext initialContext;
      private static objects objetosExistentes=null;
      private static Hashtable objetosReales;
       /// <summary>
       /// Constructor por omisión
       /// </summary>
      public IUSApplicationContext()
      {
          Assembly ensamblado = Assembly.GetExecutingAssembly();
          if (initialContext == null)
          {
              try
              {
                  Stream flujo = ensamblado.GetManifestResourceStream("mx.gob.scjn.ius_common.context.ApplicationContext.xml");
                  initialContext = new IUSApplicationContext(flujo);
              }
              catch (Exception e)
              {
                  log.Debug("Error al obtener el archivo del recurso");
                  log.Debug(e.StackTrace);
              }
          }
      }
       /// <summary>
       /// Obtiene los objetos existentes
       /// </summary>
       /// <returns>La lista de objetos existentes</returns>
      public objects getObjetosExsitentes()
      {
          return objetosExistentes;
      }
       /// <summary>
       /// Define cuales son los objetos existentes
       /// </summary>
       /// <param name="parametro">Los objetos existentes</param>
      public void setObjetosExistentes(objects parametro)
      {
          objetosExistentes = parametro;
      }
       /// <summary>
       /// Devuelve el log.
       /// </summary>
       /// <returns>Devuelve el log para registrar los eventos de bitácora</returns>
       public  ILog getLog() {
		return log;
	  }
       /// <summary>
       /// Regresa un contexto para la aplicación, un singleton.
       /// </summary>
       /// <returns>El singleton del contexto.</returns>
	  public IUSApplicationContext getInitialContext() {
		return initialContext;
	  }
       /// <summary>
       /// Inicia el contexto con el archivo dado
       /// </summary>
       /// <param name="fileToResource">El archivo de configuración del contexto</param>
      public IUSApplicationContext(Stream fileToResource)
      {
          try
          {
              TextReader archivo = new StreamReader(fileToResource);
              XmlSerializer serializer = new XmlSerializer(typeof(objects));
              objects objeto = (objects)serializer.Deserialize(archivo);
              objetosReales = new Hashtable();
              foreach (object item in objeto.Items)
              {
                  vanillaObject objetoRealSolicitado = (vanillaObject)item;
                  ContextoTO datosActuales = new ContextoTO();
                  datosActuales.SetTipo(objetoRealSolicitado.type);
                  datosActuales.SetObjeto(null);
                  objetosReales.Add(objetoRealSolicitado.id, datosActuales);
              }
          }
          catch (Exception e)
          {
              log.Debug("Error al estar leyendo el XML: " + e.StackTrace);
          }
      }
       /// <summary>
       /// Obtiene un singleton de un objeto solicitado.
       /// </summary>
       /// <param name="objeto">El objeto solicitado</param>
       /// <returns>La instancia del objeto.</returns>
      public Object GetObject(String objetoParam)
      {
          ContextoTO objetoActual = (ContextoTO)objetosReales[objetoParam];
          if (objetoActual == null) objetoActual = new ContextoTO();
          if(objetoActual.GetObjeto()!=null){
              return objetoActual.GetObjeto();
          }else{
              /****Hay que generar el nuevo objeto y pasarlo como resultado, en dado caso
               * gurdarlo dentro de nuestro arreglo de objetos reales
               * */
              ContextoTO objetoRealActual = (ContextoTO)objetosReales[objetoParam];
              String tipoEsperado = objetoRealActual.GetTipo();
              Assembly ensamblado = Assembly.GetExecutingAssembly();
              Object objetoRealVerdadero = ensamblado.CreateInstance(tipoEsperado);
              objetoRealActual.SetObjeto(objetoRealVerdadero);
              objetosReales[objetoParam] = objetoRealActual;
              return objetoRealVerdadero;
          }
      }
   }
}

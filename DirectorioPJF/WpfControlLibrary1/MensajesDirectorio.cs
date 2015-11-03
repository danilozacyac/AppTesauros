using System;
using System.Linq;

namespace mx.gob.scjn.directorio
{

    public class MensajesDirectorio
    {

        public const String MENSAJE_PORTAPAPELES_DIR = "La Información ha sido enviada al portapapeles";
        public const String TITULO_PORTAPAPELES_DIR = "Directorio";
        public const String MENSAJE_Y_LA_IMPRESORA = "No se puede imprimir debido a un problema con la impresora seleccionada." +
            " \n Pruebe lo siguiente:" +
            " \n * Imprima una página de prueba y revise la impresora." +
            " \n * Compruebe que está conectada y en línea." +
            " \n * Instale de nuevo el controlador de la impresora.";
 
        public const String TITULO_MENSAJES = "Jurisprudencia y Tesis Aisladas";
        public const String MENSAJE_ARCHIVO_ABIERTO = "El archivo no pudo ser escrito, verifique que no este abierto y/o que tenga permisos de escritura";
        public const String TITULO_ARCHIVO_ABIERTO = "Jurisprudencia y Tesis Aisladas";
        public const String MENSAJE_NO_SE_PUEDE_VER_CORTE = "Ya que usted está usando la versión Web del programa éste no puede abrir la página de la Suprema Corte de Justicia de la Nación, por favor use la barra de dirección de su navegador e ingrese a www.scjn.gob.mx";
        public const String TITULO_NO_SE_PUEDE_VER_CORTE = "Restricción de Seguridad en versión Web";
        public const String C_AYUDA = "Para aplicación web la ayuda debe abrirse desde la liga en la página que contiene la aplicación";
        public const String TITULO_AYUDA = "Ayuda en línea";
        public const String MENSAJE_ENVIAR_AL_PORTAPAPELES = "Para enviar al portapapeles: "
               + "\n - Haga clic con el botón derecho del mouse y elija la opción seleccionar todo"
               + "\n - Haga clic con el botón derecho del mouse en cualquier parte del texto seleccionado"
               + "\n - Seleccione la opción copiar";

        public const String MENSAJE_IMPRIMIR_SEMBLANZA = "Para imprimir haga clic con el botón derecho del mouse y seleccione la opción imprimir";

        public const String TITULO_AVISO = "Aviso";
        public const String MENSAJE_SIN_DATOS = "Sin información";
    }
}

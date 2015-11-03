﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.directorio
{
    public class Mensajes
    {
        public const String MENSAJE_NO_SE_PUEDE_VER_CORTE = "Ya que usted está usando la versión Web del programa este no puede abrir la página de la corte, por favor use la barra de dirección de su navegador y navegue a www.scjn.gob.mx";
        public const String TITULO_NO_SE_PUEDE_VER_CORTE = "Restricción de Seguridad en versión Web";
        public const String MENSAJE_BASE_DATOS_NO_ACCESS = "La base de datos configurada no es Access, se intentará conectar a un servidor SQL";
        public const String TITULO_BASE_DATOS_NO_ACCESS = "Base de datos SQLServer";
        public const String MENSAJE_BASE_DATOS_NO_LISTA = "La base de datos no está disponible";
        public const String TITULO_BASE_DATOS_NO_LISTA = "Base de datos fuera de ruta";
        public const String MENSAJE_INTENTE_DE_NUEVO = "Reintente la acción, el servicio de IUS está ocupado";
        public const String TITULO_INTENTE_DE_NUEVO = "Intente de nuevo";
        public const String MENSAJE_BUSQUEDA_ELIMINADA = "La búsqueda se eliminó de la BD";
        public const String TITULO_BUSQUEDA_ELIMINADA = "Busqueda Eliminada";
        public const String MENSAJE_BUSQUEDA_SIN_EXPRESIONES = "La búsqueda que seleccionó no contiene expresiones";
        public const String TITULO_BUSQUEDA_SIN_EXPRESIONES = "Búsqueda sin expresiones";
        public const String MENSAJE_BUSQUEDA_GUARDADA = "Busqueda guardada dentro de Búsquedas Almacenadas";
        public const String TITULO_BUSQUEDA_GUARDADA = "Busqueda Almacenada";
        public const String MENSAJE_MUCHOS_REGISTROS = "El número de registros es muy grande. Su impresión será de más de 100 páginas y algunas computadoras con poca memoria podrían tener problemas para generarla. ¿Desea Continuar?";
        public const String TITULO_MUCHOS_REGISTROS = "Registros a imprimir";
        public const String MENSAJE_GUARDAR_ID = "¿Desea Guardar los registros?";
        public const String TITULO_GUARDAR_ID = "Datos para almacenar";
        public const String MENSAJE_GUARDADO = "El archivo fue guardado como: ";
        public const string TITULO_GUARDADO = "Archivo guardado";
        public const String MENSAJE_CAMPO_TEXTO_VACIO = "Debe introducir una expresión para consultar";
        public const String MENSAJE_INCORPORAR_PALABRA = "¿Desea incorporar la expresión actual a la búsqueda?";
        public const String TITULO_INCORPORAR = "Palabra para incorporar";
        public const String TITULO_CAMPO_TEXTO_VACIO = "Campo Vacio";
        public const String MENSAJE_CAMPO_NO_SELECCIONADO = "Se debe seleccionar una sección para consulta";
        public const String TITULO_CAMPO_NO_SELECCIONADO = "No se seleccionaron campos";
        public const String MENSAJE_TODOS_PORTAPAPELES = "Se han marcado ";
        public const String MENSAJE_TODOS_PROTAPAPELES2 = " registros.";
        public const String MENSAJE_DESMARCAR_TODO = "¿Está seguro de desmarcar todas las tesis seleccionadas?";
        public const String MENSAJE_SIN_MARCAS = "No existen registros marcados.";
        public const String TITULO_DESMARCAR_TODO = "Desmarcar todo";
        public const String TOOLTIP_SIN_MARCAR = "Marcar Registro";
        public const String TOOLTIP_MARCADO = "Desmarcar Registro";
        public const String TITULO_TODOS_PORTAPAPELES = "Marcar para salvar e imprimir";
        public const String MENSAJE_ENVIADO_PORTAPAPELES = "Documento enviado al portapapeles.";
        public const String TITULO_ENVIADO_PORTAPAPELES = "Tarea Realizada";
        public const String MENSAJE_RANGO_MUCHOS_SELECCIONADOS = "La diferencia de rangos es mayor a la cantidad de registros que le falta por almacenar que es de ";
        public const String MENSAJE_RANGO_MUY_ALTO = "El número final no puede ser mayor al último registro que es el ";
        public const String MENSAJE_RANGO_FINAL_MENOR = "El número inicial no puede ser mayor al final";
        public const String MENSAJE_RANGO_INICIAL_VACIO = "El rango inicial está vacio.";
        public const String TITULO_RANGO = "Error en el rango seleccionado";
        public const String MENSAJE_CASILLAS_SIN_SELECCIONAR = "Se debe seleccionar sólo una casilla";
        public const String TITULO_CASILLAS_SIN_SELECCIONAR = "Advertencia";
        public const String MENSAJE_NO_HAY_REGISTROS = "No existen registros en la lista de la búsqueda, indique el registro y oprima >>>";
        public const String TITULO_NO_HAY_REGISTROS = "Sin registros para la búsqueda";
        public const String MENSAJE_SELECIONE_UNA_CASILLA = "Debe seleccionar al menos una casilla de consulta";
        public const String TITULO__SELECIONE_UNA_CASILLA = "Datos Insuficientes";
        public const String MENSAJE_CONSECUTIVO_NO_VALIDO = "El consecutivo seleccionado no es válido, por favor verifique  sus datos.";
        public const String TITULO_CONSECUTIVO_NO_VALIDO = "Consecutivo Inválido";
        public const String MENSAJE_REGISTRO_NO_VALIDO = "El registro seleccionado no es válido, por favor verifique  sus datos.";
        public const String TITULO_REGISTRO_NO_VALIDO = "Registro Inválido";
        public const String MENSAJE_BUSQUEDA_VACIA = "La búsqueda no produjo ningún registro. Intente de nuevo";
        public const String TITULO_BUSQUEDA_VACIA = "Búsqueda sin Resultados.";
        public const String CAMPO_NOMBRE = "Nombre";
        public const String CAMPO_APELLIDOS = "Apellidos";
        public const String CAMPO_USUARIO = "Usuario";
        public const String CAMPO_CORREO_ELECTRONICO = "Correo Electrónico";
        public const String CAMPO_PASSWORD = "Contraseña";
        public const String MENSAJE_ERROR_LIGA_1 = "Liga mal formada, favor de reportar al soporte IUS: ";
        public const String MENSAJE_ERROR_LIGA_2 = "\nValores de los servicios: ";
        public const String MENSAJE_CAMPO_REQUERIDO = "No ha llenado todos los campos requeridos, por favor revise la información";
        public const String TITULO_CAMPO_REQUERIDO = "Falta llenar campos: ";
        public const String MENSAJE_PROBLEMAS_FACHADA = "Ocurrió un problema en las comunicaciones, por favor verifique que tenga acceso a los servicios del IUS. Para Información contacte a soporteIUS@mail.scjn.gob.mx";
        public const String TITULO_PROBLEMAS_FACHADA = "Problemas de comunicación";
        public const String MENSAJE_USUARIO_EXISTENTE = "El usuario que seleccionó ya existe, por favor escoja otro nombre de usuario";
        public const String TITULO_USUARIO_EXISTENTE ="Usuario existente";
        public const String MENSAJE_USUARIO_ALTA_EXITOSA = "El usuario fue dado de alta y será utilizado para sus búsquedas almacenadas, por favor memorice el usuario y contraseñas seleccionados para su uso futuro.";
        public const String TITULO_USUARIO_ALTA_EXITOSA ="Alta exitosa";
        public const String MENSAJE_NO_COINCIDEN_CONTRASENAS = "La contraseña y su confirmación no coinciden, por favor verifique los datos";
        public const String TITULO_NO_COINCIDEN_CONTRASENAS = "Verifique la información";
        public const String MENSAJE_NO_PERMITIDOS = "La cadena posee caracteres no permitidos";
        public const String TITULO_NO_PERMITIDOS = "Existe un caracter no permitido";
        public const String MENSAJE_COMILLAS_IMPARES = "El número de comillas en su expresión es impar, por favor verifique que esté bien escrita.";
        public const String TITULO_COMILLAS_IMPARES = "Número de comillas impar.";
        public const String MENSAJE_VALOR_LOGICO_INICIAL = "La cadena no puede empezar con Y, O o N";
        public const String TITULO_VALOR_LOGICO_INICIAL = "Comparador lógico al inicio";
        public const String MENSAJE_VALOR_LOGICO_FINAL = "La cadena no puede terminar con Y, O o N";
        public const String TITULO_VALOR_LOGICO_FINAL = "Comparador lógico al final";
        public const String MENSAJE_ASTERISCO_FRASE = "El comodín no puede colocarse enmedio de una frase";
        public const String TITULO_ASTERISCO_FRASE = "Asterísco en una frase";
        public const String MENSAJE_ASTERISCO_INICIO_PALABRA = "El comodin no puede colocarse al inicio de una palabra";
        public const String TITULO_ASTERISCO_INICIO_PALABRA = "Asterísco al principio de una palabra";
        public const String MENSAJE_USUARIO_PASS_EQUIVOCADO = "Usuario o contraseña equivocados, por favor verifíquelos.";
        public const String TITULO_USUARIO_PASS_EQUIVOCADO = "Datos incorrectos";
        public const String MENSAJE_FALTA_NUMERO_REGISTRO = "Debe Introducir un Numero de Regstro";
        public const String TITULO_FALTA_NUMERO_REGISTRO = "Campo vacio";
        public const String MENSAJE_NO_RESULTADO_TEMATICA = "No se encontraron temas con testa condición";
        public const String TITULO_NO_RESULTADO_TEMATICA = "Sin temas";
    }
}

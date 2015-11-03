using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.utils
{
    public class CalculosPropiedadesGlobales:DependencyObject
    {
#if STAND_ALONE
        public static bool PropiedadAcuLoc { get { return getAcuLoc(); } set { setAcuLoc(); } }

        private static void setAcuLoc()
        {

        }
        private static bool getAcuLoc()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.ACUE_LOC).Equals("1");
        }
        public static bool PropiedadAcuRubro { get { return getAcuRubro(); } set { } }
        private static bool getAcuRubro()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.ACUE_TEM).Equals("1");
        }
        public static bool PropiedadAcuTexto { get { return getAcuTexto(); } set { } }
        private static bool getAcuTexto()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.ACUE_TEX).Equals("1");
        }



        public static bool PropiedadEjeLoc { get { return getEjeLoc(); } set { } }
        private static bool getEjeLoc()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.EJE_PREC).Equals("1");
        }
        public static bool PropiedadEjeRubro { get { return getEjeRubro(); } set { } }
        private static bool getEjeRubro()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.EJE_TEMA).Equals("1");
        }
        public static bool PropiedadEjeTexto { get { return getEjeTexto(); } set { } }
        private static bool getEjeTexto()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.EJE_TEXTO).Equals("1");
        }
        public static bool PropiedadEjeAsunto { get { return getEjeAsunto(); } set { } }
        private static bool getEjeAsunto()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.EJE_ASUNTO).Equals("1");
        }



        public static bool PropiedadLoc { get { return getLoc(); } set { } }
        private static bool getLoc()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.TESIS_LOC).Equals("1");
        }
        public static bool PropiedadRubro { get { return getRubro(); } set { } }
        private static bool getRubro()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.TESIS_RUBRO).Equals("1");
        }
        public static bool PropiedadTexto { get { return getTexto(); } set { } }
        private static bool getTexto()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.TESIS_TEXTO).Equals("1");
        }
        public static bool PropiedadPrec { get { return getPrec(); } set { } }
        private static bool getPrec()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.TESIS_PREC).Equals("1");
        }



        public static bool PropiedadVotoLoc { get { return getVotoLoc(); } set { } }
        private static bool getVotoLoc()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.VOTO_LOC).Equals("1");
        }
        public static bool PropiedadVotoEmi { get { return getVotoEmi(); } set { } }
        private static bool getVotoEmi()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.VOTO_EMI).Equals("1");
        }
        public static bool PropiedadVotoTexto { get { return getVotoTexto(); } set { } }
        private static bool getVotoTexto()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.VOTO_TEX).Equals("1");
        }
        public static bool PropiedadVotoAsu { get { return getVotoAsu(); } set { } }
        private static bool getVotoAsu()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            return reg.GetValue(IUSConstants.VOTO_ASU).Equals("1");
        }
#else
        public static readonly bool PropiedadAcuLoc = false;
        public static readonly bool PropiedadAcuRubro = true;
        public static readonly bool PropiedadAcuTexto = true;
#endif
        public static double FontSize = Constants.FONTSIZE;
        public int RowHeight { get { return (int)GetValue(RowHeightProperty); } set { if (RowHeight > 30) { SetValue(RowHeightProperty, value); } } }
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(CalculosPropiedadesGlobales), new UIPropertyMetadata(99));
    }
}

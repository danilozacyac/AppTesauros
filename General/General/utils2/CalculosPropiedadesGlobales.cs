using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.utils;
using System.Windows;
using System.Windows.Controls;

namespace mx.gob.scjn.ius_common.gui.utils
{
    public class CalculosPropiedadesGlobales:DependencyObject
    {
        public static double FontSize = 10;
        public int RowHeight { get { return (int)GetValue(RowHeightProperty); } set { if (RowHeight > 30) { SetValue(RowHeightProperty, value); } } }
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(CalculosPropiedadesGlobales), new UIPropertyMetadata(99));
    }
}

   M S B A M L   `   `   `  ����B  >General, Version=1.5.0.0, Culture=neutral, PublicKeyToken=nulltGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities  5      F BPresenacion, Version=1.5.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos!mx.gob.scjn.ius_common.gui.apoyos 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      clr-namespace:IUSIUS 5         IUS.tablaResultadoAcuerdo   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      WutilsGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    5      ?local/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos    5      ` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h	 dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l
 hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid  	 
   5      7my&clr-namespace:System;assembly=mscorlib  	 
   5      #iusclr-namespace:IUS  	 
   5      -   5          N� WindowTitle$#  Visualizar registros acuerdos��5	       � 	Resources 5
       �� Source�� 5   
   $' !/General;component/Resources.xaml?�6   %    .��5      ��    BorderStyle#��[  6   ._�5   
   �  � ColumnDefinitions 5      �� 5      $��10��6#   $��10 �61   �� 5      $��42��6#   $��42 �61   �� 5      $	��250��6#   $	��250 �62   �� 5      $��* �6#   �� 5      $��42��6#   $��42 �61   �� 5      $��10��6#   $��10 �61    � RowDefinitions 5      �� 5      $��40 �6    �� 5      $��42 �6    �� 5      $
��250* �6    �� 5      $��27 �6    �� 5      $��10 �6    .+�5      �� -   5        $��Titulo��5        
LabelStyle#��[ 6"   $��3��5      $��0,0,0,0q�5       .��6%   Resultado de la búsqueda6?   � 5!      $��3��6   $��0��6#    5"      �� 5#      $��40 �6'   �� 5$      $��40 �6'   �� 5%      $��40 �6'   �� 5&      $��40 �6'   .+�5(      �� -   5        $��inicio��5(   )   $��0��6    � ToolTip$ Ir al primer registro��5)        InicioBoton#��[ 5+      $��Right=�5,      $��0,0,0,0q�65   �� 5-      -   5        $��anterior��5-      $��1��6)   $ Registro anterior��5/        AnteriorBoton#��[ 50      $��0,0,0,0q�51      $��2��6*   �� 52      -   5        $��	siguiente��52      $ Siguiente registro��54        SiguienteBoton#��[ 55      $��2��56      $��0,0,0,0q�6)   �� 57      -   5        $��final��57   )   $��3��6   $ Ir al último registro��59        
FinalBoton#��[ 5:      $��0,0,0,0q�5;      5<      �� 5=      -   5        $��BtnAlmacenar��5>      $��3��5=      $y�Hidden=�6%   $ Almacenar expresión��5?      $��
160,0,40,0q�5@      $
��Left=�5A        	BuroBoton#��[ 5C      �� 5D      -   5        $��
BtnGuardar��5D      $+ %Guardar información en un archivo...��5E      $��2��5F      $��40��6%   $��40��60     GuardarBoton#��[ 5I      y�5H      ��   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet�5I      �� 5J      -	   5        $��OrdenarPorImage��5K      $��3��5J      $��
200,0,40,0q�5L      $ Ordenar por...��5M      $
��Left=�5O       	 OrdenarBoton#��[	 5P      �� 5Q      -
   5        $��lblIrA��5T      $��3��5Q      #��[ 6$   $��Center=�5R      $��	240,0,0,0q�5S      .��5U      �� $��WrapWithOverflow=�6   Ir a Consecutivo:6O   5V      '  !Xceed.Wpf.Controls.NumericTextBox  5W      -   5        $��IrANum��5[   &   $��3��5W   "   $��25��62   $��	335,0,0,0q�5X   &      NumberStyles$
 None=�69   $
��Left=�5Y   "   $��80��5[   4   *  $Xceed.Wpf.Controls.ValueRangeTextBox	   ValueDataType"	 ��5\   &   
 �� 	MaxLength$
 8��6?   �� 5]      -   5        $��btnIrA��5b      $��3��5]      $
��Left=�6%   $ Ir al consecutivo��5^      $��30��65   $��	415,0,0,0q�5_      
�������
�������$��40��5b   '   
���  ��.��5d   -   Ir61   �� 5f      -   5        $��imprimir��5f   2   $��0,0,0,0q�6   $��40��6&   $��1��5g      $��1��5h      $��40��5i      $( "Ver vista preliminar de impresión��6     
 ImprimeBoton#��[
 5j      5k      �� 5l      -   5        $��BtnVisualizar��5l      $��1��5m      $��3��6"   $ 
Visualizar��5n        VisualizarBoton#��[ 5o      $��Right=�5p      $��40��61   $��40��6<   5q      �� 5r      -   5        $��BtnAumentaAlto��5r      $��3��6+   $��1��6;   
���  ��
�������
�������$��25��5w      $
��80,0q�6!   $��20��6/   $# Aumenta el alto de las celdas��5x      $��Right=�5y   2   .��6N   +6Q   �� 5z      -   5        $��BtnDisminuyeAlto��5z      $��3��6-   $��1��6=   $��25��5{      $
��50,0q�6!   $��20��6/   $��Right=�6;   
���  ��
�������
�������$% Disminuye el alto de las celdas��5�      .��5�   4   -67   �� 5�      -   5        $��salir��5�      $��1��5�      $��4��5�      $% Regresar a la pantalla anterior��5�        RegresarBoton#��[ 5�      $��0,0,0,0q�5�      5�      �� 5�      -   5        $��RegistrosLabel��5�      $��1��5�      #��[ 5�      $��2��5�      $��14(�6$   $
��Bold"�62   .��5�   *   
Registros:66   � 5�      $��0,0,0,0q�6   $��2��5�      $��1��5�      $��4��5�       5�      �� 5�      $��* �6$    5�      �� 5�      .+�5�      (  "Xceed.Wpf.DataGrid.DataGridControl  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid  	 
   5�   '   -   5        $��tablaResultado��5�   '   $��0,0,0,0q�5�   '   $��0��5�   '   $��0��5�   '   
�������$	��200��5�   '   $	��500��5�   '   $	��800��5�   '      ReadOnly .    EditTriggers$
 None=�5�   '      NavigationBehavior$ RowOnly=�5�   '      SelectionMode$ Single=�5�         AutoCreateColumns .     Columns 5�        Xceed.Wpf.DataGrid.Column  5�         	FieldName$ Tesis��6&      IsMainColumn . -   5        TituloAcuerdo��   Title$ Acuerdo��5�         Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 100 6.      TextTrimming$ WordEllipsis=�5�         TextWrapping$ WrapWithOverflow=�5�         ReadOnly . 6>     5�      $ Rubro��6&    . $ WordEllipsis=�5�      $ WrapWithOverflow=�5�      $	 400 5�   &   $
 Tema��5�        5�      $	 Loc��6&    . $ WordEllipsis=�5�      $ WrapWithOverflow=�5�      $ Localización��5�        5�      $ Id��6&   $ Registro��65      View 5�      (  "Xceed.Wpf.DataGrid.Views.TableView  5�      '  !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     RowSelectorPaneWidth$ 50Q�6J      FixedHeaders 5�      �� 5�   "   .R�5�   &   )  #Xceed.Wpf.DataGrid.ColumnManagerRow  $��Stretch=�6<      	AllowSort .     AllowColumnReorder . $	��Top=�5�   4   5�   #      Theme 5�      3	  -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme	  5�   "   5�      ��5�      �� 5�       �� 
TargetType 
  Xceed.Wpf.DataGrid.DataRow" 
 6    .�5�      �� $  Xceed.Wpf.DataGrid.RowSelector  �� Property  �@ RowSelectorStyle  itemIndexSelectorStyle! �� Value#! [ 5�      �� 5�        �@ RowSelectorStyle#! [ 5�   !   �� 5�        � ��! 5�      �� " �� RelativeSource" �� # �� AncestorType  IUS.tablaResultadoAcuerdo"#  $ 	RowHeight�$ �� Mode$$ TwoWay=��� 5�        � ��$! 15��6G   -   5        5�      5�      .� 5�      -   5        $��impresionViewer��5�   )     FDPVTemplate#��[ 5�   )   $y�Hidden=�5�   )   
�������$��0,0,0,40q�5�   )   $��0��5�   )   $��0��5�   )   �� 5�      -   5        $��imprimePapel��5�      $	��0,0q�5�      $y�Hidden=�5�        ImprimePapelBoton#��[ 5�      $��40��5�      $��40��6&   $��3��62   $
��Left=�5�      $	��Top=�5�      $ Imprimir el documento��5�      5�      �� 5�      -   5        $��BtnTache��5�      $y�Hidden=�6)   $
��40,0q�5�      $��40��5�      $��40��6%   $
��Left=�5�      $	��Top=�5�      $ Cerrar vista preliminar��5�        
TacheBoton#��[ 5�      5�      5�      �� 5�      .+�5�      :  4mx.gob.scjn.ius_common.gui.apoyos.AcuerdosOrdenarPor  -   5        %  Name$% 
PnlOrdenar��5�   +   & �� Left$& 50��5�   +   
' �� Top$' 50��5�   +   $y�Hidden=�5�   +   5�   +   8  2mx.gob.scjn.ius_common.gui.apoyos.AlmacenaBusqueda  5�      -   5        (  Name$( PnlAlmacenar��5�   )   $& 50��5�   )   $' 50��5�   )   $y�Hidden=�5�   )   5�   )   5�      �� 5�      -   5        $��	Expresion��5�      $��2��5�      $��3��6$   $��2��61   
�������
���   �$��2q�5�      �� 5�      -   5        $��Esperar��5�      $��0��5�      $��20��5�      $��0��5      $��10��5     
���   �) J� Opacity$	) 0.7Q�5     $y�Hidden=�5     .+�5     �� -   5        $��EsperarStack��5     
�������$��150,150,0,0q�5     $) 3Q�5     .+�5	     �� Espere por favor62   � 5
     -   5        $��EsperaBarra��5  "   $��0,0,0,0q�5
  "   $��26��5  "   $	��393��5  "   5     5     �� 5     -   5        $��DocumentoGuardar��5     $y�Hidden=�62   5     5     5     
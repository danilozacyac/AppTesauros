   M S B A M L   `   `   `  ���� P  LElectoral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ae5cf9d2702abc06=#clr-namespace:mx.gob.scjn.electoralmx.gob.scjn.electoral  N JGeneral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=c866414512d5116dtGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities O Kmscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem _ [JaStDev.ControlFramework, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3a25f60670705855wQclr-namespace:JaStDev.ControlFramework.Controls;assembly=JaStDev.ControlFramework!JaStDev.ControlFramework.Controls +    %mx.gob.scjn.electoral.TablaResultados   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    1ius#clr-namespace:mx.gob.scjn.electoral    l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6`	 \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h
 dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   	 
  8x,http://schemas.microsoft.com/winfx/2006/xaml    WutilsGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    3my&clr-namespace:System;assembly=mscorlib    `helpQclr-namespace:JaStDev.ControlFramework.Controls;assembly=JaStDev.ControlFramework    -     N� WindowTitle$   Visualizar registros tesis��,  &JaStDev.ControlFramework.Controls.Help   HelpFile$ IUS.chm��   Topic$  Visualizacion_de_las_tesis��}�.   � 	Resources  �� MergedDictionaries �� Source��  �� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��   BorderStyle#��[  ._�� =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   	 
   � ColumnDefinitions �� $��10��$��10 ��� $��40��$��40 ��� $��40��$��40 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $��40 ��� $��* ��� $��40��$��40 ��� $��40 ��� $��80 ��� $��10��$��10 � � RowDefinitions �� $��30 ��� $��40 ��� $��20 ��� $��* ��� $��30 ��� $��10 �.+�� $��4��$��0�� �� $	��12* ��� $	��18* � �� �� �� �� .+��� -   $��anterior�� � ToolTip$ Registro anterior��  AnteriorBoton#��[ $��-6,0,1,0q�$��2���� -   $��	siguiente��$ Siguiente registro��  SiguienteBoton#��[ $��2��$��0,0,20,0q�$��2��$��1���� -   $��final��$��2��$ Ir al último registro��  
FinalBoton#��[ $��9,0,11,0q�$��2��$��2���� -   $��Titulo��$��0��$��4��  UIStyle#��[ $��0��.��Resultado de la búsqueda�� -   $��imprimir��$��1��$��1��$��0,0,0,0q�$��40��$( "Ver vista preliminar de impresión��  ImprimeBoton#��[ �� -   $��Guardar��$��9��$��1��  GuardarBoton#��[ $&  Guardar resultado de la consulta��y���   Visibilidad	 �� Source#	 [ 
 �� Path$
 ObtenVisibilidadInternet��� -	   $��	Contenido��$y�Hidden=��� -
   $��salir��$��12��$��1��$��0,0,0,0q�  RegresarBoton#��[ $% Regresar a la pantalla anterior���� -   $��RegistrosLabel��$��5��#��[ $��14(�$
��Bold"�$��2��.��
Registros:( 	 "Xceed.Wpf.DataGrid.DataGridControl  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   	 
  -   $��tablaResultados��$��3��$��14��$��0,0,0,0q�
�������   SelectedIndex$ 0��   AutoCreateColumns .     ReadOnly . $	��100�� �� IsTextSearchEnabled . $
��Left=�   NavigationBehavior$ RowOnly=�   SelectionMode$ Single=�
�������   AllowDetailToggle .     Columns  	 Xceed.Wpf.DataGrid.Column     	FieldName$ Tesis��   ReadOnly .    TextWrapping$ WrapWithOverflow=�   Title$ Tesis��   Width8 	 2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$ 75   $ Rubro�� .    TextTrimming$ WordEllipsis=�$ WrapWithOverflow=�$ Rubro��$	 260   $ LocAbr�� . $ Localización��$ CharacterEllipsis=�$ WrapWithOverflow=�  $
 Sala�� . $ 	Instancia��$ 60   $	 Ius��   IsMainColumn .  . $ Registro��$ 50    View ( 	 "Xceed.Wpf.DataGrid.Views.TableView  -     Name$ 	tableView��' 	 !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     RowSelectorPaneWidth$ 50Q�   FixedHeaders �� .R�) 	 #Xceed.Wpf.DataGrid.ColumnManagerRow  $��Stretch=�   	AllowSort .      AllowColumnReorder  . $	��Top=�!   Theme! 3 	 -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme  ���� .�-   �� $	 	 Xceed.Wpf.DataGrid.RowSelector" �� Property" �@	 RowSelectorStyle 	 itemIndexSelectorStyle# �� Value## [	 �� " � ��# �� $ �� RelativeSource$ �� % �� AncestorType+
   %mx.gob.scjn.electoral.TablaResultados"% 
 $
 	RowHeight�& �� Mode$& TwoWay=��� " � ��$# 15��' �� Triggers' �� ( �� Binding( �� $
 IsJuris�) �� Value) true.a��� " � ��$
# Bold"�.� -   $��impresionViewer��$y�Hidden=�
�������
���   � 
 FDPVTemplate#��[
 $��1��$��3��$��10��$��0,0,0,0q��� -   $��imprimePapel��$��1��$��0,0,0,0q�$y�Hidden=�$ Imprimir el documento��$��40��  ImprimePapelBoton#��[ $��40��$��3��$	��Top=��� -   $��BtnTache��$��2��$��4��$��3��$y�Hidden=�$	��0,0q�$ Cerrar vista preliminar��$
��Left=�$��40��$��40��$	��Top=�  
TacheBoton#��[ �� -   $��inicio��$��3��$ Ir al primer registro��  InicioBoton#��[ $��Right=�$��30��$��0,0,6,0q��� -   $��lblIrA��$��8��$��2��$
��Left=�$��0,0,0,0q�$��80��$��2��$	��Top=�#��[ .���� $��WrapWithOverflow=�$��2��Ir a consecutivo:'  !Xceed.Wpf.Controls.NumericTextBox  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   	 
  -   $��IrANum��$��9��$��2��$��0,0,0,0q�* �� 	MaxLength$* 6��*  $Xceed.Wpf.Controls.ValueRangeTextBox+   ValueDataType"+ ��,   NumberStyles$
, None=��� -   $��btnIrA��$��11��$��30��$��0,0,0,0q�
�������
�������$ Ir a número consecutivo��
���  ��.��Ir�� -   $��BtnVisualizar��$��11��$��1��$ 
Visualizar��  VisualizarBoton#��[ �� -   $��BtnAumentaAlto��$��11��$��2��
���  ��
�������
�������$# Aumenta el alto de las celdas��.��+�� -   $��BtnDisminuyeAlto��$��12��$��2��
���  ��
�������
�������$% Disminuye el alto de las celdas��.��-�� -   $��idExpresion��#��[ $��1��$��4��$��2��.��Expresión:�� -   $��LblExpresion��
�������
���   �$��2q�$��3��$��4��$��0,0,0,0q���. $
��Auto=�$
��Auto=�$��6��.���� .���� -   $��	Expresion��$
��Auto=�$
��Auto=���. $��WrapWithOverflow=�$��30��� -   $��PgbTotalTesis��$��10��$��4��$��4��$��15���� -   $��Esperar��$��0��$��20��$��0��$��10��
���   �- J� Opacity$	- 0.8Q�$y�Hidden=�.+��� -   $��EsperarStack��
�������$��150,150,0,0q�$- 3Q�.+��� -   $��
TxbEsperar��Espere por favor� -    $��EsperaBarra��$��0,0,0,0q�$��26��$	��393���� -!   $��Cancelar��.��
Cancelar
   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  N JGeneral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=c866414512d5116dtGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities P LElectoral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ae5cf9d2702abc06a5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos'mx.gob.scjn.electoral_common.gui.apoyos =#clr-namespace:mx.gob.scjn.electoralmx.gob.scjn.electoral 4   .mx.gob.scjn.electoral.TablaResultadoEjecutoria   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    3my&clr-namespace:System;assembly=mscorlib    l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h	 dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l
 hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid    	 
 [utilsGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    	 
 Ilocal5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos    	 
 5ius#clr-namespace:mx.gob.scjn.electoral    	 
 -     N� WindowTitle$%  Visualizar registros sentencias�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._�� $��Center=� � ColumnDefinitions �� $��10��$��10 ��� $��42��$��42 ��� $	��250��$	��250 ��� $	��80* ��� $��42��$��42 ��� $��10��$��10 � � RowDefinitions �� $��40 ��� $��42 ��� $
��250* ��� $��10 �.+�� $��0��$��0��$��6�� �� $��10 ��� $	��150 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��70 ��� $��70 ��� $��40 �.+��� -   $��Titulo��$��1��$��3��  UIStyle#��[ $��0,0,0,0q�.��Resultado de la búsqueda�� -   $��inicio��$��2�� � ToolTip$ Ir al primer registro��  InicioBoton#��[ $��Right=�$��0,0,0,0q��� -   $��anterior��$ Registro anterior��  AnteriorBoton#��[ $��0,0,0,0q�$��3���� -   $��	siguiente��$ Siguiente registro��  SiguienteBoton#��[ $��4��$��0,0,0,0q��� -   $��final��$��5��$ Ir al último registro��  
FinalBoton#��[ $��0,0,0,0q��� -   $��lblIrA��$��8��$��0,0,0,0q�#��[ .���� $��WrapWithOverflow=�Ir a consecutivo:'  !Xceed.Wpf.Controls.NumericTextBox  -   $��IrANum��$��9��$��0,0,0,0q�$��25��   NumberStyles$
 None=�*  $Xceed.Wpf.Controls.ValueRangeTextBox   ValueDataType" �� �� 	MaxLength$ 8���� -	   $��btnIrA��$��10��$��0,0,0,0q�$ Ir a número consecutivo��
�������
�������
���  ��.��Ir�� -
   $��imprimir��$	��0,1q�$( "Ver vista preliminar de impresión��$��1��$��1��  ImprimeBoton#��[ �� -   $��
BtnGuardar��$��1��$��3��  GuardarBoton#��[ $��40��$��40��$��140,0q�$	��Top=�$��Right=�$ Guardar��y���   Visibilidad	 �� Source#	 [ 
 �� Path$
 ObtenVisibilidadInternet��� -   $��	Contenido��$y�Hidden=��� -   $��BtnVisualizar��$��1��$��3�� 	 VisualizarBoton#��[	 $��40��$��40��$��100,0q�$	��Top=�$��Right=�$ 
Visualizar���� -   $��BtnAumentaAlto��$��3��$��1��$��20��$��30��$
��50,0q�$��Right=�
���  ��
�������
�������$# Aumenta el alto de las celdas��.��+�� -   $��BtnDisminuyeAlto��$��3��$��1��$��20��$��30��$
��20,0q�$��Right=�
���  ��
�������
�������$% Disminuye el alto de las celdas��.��-�� -   $��salir��$% Regresar a la pantalla anterior��$��1��$��4�� 
 RegresarBoton#��[
 $��0,0,0,0q��� -   $��RegistrosLabel��$��1��$��2��$��14(�$
��Bold"�#��[ .��
Registros:� $��0,0,0,0q�$��2��$��1��$��4�� ��  �� .+�(  "Xceed.Wpf.DataGrid.DataGridControl  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid    	 
 -   $��tablaResultado��$��0,0,0,40q�$��0��$��0��
�������$	��200��$	��500��$	��800��   ReadOnly .    EditTriggers$
 None=�   NavigationBehavior$ RowOnly=�   SelectionMode$ Single=�   AutoCreateColumns .     Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ 	TpoAsunto��   IsMainColumn .    Title$ 
Expediente��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 300    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�   ReadOnly .   $	 Loc�� . $ WordEllipsis=�$ WrapWithOverflow=�$ Localización��  $ Id��$ Registro��   View (  "Xceed.Wpf.DataGrid.Views.TableView  '  !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     RowSelectorPaneWidth$ 50Q�   ShowRowSelectorPane .    ShowFixedColumnSplitter .     FixedHeaders �� .R�)  #Xceed.Wpf.DataGrid.ColumnManagerRow  $��Stretch=�   	AllowSort .     AllowColumnReorder . $	��Top=�    Theme  3	  -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme	  ���� ! �� 
TargetType 
  Xceed.Wpf.DataGrid.DataRow"! 
 .��� $  Xceed.Wpf.DataGrid.RowSelector" �� Property" �@ RowSelectorStyle  itemIndexSelectorStyle# �� Value## [ �� " �@ RowSelectorStyle## [ �� " � ��# �� $ �� RelativeSource$ �� % �� AncestorType4  .mx.gob.scjn.electoral.TablaResultadoEjecutoria"%  $
 	RowHeight�& �� Mode$& TwoWay=��� " � ��$# 15��-   .� -   $��impresionViewer��$y�Hidden=�  FDPVTemplate#��[ 
�������$��0,0,0,40q�$��0��$��0���� -   $��imprimePapel��$��0��$��0,0,0,0q�$y�Hidden=�$ Imprimir el documento��  ImprimePapelBoton#��[ $��40��$��40��$��3��$
��Left=�$	��Top=��� -   $��BtnTache��$��1��$y�Hidden=�$ Cerrar vista preliminar��$
��40,0q�  
TacheBoton#��[ $��40��$��40��$
��Left=�$	��Top=��� -   $��	Expresion��$��27��$��0,0,0,6q�{�.  $��Bottom=��� .+�C  =mx.gob.scjn.electoral_common.gui.apoyos.EjecutoriasOrdenarPor  -   '  Name$' ListaOrdenar��( �� Left$	( 100��
) �� Top$	) 100��$y�Hidden=��� -   $��Esperar��$��0��$��20��$��0��$��10��
���   �* J� Opacity$	* 0.8Q�$y�Hidden=�.+��� -   $��EsperarStack��
�������$��150,150,0,0q�$* 3Q�.+��� -   $��	TxbEspere��Espere por favor� -   $��EsperaBarra��$��0,0,0,0q�$��26��$	��393���� -   $��BtnCancelar��.��
Cancelar
   M S B A M L   `   `   `  ���� N  JGeneral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=c866414512d5116dtGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities  O Kmscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem P LElectoral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ae5cf9d2702abc06a5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos'mx.gob.scjn.electoral_common.gui.apoyos =#clr-namespace:mx.gob.scjn.electoralmx.gob.scjn.electoral /   )mx.gob.scjn.electoral.TablaResultadoVotos   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    WutilsGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h	 dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l
 hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid    	 
 7my&clr-namespace:System;assembly=mscorlib    	 
 Ilocal5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos    	 
 5ius#clr-namespace:mx.gob.scjn.electoral    	 
 -     N� WindowTitle$   Visualizar registros votos�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._�� $��0,0,0,0q� � ColumnDefinitions �� $��10 �$��10���� $��40 �$��40���� $	��100 �$	��100���� $��* ��� $��40 ��� $��40 ��� $��10 �$��10�� � RowDefinitions �� $��40 ��� $��40 ��� $��* ��� $��10 �.+�� $��0��$��0��$��6�� �� $��10 ��� $	��150 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $	��120 ��� $��80 ��� $	��100 ��� $��40 �.+��� -   $��Titulo��$��1��$��3��  UIStyle#��[ $��0,0,0,0q�.��Resultado de la búsqueda�� -   $��inicio��$��2�� � ToolTip$ Ir al primer registro��  InicioBoton#��[ $��Right=�$��0,0,0,0q��� -   $��anterior��$ Registro anterior��  AnteriorBoton#��[ $��0,0,0,0q�$��3���� -   $��	siguiente��$ Siguiente registro��  SiguienteBoton#��[ $��4��$��0,0,0,0q��� -   $��final��$��5��$ Ir al último registro��  
FinalBoton#��[ $��0,0,0,0q��� -   $��RegistrosLabel��#��[ $��6��$��0,0,0,0q�$��14(�$
��Bold"�.��
Registros:�� -   $��lblIrA��$��7��$��0,0,0,0q�#��[ .���� $��WrapWithOverflow=�Ir a consecutivo:'  !Xceed.Wpf.Controls.NumericTextBox  -	   $��IrANum��$��8��$��25��$��0,0,0,0q�   NumberStyles$
 None=�*  $Xceed.Wpf.Controls.ValueRangeTextBox   ValueDataType" �� �� 	MaxLength$ 6���� -
   $��btnIrA��$��9��$ Ir a número consecutivo��$��0,0,0,0q�$��30��
�������
�������
���  ��.��Ir�� -   $��imprimir��$��1��$��1��$��0,0,0,0q�$( "Ver vista preliminar de impresión��  ImprimeBoton#��[ �� -   $��salir��$��5��$��1��$��0,0,0,0q�$% Regresar a la pantalla anterior��  RegresarBoton#��[ �� -   $��	Contenido��$y�Hidden=��� -   $��
BtnGuardar��$��1��$��3��$��Right=�$ Guardar��  GuardarBoton#��[ y���  	 Visibilidad	 �� Source#	 [	 
 �� Path$
 ObtenVisibilidadInternet��� -   $��BtnVisualizar��$��1��$��3��$��Right=�$ 
Visualizar��$
��40,0q� 
 VisualizarBoton#��[
 �� -   $��BtnAumentaAlto��$��3��$��1��$��25��$��120,0q�$��20��
���  ��
�������
�������$# Aumenta el alto de las celdas��$��Right=�.��+�� -   $��BtnDisminuyeAlto��$��3��$��1��$��25��$
��90,0q�$��20��$��Right=�$% Disminuye el alto de las celdas��
���  ��
�������
�������.��-(  "Xceed.Wpf.DataGrid.DataGridControl  -   $��tablaResultado��$��0,0,0,0q�$��1��$��2��$��4��   ReadOnly . 
�������   EditTriggers$
 None=�   NavigationBehavior$ RowOnly=�   SelectionMode$ Single=�   AutoCreateColumns .     Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ 	TpoAsunto��   IsMainColumn .    Title$ Asunto��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 200    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�  $ Rubro��   ReadOnly . $ Rubro��$ WordEllipsis=�$	 200 $ WrapWithOverflow=�  $	 Loc�� . $ Localización��$ WordEllipsis=�$ WrapWithOverflow=�  $ Id��$ Registro��   View (  "Xceed.Wpf.DataGrid.Views.TableView     RowSelectorPaneWidth$ 50Q�   ShowFixedColumnSplitter .  '  !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     FixedHeaders �� .R�)  #Xceed.Wpf.DataGrid.ColumnManagerRow     Theme 3	  -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme	  ����  �� 
TargetType 
  Xceed.Wpf.DataGrid.DataRow" 
 .��� $  Xceed.Wpf.DataGrid.RowSelector �� Property �@ RowSelectorStyle  itemIndexSelectorStyle  �� Value#  [ ��  � ��  �� ! �� RelativeSource! �� " �� AncestorType/  )mx.gob.scjn.electoral.TablaResultadoVotos""  $
 	RowHeight�# �� Mode$# TwoWay=���  � ��$  15��-   .� -   $��impresionViewer��  FDPVTemplate#��[ $y�Hidden=�
�������$��0,0,0,0q�$��1��$��2��$��4���� -   $��imprimePapel��$��1��$��0,0,0,0q�$y�Hidden=�  ImprimePapelBoton#��[ $��40��$��40��$��2��$
��Left=�$	��Top=�$ Imprimir el documento���� -   $��BtnTache��$��1��$��2��$
��40,0q�$y�Hidden=�  
TacheBoton#��[ $��40��$��40��$��2��$
��Left=�$	��Top=�$ Cerrar vista preliminar���� -   $��Esperar��$��0��$��20��$��0��$��10��
���   �$ J� Opacity$	$ 0.7Q�$y�Hidden=�.+��� -   $��EsperarStack��
�������$��150,150,0,0q�.+��� Espere por favor� -   $��EsperaBarra��$��0,0,0,0q�$��26��$	��393��
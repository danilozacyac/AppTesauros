   M S B A M L   `   `   `  ���� N  JGeneral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=c866414512d5116dtGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities  O Kmscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem R NPresenacion, Version=1.5.0.0, Culture=neutral, PublicKeyToken=e517cdc7fcfe666dU/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos!mx.gob.scjn.ius_common.gui.apoyos clr-namespace:IUSIUS    IUS.tablaResultadoVotos   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    WutilsGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6`	 \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6`
 \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   	 
  7my&clr-namespace:System;assembly=mscorlib   	 
  Clocal/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos   	 
  #iusclr-namespace:IUS   	 
  -     N� WindowTitle$   Visualizar registros votos�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._�� $��0,0,0,0q� � ColumnDefinitions �� $��10 �$��10���� $��40 �$��40���� $	��100 �$	��100���� $��* ��� $��40 ��� $��40 ��� $��10 �$��10�� � RowDefinitions �� $��40 ��� $��40 ��� $��* ��� $��10 �.+�� $��0��$��0��$��6�� �� $��10 ��� $	��150 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $	��120 ��� $��80 ��� $	��100 ��� $��40 �.+��� -   $��Titulo��$��1��$��3��  
LabelStyle#��[ $��0,0,0,0q�.��Resultado de la búsqueda�� -   $��inicio��$��2�� � ToolTip$ Ir al primer registro��  InicioBoton#��[ $��Right=�$��0,0,0,0q��� -   $��anterior��$ Registro anterior��  AnteriorBoton#��[ $��0,0,0,0q�$��3���� -   $��	siguiente��$ Siguiente registro��  SiguienteBoton#��[ $��4��$��0,0,0,0q��� -   $��final��$��5��$ Ir al último registro��  
FinalBoton#��[ $��0,0,0,0q��� -   $��RegistrosLabel��#��[ $��6��$��0,0,0,0q�$��14(�$
��Bold"�.��
Registros:�� -   $��lblIrA��$��7��$��0,0,0,0q�#��[ .���� $��WrapWithOverflow=�Ir a consecutivo:' 
 !Xceed.Wpf.Controls.NumericTextBox  -	   $��IrANum��$��8��$��25��$��0,0,0,0q�   NumberStyles$
 None=�* 
 $Xceed.Wpf.Controls.ValueRangeTextBox   ValueDataType" �� �� 	MaxLength$ 6���� -
   $��btnIrA��$��9��$ Ir a número consecutivo��$��0,0,0,0q�$��30��
�������
�������
���  ��.��Ir�� -   $��imprimir��$��1��$��1��$��0,0,0,0q�$( "Ver vista preliminar de impresión��  ImprimeBoton#��[ �� -   $��salir��$��5��$��1��$��0,0,0,0q�$% Regresar a la pantalla anterior��  RegresarBoton#��[ �� -   $��btnClasificacion��$��1��$��3��$��0,0,0,0q�$! Clasificar por tipo de voto��$
��Left=�$��Bottom=�$��40��$��40��$,��&/General;component/images/Filtrar1.png���� -   $��BtnOriginal��$��1��$��2��$	��0,0q�$��Right=�$��40��$��40��  OriginalBoton#��[ $ Restablecer���� -   $��
btnOrdenar��$��1��$��4��$��0,0,0,0q�$ Ordenar por...��$
��Left=�$��Bottom=�$��40��$��40�� 	 OrdenarBoton#��[	 �� -   $��	Contenido��$y�Hidden=��� -   $��
BtnGuardar��$��1��$��3��$��Right=�$ Guardar�� 
 GuardarBoton#��[
 y���   Visibilidad	 �� Source#	 [ 
 �� Path$
 ObtenVisibilidadInternet��� -   $��BtnVisualizar��$��1��$��3��$��Right=�$ 
Visualizar��$
��40,0q�  VisualizarBoton#��[ �� -   $��BtnAumentaAlto��$��3��$��1��$��25��$��120,0q�$��20��
���  ��
�������
�������$# Aumenta el alto de las celdas��$��Right=�.��+�� -   $��BtnDisminuyeAlto��$��3��$��1��$��25��$
��90,0q�$��20��$��Right=�$% Disminuye el alto de las celdas��
���  ��
�������
�������.��-�� -   $��TbcDataGrids��$��1��$��2��$��4��$��0,0,0,0q�.@��� -   $��TbiTodos�� �� 
IsSelected . $y�Visible=�$��Todos��.��( 	 "Xceed.Wpf.DataGrid.DataGridControl  -   $��tablaResultadoTodos��   ReadOnly . 
�������   EditTriggers$
 None=�   NavigationBehavior$ RowOnly=�   SelectionMode$ Single=�   AutoCreateColumns .     Columns  	 Xceed.Wpf.DataGrid.Column     	FieldName$ 	TpoAsunto��   IsMainColumn .    Title$ Asunto��   Width8 	 2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 200    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�  $ Complemento��   ReadOnly . $ Emisor��$ WordEllipsis=�$ WrapWithOverflow=�  $	 Loc�� . $ Localización��$ WordEllipsis=�$ WrapWithOverflow=�  $ Id��$ Registro��   View ( 	 "Xceed.Wpf.DataGrid.Views.TableView     RowSelectorPaneWidth$ 50Q�   ShowFixedColumnSplitter .  ' 	 !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     FixedHeaders �� .R�) 	 #Xceed.Wpf.DataGrid.ColumnManagerRow     Theme 3	 	 -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme	  ����  �� 
TargetType 
 	 Xceed.Wpf.DataGrid.DataRow" 
 .��� $ 	 Xceed.Wpf.DataGrid.RowSelector  �� Property  �@ RowSelectorStyle  itemIndexSelectorStyle! �� Value#! [ ��   � ��! �� " �� RelativeSource" �� # �� AncestorType  IUS.tablaResultadoVotos"#  $
 	RowHeight�$ �� Mode$$ TwoWay=���   � ��$! 15��-   �� -   $��TbiConcurrentes�� .  $y�Visible=�$��Concurrentes��.��  -   $ ��tablaResultadoConcurrentes�� . 
�������$
 None=�$ RowOnly=�$ Single=� .     $ 	TpoAsunto�� . $ Asunto��$	 200 $ WordEllipsis=�$ WrapWithOverflow=�  $ Complemento�� . $ Emisor��$ WordEllipsis=�$ WrapWithOverflow=�  $	 Loc�� . $ Localización��$ WordEllipsis=�$ WrapWithOverflow=�  $ Id��$ Registro��   $ 50Q� .   .   �� .R�   	  ���� " 
 .���   �@ RowSelectorStyle#! [ ��   � ��! �� " �� "#  $
 	RowHeight�$$ TwoWay=���   � ��$! 15��-   �� -   $��
TbiMinoria��$y�Visible=�$��Minoritarios��.��  -   $��tablaResultadoMinoritario�� . 
�������$
 None=�$ RowOnly=�$ Single=� .     $ 	TpoAsunto�� . $ Asunto��$	 200 $ WordEllipsis=�$ WrapWithOverflow=�  $ Complemento�� . $ Emisor��$ WordEllipsis=�$ WrapWithOverflow=�  $	 Loc�� . $ Localización��$ WordEllipsis=�$ WrapWithOverflow=�  $ Id��$ Registro��   $ 50Q� .   .   �� .R�   	  ���� " 
 .���   �@ RowSelectorStyle#! [ ��   � ��! �� " �� "#  $
 	RowHeight�$$ TwoWay=���   � ��$! 15��-   �� -   $��TbiParticulares��$y�Visible=�$��Particulares��.��  -    $ ��tablaResultadoParticulares�� . 
�������$
 None=�$ RowOnly=�$ Single=� .     $ 	TpoAsunto�� . $ Asunto��$	 200 $ WordEllipsis=�$ WrapWithOverflow=�  $ Complemento�� . $ Emisor��$ WordEllipsis=�$ WrapWithOverflow=�  $	 Loc�� . $ Localización��$ WordEllipsis=�$ WrapWithOverflow=�  $ Id��$ Registro��   $ 50Q� .   .   �� .R�   	  ���� " 
 .���   �@ RowSelectorStyle#! [ ��   � ��! �� " �� "#  $
 	RowHeight�$$ TwoWay=���   � ��$! 15��-!   �� -"   $��TbiOtros�� . $��Otros��.��  -#   $��tablaResultado�� . 
�������$
 None=�$ RowOnly=�$ Single=� .     $ 	TpoAsunto�� . $ Asunto��$	 200 $ WordEllipsis=�$ WrapWithOverflow=�  $ Complemento�� . $ Emisor��$ WordEllipsis=�$ WrapWithOverflow=�  $	 Loc�� . $ Localización��$ WordEllipsis=�$ WrapWithOverflow=�  $ Id��$ Registro��   $ 50Q� .   .   �� .R�   	  ���� " 
 .���   �@ RowSelectorStyle#! [ ��   � ��! �� " �� "#  $
 	RowHeight�$$ TwoWay=���   � ��$! 15��-$   .� -%   $��impresionViewer��  FDPVTemplate#��[ $y�Hidden=�
�������$��0,0,0,0q�$��1��$��2��$��4���� -&   $��imprimePapel��$��1��$��0,0,0,0q�$y�Hidden=�  ImprimePapelBoton#��[ $��40��$��40��$��2��$
��Left=�$	��Top=�$ Imprimir el documento���� -'   $��BtnTache��$��1��$��2��$
��40,0q�$y�Hidden=�  
TacheBoton#��[ $��40��$��40��$��2��$
��Left=�$	��Top=�$ Cerrar vista preliminar���� .+�<  6mx.gob.scjn.ius_common.gui.apoyos.ListadoClasificacion  -(   %  Name$% Clasificacion��
& �� Top$& 75��' �� Left$' 56��$y�Hidden=�$	��251��$	��524��7  1mx.gob.scjn.ius_common.gui.apoyos.VotosOrdenarPor  -)   (  Name$( 
pnlOrdenar��$y�Hidden=�$	��191��$' 33��$& 75��$	��524��8  2mx.gob.scjn.ius_common.gui.apoyos.AlmacenaBusqueda  -*   )  Name$) PnlAlmacenar��$' 50��$& 50��$y�Hidden=��� -+   $��Esperar��$��0��$��20��$��0��$��10��
���   �* J� Opacity$	* 0.7Q�$y�Hidden=�.+��� -,   $��EsperarStack��
�������$��150,150,0,0q�.+��� Espere por favor� --   $��EsperaBarra��$��0,0,0,0q�$��26��$	��393��
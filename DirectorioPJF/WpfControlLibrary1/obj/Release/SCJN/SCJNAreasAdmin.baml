   M S B A M L   `   `   `  ���� T  PDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ef529e1531468d11?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  0    *mx.gob.scjn.directorio.SCJN.SCJNAreasAdmin   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    4local$clr-namespace:mx.gob.scjn.directorio    8x,http://schemas.microsoft.com/winfx/2006/xaml      N� Title$  SCJNAreasAdmin��$	��450��$	��702�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��$��0q�   BorderStyle#��[  ._��  � RowDefinitions �� $��1 ��� $��467.277* � � ColumnDefinitions �� �� .+�� -   $��grdListaIzq��$��1,1,1,16q�$��1�� �� $
��400* ��� $��30 �.+��� -   $��lblPonencia��$��0,0,0,184.36q�$��Center=�$��Bottom=�  ColorFuenteEtiqueta#��[ $��23.277��$��64.607��.��Presidencia` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��
AreasAdmin��   AutoCreateColumns .  
�������   EditTriggers$
 None=�$��5,5,5,4q�   NavigationBehavior$ RowOnly=�   ReadOnly . 	   SelectionMode$	 Single=�$��15(�
   Columns
   Xceed.Wpf.DataGrid.Column     	FieldName$ NombreOrganoJur��   Title$ Nombre��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 300    IsMainColumn .    ReadOnly .    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�  $ IdOrganoJur��$ Id��   Visible .    $ DomOrganoJur��$ 	Domicilio�� .    $ TelOrganoJur��$ Telefono�� .    $ NombreStrOrganoJur��$ NombreParaBuscar�� .    $ TitularSolo��$ Titular�� .     View (  "Xceed.Wpf.DataGrid.Views.TableView     ShowFixedColumnSplitter .     ShowRowSelectorPane .  '  !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .  �� -   $��rectangleAccesoRapido��
�������{�.  J� IsHitTestVisible .   J� Opacity$ 9Q� J� OpacityMask
 ����� �� RadiusX$ 10�� �� RadiusY$ 10��
���   � �� StrokeDashCap$ Round=� �� StrokeDashOffset$ 5Q� �� StrokeEndLineCap$ Square=� �� StrokeLineJoin$ Bevel=�  �� StrokeStartLineCap$  Triangle=�$y�Visible=�$��2��$
��Left=�$	��148���� -   $��txtAccesoRapido��$��8,2,1,2q�$��15(�
�������
�������$��1��$ 1Q�
�������$
��Left=�$	��130��.��Acceso rápido�� -   $��lblHayDatos��$��1��$��
154,1,50,1q�#��[ $y�Hidden=�$��13(�.��No hay coincidencias�� -   $��rectangleTotal��
�������{�.  .  $ 9Q�
 �����$ 10��$ 10��
���   �$ Round=�$ 5Q�$ Square=�$ Bevel=�$  Triangle=�$y�Visible=�$��1��$��0,0,5,0q�$��Right=�$��38.961���� -	   $��textCuantos��
�������$��14(�$��0,6.321,13.32,6q�$��WordEllipsis=�$��WrapWithOverflow=�$��1��$��Right=�$��20.979��! �� TextAlignment$! Center=�� -
   $��grdDetalleDer��$��1��$��5,1,1,1q�$��1�� �� $��5.454* ��� $��339.796* � �� $
��186* ��� $��31 ��� $��116.069* ��� $��29 ��� $��121.006* ��� $��68.014* �.+��� -   $��lblDom��$��3��$��1,1,1,1q�#��[ $��Center=�$��1��.��DOMICILIO Y TELÉFONO�� -   $��rectangleNombre��$��1��$��1,5,1,1q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$ Round=�$ 5Q�$ Square=�$ Bevel=�$  Triangle=�
 �����{�. �� -   $��rectangleDom��$��4��$��1,1,1,1q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$ Round=�$ 5Q�$ Square=�$ Bevel=�$  Triangle=�
 �����{�. $��1���� -   $��rectangleCargo��$��2��$��1��$��1,1,1,1q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$ Round=�$ 5Q�$ Square=�$ Bevel=�$  Triangle=�
 �����{�. �� -   $��
textDomTel��$��5,5,5,5q�$��4��
�������$��14(�$��WrapWithOverflow=�$��1���� -   $��textTitular��$��10,10,10,10q�$��2��
�������$��19(�$��WrapWithOverflow=�$��1��$��Center=�$��Center=��� -   $��textNombreArea��$��10,10,10,15q�
�������$��23(�$��Nombre��$��WrapWithOverflow=�$! Center=�$��1��$��Center=�$��Center=��� -   $��
lblTitular��#��[ $��0,2.058,0,1.084q�$��1��$��12(�$��Center=�$��56.973��$��1��.��	TITULAR� -   $��
grdBotones��$��1��$��0,0,0,0q�$��5��$��Right=�$	��150�� �� $��50 ��� $��50 ��� $��50 �.+��� -   $��Guardar_��$��0��$��0q�" � ToolTip$" Guardar en archivo��  GuardarBoton#��[ y���   Visibilidad# �� Source## [ $ �� Path$$ ObtenVisibilidadInternet��� -   $��	Imprimir_��$��1��$��0��$��0q�$" Imprimir��  ImprimeBoton#��[ $	��Top=��� -   $��PortaPapeles_��$��3��$��1,1,1,1q�$" Portapapeles��  PortapapelesBoton#��[ $��Right=��� -   $��contenidoTexto��$��1��$��49��$��20,12.002,162.707,7q�$y�Hidden=�"% �� VerticalScrollBarVisibility$
% Auto=�$��155.859��$��5��.� -   $��	impresion��{�. 
�������$��1��$��1��$y�Hidden=�$��43��$��Bottom=�$
��Left=�$��66��.   (mx.gob.scjn.directorio.FondoTransparente  -   &  Name$& FTransparente��$y�Hidden=�$��1,1,1,1q�$��2��$��3��-   'mx.gob.scjn.directorio.OpcionesImprimir  -   '  Name$' 
OpImprimir��$y�Hidden=�$��103,15.199,-242,52.77q�$��5��$��2��+   %mx.gob.scjn.directorio.MensajesAvisos  -   (  Name$( Aviso��$y�Hidden=�$ ��167.876,88.568,174.124,145q�$��2��$��1��
   M S B A M L   `   `   `  ���� T  PDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ef529e1531468d11?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  .    (mx.gob.scjn.directorio.CJF.CJFAreasAdmin   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    4local$clr-namespace:mx.gob.scjn.directorio    8x,http://schemas.microsoft.com/winfx/2006/xaml      N� Title$  CJFAreasAdmin��$	��500��$	��740�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��$��0q�   BorderStyle#��[  ._�� $	��500��$	��738�� � RowDefinitions �� $��0* ��� $
��440* ��� $��60 � � ColumnDefinitions �� �� .+�� -   $��grdDetalleDer��$��1��$��17.5,11.751,6,2q�$��1�� �� $��5.454* ��� $��339.796* � �� $��141.229* ��� $��25 ��� $	��77* ��� $��30 ��� $
��102* �.+��� -   $��lblDom��$��3��$��1,1,1,1q�  ColorFuenteEtiqueta#��[ $��Center=�$��1��.��DOMICILIO Y TELÉFONO�� -   $��textTel��
�������$��20,20,20,20q�$��4��$��WrapWithOverflow=�$��14(�$��1���� -   $��rectangleNombre��$��1��$��1,1,1,1q�
���   � �� RadiusX$ 10�� �� RadiusY$ 10��
������� J� IsHitTestVisible .   J� Opacity$ 9Q�$y�Visible=�	 �� StrokeDashCap$	 Round=�
 �� StrokeDashOffset$
 5Q� �� StrokeEndLineCap$ Square=� �� StrokeLineJoin$ Bevel=� �� StrokeStartLineCap$ Triangle=� J� OpacityMask
 �����{�. �� -   $��rectangleCargo��$��2��$��1��$��1,1,1,1q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. �� -   $��rectangleDom��$��4��$��1��$��1,1,1,1q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. �� -   $��textTitular��$��10,10,10,10q�$��2��
�������$��21(�$��WrapWithOverflow=�$��1��$��Center=�$��Center=��� -	   $��textNombreArea��$��10,10,10,10q�
�������$��25(�$��Nombre��$��WrapWithOverflow=� �� TextAlignment$ Center=�$��1��$��Center=�$��Center=��� -
   $��
textDomTel��$��10,10,10,10q�$��4��
�������$��14(�$��WrapWithOverflow=�$��1��$��Center=�$��Center=��� -   $��
lblTitular��#��[ $��0,2.058,0,1.084q�$��1��$��12(�$��Center=�$��56.973��$��1��.��	TITULAR� -   $��
grdBotones��$��4��$��50.001��$��Right=�$��	0,0,0,-55q�$��Bottom=�$	��150��$��1�� �� $��50 ��� $��50 ��� $��50 �.+��� -   $��Guardar_��$��0q�$��0�� � ToolTip$ Guardar en archivo��  GuardarBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��	Imprimir_��$��1��$��0��$��0q�$ Imprimir��  ImprimeBoton#��[ $	��Top=��� -   $��PortaPapeles_��$��2��$��0q�$ Portapapeles��  PortapapelesBoton#��[ $��Right=�� -   $��grdListaIzq��$��15.453,5.751,0,8q�$��1��$��2�� �� $��319.8* ��� $
��3.6* ��� $	��23* ��� $
��5.4* ��� $��1.747* � �� $��462.248* ��� $��34.001* �.+��� -   $��lblPonencia��$��144.47,0,85.923,187.736q�$��Bottom=�
�������$��23.277��.��Presidencia` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��
AreasAdmin��   AutoCreateColumns .  
�������   EditTriggers$
 None=�$��5,5,1.653,4.751q�   NavigationBehavior$ RowOnly=�   ReadOnly .    SelectionMode$ Single=�$��15(�$��4��   Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ NombreOrganoJur��   Title$ Nombre��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 320    IsMainColumn .    ReadOnly .    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�  $ IdOrganoJur��$ Id��    Visible  .    $ DomOrganoJur��$ 	Domicilio��  .    $ TelOrganoJur��$ Telefono��  .    $ NombreStrOrganoJur��$ NombreParaBuscar��  .    $ TitularSolo��$ Titular��  .  !   View! (  "Xceed.Wpf.DataGrid.Views.TableView  "   ShowFixedColumnSplitter" .  #   ShowRowSelectorPane# .  '  !Xceed.Wpf.DataGrid.Views.ViewBase$   UseDefaultHeadersFooters$ .  �� -   $��rectangleAccesoRapido��
�������{�.  .  $ 9Q�
 �����$ 10��$ 10��
���   �$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��1��$��	0,0,147,0q��� -   $��rectangleTotal��
�������{�.  .  $ 9Q�
 �����$ 10��$ 10��
���   �$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��1��$��0,1,0,0q�$��1��$��3���� -   $��txtAccesoRapido��$��8,2,0,2q�$��15(�
�������
�������$��1��$ 1Q�
�������$
��Left=�$	��130��.��Acceso rápido�� -   $��lblHayDatos��$��1��$��0,1,3.453,1q�#��[ $y�Hidden=�$��13(�$��Right=�$��137.547��.��No hay coincidencias�� -   $��textCuantos��
�������$��14(�$	��0,5q�$��WordEllipsis=�$��WrapWithOverflow=�$��Center=�$��1��$��2��99.� -   $��	impresion��{�. 
�������$y�Hidden=�$��0,0,-207,3.899q�$��Right=�$��180.603��$��1��$��4��.   (mx.gob.scjn.directorio.FondoTransparente  -   %  Name$% FTransparente��$y�Hidden=�$��1,0,-369,20q�$��5��-   'mx.gob.scjn.directorio.OpcionesImprimir  -   &  Name$& 
OpImprimir��$y�Hidden=�$%��165.649,84.056,-178.102,105.302q�$��5��+   %mx.gob.scjn.directorio.MensajesAvisos  -   '  Name$' Aviso��$y�Hidden=�$%��169.649,85.056,-174.102,106.302q�$��5���� -   $��contenidoTexto��$��2��$y�Hidden=�"( �� VerticalScrollBarVisibility$
( Auto=�$��84.979,4,176.021,0q�$��22.95��$	��108��$��1��$	��Top=�
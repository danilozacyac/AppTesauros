   M S B A M L   `   `   `  ���� T  PDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ef529e1531468d11?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  0    *mx.gob.scjn.directorio.CJF.CJFFuncionarios   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    4local$clr-namespace:mx.gob.scjn.directorio    8x,http://schemas.microsoft.com/winfx/2006/xaml    l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid        N� Title$  CJFFuncionarios�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��$��0q�   BorderStyle#��[  ._�� -   $��grid1��$	��500��$	��744�� � RowDefinitions �� $
��400* ��� $
��100* ��� $��6.847* ��� $��50 �.+�� $��1,1,1,1q�$��2�� �� $��0* ��� $
��374* ��� $��55.638* � � ColumnDefinitions �� $
��310* ��� $
��340* �.+�� -   $��grdDetalleDer��$��1��$��1��$��2,1,2,1q� �� $��5 ��� $��5 ��� $��390.992* ��� $��5 � �� $	��97* ��� $��30 ��� $��126.564* ��� $��30 ��� $	��85* ��� $��5* �.+��� -   $��rectangleTel��
�������$��1��{�.  J� IsHitTestVisible .  $��1,1,1,1q� J� Opacity$ 9Q� J� OpacityMask
 ����� �� RadiusX$ 10��	 �� RadiusY$	 10��
���   �
 �� StrokeDashCap$
 Round=� �� StrokeDashOffset$ 5Q� �� StrokeEndLineCap$ Square=� �� StrokeLineJoin$ Bevel=� �� StrokeStartLineCap$ Triangle=�$y�Visible=�$��2���� -   $��label10��  ColorFuenteEtiqueta#��[ $��3��$��1,1,1,1q�$��2��$��13(�$��Center=�$��Center=�$��25.69��.��Domicilio y Teléfono�� -   $��lblOJ��#��[ $��1,1,1,1q�$��2��$��13(�$��Center=�$��Center=�$��1��.��Órgano Jurisdiccional�� -   $��
rectangle1��
�������$��1��{�.  .  $��1,1,1,1q�$ 9Q�
 �����$ 10��$	 10��
���   �$
 Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��2��$��2���� -   $��
textNombre��
�������$��25(�$��2��$��5,5,5,5q�$��Nombre�� �� TextAlignment$ Center=�$��WrapWithOverflow=�$��Center=��� -	   $��textOJ��
�������$��21(�$��2��$��2��$��WrapWithOverflow=�$��4,4,4,4q�$��Center=�$��Center=��� -
   $��
rectangle2��
�������{�.  .  $��1,1,1,1q�$ 9Q�
 �����$ 10��$	 10��
���   �$
 Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��4��$��1��$��3���� -   $��
textDomTel��
�������$��14(�$��2��$��4��$��WrapWithOverflow=�$	��1,1q�$��Center=�	DOMICILIO� -   $��
grdBotones��$��2��$��5��$��	0,0,5,-50q�$��Right=�$	��151��$��48.841��$��Bottom=� �� $��50 ��� $��50 ��� $��50 �.+��� -   $��Guardar_��$��0q�$��0�� � ToolTip$ Guardar en archivo��  GuardarBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��	Imprimir_��$��1��$��0��$��0q�$ Imprimir��  ImprimeBoton#��[ $	��Top=��� -   $��PortaPapeles_��$��3��$��1,1,1,1q�$ Portapapeles��  PortapapelesBoton#��[ $��Right=�� -   $��grdListaIzq��$��1��$��2,1,2,0q�$��2�� �� $��373.544* ��� $��39.895* �.+�(  "Xceed.Wpf.DataGrid.DataGridControl  -   $��Funcionarios��   AutoCreateColumns .  
�������   EditTriggers$
 None=�   NavigationBehavior$ RowOnly=�   ReadOnly .    SelectionMode$ Single=�$��14(�$��0,0,0,1q�   Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ NombrePersona��   IsMainColumn .    ReadOnly .    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�   Title$ Nombre��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 320   $ 	IdPersona��$ Id��    Visible  .    $ 
DomPersona��$ 	Domicilio��  .    $ 
TelPersona��$ Telefono��  .  !   View! (  "Xceed.Wpf.DataGrid.Views.TableView  "   ShowFixedColumnSplitter" .  #   ShowRowSelectorPane# .  '  !Xceed.Wpf.DataGrid.Views.ViewBase$   UseDefaultHeadersFooters$ .  �� -   $��rectangleAccesoRapido��
�������{�.  .  $y�Visible=�$	��0,5q�$ 9Q�
 �����$ 10��$	 10��
���   �$
 Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$��1��$
��Left=�$	��172���� -   $��txtAccesoRapido��$��8,8,0,8q�$��15(�
�������$y�Visible=�
�������$��1��$ 1Q�
�������$
��Left=�$	��152��.��Acceso rápido.   (mx.gob.scjn.directorio.FondoTransparente  -   %  Name$% FTransparente��$y�Hidden=�$��0,1,1,-2q�$��2��-   'mx.gob.scjn.directorio.OpcionesImprimir  -   &  Name$& 
OpImprimir��$y�Hidden=�$��0,121.262,-219,16q�$��Right=�$	��366��+   %mx.gob.scjn.directorio.MensajesAvisos  -   '  Name$' Aviso��$y�Hidden=�$$��107.149,50.186,-236.602,71.432q��� -   $��rectangleTotal��
�������$��Right=�{�.  .  $��0,5,6,7q�$ 9Q�
 �����$ 10��$	 10��
���   �$
 Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��47��$��1���� -   $��label1��$��13(�#��[ $��28��$��10,11,172.127,0q�$	��Top=�$y�Hidden=�.��No hay coincidencias�� -   $��textCuantos��
�������$��14(�$��301.25,0,10.125,12q�$ Center=�$��WordEllipsis=�$��WrapWithOverflow=�$��1��$��20.547��$��Bottom=��� -   $��lblHayDatos��$��12(�#��[ $��174,5,54.625,10q�$y�Hidden=�$��1��.��No hay coincidencias�� -   $��contenidoTexto��$��2��$
��32.3��$��166,3,166,21q�$y�Hidden=�"( �� VerticalScrollBarVisibility$
( Auto=�$	��103��$��1��.� -   $��	impresion��
�������$��2��$
��Left=�{�. $��	49,10,0,4q�$y�Hidden=�$��69��$��1��� -   $��grid2��$��1,1,1,1q�$��3�� �� $��5 ��� $��27.889* ��� $��5 � �� $��6.012* ��� $��90.681* ��� $��4.509* ��� $��91.85* ��� $��5.344* ��� $��3.604* ��� $��303.553* ��� $��7.247* ��� $
��7.6* ��� $��258.6* �.+��� -   $��	btnBuscar��$��0,0,0,0q�$��15(�$��3��$��1��  
ColorClaro#��[   	ColorBase#��[ $y�Hidden=�  ColorFuenteBoton#��[ .��Buscar�� -   $��btnRestablecer��$��15(�#��[ #��[ #��[ $��2q�$y�Hidden=�$ Mostrar listado completo��$��1��$��1��.��Restablecer�� -    $��	groupBox1��$�� ��$��0,0,0,0q�
���   �
 �����
���   �
�������$ Criterio de búsqueda��$��8��$��2��$��2��$y�Hidden=�.��� $��249.8��$
��17.6��$��0,0,0,0q� �� $
��2.4* ��� $��94.4* ��� $
��2.4* ��� $��140.4* ��� $
��2.8* �.+�	� -!   $��	optInicio��$��1.837,-0.13,2.672,2.13q�) d� 	IsChecked$) FalseY�$��12(�$��2��.��	Al Inicio	� -"   $��optCualquierParte��$��1,1,1,1q�$
) TrueY�$��12(�$��3��.��En cualquier parte�� -#   $��
rectangle3��
�������{�.  .  $��4.948,0,1.8,0q�$ 9Q�
 �����$ 10��$	 10��
���   �$
 Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Hidden=�$��4��$��4��$��1���� -$   $��txtCampoBusqueda��$��	10,3,10,3q�$��15(�
�������
�������$��6��$��1��$y�Hidden=�
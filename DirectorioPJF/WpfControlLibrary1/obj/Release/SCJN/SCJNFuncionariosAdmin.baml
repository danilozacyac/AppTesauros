   M S B A M L   `   `   `  ���� T  PDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ef529e1531468d11?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  7    1mx.gob.scjn.directorio.SCJN.SCJNFuncionariosAdmin   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    4local$clr-namespace:mx.gob.scjn.directorio      N� Title$  SCJNFuncionariosAdmin��$	��442��$	��702�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��$��0q�   BorderStyle#��[  ._�� $	��440��$	��700�� � RowDefinitions �� $��1 ��� $
��357* ��� $	��42* ��� $	��40* � � ColumnDefinitions �� �� .+�� -   $��grdDetalleDer��$��1��$��5,1,0,4q�$��1�� �� $��5.01* ��� $��4.676* ��� $��8.349* ��� $��314.962* ��� $��7.849* ��� $��4.174* � �� $
��101* ��� $��32 ��� $
��118* ��� $��27 ��� $	��59* ��� $��37.696* ��� $��3* �.+��� -   $��lblDom��$��3��$��77.007,1,81.007,1q�  ColorFuenteEtiqueta#��[ $��3��.��DOMICILIO Y TELÉFONO�� -   $��lblCargo��$��1��$��117.23,1,121.23,1q�#��[ $��13(�$��3��.��CARGO�� -   $��rectangleNombre��$��1��$��1,1,1,1q�
���   � �� RadiusX$ 10�� �� RadiusY$ 10��
������� J� IsHitTestVisible .   J� Opacity$ 9Q�$y�Visible=�	 �� StrokeDashCap$	 Round=�
 �� StrokeDashOffset$
 5Q� �� StrokeEndLineCap$ Square=� �� StrokeLineJoin$ Bevel=� �� StrokeStartLineCap$ Triangle=� J� OpacityMask
 �����{�. $��5���� -   $��rectangleCargo��$��2��$��1��$��1,1,1,1q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. $��5���� -   $��rectangleDom��$��4��$��1��$��1,1,1,1q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. $��2��$��5���� -   $��	textCargo��$��0,0,0,0q�$��2�� �� TextAlignment$ Center=�
�������$��20(�$��WrapWithOverflow=�$��3��$��Center=�$��Center=�CARGO�� -	   $��
textNombre��$��0,0,0,0q�
�������$��22(�$��Nombre��$��WrapWithOverflow=�$ Center=�$��3��$��Center=�$��Center=��� -
   $��textDom��$��0,17.811q�$��4��
�������$��14(�$��WrapWithOverflow=�$��2��$��3��	Domicilio�� -   $��textTel��$��3,0,3,4q�$��5��
�������$��14(�$��WrapWithOverflow=�$��2��$��3��	Teléfono� -   $��
grdBotones��$��6��$��160,0,6.826,-46q�$��3��$��2��$��42��$��Bottom=� �� $��50 ��� $��50 ��� $��50 �.+��� -   $��Guardar_��$��0��$��0q� � ToolTip$ Guardar en archivo��  GuardarBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��	Imprimir_��$��1��$��0��$��0q�$ Imprimir��  ImprimeBoton#��[ $	��Top=��� -   $��PortaPapeles_��$��3��$��1,1,1,1q�$ Portapapeles��  PortapapelesBoton#��[ $��Right=�� -   $��grdListaIzq��$��1,1,1,0q�$��1��$��2�� �� $
��4.4* ��� $��338.4* ��� $
��5.2* � �� $��360.873* ��� $��3 ��� $��29.997* �.+��� -   $��lblPonencia��$��137.297,143.308,136.496,0q�$	��Top=�#��[ $��23.277��$��1��.��Presidencia` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��Funcionarios��   AutoCreateColumns .  
�������   EditTriggers$
 None=�$��1,5,5,1q�   NavigationBehavior$ RowOnly=�   ReadOnly .    SelectionMode$ Single=�$��16(�$��1��$��2��   Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ NombrePersona��   Title$ Nombre��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 300    IsMainColumn .    ReadOnly .    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�  $ 	IdPersona��$ Id��    Visible  .    $ 
DomPersona��$ 	Domicilio��  .    $ 
TelPersona��$ Telefono��  .  !   View! (  "Xceed.Wpf.DataGrid.Views.TableView  "   ShowFixedColumnSplitter" .  #   ShowRowSelectorPane# .  '  !Xceed.Wpf.DataGrid.Views.ViewBase$   UseDefaultHeadersFooters$ .  �� -   $��rectangleAccesoRapido��
�������{�.  .  $ 9Q�
 �����$ 10��$ 10��
���   �$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��2��$
��Left=�$��143.6��$��1��$��0,0,0,0q��� -   $��txtAccesoRapido��$��6.8,3.289,0,3.575q�$��13(�
�������
�������$��2��$ 1Q�
�������$
��Left=�$��128.4��$��1��.��Acceso rápido�� -   $��lblHayDatos��$��2��$��149.6,1,44.8,1q�#��[ $y�Hidden=�$��12(�$��1��.��No hay coincidencias�� -   $��rectangleTotal��
�������{�.  .  $ 9Q�
 �����$ 10��$ 10��
���   �$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��1��$��0,7.019,5,0q�$��Right=�$��38.961��$��1��$��2��$��2���� -   $��textCuantos��
�������$��14(�$��0,6.321,8.12,6q�$��WordEllipsis=�$��WrapWithOverflow=�$��2��$��Right=�$��20.979��$ Center=�$��1��.   (mx.gob.scjn.directorio.FondoTransparente  -   %  Name$% FTransparente��$y�Hidden=�$��3��$��0,0,0,41.814q�-   'mx.gob.scjn.directorio.OpcionesImprimir  -   &  Name$& 
OpImprimir��$y�Hidden=�$��0,58.473,-195.647,52.453q�$��1��$��2��$��Right=�$	��366��+   %mx.gob.scjn.directorio.MensajesAvisos  -   '  Name$' Aviso��$y�Hidden=�$ ��163.8,81.601,-177.2,65.325q�$��2��$��1���� -   $��contenidoTexto��$��2��$��35��$
��Left=�$��23.052,2,0,5q�$y�Hidden=�"( �� VerticalScrollBarVisibility$
( Auto=�$��57��$��1��.� -   $��	impresion��{�. 
�������$��1��$��2��$y�Hidden=�$��111,18,0,15q�$
��Left=�$��51��� -   $��grdBusqueda��$��2��$��3��$��1,1,1,1q�$y�Hidden=� �� $��1.503 ��� $��34.058* ��� $��2.439 � �� $��5.328* ��� $��89.91* ��� $��3.04* ��� $��74.637* ��� $��4.662* ��� $��3.33* ��� $��272.061* ��� $��6.327* ��� $��6.66* ��� $��231.768* �.+��� -   $��	btnBuscar��  
ColorClaro#��[   	ColorBase#��[   ColorFuenteBoton#��[ $��15(�$��1q�$��3��$��1��$��0q�.��Buscar�� -   $��btnRestablecer��#��[ #��[ #��[ $��1q�$��15(�$��1��$��1��$ Mostrar listado completo��.��Restablecer�� -    $��
gpOpciones��
�������
���   �
���   �$��8��$��2��$��2��$�� ��$��0,-4.342,0,0q�
 �����$ Criterio de búsqueda��.��� $��16.65��$��224.775�� �� $
��2.4* ��� $��94.4* ��� $
��2.4* ��� $��140.4* ��� $
��2.8* �.+�	� -!   $��	optInicio��$��12(�$��2��) d� 	IsChecked$) FalseY�$��0,0,0,0q�.��	Al Inicio	� -"   $��optCualquierParte��$��12(�$��3��$
) TrueY�$��0,0,0,0q�.��En cualquier parte�� -#   $��
rectangle3��
�������$��4��$��4��$��1��{�.  .  $��4.948,0,1.8,0q�$ 9Q�
 �����$ 10��$ 10��
���   �$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=��� -$   $��txtCampoBusqueda��
�������
�������$��15(�$��6��$��1��$
��10,2q�
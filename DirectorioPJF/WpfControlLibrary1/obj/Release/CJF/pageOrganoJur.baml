   M S B A M L   `   `   `  ���� T  PDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ef529e1531468d11?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  .    (mx.gob.scjn.directorio.CJF.pageOrganoJur   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    4local$clr-namespace:mx.gob.scjn.directorio    8x,http://schemas.microsoft.com/winfx/2006/xaml    l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -     N� Title$  pageOrganoJur��$	��500��$	��780�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��$��0q�   BorderStyle#��[  ._�� -   $��grdExterior��$��1,1,1,1q�$	��500��$��781.823�� � RowDefinitions �� $	��500 ��� $��0 �.+�� $��0,0,0,73q� �� $
��450* ��� $��50 � � ColumnDefinitions �� $��368.682* ��� $��352.851* �.+�� -   $��
grdDetalle��$��1��$��10,1,6,1q� �� $��147.759* ��� $��23 ��� $��84.904* ��� $��23 ��� $
��142* �.+��� -   $��lblDomSPAdmin��$��14(�  ColorFuenteEtiqueta#��[ $��26��$��Center=�$��0,0,0,0q�$��Bottom=�$��2��.��	DOMICILIO�� -   $��lblAreaAdmin��$��14(�#��[ $��3��$��Center=�$��0,0,0,0q�$��2��.��	TITULARES�� -   $��rectangleInstancia��$��0��$��1,1,1,1q�
���   � �� RadiusX$ 10�� �� RadiusY$ 10��
������� J� IsHitTestVisible .   J� Opacity$ 9Q�$y�Visible=�	 �� StrokeDashCap$	 Round=�
 �� StrokeDashOffset$
 5Q� �� StrokeEndLineCap$ Square=� �� StrokeLineJoin$ Bevel=� �� StrokeStartLineCap$ Triangle=� J� OpacityMask
 �����{�. �� -   $��rectangleDom��$��2��$��1,0.942,1,0.937q�
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. �� -	   $��rectangleTit��$��4��
���   �$ 10��$ 10��
������� .  $ 9Q�$y�Visible=�$	 Round=�$
 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. �� -
   $��txtNombreOrg��
�������$��tahoma)�$��20(�$��8,8,8,8q� �� TextAlignment$ Center=�$��WordEllipsis=�$��WrapWithOverflow=�$��Center=�$��Center=�	NOMBRE OJ�� -   $��txtDomicilio��
�������$��13(�$��2��$��8,7.942,8,7.937q�$��WordEllipsis=�$��WrapWithOverflow=�$��Center=�$��Center=�	Domicilio(  "Xceed.Wpf.DataGrid.DataGridControl  -   $��Funcionarios��   AutoCreateColumns .  
�������   EditTriggers$
 None=�$��13(�$��4��$��8,8,8,8q�   NavigationBehavior$ RowOnly=�   ReadOnly .    SelectionMode$ Single=�   ItemScrollingBehavior$ 	Immediate=�   Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ NombrePersona��   IsMainColumn .    ReadOnly .    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�   Title$ Nombre��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 180   $ 
TelPersona��$ Telefono�� .    Visible . $ WrapWithOverflow=�$	 115    View (  "Xceed.Wpf.DataGrid.Views.TableView  '  !Xceed.Wpf.DataGrid.Views.ViewBase    UseDefaultHeadersFooters  .  � -   $��grdXgrdLista��$��1,1,1,2q�$��2��.+�  -   $��grdOrganismos�� .  $
 None=�$��1,1,1,1q�$ RowOnly=� . $ Single=�
�������$��13(�   $ NombreOrganoJur�� . $ WrapWithOverflow=�$  Organo Jur�� . $	 350  .   $ DomOrganoJur��$ DomOrganoJur�� .    $ TelOrganoJur��$ TelOrganoJur�� .     !   ShowFixedColumnSplitter! .  "   ShowRowSelectorPane" .    .  #   Theme# 3  -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme  .   (mx.gob.scjn.directorio.FondoTransparente  -   $  Name$$ FTransparente��$y�Hidden=�$��1,1,1,1q�$��2��$��3��+   %mx.gob.scjn.directorio.MensajesAvisos  -   %  Name$% Aviso��$y�Hidden=�$��191.62,84.982,-152.132,75q�-	   'mx.gob.scjn.directorio.OpcionesImprimir	  -   & 	 Name$& 
OpImprimir��$y�Hidden=�$��0,87.086,-173.414,69.914q�$��Right=�$	��366��� -   $��
grdBotones��$��Right=�$��0,1,17,1q�$��148.019��$��1��$��1�� �� $��50 ��� $��50 ��� $��50 �.+��� -   $��Guardar_��$��0q�$��0��' � ToolTip$' Guardar en archivo��  GuardarBoton#��[ y���   Visibilidad( �� Source#( [ ) �� Path$) ObtenVisibilidadInternet��� -   $��	Imprimir_��$��1��$��0��$��0q�$' Imprimir��  ImprimeBoton#��[ $	��Top=��� -   $��PortaPapeles_��$��3��$��1,1,1,1q�$' Portapapeles��  PortapapelesBoton#��[ $��Right=��� -   $��contenidoTexto��$��49��$��0,1,171.059,-1q�$y�Hidden=�"* �� VerticalScrollBarVisibility$
* Auto=�$��133.829��$��1��$��Right=�$��1��.� -   $��	impresion��{�. 
�������$��1��$y�Hidden=�$
��Left=�$��67��$��6,5,0,9q�$��1��� -   $��
grdFiltros��$��5,0,6,8.335q�$��58��$��Bottom=� �� $��5.336 ��� $��24.664* ��� $��4 ��� $��21 ��� $��3* � �� $��1 ��� $
��120* ��� $��3 ��� $
��120* ��� $��3 ��� $
��120* ��� $��3 ��� $��70 ��� $��3 ��� $��60 ��� $��3 ��� $
��120* ��� $��3 ��� $��60 ��� $��1 �.+��� -   $��comboOrdinal��+ �� 
IsEditable+ .  , �� 
IsReadOnly, . {�.  - �� Text$- Ordinal��. �� DisplayMemberPath$. NombreElemento��$��14(�$��1��$��1,1,1,1q�$��1���� -   $��comboMateria��+ .  , . {�.  $- Materia��$. NombreElemento��$��14(�$��3��$��1,1,1,1q�$��1���� -   $��comboCircuito��+ .  , . {�.  $- Circuito��$. NombreElemento��$��14(�$��5��$��1,1,1,1q�$��1���� -   $��comboCA��$. NombreElemento��$��11��$- Centros Auxiliares��{�.  , . $ 1Q�$��1,1,1,1q�$��3���� -   $��comboOA��$��11��$	- OJA��{�.  $��0,1,1,1q�$��1��$��2���� -   $��
btnFiltrar��  
ColorClaro#��[   	ColorBase#��[   ColorFuenteBoton#��[ $��1,1,1,1q�$��7��$��2��$��1q�$��2��.��	Filtrar�� -   $��btnTodos��$��1,1,1,1q�#��[ #��[ #��[ $��9��$��2��$��2��.��Todos�� -    $��
chkOrdinal��$��0,0,0,0q�#��[ $��14(�$��1��$��3��.��	Ordinal�� -!   $��
chkMateria��$��0,0,0,0q�#��[ $��14(�$��3��$��3��.��	Materia�� -"   $��chkCircuito��$��0,0,0,0q�#��[ $��14(�$��5��$��3��.��
Circuito�� -#   $��chkOtros��#��[ $��14(�$��13��$y�Visible=�$��1,1,1,1q�$��1��.��Otros�� -$   $��textCuantos��
�������$��11(�$��WordEllipsis=�$��WrapWithOverflow=�$��7��$��1,1,1,1q�$��1��Total:�� -%   $��	circuitos��$��9(�#��[ $��13��$��3��$��Center=�$	��1,0q�$y�Hidden=�.��	CIRCUITOS
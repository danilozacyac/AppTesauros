   M S B A M L   `   `   `  ���� T  PDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ef529e1531468d11?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  *    $mx.gob.scjn.directorio.SCJN.SCJNPage   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    4local$clr-namespace:mx.gob.scjn.directorio    -   $��SCJNPonencias��  N� WindowTitle$  Directorio de la SCJN��$	��450��$	��701�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��$��0q�   BorderStyle#��[  ._�� $	��440��$	��700�� � RowDefinitions �� $
��400* ��� $��0 ��� $��0 � � ColumnDefinitions �� �� .+��� -   $��TextoParaImprimir��$��38��$��2,2,2,2q�" �� VerticalScrollBarVisibility$
 Auto=�$y�Hidden=�$��683.052��$��1�� �� 
AcceptsTab .  �� IsDocumentEnabled . $��2��� -   $��grdDetalleDer��$��1��$��5,1,0,51q� �� $��8.671* ��� $��326.83* ��� $��9.499* � �� $��46.287* ��� $��4.995* ��� $��84.718* ��� $��33 ��� $	��62* ��� $��28 ��� $	��54* ��� $��30 ��� $	��44* ��� $��1 �.+��� -   $��lblDom��$��119.706,1,118.878,1q�$��5��  ColorFuenteEtiqueta#��[ $��13(�$��1��.��	DOMICILIO�� -   $��	lblTelExt��$��7��$��84.329,1,85.501,1q�#��[ $��13(�$��1��.��TELÉFONO Y EXTENSIÓN�� -   $��rectangleNombre��$��1,1,1,1q�
���   � �� RadiusX$ 10��	 �� RadiusY$	 10��
�������
 J� IsHitTestVisible
 .   J� Opacity$ 9Q�$y�Visible=� �� StrokeDashCap$ Round=� �� StrokeDashOffset$ 5Q� �� StrokeEndLineCap$ Square=� �� StrokeLineJoin$ Bevel=� �� StrokeStartLineCap$ Triangle=� J� OpacityMask
 �����{�. $��3���� -   $��textInstancia��$��5,5,5,5q�
�������$��27(�$��Nombre��$��WrapWithOverflow=� �� TextAlignment$ Center=�$��WordEllipsis=�$��1���� -	   $��rectangleCargo��$��4��$��1,1,1,1q�
���   �$ 10��$	 10��
�������
 .  $ 9Q�$y�Visible=�$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. $��3���� -
   $��	textCargo��$��5q�$��4��
�������$��18(�$��WrapWithOverflow=�$ Center=�$��1��$��Center=�CARGO�� -   $��rectangleDom��$��6��$��1,1,1,1q�
���   �$ 10��$	 10��
�������
 .  $ 9Q�$y�Visible=�$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. $��3���� -   $��textDomicilio��$��5,5,5,5q�$��6��
�������$��WrapWithOverflow=�$��14(�$��Center=�$
��Auto��$��1��$��Center=�$ Center=�	Domicilio�� -   $��rectangleTelDom��$��8��$��1,0.828,6,1.339q�
���   �$ 10��$	 10��
�������
 .  $ 9Q�$y�Visible=�$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�
 �����{�. $��3���� -   $��textTel��
�������$��3,8,3,8q�$��8��$��WrapWithOverflow=�$��14(�$��1��	Teléfono� -   $��
grdBotones��$��9��$��	0,0,6,-50q�$��50��$��Bottom=�$��Right=�$	��150��$��2��$��1�� �� $��50 ��� $��50 ��� $��50 �.+��� -   $��Guardar_��$��0��$��0q� � ToolTip$ Guardar en archivo��  GuardarBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��	Imprimir_��$��1��$��0��$��0q�$ Imprimir��  ImprimeBoton#��[ $	��Top=��� -   $��PortaPapeles_��$��3��$��1,1,1,1q�$ Portapapeles��  PortapapelesBoton#��[ $��Right=��� -   $��
rectNombre��
�������$��3��{�. 
 .  $��1,1,1,1q�$ 9Q�
 �����$ 10��$	 10��
���   �$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��2���� -   $��	txtNombre��
�������$��20(�$��5,5,5,5q�$��Nombre��$ Center=�$
��None=�$��WrapWithOverflow=�$��2��$��1��$��Center=��� -   $��lblCargo��$��1,1,1,1q�$��3��#��[ $��13(�$��1��$��Center=�.��CARGO� -   $��grdListaIzq��$��1,1,5,0q� �� $��1 ��� $��35 ��� $��35 ��� $��25 ��� $
��200* ��� $��25 ��� $	��40* ��� $��25 ��� $	��65* ��� $��1 �.+�` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��Funcionarios��   AutoCreateColumns .  
�������   EditTriggers$
 None=�$��1q�   NavigationBehavior$ RowOnly=�   ReadOnly .    SelectionMode$ Single=�$��4��$��15(�   Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ NombrePersona��   Title$ Nombre��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 300    IsMainColumn .     ReadOnly  . !   TextTrimming$! WordEllipsis=�"   TextWrapping$" WrapWithOverflow=�  $ 	IdPersona��$ Id��#   Visible# .    $ 
DomPersona��$ 	Domicilio��# .    $ 
TelPersona��$ Telefono��# .  $   View$ (  "Xceed.Wpf.DataGrid.Views.TableView  %   ShowFixedColumnSplitter% .  &   ShowRowSelectorPane& .  '  !Xceed.Wpf.DataGrid.Views.ViewBase'   UseDefaultHeadersFooters' .  �� -   $��comboBoxInstancia��$��1,1,1,1q�$��15(�$��1���� -   $��comboBoxSalas��$��1,1,1,1q�( �� DisplayMemberPath$( NombreCompleto��$��2��$��15(��� -   $��lblPonencia��$��3��$��1,1,1,1q�$��Center=�$��Center=�#��[ $��11(�.��
PONENCIA�� -   $��lblSGA��$��5��$��6,0,9.413,0q�$��10(�#��[ $��Center=�.��SECRETARIO DE ACUERDOS�� -   $��	lblSecTes��$��9.413,0,9.413,1q�$��7��$��10(�#��[ $��Center=�.��75SECRETARIO DE ESTUDIO Y CUENTA ESPECIALIZADO EN TESIS  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��grdSecTesis�� .  
�������$
 None=�$��0,0,0,0q�$ RowOnly=� . $ Single=�$��8��$��15(�   $ NombrePersona�� .   . $! WordEllipsis=�$" WrapWithOverflow=�$ Nombre��$	 320   $ 	IdPersona��$ Id��# .    $ 
DomPersona��$ 	Domicilio��# .    $ 
TelPersona��$ Telefono��# .  $   % .  & .  ' .    =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��grdSecAcuerdos�� .  
�������$
 None=�$��1,0,1,1q�$ RowOnly=� . $ Single=�$��15(�$��6��   $ NombrePersona�� .   . $! WordEllipsis=�$" WrapWithOverflow=�$ Nombre��$	 320   $ 	IdPersona��$ Id��# .    $ 
DomPersona��$ 	Domicilio��# .    $ 
TelPersona��$ Telefono��# .  $   % .  & .  ' .  �� $��152,40,-192,14q�$��4��$��3��.� -   $��	impresion��{�. 
�������$��8��$y�Hidden=�$��Right=�$��0,22.011,-192.763,13.377q�$��89.763��.   (mx.gob.scjn.directorio.FondoTransparente  -    )  Name$) FTransparente��$y�Hidden=�$��-2,0,-349,3q�$��8��$��1��-   'mx.gob.scjn.directorio.OpcionesImprimir  -!   *  Name$* 
OpImprimir��$y�Hidden=�$��103,15.199,-242,52.77q�$��5��$��2��+   %mx.gob.scjn.directorio.MensajesAvisos  -"   +  Name$+ Aviso��$y�Hidden=�$$��164.876,34.457,-178.876,24.729q�$��2��$��4���� -#   $��contenidoTexto��$��8��$y�Hidden=�$
 Auto=�$��111,22,162,6q�$��22.95��$	��108��
   M S B A M L   `   `   `  ���� T  PDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ef529e1531468d11?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  5    /mx.gob.scjn.directorio.CJF.CJFFuncionariosAdmin   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    4local$clr-namespace:mx.gob.scjn.directorio    8x,http://schemas.microsoft.com/winfx/2006/xaml      N� Title$  CJFFuncionariosAdmin�� N� FontSize$ 13(�$	��500��$��739.5�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��$��0q�   BorderStyle#��[  ._�� $	��500��$��737.5�� � RowDefinitions �� $��5 ��� $
��440* ��� $��50 ��� $��5 � � ColumnDefinitions �� $��378.75* ��� $��358.75* �.+�� -   $��grdDetalleDer��$��1��$��5,5,5,49.177q�$��1�� �� $��5 ��� $��5 ��� $��338.038* ��� $��5 � �� $��1 ��� $	��89* ��� $	��19* ��� $	��91* ��� $	��24* ��� $��69.271* ��� $��40 ��� $��3 �.+��� -   $��	lblDomTel��$��4��$��95.835,1,93.089,1q�  ColorFuenteEtiqueta#��[ $��2��$��12(�.��DOMICILIO Y TELÉFONO�� -   $��lblAA��$��1��$��98.35,0,95.604,0.005q�#��[ $��2��$��12(�$��26.363��$��2��$��Bottom=�.��ÁREA ADMINISTRATIVA�� -   $��rectangleTel��
�������$��1��{�.  J� IsHitTestVisible .  $��0.996,0,1,1.005q� J� Opacity$ 9Q� J� OpacityMask
 �����	 �� RadiusX$	 10��
 �� RadiusY$
 10��
���   � �� StrokeDashCap$ Round=� �� StrokeDashOffset$ 5Q� �� StrokeEndLineCap$ Square=� �� StrokeLineJoin$ Bevel=� �� StrokeStartLineCap$ Triangle=�$y�Visible=�$��3��$��1���� -   $��rectangleCargo��
�������$��1��$��3��{�.  .  $��0.996,1,1,1q�$ 9Q�
 �����$	 10��$
 10��
���   �$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��3���� -   $��rectangleDom��
�������$��1��$��5��{�.  .  $��0.996,1.003,1,1q�$ 9Q�
 �����$	 10��$
 10��
���   �$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��2��$��3���� -   $��	textCargo��$��0,10.505q�$��3��
�������$��20(�$��WrapWithOverflow=�$��2��$��Center=�$
��Auto�� �� TextAlignment$ Center=�CARGO�� -	   $��textDom��
�������$	��0,5q�$��5��$��WrapWithOverflow=�$��14(�$��2��$	��DOM���� -
   $��textTel��
�������$	��0,5q�$��6��$��WrapWithOverflow=�$��14(�$��2��$	��TEL���� -   $��
textNombre��$��0,10.505q�
�������$��25(�$��Nombre��$��WrapWithOverflow=�$ Center=�$��2��$��1��$��Center=�$
��Auto��� -   $��
grdBotones��$��7��$��40.411��$��Right=�$��0,0,5.712,-41.412q�$��Bottom=�$��149.003��$��2��$��2�� �� $��50 ��� $��50 ��� $��50 �.+��� -   $��Guardar_��$��0��$��0q� � ToolTip$ Guardar en archivo��  GuardarBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��	Imprimir_��$��1��$��0��$��0q�$ Imprimir��  ImprimeBoton#��[ $	��Top=��� -   $��PortaPapeles_��$��3��$��1,1,1,1q�$ Portapapeles��  PortapapelesBoton#��[ $��Right=�� -   $��grdListaIzq��$��6,5,1,3q�$��1�� �� $
��451* ��� $��30 �.+��� -   $��lblPonencia��$��0,0,0,184.733q�$��Center=�$��Bottom=�#��[ $��25.69��$��74.54��.��Presidencia` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��Funcionarios��   AutoCreateColumns .  
�������   EditTriggers$
 None=�$��0,6,6,1q�   NavigationBehavior$ RowOnly=�   ReadOnly .    SelectionMode$ Single=�$��14(�   Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ NombrePersona��   Title$ Nombre��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 320    IsMainColumn .    ReadOnly .    TextTrimming$ WordEllipsis=�    TextWrapping$  WrapWithOverflow=�  $ 	IdPersona��$ Id��!   Visible! .    $ 
DomPersona��$ 	Domicilio��! .    $ 
TelPersona��$ Telefono��! .  "   View" (  "Xceed.Wpf.DataGrid.Views.TableView  #   ShowFixedColumnSplitter# .  $   ShowRowSelectorPane$ .  '  !Xceed.Wpf.DataGrid.Views.ViewBase%   UseDefaultHeadersFooters% .  �� -   $��rectangleAccesoRapido��
�������{�.  .  $ 9Q�
 �����$	 10��$
 10��
���   �$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��1��$
��Left=�$	��148���� -   $��txtAccesoRapido��$��8,2,1,2q�$��15(�
�������
�������$��1��$ 1Q�
�������$
��Left=�$	��130��$ Búsqueda por apellido��.��Acceso rápido�� -   $��rectangleTotal��
�������{�.  .  $ 9Q�
 �����$	 10��$
 10��
���   �$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=�$��1��$��0,0,5,0q�$��Right=�$��38.961���� -   $��lblHayDatos��$��1��$��
154,1,50,1q�#��[ $y�Hidden=�$��13(�.��No hay coincidencias�� -   $��textCuantos��
�������$��14(�$��
0,5,11.5,5q�$��WordEllipsis=�$��WrapWithOverflow=�$��1��$��Right=�$��25.625��$ Center=�.   (mx.gob.scjn.directorio.FondoTransparente  -   &  Name$& FTransparente��$y�Hidden=�$��
1,1,-358,1q��� $��144,67,-193,26q�-   'mx.gob.scjn.directorio.OpcionesImprimir  -   '  Name$' 
OpImprimir��$y�Hidden=�$��127.156,105,-212,17.817q�+   %mx.gob.scjn.directorio.MensajesAvisos  -   (  Name$( Aviso��$y�Hidden=�$$��107.149,50.186,-236.602,71.432q��� -   $��contenidoTexto��$��1��$
��32.3��$��100,0,165.75,11q�$y�Hidden=�$��Bottom=�") �� VerticalScrollBarVisibility$
) Auto=�$	��103��$��1��.� -   $��	impresion��{�. 
�������$��1��$��1��$y�Hidden=�$
��41.3��$��Bottom=�$��	16,0,0,10q�$
��Left=�$��69��� -   $��grdBusqueda��$��2��$��2��$��1,1,1,1q�$y�Hidden=� �� $��5 ��� $��27.889* ��� $��5 � �� $��6.012* ��� $��90.681* ��� $��4.509* ��� $��91.85* ��� $��5.344* ��� $��3.604* ��� $��303.553* ��� $��7.247* ��� $
��7.6* ��� $��258.6* �.+��� -   $��	btnBuscar��  
ColorClaro#��[   	ColorBase#��[   ColorFuenteBoton#��[ $��15(�$��3��$��1��$��0q�.��Buscar�� -   $��btnRestablecer��#��[ #��[ #��[ $��2q�$��15(�$��1��$��1��$ Mostrar listado completo��.��Restablecer�� -    $��
gpOpciones��
�������
���   �
���   �$��8��$��2��$��2��$�� ��$��0q�
 �����$ Criterio de búsqueda��.��� $
��17.6��$��0q�$��249.8�� �� $
��2.4* ��� $��94.4* ��� $
��2.4* ��� $��140.4* ��� $
��2.8* �.+�	� -!   $��	optInicio��$��12(�$��2��* d� 	IsChecked$* FalseY�$��1.837,-0.13,2.672,2.13q�.��	Al Inicio	� -"   $��optCualquierParte��$��12(�$��3��$
* TrueY�$��1q�.��En cualquier parte�� -#   $��
rectangle3��
�������$��4��$��4��$��1��{�.  .  $��4.948,0,1.8,0q�$ 9Q�
 �����$	 10��$
 10��
���   �$ Round=�$ 5Q�$ Square=�$ Bevel=�$ Triangle=�$y�Visible=��� -$   $��txtCampoBusqueda��
�������
�������$��15(�$��6��$��1��$
��10,3q�
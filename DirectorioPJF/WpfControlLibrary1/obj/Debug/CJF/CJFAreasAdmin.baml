   M S B A M L   `   `   `  ����H  DDirectorioPJF, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:mx.gob.scjn.directoriomx.gob.scjn.directorio  5      .    (mx.gob.scjn.directorio.CJF.CJFAreasAdmin   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      4local$clr-namespace:mx.gob.scjn.directorio    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5        N� Title$  CJFAreasAdmin��5      $	��500��6   $	��740��6(    � 	Resources 5       �� Source�� 5   
   $' !/General;component/Resources.xaml?�6   %    5      .��5      �� -   5        $��Borde��5      $��0q�6      BorderStyle#��[  6.   ._�5   
   � $	��500��6   $	��738��6    � RowDefinitions 5      �� 5      $��0* �6    �� 5      $
��440* �6    �� 5      $��60 �6     � ColumnDefinitions 5      �� 5      �� 5      .+�5      � -   5        $��grdDetalleDer��5   <   $��1��6   $��17.5,11.751,6,2q�6#   $��1��6Q    5      �� 5      $��5.454* �6'   �� 5      $��339.796* �6'    5      �� 5      $��141.229* �6$   �� 5      $��25 �6$   �� 5      $	��77* �6$   �� 5      $��30 �6$   �� 5       $
��102* �6$   .+�5"      �� -   5        $��lblDom��5"   6   $��3��6   $��1,1,1,1q�6%     ColorFuenteEtiqueta#��[ 6D   $��Center=�5#      $��1��65   .��6E   DOMICILIO Y TELÉFONO6[   �� 5$      -   5        $��textTel��5$   E   
�������$��20,20,20,20q�60   $��4��6T   $��WrapWithOverflow=�5%      $��14(�6<   $��1��6J   �� 5&      -   5        $��rectangleNombre��5&      $��1��63   $��1,1,1,1q�6C   
���   � �� RadiusX$ 10��6c    �� RadiusY$ 10��6p   
������� J� IsHitTestVisible .   J� Opacity$ 9Q�6�   $y�Visible=�6�   	 �� StrokeDashCap$	 Round=�6�   
 �� StrokeDashOffset$
 5Q�6�    �� StrokeEndLineCap$ Square=�6�    �� StrokeLineJoin$ Bevel=�6
   �� StrokeStartLineCap$ Triangle=�6!   J� OpacityMask
 �����{�. 6T  �� 5'      -   5        $��rectangleCargo��5'      $��2��62   $��1��6?   $��1,1,1,1q�6O   
���   �$ 10��6o   $ 10��6|   
������� .  $ 9Q�6�   $y�Visible=�6�   $	 Round=�6�   $
 5Q�6�   $ Square=�6�   $ Bevel=�6  $ Triangle=�6-  
 �����{�. 6`  �� 5(      -   5        $��rectangleDom��5(      $��4��60   $��1��6=   $��1,1,1,1q�6M   
���   �$ 10��6m   $ 10��6z   
������� .  $ 9Q�6�   $y�Visible=�6�   $	 Round=�6�   $
 5Q�6�   $ Square=�6�   $ Bevel=�6  $ Triangle=�6+  
 �����{�. 6^  �� 5)      -   5        $��textTitular��5)   1   $��10,10,10,10q�6   $��2��6D   
�������$��21(�6e   $��WrapWithOverflow=�6s   $��1��6�   $��Center=�6�   $��Center=�6�   �� 5*      -	   5        $��textNombreArea��5*   1   $��10,10,10,10q�6   
�������$��25(�6[   $��Nombre��6i   $��WrapWithOverflow=�6w    �� TextAlignment$ Center=�6�   $��1��6�   $��Center=�6�   $��Center=�6�   �� 5+      -
   5        $��
textDomTel��5+   1   $��10,10,10,10q�6   $��4��6C   
�������$��14(�6d   $��WrapWithOverflow=�6r   $��1��6�   $��Center=�6�   $��Center=�6�   �� 5,      -   5        $��
lblTitular��5,   c   #��[ 6   $��0,2.058,0,1.084q�6J   $��1��6u   $��12(�6�   $��Center=�6�   $��56.973��6�   $��1��6�   .��6�   	TITULAR6�   � 5-      -   5        $��
grdBotones��5-   c   $��4��6   $��50.001��6$   $��Right=�64   $��	0,0,0,-55q�6P   $��Bottom=�6u   $	��150��6�   $��1��6�    5.      �� 5/      $��50 �6+   �� 50      $��50 �6+   �� 51      $��50 �6+   .+�54      �� -   5        $��Guardar_��54   5   $��0q�6   $��0��6(    � ToolTip$ Guardar en archivo��6F     GuardarBoton#��[ 56      y�55      ��   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet�56      �� 57      -   5        $��	Imprimir_��58      $��1��57      $��0��6-   $��0q�6:   $ Imprimir��59        ImprimeBoton#��[ 5:      $	��Top=�5;      5<      �� 5=      -   5        $��PortaPapeles_��5=   -   $��2��6   $��0q�6B   $ Portapapeles��5>        PortapapelesBoton#��[ 5@      $��Right=�5A      5G      5H      � 5J      -   5        $��grdListaIzq��5J   -   $��15.453,5.751,0,8q�6   $��1��6@   $��2��6M    5K      �� 5L      $��319.8* �6'   �� 5M      $
��3.6* �6'   �� 5N      $	��23* �6'   �� 5O      $
��5.4* �6'   �� 5P      $��1.747* �6'    5R      �� 5S      $��462.248* �6$   �� 5T      $��34.001* �6$   .+�5V      �� -   5        $��lblPonencia��5V   9   $��144.47,0,85.923,187.736q�6   $��Bottom=�6L   
�������$��23.277��6   .��6�   Presidencia6�   ` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  5W      l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      5Z   >   -   5        $��
AreasAdmin��5Y   '      AutoCreateColumns .  
�������   EditTriggers$
 None=�5W   T   $��5,5,1.653,4.751q�5X   '      NavigationBehavior$ RowOnly=�5Y   9      ReadOnly .    SelectionMode$ Single=�5Z   '   $��15(�5[   '   $��4��6V      Columns 5\        Xceed.Wpf.DataGrid.Column  5]         	FieldName$ NombreOrganoJur��6&      Title$ Nombre��6B      Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 320 6Q      IsMainColumn .    ReadOnly .    TextTrimming$ WordEllipsis=�5^   6      TextWrapping$ WrapWithOverflow=�6R     5`      $ IdOrganoJur��6&   $ Id��6>       Visible  .  6I     5a      $ DomOrganoJur��6&   $ 	Domicilio��6?     .  6Q     5b      $ TelOrganoJur��6&   $ Telefono��6?     .  6P     5c      $ NombreStrOrganoJur��6&   $ NombreParaBuscar��6E     .  6^     5d      $ TitularSolo��6&   $ Titular��6>     .  6N   !   View! 5f      (  "Xceed.Wpf.DataGrid.Views.TableView  5g      "   ShowFixedColumnSplitter" .  #   ShowRowSelectorPane# .  '  !Xceed.Wpf.DataGrid.Views.ViewBase$   UseDefaultHeadersFooters$ .  5h   L   5j      �� 5l      -   5        $��rectangleAccesoRapido��5l      
�������{�.  .  $ 9Q�5m      
 �����$ 10��6=   $ 10��6J   
���   �$	 Round=�5n      $
 5Q�62   $ Square=�6G   $ Bevel=�5o      $ Triangle=�63   $y�Visible=�6Q   $��1��5p      $��	0,0,147,0q�6)   �� 5q      -   5        $��rectangleTotal��5q      
�������{�.  .  $ 9Q�5r      
 �����$ 10��6=   $ 10��6J   
���   �$	 Round=�5s      $
 5Q�62   $ Square=�6G   $ Bevel=�5t      $ Triangle=�63   $y�Visible=�6Q   $��1��5u      $��0,1,0,0q�6)   $��1��6:   $��3��6J   �� 5v      -   5        $��txtAccesoRapido��5v   +   $��8,2,0,2q�6   $��15(�6B   
�������
�������$��1��5w   3   $ 1Q�6@   
�������$
��Left=�5z   Z   $	��130��6u   .��6�   Acceso rápido6�   �� 5{      -   5        $��lblHayDatos��5{   :   $��1��6   $��0,1,3.453,1q�6%   #��[ 6M   $y�Hidden=�6   $��13(�6�   $��Right=�6�   $��137.547��6�   .��6�   No hay coincidencias6�   �� 5~      -   5        $��textCuantos��5~      
�������$��14(�6C   $	��0,5q�6Q   $��WordEllipsis=�5      $��WrapWithOverflow=�68   $��Center=�6X   $��1��6u   $��2��6�   996�   .� 5�      -   5        $��	impresion��5�   )   {�. 
�������$y�Hidden=�6^   $��0,0,-207,3.899q�6r   $��Right=�6�   $��180.603��6�   $��1��6�   $��4��6�   .   (mx.gob.scjn.directorio.FondoTransparente  5�      -   5        %  Name$% FTransparente��5�   *   $y�Hidden=�6A   $��1,0,-369,20q�6U   $��5��6j   -   'mx.gob.scjn.directorio.OpcionesImprimir  5�      -   5        &  Name$& 
OpImprimir��5�   )   $y�Hidden=�6=   $%��165.649,84.056,-178.102,105.302q�6Q   $��5��6z   +   %mx.gob.scjn.directorio.MensajesAvisos  5�      -   5        '  Name$' Aviso��5�   '   $y�Hidden=�66   $%��169.649,85.056,-174.102,106.302q�6J   $��5��6s   5�      �� 5�      -   5        $��contenidoTexto��5�   '   $��2��6   $y�Hidden=�6=   "( �� VerticalScrollBarVisibility$
( Auto=�6Q   $��84.979,4,176.021,0q�6t   $��22.95��6�   $	��108��6�   $��1��6�   $	��Top=�6�   5�      5�      5�      
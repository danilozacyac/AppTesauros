   M S B A M L   `   `   `  ���� R  NPresenacion, Version=1.5.0.0, Culture=neutral, PublicKeyToken=e517cdc7fcfe666dU/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos!mx.gob.scjn.ius_common.gui.apoyos  :    4mx.gob.scjn.ius_common.gui.Tematica.ConsultaTematica   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     Alocal/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos     8x,http://schemas.microsoft.com/winfx/2006/xaml      N� WindowTitle$%  Jurisprudencia y Tesis Aisladas�� � 	Resources  �� MergedDictionaries �� Source��  �� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._��  � RowDefinitions �� $��40 ��� $��40 ��� $��* �.+��  � ColumnDefinitions �� $��* ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��* �.+��� $��18(�  UIStyle#��[ .��Consulta temática�� -   $��Regresar��$��9��$��40��$��40��$��Right=� � ToolTip$! Regresar al menú principal��  RegresarBoton#��[ �� -   $��
Visualizar��$ 
Visualizar��  VisualizarBoton#��[ $��3���� -   $��ImgExpandir��$ Expandir��$��4��  ExpandirBoton#��[ �� -   $��ImgRestaurar��$ 	Restaurar��$��5��  RestaurarBoton#��[ �� -   $��BtnImprimir��$ Imprimir��$��6��  ImprimeBoton#��[ �� -   $��BtnBusquedaAlmacenada��$ Guardar consulta��$��7��  	BuroBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��BtnVease��$ Véase��$��8��$y�Hidden=� 	 
VeaseBoton#��[	 � $��1�� �� $	��300 ��� $	��300 ��� $��40 �.+��� -   $��Buscar��$��25��$��1���� -	   $��	BtnBuscar�� 
 BuscarBoton#��[
 $ Buscar��$��2��$��40��$��40��� $��2�� �� $	��300 ��� $	��80* ��� $
��220* � �� $��* ��� $	��100 �.+�R� -
   $��Temas��$��2��	 �� IsTextSearchEnabled	 . 
 w� TextPath$
 Header��$
��Auto=�
�������� -   $��GrdSubtemas��$��1��$��2�� �� $��30 ��� $��* �.+��� -   $��LblSubtemas��
�������$
��Bold"�
�������.��
SubtemasR� -   $��SubtemasTree��$��1��	 . $
 Header��$
��Auto=�
��������� -   $��StkSinonimos��$��0,0,0,0q�$��1��
�������$��1��$��2��.+��� 
�������$
��Bold"�
�������.��
SinónimosR� -   $��	Sinonimos��	 . $
 Header��$
��Auto=�
�������$��2���� -   $��ImprimePapel��$y�Hidden=�$ Imprimir documento��  ImprimePapelBoton#��[ $��40��$��40��$	��Top=�$
��Left=�$	��0,0q�$��0���� -   $��BtnTache��$y�Hidden=�  
TacheBoton#��[ $��40��$��40��$
��40,0q�$��0��$ Cerrar vista preliminar��$	��Top=�$
��Left=�.� -   $��Imprimir��$y�Hidden=�  FDPVTemplate#��[ $��3��
�������$��3���� $��0��$��0��$��0��$��0��.+�8   2mx.gob.scjn.ius_common.gui.apoyos.AlmacenaBusqueda  -     Name$ GuardarExpresion�� �� Left$
 -200��
 �� Top$	 100��$y�Hidden=�
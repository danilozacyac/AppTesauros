   M S B A M L   `   `   `  ���� N  JGeneral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=c866414512d5116d\;clr-namespace:mx.gob.scjn.ius_common.utils;assembly=Generalmx.gob.scjn.ius_common.utils  R NPresenacion, Version=1.5.0.0, Culture=neutral, PublicKeyToken=e517cdc7fcfe666dU/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos!mx.gob.scjn.ius_common.gui.apoyos U/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos!mx.gob.scjn.ius_common.gui.apoyos @   :mx.gob.scjn.ius_common.gui.Tematica.ThesauroConstitucional   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     Mlocal;clr-namespace:mx.gob.scjn.ius_common.utils;assembly=General     Bapoyos/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos     Dventanas/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos     8x,http://schemas.microsoft.com/winfx/2006/xaml      � 	Resources   �� MergedDictionaries �� Source��  �� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._��  � RowDefinitions �� $��40 ��� $��40 ��� $��* �.+��  � ColumnDefinitions �� $��* ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $	��240 ��� $	��280 ��� $��40 ��� $��* ��� $��40 �.+��� -   $��RtbDocumentoGuardar��$y�Hidden=��� -   $��Regresar��$��10��$��Right=�$��40��$��40�� � ToolTip$% Regresar a la pantalla anterior��  RegresarBoton#��[ �� -   $��BtnRestaurar��$��2��$ 	Restaurar��  RestaurarBoton#��[ �� -   $��BtnImprimir��$��3��$ Imprimir��  ImprimeBoton#��[ �� -   $��BtnAlmacenar��$��4��$ 	Almacenar��  	BuroBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��
Visualizar��$ 
Visualizar��  VisualizarBoton#��[ $��1���� -   $��IDSLabel��
���r���$��6��$	��Top=�$
��Left=�$��5��
���   �$	��240��$��40��$
��Bold"�.���� $��WrapWithOverflow=�$��Arial)�$��11(�$��30��$	��220��RPIA Inclusión ascendente (tema general) CE Cabeza de estructura (tema principal)�� -   $��IDLabel��
���r���$��7��$	��0,0q�$
��Left=�
���   �$	��370��$��40��$��Bottom=�$
��Bold"�.���� $��WrapWithOverflow=�$��Arial)�$��11(�$��30��$	��230��DBID Inclusión descendente (tema específico) RP Relación próxima� $��1�� �� $	��300 ��� $	��300 ��� $��40 �.+��� -	   $��Buscar��$��25��$	��Top=�$	��0,7q�$��1���� -
   $��	BtnBuscar��$	��Top=�  BuscarBoton#��[ $ Buscar��$��2��$��40��$��40��� $��1��$��0,39,0,1q�$��2�� �� $	��300 ��� $	��80* ��� $
��220* � �� $��* ��� $	��100 ��� $	��100 �.+�.� -   $��Imprimir��$��3��  FDPVTemplate#��[ $��3��
��������� -   $��ImprimePapel��$y�Hidden=�$ Imprimir�� 	 ImprimePapelBoton#��[	 $��0��$
��Left=�$��40��$��40��$	��0,0q�$	��Top=��� -   $��BtnTache��$y�Hidden=�$ Cerrar vista preliminar�� 
 
TacheBoton#��[
 $��0��$
��Left=�$
��40,0q�$��40��$��40��$	��Top=��� -   $��lblFiltrado��$��27��
�������$	��0,0q�$
��Bold"�$	��Top=�.��TESAURO CONSTITUCIONALR� -   $��Temas��$��3��$��0,28,0,0q�$
��Auto=�
������� �� IsTextSearchEnabled . 	 w� TextPath$	 Header��  
 �� 
TargetType �� Property �� Value��%6   'Q�       �� "
 Q�.���  �@Q�
IsExpanded . � -   $��GrdEstructura��$��1��$��0,0,0,0q�
���z���$��2�� �� $��30 ��� .+��� 
���z���$
��Bold"�$��0��
���z���.��
ESTRUCTURAR� -   $��SubtemasTree��$��1�� . $	 Header��$
��Auto=�
���z���  ��%6   'Q�       �� "
 Q�.���  �@Q�
IsExpanded . ����  �� ItemsSource �� $ HijosSubtemas�.R���  ��Name$ BloqueSubtema������ $ NodoSubtemaActual.Label�
 � Tag �� $ NodoSubtemaActual����� $ ColorSubTema�� -   $��GrdProximidad��$��1��$��1��$��2��
�������$��0,0,0,0q� �� $��30 ��� .+��� 
�������$
��Bold"�.��Relación próximaR� -   $��
Proximidad��$��1�� . $	 Header��$
��Auto=�
�������  ��%6   'Q�       �� "
 Q�.���  �@Q�
IsExpanded . ����  �� $ HijosSinonimos�.R��� ���� $ NodoSinonimoActual.Label� �� $ NodoSinonimoActual�� -   $��GrdSinonimos��$��1��$��2��
�������$��2�� �� $��30 ��� .+��� 
�������$
��Bold"�.��
SinónimosR� -   $��	Sinonimos��$��1�� . $	 Header��$
��Auto=�
�������$��2��  ��%6   'Q�       �� "
 Q�.���  �@Q�
IsExpanded . ����  �� $ HijosSinonimos�.R��� ���� $ NodoSinonimoActual.Label� �� $ NodoSinonimoActual��� $��1��$��1��$��0,0,0,0q�$	��Top=�$
��Left=�.+�8  2mx.gob.scjn.ius_common.gui.apoyos.AlmacenaBusqueda  -     Name$ PnlAlmacenar�� �� Left$	 100��
 �� Top$	 100��$y�Hidden=�
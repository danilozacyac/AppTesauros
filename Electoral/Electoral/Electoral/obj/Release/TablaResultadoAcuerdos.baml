   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  P LElectoral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ae5cf9d2702abc06a5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos'mx.gob.scjn.electoral_common.gui.apoyos =#clr-namespace:mx.gob.scjn.electoralmx.gob.scjn.electoral 2   ,mx.gob.scjn.electoral.TablaResultadoAcuerdos   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    3my&clr-namespace:System;assembly=mscorlib    Elocal5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos    l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l	 hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid     	 5ius#clr-namespace:mx.gob.scjn.electoral     	 -     � 	Resources   �� Source�� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._��  � ColumnDefinitions �� $��10��$��10 ��� $��42��$��42 ��� $	��250��$	��250 ��� $��* ��� $��42��$��42 ��� $��10��$��10 � � RowDefinitions �� $��40 ��� $��42 ��� $
��250* ��� $��27 ��� $��10 �.+��� -   $��Titulo��  UIStyle#��[ $��3��$��0,0,0,0q�.��Resultado de la búsqueda� $��3��$��0�� �� $��40 ��� $��40 ��� $��40 ��� $��40 �.+��� -   $��inicio��$��0�� � ToolTip$ Ir al primer registro��  InicioBoton#��[ $��Right=�$��0,0,0,0q��� -   $��anterior��$��1��$ Registro anterior��  AnteriorBoton#��[ $��0,0,0,0q�$��2���� -   $��	siguiente��$ Siguiente registro��  SiguienteBoton#��[ $��2��$��0,0,0,0q��� -   $��final��$��3��$ Ir al último registro��  
FinalBoton#��[ $��0,0,0,0q��� -   $��
BtnGuardar��$+ %Guardar información en un archivo...��$��2��$��40��$��40��  GuardarBoton#��[ y���   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet��� -   $��lblIrA��$��3��$��Center=�$��	240,0,0,0q�#��[ .���� $��WrapWithOverflow=�Ir a Consecutivo:'  !Xceed.Wpf.Controls.NumericTextBox  -	   $��IrANum��$��3��$��25��$��	335,0,0,0q�   NumberStyles$
 None=�$
��Left=�$��80��*  $Xceed.Wpf.Controls.ValueRangeTextBox   ValueDataType" ��	 �� 	MaxLength$	 8���� -
   $��btnIrA��$��3��$
��Left=�$ Ir al consecutivo��$��30��$��	415,0,0,0q�
�������
�������$��40��
���  ��.��Ir�� -   $��imprimir��$��0,0,0,0q�$��40��$��1��$��1��$��40��$( "Ver vista preliminar de impresión��  ImprimeBoton#��[ �� -   $��BtnVisualizar��$��1��$��3��$ 
Visualizar�� 	 VisualizarBoton#��[	 $��Right=�$��40��$��40���� -   $��BtnAumentaAlto��$��3��$��1��
���  ��
�������
�������$��25��$
��80,0q�$��20��$# Aumenta el alto de las celdas��$��Right=�.��+�� -   $��BtnDisminuyeAlto��$��3��$��1��$��25��$
��50,0q�$��20��$��Right=�
���  ��
�������
�������$% Disminuye el alto de las celdas��.��-�� -   $��salir��$��1��$��4��$% Regresar a la pantalla anterior�� 
 RegresarBoton#��[
 $��0,0,0,0q��� -   $��RegistrosLabel��$��1��#��[ $��2��$��14(�$
��Bold"�.��
Registros:� $��0,0,0,0q�$��2��$��1��$��4�� �� $��* � �� .+�(  "Xceed.Wpf.DataGrid.DataGridControl  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid     	 -   $��tablaResultado��$��0,0,0,0q�$��0��$��0��
�������$	��200��$	��500��$	��800��
   ReadOnly
 .    EditTriggers$
 None=�   NavigationBehavior$ RowOnly=�   SelectionMode$ Single=�   AutoCreateColumns .     Columns   Xceed.Wpf.DataGrid.Column     	FieldName$ Tesis��   IsMainColumn . -   TituloAcuerdo��   Title$ Acuerdo��   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	 100    TextTrimming$ WordEllipsis=�   TextWrapping$ WrapWithOverflow=�   ReadOnly .   $ Rubro�� . $ WordEllipsis=�$ WrapWithOverflow=�$	 400 $
 Tema��  $	 Loc�� . $ WordEllipsis=�$ WrapWithOverflow=�$ Localización��  $ Id��$ Registro��   View (  "Xceed.Wpf.DataGrid.Views.TableView  '  !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     RowSelectorPaneWidth$ 50Q�   FixedHeaders �� .R�)  #Xceed.Wpf.DataGrid.ColumnManagerRow  $��Stretch=�   	AllowSort .     AllowColumnReorder . $	��Top=�   Theme 3	  -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme	  ����  �� 
TargetType 
  Xceed.Wpf.DataGrid.DataRow" 
 .��� $  Xceed.Wpf.DataGrid.RowSelector �� Property �@ RowSelectorStyle  itemIndexSelectorStyle  �� Value#  [ ��  �@ RowSelectorStyle#  [ ��  � ��  �� ! �� RelativeSource! �� " �� AncestorType2  ,mx.gob.scjn.electoral.TablaResultadoAcuerdos""  $ 	RowHeight�# �� Mode$# TwoWay=���  � ��$  15��-   .� -   $��impresionViewer��  FDPVTemplate#��[ $y�Hidden=�
�������$��0,0,0,40q�$��0��$��0���� -   $��imprimePapel��$	��0,0q�$y�Hidden=�  ImprimePapelBoton#��[ $��40��$��40��$��3��$
��Left=�$	��Top=�$ Imprimir el documento���� -   $��BtnTache��$y�Hidden=�$
��40,0q�$��40��$��40��$
��Left=�$	��Top=�$ Cerrar vista preliminar��  
TacheBoton#��[ �� -   $��	Expresion��$��2��$��3��$��2��{�.  �� -   $��Esperar��$��0��$��20��$��0��$��10��
���   �$ J� Opacity$	$ 0.7Q�$y�Hidden=�.+��� -   $��EsperarStack��
�������$��150,150,0,0q�$$ 3Q�.+��� Espere por favor� -   $��EsperaBarra��$��0,0,0,0q�$��26��$	��393���� -   $��DocumentoGuardar��$y�Hidden=�
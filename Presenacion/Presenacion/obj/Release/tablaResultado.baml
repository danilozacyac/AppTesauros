   M S B A M L   `   `   `  ���� R  NPresenacion, Version=1.5.0.0, Culture=neutral, PublicKeyToken=e517cdc7fcfe666dU/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos!mx.gob.scjn.ius_common.gui.apoyos  N JGeneral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=c866414512d5116dtGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities clr-namespace:IUSIUS  O Kmscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem _ [JaStDev.ControlFramework, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3a25f60670705855wQclr-namespace:JaStDev.ControlFramework.Controls;assembly=JaStDev.ControlFramework!JaStDev.ControlFramework.Controls     IUS.tablaResultado   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l	 hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6`
 \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid  	 
   8x,http://schemas.microsoft.com/winfx/2006/xaml    ?local/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos    WutilsGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    iusclr-namespace:IUS    3my&clr-namespace:System;assembly=mscorlib    `helpQclr-namespace:JaStDev.ControlFramework.Controls;assembly=JaStDev.ControlFramework    -     N� WindowTitle$   Visualizar registros tesis��,  &JaStDev.ControlFramework.Controls.Help   HelpFile$ IUS.chm��   Topic$  Visualizacion_de_las_tesis��}�.   � 	Resources  �� MergedDictionaries �� Source��  �� $' !/General;component/Resources.xaml?�%    .���� -   $��Borde��   BorderStyle#��[  ._�� =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid  	 
    � ColumnDefinitions �� $��10��$��10 ��� $��40��$��40 ��� $��40��$��40 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $	��100��$	��100 ��� $��40 ��� $��* ��� $��40��$��40 ��� $��40 ��� $��80 ��� $��10��$��10 � � RowDefinitions �� $��30 ��� $��40 ��� $��20 ��� $��* ��� $��30 ��� $��10 �.+�� $��4��$��0�� �� $	��12* ��� $	��18* � �� �� �� �� .+��� -   $��anterior�� � ToolTip$ Registro anterior��  AnteriorBoton#��[ $��-6,0,1,0q�$��2���� -   $��	siguiente��$ Siguiente registro��  SiguienteBoton#��[ $��2��$��0,0,20,0q�$��2��$��1���� -   $��final��$��2��$ Ir al último registro��  
FinalBoton#��[ $��9,0,11,0q�$��2��$��2��� -   $��Letras��$y�Hidden=�$��2��$��1��$��0,0,0,0q�$��9�� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� �� .+��� -   $��LetraA��$��0��.��A�� -   $��LetraB��$��1��.��B�� -	   $��LetraC��$��2��.��C�� -
   $��LetraD��$��3��.��D�� -   $��LetraE��$��4��.��E�� -   $��LetraF��$��5��.��F�� -   $��LetraG��$��6��.��G�� -   $��LetraH��$��7��.��H�� -   $��LetraI��$��8��.��I�� -   $��LetraJ��$��9��.��J�� -   $��LetraK��$��10��.��K�� -   $��LetraL��$��11��.��L�� -   $��LetraM��$��12��.��M�� -   $��LetraN��$��13��.��N�� -   $��LetraO��$��14��.��O�� -   $��LetraP��$��15��.��P�� -   $��LetraQ��$��16��.��Q�� -   $��LetraR��$��17��.��R�� -   $��LetraS��$��18��.��S�� -   $��LetraT��$��19��.��T�� -   $��LetraU��$��20��.��U�� -   $��LetraV��$��21��.��V�� -   $��LetraW��$��22��.��W�� -   $��LetraX��$��23��.��X�� -   $��LetraY��$��24��.��Y�� -    $��LetraZ��$��25��.��Z�� -!   $��
LetraOtros��$��26��.��#�� -"   $��Titulo��$��0��$��4��  
LabelStyle#��[ $��0��.��Resultado de la búsqueda�� -#   $��imprimir��$��1��$��1��$��0,0,0,0q�$��40��$( "Ver vista preliminar de impresión��  ImprimeBoton#��[ �� -$   $��original��$��2��$��1��$��40��  OriginalBoton#��[ $ Restablecer���� -%   $��jurisprudencia��$��4��$��1��$��0,0,0,0q�$
��Fill=�$4��./General;component/images/jurisprudencia-1.png��$% Mostrar tesis de jurisprudencia���� -&   $��aisladas��$��3��$��1��$��0,0,0,0q�$ Mostrar tesis aisladas��$,��&/General;component/images/Aisladas.png��$
��Fill=��� -'   $��contradiccion��$��8��$��1��$��0,5,0,5q�$B <Mostrar jurisprudencias derivadas de contradicción de tesis��$
��Fill=�$1��+/General;component/images/contradiccion.png���� -(   $��controversias��$��6��$��1��$��0,5,0,5q�$I CMostrar jurisprudencias derivadas de controversias constitucionales��$1��+/General;component/images/CONTROVERSIAS.png��$
��Fill=��� -)   $��reiteraciones��$��7��$��1��$��0,5,0,5q�$. (Mostrar jurisprudencias por reiteración��$1��+/General;component/images/REITERACIONES.png��$
��Fill=��� -*   $��acciones��$��5��$��1��$��0,5,0,5q�$,��&/General;component/images/ACCIONES.png��$
��Fill=�$K EMostrar jurisprudencias derivadas de acciones de inconstitucionalidad���� -+   $��Guardar��$��9��$��1��  GuardarBoton#��[ $&  Guardar resultado de la consulta��y���   Visibilidad	 �� Source#	 [ 
 �� Path$
 ObtenVisibilidadInternet��� -,   $��	Contenido��$y�Hidden=��� --   $��	Almacenar��$��10��$��1��$��0,0,0,0q�$ Almacenar consulta��$y�Hidden=� 	 	BuroBoton#��[	 �� -.   $��salir��$��12��$��1��$��0,0,0,0q� 
 RegresarBoton#��[
 $% Regresar a la pantalla anterior���� -/   $��RegistrosLabel��$��5��#��[ $��14(�$
��Bold"�$��2��.��
Registros:�� -0   $��
LblFiltros��$��13��$��2��#��[ .���� $��WrapWithOverflow=�Filtros realizados:�� -1   $��Filtros��$��2��$��1��$��13��( 
 "Xceed.Wpf.DataGrid.DataGridControl  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid  	 
   -2   $��tablaResultados��$��3��$��14��$��0,0,0,0q�
�������   SelectedIndex$ 0��   AutoCreateColumns .     ReadOnly . $	��100�� �� IsTextSearchEnabled . $
��Left=�   NavigationBehavior$ RowOnly=�   SelectionMode$ Single=�
�������   AllowDetailToggle .     Columns  
 Xceed.Wpf.DataGrid.Column     	FieldName$ Tesis��   ReadOnly .  �� TextAlignment$ Center=�   TextWrapping$ WrapWithOverflow=�   Title$ Tesis��   Width8 
 2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$ 75   $ Rubro�� .    TextTrimming$ WordEllipsis=�$ WrapWithOverflow=�$ Rubro��$	 260   $ LocAbr�� . $ Localización��$ CharacterEllipsis=�$ WrapWithOverflow=�  $
 Sala�� . $ 	Instancia��$ 60   $ 
ImageOther�� . $ Tipo de Tesis��  ImageContentPresenter   CellContentTemplate# [   $	 Ius��   IsMainColumn .  . $	 IUS��$ 50    View ( 
 "Xceed.Wpf.DataGrid.Views.TableView  -3     Name$ 	tableView��' 
 !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     RowSelectorPaneWidth$ 50Q�    FixedHeaders  �� .R�) 
 #Xceed.Wpf.DataGrid.ColumnManagerRow  $��Stretch=�!   	AllowSort! .  "   AllowColumnReorder" . $	��Top=�#   Theme# 3 
 -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme  ���� .�-4   �� $	 
 Xceed.Wpf.DataGrid.RowSelector$ �� Property$ �@	 RowSelectorStyle  itemIndexSelectorStyle% �� Value#% [ �� $ � ��% �� & �� RelativeSource& �� ' �� AncestorType
   IUS.tablaResultado"' 
 $
 	RowHeight�( �� Mode$( TwoWay=��� $ � ��$% 15��) �� Triggers) �� * �� Binding* �� $
 IsJuris�+ �� Value+ true.a��� $ � ��$
% Bold"�.� -5   $��impresionViewer��$y�Hidden=�
�������
���   �  FDPVTemplate#��[ $��1��$��3��$��10��$��0,0,0,0q��� -6   $��imprimePapel��$��1��$��0,0,0,0q�$y�Hidden=�$ Imprimir el documento��$��40��  ImprimePapelBoton#��[ $��40��$��3��$	��Top=��� -7   $��BtnTache��$��2��$��4��$��3��$y�Hidden=�$	��0,0q�$ Cerrar vista preliminar��$
��Left=�$��40��$��40��$	��Top=�  
TacheBoton#��[ �� -8   $��inicio��$��3��$ Ir al primer registro��  InicioBoton#��[ $��Right=�$��30��$��0,0,6,0q��� -9   $��OrdenarPorImage��$��7��$��30��$��0,0,70,0q�$ Ordenar por...��$��Right=�  OrdenarBoton#��[ �� -:   $��ImgPonentes��$��7��$��Right=�$ Localizar por ponente��$
��30,0q�$��30��$��30��$5��//General;component/images/Filtrar-PONENCIA1.png���� -;   $��
ImgAsuntos��$��30��$��30��$��7��$	��0,0q�$��Right=�$ Localizar por asunto��$3��-/General;component/images/Filtrar-ASUNTO1.png���� -<   $��lblIrA��$��8��$��2��$��Right=�$��0,0,0,0q�$��80��$��2��$	��Top=�#��[ .���� $��WrapWithOverflow=�$��2��Ir a:'  !Xceed.Wpf.Controls.NumericTextBox  =xcdg*http://schemas.xceed.com/wpf/xaml/datagrid  	 
   -=   $��IrANum��$��9��$��2��$��0,0,0,0q�, �� 	MaxLength$, 6��*  $Xceed.Wpf.Controls.ValueRangeTextBox-   ValueDataType"- ��.   NumberStyles$
. None=��� ->   $��btnIrA��$��11��$��30��$��0,0,0,0q�
�������
�������$* $Ir a un número consecutivo/registro��
���  ��.��Ir�� -?   $��BtnVisualizar��$��11��$��1��$ 
Visualizar��  VisualizarBoton#��[ �� -@   $��BtnAumentaAlto��$��11��$��2��
���  ��
�������
�������$# Aumenta el alto de las celdas��.��+�� -A   $��BtnDisminuyeAlto��$��12��$��2��
���  ��
�������
�������$% Disminuye el alto de las Ccldas��.��-�� -B   $��idExpresion��#��[ $��1��$��4��$��2��.��Expresión:�� -C   $��LblExpresion��
�������
���   �$��2q�$��3��$��4��$��0,0,0,0q���. $
��Auto=�$
��Auto=�$��6��.���� .���� -D   $��	Expresion��$
��Auto=�$
��Auto=���. $��WrapWithOverflow=�$��30��� -E   $��PgbTotalTesis��$��10��$��4��$��4��$��15���� .+�2   ,mx.gob.scjn.ius_common.gui.apoyos.OrdenarPor  -F   /  Name$/ 
PnlOrdenar��0 �� Left$0 50��
1 �� Top$1 50��$y�Hidden=�6   0mx.gob.scjn.ius_common.gui.apoyos.ListadoPonente  -G   2  Name$2 PnlPonentes��$0 50��$1 50��$y�Hidden=�6   0mx.gob.scjn.ius_common.gui.apoyos.ListadoAsuntos  -H   3  Name$3 
PnlAsuntos��$0 50��$1 50��$y�Hidden=�8   2mx.gob.scjn.ius_common.gui.apoyos.AlmacenaBusqueda  -I   4  Name$4 PnlAlmacenar��$0 50��$1 50��$y�Hidden=��� -J   $��Esperar��$��0��$��20��$��0��$��10��
���   �5 J� Opacity$	5 0.8Q�$y�Hidden=�.+��� -K   $��EsperarStack��
�������$��150,150,0,0q�$5 3Q�.+��� -L   $��
TxbEsperar��Espere por favor� -M   $��EsperaBarra��$��0,0,0,0q�$��26��$	��393���� -N   $��Cancelar��.��
Cancelar
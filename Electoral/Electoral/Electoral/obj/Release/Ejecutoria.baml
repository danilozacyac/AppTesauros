   M S B A M L   `   `   `  ���� P  LElectoral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ae5cf9d2702abc06a5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos'mx.gob.scjn.electoral_common.gui.apoyos  &     mx.gob.scjn.electoral.Ejecutoria   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    Elocal5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos    l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -     N� WindowTitle$  	Sentencia��$	��750�� � 	Resources  �� MergedDictionaries �� Source��  �� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._�� -   $��	Controles�� � ColumnDefinitions �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��80 ��� $��* ��� $��40 � � RowDefinitions �� $��40 ��� $��40 ��� $��27 ��� $��27 ��� $��* ��� $��30 ��� $��10 �.+���  ��Language$ es-MX�-   $��RtbCopyPaste��$y�Hidden=��� -   $��	TbxCopiar��$y�Hidden=��� -   $��Guardar��$y�	Collapsed=� � ToolTip$ Guardar en archivo��  GuardarBoton#��[ �� -   $��Portapapeles��$y�	Collapsed=�$ Portapapeles��$��1��  PortapapelesBoton#��[ �� -   $��	FontMayor��$ Aumentar tamaño de letra��$��2��$��0��$��0,0,0,0q�$��40��$
��Left=�  FontMayorBoton#��[ �� -   $��	FontMenor��$��3��$��0��$  Disminuir tamaño de letra��$��0,0,0,0q�$��40��$
��Left=�  FontMenorBoton#��[ �� -	   $��
MarcarTodo��$��0��$y�Hidden=�$
��Left=�$��0,0,0,0q�$��40��$��40��$��4��$ Marcar todo��  MarcarTodoBoton#��[ �� -
   $��	Desmarcar��$��0��$y�Hidden=�$��0,0,0,0q�$��Right=�$��40��$��40��$��5��$ Desmarcar todo��  DesmarcarBoton#��[ �� -   $��Marcar��$��0��$y�Hidden=�$��0,0,0,0q�$��40��$��40��$��6��$
��Fill=�$ Marcar registro��$+��%/General;component/images/MARCAR1.png���� -   $��inicioLista��$��30��$��8��$ Ir al primer registro��$	��Top=�  InicioListaBoton#��[ $
��Left=�$��35���� -   $��anteriorLista��$��40��$��9��$ Registro anterior��  AnteriorListaBoton#��[ $	��Top=�$
��Left=�$��40���� -   $��siguienteLista��$��40��$��10��$ Siguiente registro�� 	 SiguienteListaBoton#��[	 $	��Top=�$
��Left=�$��40���� -   $��ultimoLista��$��30��$
��Left=�$��11��$ Ir al último registro�� 
 FinalListaBoton#��[
 $	��Top=�$��35���� -   $��IrALabel��  UIStyle#��[ $��40��$��Right=�$��13��$��2��$	��Top=�$��80��.���� $��WrapWithOverflow=�Ir al Consecutivo:'  !Xceed.Wpf.Controls.NumericTextBox  -   $��regNum��$��25.001��   NumberStyles$
 None=�$��15��	 �� 	MaxLength$	 6��$	��Top=��� -   $��IrBoton��$��30��
���""��
�������
�������$��16��$ Ir a número consecutivo��$	��Top=�$��40��.��Ir�� -   $��RegNum��$��25��$��17��$	��Top=�
�������
���   �$��2q�.��Label�� -   $��Salir��$��40��$��Right=�$	��Top=�$��40��  RegresarBoton#��[ $% Regresar a la pantalla anterior��$��19���� -   $��Imprimir��$��40��$
��Left=�$��1��$��1��$ Imprimir��$y�	Collapsed=�  ImprimeBoton#��[ $	��Top=�$��40���� -   $��docCompletoImage��$��40��$
��Left=�$ Ver documento completo��$��2��$��1��$	��Top=�$��40��  DocumentoCompletoBoton#��[ �� -   $��TablasAnexos��$��3��$��1��$ Tablas y anexos��  
TablaBoton#��[ �� -   $��tesis��$��40��$
��Left=�$ Tesis relacionadas��$��4��$��1��  
TesisBoton#��[ $	��Top=�$��40���� -   $
��voto��$��Center=�$��0,0,0,0q�$��67��$��22��$��5��$��1��$��3��  	VotoBoton#��[ $��Center=�$ Votos relacionados���� -   $��parteInicio��$��30��$
��Left=�$��8��$��1��  InicioBoton#��[ $	��Top=�$��30��$ Ir a la primera parte���� -   $��parteAnterior��$��9��$��1��$ Ir a la parte anterior��  AnteriorBoton#��[ $	��Top=�$
��Left=��� -   $��parteSiguiente��$��10��$��1��  SiguienteBoton#��[ $ Ir a la siguiente parte��$	��Top=�$
��Left=��� -   $��
parteFinal��$��30��$��11��$��1��$ Ir a la última parte��  
FinalBoton#��[ $	��Top=�$
��Left=�$��30���� -   $��
EpocaLabel��$��0��$��2��$��27��$
��Left=�$��3��$	��Top=�$	��120��$��2q�
�������
���   �.��Label�� -   $��VolumenLabel��$��27��$
��Left=�$��3��$��2��$��4��$	��Top=�$	��160��$��2q�
�������
���   �.��Fecha�� -    $��PaginaLabel��$��27��$��7��$��2��$��4��$	��Top=�$
��Left=�$	��160��$��2q�
�������
���   �.��Label�� -!   $��IdLabel��$��27��$
��Left=�$��17��$��2��$	��Top=�$��80��
���  ��$��2q�
�������
���   �.��Id�� -"   $��NumeroPartes��$��27��$��3��$��12��$��4��$	��Top=�
�������$��2q�
���   �$
��Left=�$	��160��.��Label�� -#   $��fuenteLabel��$��27��$��3��$��0��$��7��$	��Top=�$
��Left=�$	��280��$��2q�
�������
���   �.��Fuente�� -$   $��	SalaLabel��$��27��$��7��$��3��$��5��$	��Top=�$
��Left=�$	��200��$��2q�
�������
���   �.��	Instancia.� -%   $��	impresion��
�������$��4��$��22��  FDPVTemplate#��[ $y�Hidden=��� -&   $��imprimePapel��$ Imprimir el documento��$��40��$��40��$��0��$��4��$	��Top=�$��Right=�  ImprimePapelBoton#��[ �� -'   $��BtnTache��$	��Top=�$y�Hidden=�$ Cerrar vista preliminar��$��40��$��40��$��1��$��4��  
TacheBoton#��[ $
��Left=��� -(   $��tabControl1��$��0��$��22��$��4��$
��Auto��.@��� -)   $��TabTexto��$��Texto��$: 4Para imprimir o copiar use el documento digitalizado��
 K� ZIndex$
 1��.���� $ es-MX�-*   $��contenidoTexto��$
��Left=�$
 0��" �� VerticalScrollBarVisibility$
 Auto=�$��0,0,0,0q��� -+   $��TabDoc��$��Documento digitalizado��.�� � -,   $��FrmDoc���� --   $��LblPalabraBuscar��#��[ $��3��$��5��.��Palabra a buscar:�� -.   $��TextoABuscar��$��5��$��6��$��3��$��27��$
��Left=�$	 80��$��Bottom=�$	��230���� -/   $��BuscarImage��$��5��$��9��$��0,0,0,0q�$ Buscar��$��Bottom=�$
��Left=�  BuscarBoton#��[ �� -0   $��	Expresion��$��10��$��5��$��12��
�������$��25��$��0,0,0,0q��� -1   $��ventanasEmergentes��$��0��$��0��$
 20��.+�:   4mx.gob.scjn.electoral_common.gui.apoyos.ListadoVotos  -2     Name$ ventanaListadoVotos��$y�Hidden=� �� Left$	 110��
 �� Top$	 127��$
 6��:   4mx.gob.scjn.electoral_common.gui.apoyos.ListadoTesis  -3     Name$ ventanaListadoTesis��$y�Hidden=�$	 203��$	 127��$
 5��:   4mx.gob.scjn.electoral_common.gui.apoyos.ObtenerRango  -4     Name$ ventanaRangos��$y�Hidden=�$	 200��$	 136��$	��400��$	��200��$
 4��
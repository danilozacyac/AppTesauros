   M S B A M L   `   `   `  ���� P  LElectoral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ae5cf9d2702abc06a5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos'mx.gob.scjn.electoral_common.gui.apoyos  $    mx.gob.scjn.electoral.Acuerdos   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    Elocal5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos    -     N� WindowTitle$  Acuerdo�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._�� .+��  � ColumnDefinitions �� $��10 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��10 � � RowDefinitions �� $��40 ��� $��40 ��� $��30 ��� $��30 ��� $��* ��� $��30 ��� $��10 �.+���  ��Language$ es-MX�-   $��RtbCopyPaste��$y�Hidden=��� -   $��	TbxCopiar��$y�Hidden=��� -   $��Guardar��$��0,0,0,0q�$��1��$��0��$y�	Collapsed=� � ToolTip$ Guardar en archivo��  GuardarBoton#��[ �� -   $��PortaPapeles��$��0,0,0,0q�$��2��$��0��$y�	Collapsed=�$ Portapapeles��  PortapapelesBoton#��[ �� -   $��	historial��$��40��$
��Left=�$y�Hidden=�$��1��$��1��$* $Historial de documentos relacionados��$	��Top=�$��40��  HistorialBoton#��[ �� -   $��Imprimir��$��40��$
��Left=�$��2��$��1��$y�	Collapsed=�  ImprimeBoton#��[ $	��Top=�$��40��$ Imprimir���� -   $��docCompletoImage��$��40��$
��Left=�$��3��$��1��$	��Top=�$��40��$ Ver documento completo��  DocumentoCompletoBoton#��[ �� -	   $��	FontMenor��$��40��$��3��$
��Left=�$	��Top=�$  Disminuir tamaño de letra��  FontMenorBoton#��[ �� -
   $��	FontMayor��$��40��$��4��$
��Left=�$	��Top=�$ Aumentar tamaño de letra��  FontMayorBoton#��[ �� -   $��inicioLista��$��30��$��Right=�$��9��$	��Top=�$��30��$ Ir al primer registro��  InicioListaBoton#��[ �� -   $��anteriorLista��$��40��$��10�� 	 AnteriorListaBoton#��[	 $	��Top=�$ Registro anterior��$
��Left=�$��40���� -   $��siguienteLista��$��40��$
��Left=�$��11��$ Siguiente registro�� 
 SiguienteListaBoton#��[
 $	��Top=�$��40���� -   $��ultimoLista��$��30��$
��Left=�$��12��$ Ir al último registro��  FinalListaBoton#��[ $	��Top=�$��30���� -   $��IrALabel��$��39��$��12��$��3��$��Right=�  UIStyle#��[ $	��Top=�$��85��.���� -   $��BloqueTextoIrA��$��WrapWithOverflow=�$��Right=�$	��Top=�Ir al Consecutivo:` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6'  !Xceed.Wpf.Controls.NumericTextBox  l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid      -   $��regNum��$��27��$��Right=� �� 	MaxLength$ 6��   NumberStyles$
 None=�$��15��$	��Top=�$��45���� -   $��IrBoton��$��30��
�������
���""��
�������$
��Left=�$��16��$ Ir al número consecutivo��$	��Top=�$��28��.��Ir�� -   $��RegNum��$��25��$��Right=�$��17��$��2��$	��Top=�$��71��
�������
���   �$��2q�.��Label�� -   $��Salir��$��40��$��Right=�$��19��$	��Top=�$��40��  RegresarBoton#��[ $% Regresar a la pantalla anterior���� -   $��parteInicio��$��30��$��1��$��9��$��Right=�  InicioBoton#��[ $	��Top=�$��30��$ Ir a la primera parte���� -   $��parteAnterior��$��1��$��10��$��40��$��40��  AnteriorBoton#��[ $	��Top=�$ Ir a la parte anterior��$
��Left=��� -   $��parteSiguiente��$��40��$��1��$��11��  SiguienteBoton#��[ $	��Top=�$
��Left=�$��40��$ Ir a la siguiente parte���� -   $��
parteFinal��$��12��$��1��$��30��  
FinalBoton#��[ $	��Top=�$
��Left=�$��30��$ Ir a la última parte���� -   $��
EpocaLabel��$��30��$
��Left=�$��2��$��0,0,0,0q�$��1��$��4��$	��160��$	��Top=�$��2q�
�������
���   �.��Epoca�� -   $��VolumenLabel��$��30��$
��Left=�$��5��$��2��$��5��$	��Top=�$	��200��$��2q�
�������
���   �.��Fecha�� -   $��PaginaLabel��$��30��$��10��$��2��$��4��$	��Top=�$
��Left=�$	��160��$��2q�
�������
���   �.��Pagina�� -   $��IdLabel��$��30��$��Right=�$��16��$��2��$��3��$	��Top=�$��65��
���  ��$��2q�
�������
���   �.��Id�� -   $��LblSala��$��27��$��3��$��8��$��7��$	��Top=�$
��Left=�$	��280��$��2q�
�������
���   �.���� .���� -   $��	SalaLabel��$��WrapWithOverflow=��� -   $��NumeroPartes��$��27��$��3��$��15��$��4��$	��Top=�
�������$��2q�
���   �$
��Left=�$	��200��.��Partes�� -    $��fuenteLabel��$��27��$��1��$��3��$��7��$	��Top=�$
��Left=�$	��280��$��2q�
�������
���   �.��Fuente�� -!   $��tabControl1��$��1��$��4��$��19��$
��Auto��.@��� -"   $��TabTexto��$��Texto��$: 4Para imprimir o copiar use el documento digitalizado��.���� $ es-MX�-#   $��contenidoTexto��$
��Left=�"	 �� VerticalScrollBarVisibility$
	 Auto=�$��0,0,0,0q��� -$   $��TabDocumento��$��Documento Digitalizado��.�� � -%   $��FrmDocumento��.� -&   $��	impresion��{�. 
�������$��1��$��4��$��19��$��2��  FDPVTemplate#��[ $y�Hidden=��� -'   $��LblPalabraBuscar��#��[ $��5��$��4��.��Palabra a buscar�� -(   $��textoAbuscar��$��10,0,0,0q�$��3��$��5��$��30��$	��Top=�$ 80��$��5���� -)   $��Buscar��$��8��$��5��$ Buscar��  BuscarBoton#��[ �� -*   $��	Expresion��$��0,0,0,0q�$��9��$��5��$��12��
�������$��2q�
���   ��� $��0��$��0��.+��� -+   $��imprimePapel��
 �� Left$
 40��
 �� Top$ 98��$��40��$ Imprimir el documento��$��40��$y�Hidden=�  ImprimePapelBoton#��[ �� -,   $��BtnTache��$
 80��$ 98��$��40��$��40��$y�Hidden=�$ Cerrar vista preliminar��  
TacheBoton#��[ 
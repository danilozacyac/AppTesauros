   M S B A M L   `   `   `  ���� P  LElectoral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=ae5cf9d2702abc06a5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos'mx.gob.scjn.electoral_common.gui.apoyos  N JGeneral, Version=1.5.0.0, Culture=neutral, PublicKeyToken=c866414512d5116dtGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities !    mx.gob.scjn.electoral.Votos   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    Elocal5clr-namespace:mx.gob.scjn.electoral_common.gui.apoyos    [	utilitiesGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    -     N� WindowTitle$  Consulta de votos�� � 	Resources  �� Source�� $' !/General;component/Resources.xaml?�%    .����    BorderStyle#��[  ._��  � ColumnDefinitions �� $��10 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��* ��� $��10 � � RowDefinitions �� $��40 ��� $��40 ��� $��27 ��� $��27 ��� $��* ��� $��30 ��� $��10 �.+���  ��Language$ es-MX�-   $��RtbCopyPaste��$y�Hidden=��� -   $��	TbxCopiar��$y�Hidden=��� -   $��PortaPapeles��$��2�� � ToolTip$ Portapapeles��$y�	Collapsed=�  PortapapelesBoton#��[ $��Right=�$��40���� -   $��Guardar��$��0,0,0,0q�$��1��$��0��$y�	Collapsed=�$ Guardar en archivo��  GuardarBoton#��[ �� -   $��	FontMayor��$��3��$��0��$ Aumentar tamaño de letra��$��0,0,0,0q�$��40��$
��Left=�  FontMayorBoton#��[ �� -   $��	FontMenor��$��4��$��0��$  Disminuir tamaño de letra��$��0,0,0,0q�$��40��$
��Left=�  FontMenorBoton#��[ �� -   $��docCompletoImage��$��3��$ Ver documento completo��  DocumentoCompletoBoton#��[ $��1���� -	   $��inicioLista��$��30��$��9��$��Right=�$	��Top=�$��30��$ Ir al primer registro��  InicioListaBoton#��[ �� -
   $��anteriorLista��$��40��$��10��$ Registro anterior��  AnteriorListaBoton#��[ $	��Top=�$��40��$��Right=��� -   $��siguienteLista��$��40��$��11��$ Siguiente Registro��  SiguienteListaBoton#��[ $	��Top=�$
��Left=�$��40���� -   $��ultimoLista��$��30��$
��Left=�$��12��$ Ir al último registro�� 	 FinalListaBoton#��[	 $	��Top=�$��30���� -   $��Salir��$��40��$
��Left=�$��18��$	��Top=�$��40��$% Regresar a la pantalla anterior�� 
 RegresarBoton#��[
 �� -   $��Imprimir��$��40��$
��Left=�$��2��$��1��$y�	Collapsed=�$ Imprimir��  ImprimeBoton#��[ $	��Top=�$��40���� -   $��tesis��$��40��$
��Left=�$��4��$��1��$ Tesis relacionadas��  
TesisBoton#��[ $	��Top=�$��40���� -   $��
ejecutoria��$
��Left=�$��5��$��1��$��2��  SentenciaBoton#��[ $ Sentencias relacionadas��$��Center=��� -   $��parteInicio��$��25��$��Right=�$��9��$��1��  InicioBoton#��[ $	��Top=�$��25��$ Ir a la primera parte���� -   $��parteAnterior��$��40��$��10��$��1��$ Ir a la parte anterior��  AnteriorBoton#��[ $	��Top=�$��40��$
��Left=��� -   $��parteSiguiente��$��40��$��1��$��11��$ Ir a la siguiente parte��  SiguienteBoton#��[ $	��Top=�$
��Left=�$��40���� -   $��
parteFinal��$��25��$��12��$��1��$ Ir a la última parte��  
FinalBoton#��[ $	��Top=�$
��Left=�$��25���� -   $��VolumenLabel��$
��Left=�$��4��$��2��$��4��$	��Top=�$	��160��$��2q�
�������
���   �.��Fecha�� -   $��
EpocaLabel��$
��Left=�$��2��$��1��$��3��$	��Top=�$	��120��$��2q�
�������
���   �.��Epoca�� -   $��PaginaLabel��$��8��$��2��$��4��$	��Top=�$
��Left=�$	��160��$��2q�
�������
���   �.��Pagina�� -   $��IdLabel��$
��Left=�$��15��$��2��$��3��$	��Top=�$	��120��
���  ��$��2q�
�������
���   �.��Id�� -   $��fuenteLabel��$��3��$��1��$��7��$	��Top=�$
��Left=�$	��280��$��2q�
�������
���   �.��Fuente�� -   $��	SalaLabel��$��25��$��8��$��3��$��5��$	��Top=�$
��Left=�$	��200��$��2q�
�������
���   �.��	Instancia�� -   $��IrBoton��$��30��
���  ��
�������$
��Left=�$��17��$ Ir a número consecutivo��$	��Top=�$��28��.��Ir�� -   $��RegNum��$
��Left=�$��13��$��3��$��2��$	��Top=�$��80��
�������
���   �$��2q�.��RegNum�� -   $��NumeroPartes��$��12��$��2��$��3��$	��Top=�
�������$��2q�
���   �$
��Left=�$	��120��.��Label�� -   $��IrALabel��$��Right=�  UIStyle#��[ $��13��$��2��$	��Top=�$��80��.���� -   $��BloqueTextoIrA��$��WrapWithOverflow=�$
��Left=�$	��Top=�Ir al Consecutivo:` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6'  !Xceed.Wpf.Controls.NumericTextBox  l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l	 hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid     	 -    $��regNum��$��28�� �� 	MaxLength$ 6��   NumberStyles$
 None=�$��15��$��2��$	��Top=�$
��Left=�$��80���� -!   $��tabControl1��$��1��$��4��$��19��.@��� -"   $��TabTexto��$��Texto��$: 4Para imprimir o copiar use el documento digitalizado��.���� $ es-MX�-#   $��contenidoTexto��$
��Left=�"	 �� VerticalScrollBarVisibility$
	 Auto=�$��0,0,0,0q��� -$   $��TbiDocumento��$��Documento Digitalizado��.�� � -%   $��FrmDocumento���� -&   $��LblPalabraBuscar��#��[ $��5��$��4��.��Palabra a Buscar:�� -'   $��textoAbuscar��$��10,0,0,0q�$��3��$��4��$ 80��$��5���� -(   $��Buscar��$��7��$��5��$ Buscar��  BuscarBoton#��[ �� -)   $��	Expresion��$��0,0,0,0q�$��8��$��5��$��12��
�������$��2q�
���   �.� -*   $��	impresion��
�������$��1��$��4��  FDPVTemplate#��[ $��19��$��2��$y�Hidden=��� $��0��$��0��.+�@   :mx.gob.scjn.electoral_common.gui.apoyos.ListadoEjecutorias  -+   
  Name$
 ventanaListadoEjecutorias��$y�Hidden=�$��102,80,104,27q�$	��191�� �� Left$	 -12��
 �� Top$ 58��$	��524��:   4mx.gob.scjn.electoral_common.gui.apoyos.ListadoTesis  -,     Name$ ventanaListadoTesis��$y�Hidden=�$��130,98,130,4q�$	��196��$ 1��$ 40��$	��471���� --   $��imprimePapel��$ Imprimir el documento��$ 40��$ 98��$��40��$��40��$y�Hidden=�  ImprimePapelBoton#��[ �� -.   $��BtnTache��$ 80��$ 98��$ Cerrar vista preliminar��$��40��$��40��  
TacheBoton#��[ $y�Hidden=�
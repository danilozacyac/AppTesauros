   M S B A M L   `   `   `  ����F  BPresenacion, Version=1.5.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos!mx.gob.scjn.ius_common.gui.apoyos  5      B >General, Version=1.5.0.0, Culture=neutral, PublicKeyToken=nulltGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General(mx.gob.scjn.ius_common.gui.gui.utilities 5          IUS.VotosPagina   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      ?local/clr-namespace:mx.gob.scjn.ius_common.gui.apoyos    5      [	utilitiesGclr-namespace:mx.gob.scjn.ius_common.gui.gui.utilities;assembly=General    5      -   5          N� WindowTitle$  Consulta de votos��5       � 	Resources 5       �� Source�� 5   
   $' !/General;component/Resources.xaml?�6   %    .��5
      ��    BorderStyle#��[  6   ._�5   
   �  � ColumnDefinitions 5      �� 5      $��10 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5      $��40 �6#   �� 5       $��* �6#   �� 5!      $��10 �6#    � RowDefinitions 5#      �� 5$      $��40 �6    �� 5%      $��40 �6    �� 5&      $��27 �6    �� 5'      $��27 �6    �� 5(      $��* �6    �� 5)      $��30 �6    �� 5*      $��10 �6    .+�5,      ��  ��Language$ es-MX�6?   -   5        $��RtbCopyPaste��5,      $y�Hidden=�6?   �� 5-      -   5        $��	TbxCopiar��5-      $y�Hidden=�6'   �� 5.      -   5        $��PortaPapeles��5.   %   $��2��6    � ToolTip$ Portapapeles��5/        PortapapelesBoton#��[ 51      $��Right=�52      $��40��61   �� 53      -   5        $��Guardar��54      $��0,0,0,0q�53      $��1��6&   $��0��66   $ Guardar en archivo��56        GuardarBoton#��[ 57      y�55      ��   Visibilidad �� Source# [  �� Path$ ObtenVisibilidadInternet�58      �� 59      -   5        $��	FontMayor��59      $��3��6&   $��0��66   $ Aumentar tamaño de letra��5:      $��0,0,0,0q�5;      $��40��5<      $
��Left=�6      FontMayorBoton#��[ 5=      5>      �� 5?      -   5        $��	FontMenor��5?      $��4��6&   $��0��66   $  Disminuir tamaño de letra��5@      $��0,0,0,0q�5A      $��40��5B      $
��Left=�6      FontMenorBoton#��[ 5D      �� 5E      -   5        $��
MarcarTodo��5K      $��0��5E      $y�Hidden=�6!   $
��Left=�5F      $��0,0,0,0q�5G      $��40��5H      $��40��5I      $��5��5J      $
��Fill=�5L      $ Marcar todo��5M      $1��+/General;component/images/MARCAR TODOS1.png��5Q      �� 5R      -	   5        $��	Desmarcar��5Y      $��0��5R      $y�Hidden=�6!   $��0,0,0,0q�5S      $��Right=�5T      $��40��5U      $��40��5V      $��6��5W      $ Desmarcar todo��5X      $
��Fill=�5Z      $.��(/General;component/images/DESMARCAR1.png��5^      �� 5_      -
   5        $��Marcar��5d      $��0��5_      $y�Hidden=�6!   $��0,0,0,0q�5`      $��40��5a      $��40��5b      $��7��5c      $
��Fill=�5e      $ Marcar registro��5f      $+��%/General;component/images/MARCAR1.png��5j      �� 5k      -   5        $��docCompletoImage��5o      $��40��5k      $
��Left=�5l      $��8��5m      $ Ver documento completo��5n      $
��Fill=�5p      $	��Top=�5q      $��40��5r      $2��,/General;component/images/VER DOCUMENTO1.png��5s      5v      �� 5w      -   5        $��inicioLista��5y      $��30��5w      $��9��5x      $��Right=�6$   $
��Fill=�5y   '   $	��Top=�5z      $��30��6,   $ Ir al primer registro��5{      $.��(/General;component/images/FLECHA-BB1.png��5|      5      �� 5�      -   5        $��anteriorLista��5�      $��40��5�      $��10��5�      $ Registro anterior��5�      $-��'/General;component/images/FLECHA-B1.png��5�      $
��Fill=�5�      $	��Top=�6#   $��40��5�      $��Right=�6   �� 5�      -   5        $��siguienteLista��5�      $��40��5�      $��11��6    $ Siguiente Registro��5�      $-��'/General;component/images/FLECHA-A1.png��5�      $
��Fill=�5�      $	��Top=�5�      $
��Left=�5�      $��40��6/   �� 5�      -   5        $��ultimoLista��5�      $��30��5�      $
��Left=�6    $��12��5�      $ Ir al último registro��5�      $.��(/General;component/images/FLECHA-AA1.png��5�      $
��Fill=�5�      $	��Top=�6#   $��30��6;   5�      �� 5�      -   5        $��Salir��5�      $��40��5�      $
��Left=�5�      $��18��5�      $	��Top=�5�   !   $��40��69   $% Regresar a la pantalla anterior��5�        RegresarBoton#��[ 5�      �� 5�      -   5        $��	historial��5�      $��40��5�      $
��Left=�6    $��1��5�      $��1��6$   $* $Historial de documentos relacionados��5�      $
��Fill=�5�      $	��Top=�6#   $��40��5�      $.��(/General;component/images/HISTORIAL1.png��6   5�      �� 5�      -   5        $��Imprimir��5�      $��40��5�      $
��Left=�6!   $��2��5�      $��1��6$   $ Imprimir��5�        ImprimeBoton#��[ 5�      $	��Top=�5�      $��40��6,   5�      �� 5�      -   5        $��	BtnTablas��5�      $��1��6&   $��3��63     
TablaBoton#��[ 5�      $ Tablas y anexos��5�      5�      �� 5�      -   5        $��tesis��5�      $��40��5�      $
��Left=�5�      $��4��5�      $��1��6$   $ Tesis relacionadas��5�       	 
TesisBoton#��[	 5�      $	��Top=�5�      $��40��6,   5�      �� 5�      -   5        $��
ejecutoria��5�      $
��Left=�5�      $��5��5�      $��1��6$   $��2��61    
 EjecutoriaBoton#��[
 5�   &   $ Ejecutorias relacionadas��5�      $��Center=�5�      5�      �� 5�      -   5        $��parteInicio��5�      $��25��5�      $��Right=�6!   $��9��5�      $��1��6$     InicioBoton#��[ 5�      $	��Top=�5�      $��25��6,   $ Ir a la primera parte��5�      5�      �� 5�      -   5        $��parteAnterior��5�      $��40��5�      $��10��5�      $��1��6%   $ Ir a la parte anterior��5�        AnteriorBoton#��[ 5�      $	��Top=�5�      $��40��5�      $
��Left=�6   �� 5�      -   5        $��parteSiguiente��5�      $��40��5�      $��1��5�      $��11��6!   $ Ir a la siguiente parte��5�        SiguienteBoton#��[ 5�      $	��Top=�5�      $
��Left=�6,   $��40��6G   5�      �� 5�      -   5        $��
parteFinal��5�      $��25��5�      $��12��5�      $��1��5�      $ Ir a la última parte��5�        
FinalBoton#��[ 5�      $	��Top=�5�      $
��Left=�6,   $��25��6G   5�      �� 5�      -   5        $��VolumenLabel��5�      $
��Left=�5�      $��4��5�      $��2��6$   $��4��61   $	��Top=�5�      $	��160��5�      $��2q�6    
�������
���   �.��5�   ;   Fecha6B   �� 5�      -   5        $��
EpocaLabel��5�      $
��Left=�5�      $��2��5�      $��1��6!   $��3��61   $	��Top=�5�      $	��120��5�      $��2q�5�      
�������
���   �.��6O   Epoca6V   �� 5�      -   5        $��PaginaLabel��5�      $��8��5�      $��2��6$   $��3��61   $	��Top=�5�   '   $
��Left=�5�      $	��120��6/   $��2q�5�      
�������
���   �.��5�   (   Pagina60   �� 5�      -   5        $��IdLabel��5�      $
��Left=�5�      $��15��5�      $��2��6%   $��3��62   $	��Top=�5�      $	��120��5�      
���  ��$��2q�61   
�������
���   �.��5�   ;   Id6?   �� 5�      -   5        $��fuenteLabel��5�      $��3��5�      $��1��6!   $��7��61   $	��Top=�5�      $
��Left=�5�      $	��280��5�      $��2q�6    
�������
���   �.��5   ;   Fuente6C   �� 5     -   5        $��	SalaLabel��5     $��25��5     $��8��5     $��3��6$   $��5��61   $	��Top=�5     $
��Left=�5     $	��200��5     $��2q�5     
�������
���   �.��5  (   	Instancia63   �� 5	     -    5        $��IrBoton��5     $��30��5	     
���  ��
�������$
��Left=�5     $��17��5     $ Ir a número consecutivo��5     $	��Top=�5  $   $��28��6<   .��5  +   Ir6/   �� 5     -!   5        $��RegNum��5     $
��Left=�5     $��13��5     $��3��6%   $��2��62   $	��Top=�5  "   $��80��6:   
�������
���   �$��2q�5     .��6(   RegNum60   �� 5     -"   5        $��NumeroPartes��5     $��12��5     $��2��6%   $��3��62   $	��Top=�5  (   
�������$��2q�5  '   
���   �$
��Left=�5  (   $	��120��5     .��6    Label6'   �� 5     -#   5        $��IrALabel��5     $��Right=�5     $��13��5     $��2��6%   $	��Top=�5  $   $��80��6<   .��5     �� -$   5        $��BloqueTextoIrA��5     $��WrapWithOverflow=�5     $
��Left=�5     
�������$	��Top=�5!     Ir al Consecutivo:6D   5"     ` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6'  !Xceed.Wpf.Controls.NumericTextBox  5#     h dXceed.Wpf.DataGrid.Views3D, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l	 hXceed.Wpf.DataGrid.ThemePack.1, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6`
 \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6l hXceed.Wpf.DataGrid.ThemePack.2, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6=xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   	 
  5'  "   -%   5        $��regNum��5%  G   $��28��5#  "   	 �� 	MaxLength$	 6��6.   
   NumberStyles$

 None=�6<   $
��Left=�5$  "   $��15��5%  "   $��2��63   $	��Top=�5&  "   $��80��6:   5'  "   �� 5(     -&   5        $��tabControl1��5)     $��1��5(     $��4��6)   $��19��66   .@�5*     �� -'   5        $��TabTexto��5*     .��5+     �� $ es-MX�5.     -(   5        $��contenidoTexto��5.     $
��Left=�5+  "   " �� VerticalScrollBarVisibility$
 Auto=�5,     $��0,0,0,0q�5-     5.     5/     50     �� 51     -)   5        $��LblPalabraBuscar��51     
�������$��5��52     $��4��6!   .��65   Palabra a Buscar:6H   �� 53     -*   5        $��textoAbuscar��56  '   $��10,0,0,0q�53     $��3��6(   $��4��68   $	 80��55  5   $��5��56     6=   �� 57     -+   5        $��Buscar��58     $��7��57     $��5��6$   $
��Fill=�58  &   $ Buscar��59     $-��'/General;component/images/BUSQUEDA1.png��5:     5=     �� 5>     -,   5        $��	Expresion��5@     $��0,0,0,0q�5>     $��8��5?     $��5��6(   $��12��65   
�������$��2q�5B     
���   �5C     .� 5D     --   5        $��	impresion��5J  %   
�������$��1��5E  %   $��4��5F  %     FDPVTemplate#��[ 5G  %   $��19��5H  %   $��2��5I  %   $y�Hidden=�5K  %   �� 5L     $��0��6   $��0��6%   .+�5M     :   4mx.gob.scjn.ius_common.gui.apoyos.ListadoEjecutorias  -.   5          Name$ ventanaListadoEjecutorias��5M  +   $y�Hidden=�6N   $��102,80,104,27q�6b   $	��191��6y    �� Left$	 -12��6�   
 �� Top$ 58��6�   $	��524��6�   4   .mx.gob.scjn.ius_common.gui.apoyos.ListadoTesis  5N     -/   5          Name$ ventanaListadoTesis��5N  %   $y�Hidden=�6B   $��130,98,130,4q�6V   $	��196��6l   $ 1��6y   $ 40��6�   $	��471��6�   4   .mx.gob.scjn.ius_common.gui.apoyos.ObtenerRango  5O     -0   5          Name$ ventanaRangos��5O  %   $y�Hidden=�6<   $	 190��6P   $	 138��6b   $	��200��6s   $	��300��6�   8   2mx.gob.scjn.ius_common.gui.apoyos.VentanaHistorial  5P     -1   5          Name$ ventanaHistorial��5P  )   $y�Hidden=�6C   $��152,57,145,27q�6W   $	��214��6n   $ -3��6{   $ 77��6�   $	��433��6�   5   /mx.gob.scjn.ius_common.gui.apoyos.ListadoAnexos  5Q     -2   5          Name$ ventanaListadoAnexos��5Q  &   $y�Hidden=�6D   $��130,98,130,4q�6X   �� 5R     -3   5        $��imprimePapel��5W     $ Imprimir el documento��5T     $ 40��5V     $ 98��6)   $��40��69   $
��Fill=�5W  ,   $��40��6;   $y�Hidden=�6F   $-��'/General;component/images/IMPRIMIR1.png��5X     �� 5Y     -4   5        $��BtnTache��5Y     $ 80��6)   $ 98��6:   $ Cerrar vista preliminar��5Z     $��40��5[     $��40��6$     
TacheBoton#��[ 60   $y�Hidden=�5\     6-   5]     5^     5_     5`     
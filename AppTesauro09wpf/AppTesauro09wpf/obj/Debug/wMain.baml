   M S B A M L   `   `   `  ����E  AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls  5   	   J FAppTesauro09wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:AppTesauro09wpf.ConvertersAppTesauro09wpf.Converters 5   	   =#clr-namespace:AppTesauro09wpf.UtilsAppTesauro09wpf.Utils 5   	      AppTesauro09wpf.WMain   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   C
UIControls,clr-namespace:UIControls;assembly=UIControls     5   	   >	converter(clr-namespace:AppTesauro09wpf.Converters     5   	   e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k	 gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Etelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
 5   	   9	utilities#clr-namespace:AppTesauro09wpf.Utils   	 
 5   	   -   5        $
��1148��5   	   $	��742��5	   	   
�������  � WindowStartupLocation$  CenterScreen=�5   	    � WindowState$ 	Maximized=�5   	    � 	Resources 5      3 -AppTesauro09wpf.Utils.ConvertidorLetrasCampos   Convertidor/  )AppTesauro09wpf.Utils.ConvertidorOperador 
 ConvOp  lugares_celltemplate �� 	Converter  operador_celltemplate�� 5   
   %�   &	        &	      &	 &     7   &	 a     7    5      60     5      6,   �� 5      .R�5      �� ��6   �� 8   5      �� 5      .R�5      �� ��6   �� 8   5      5      .��5      �  � RowDefinitions 5   
   �� 5       $
��Auto �6   �� 5!      $��* �6   .+�5$   
   ,  &Telerik.Windows.Controls.RadRibbonView     ApplicationName$) #Sistema de Administración de Tesis��6    +  %Telerik.Windows.Controls.StyleManager   Theme-  'Telerik.Windows.Controls.ThemeConverter$ Windows8 6U   .@�5%      +  %Telerik.Windows.Controls.RadRibbonTab  .@�5&      -  'Telerik.Windows.Controls.RadRibbonGroup  $��Temas��6)   .@�5'      .  (Telerik.Windows.Controls.RadRibbonButton  -   5          Name$ RBtnNuevoTema��5'   .   $��Nuevo��5)   .      
LargeImage$ Resources/tree2_128.png��5*   .   	   Size$	 Large=�5+   .     5,      -   5        $ RBtnModifTema��5,   .   $��	Modificar��5.   .   $ Resources/tree2_128.png��5/   .   $	 Large=�50   .     51      -   5        $ RBtnDelTema��51   .   $��Eliminar��53   .   $ Resources/tree2_128.png��54   .   $	 Large=�55   .     56      -   5        $ RBtnImportTema��56   .   $��Importar��58   .   $ Resources/tree2_128.png��59   .   $	 Large=�5:   .   �� 5;      
 �� Orientation$
 Vertical=�6!   .+�5<        -   5        $��Cortar��5=   2   $	 Medium=�5>   2      
SmallImage$ Resources/cut_32.png��5?   2     5@      -   5        $��Copiar��5A   2   $	 Medium=�5B   2   $ Resources/copy_32.png��5C   2     5D      -   5        $��Pegar��5E   2   $	 Medium=�5F   2   $ Resources/paste_32.png��5G   2   5H      5J        5K      $��Tesis��6)   .@�5L        -	   5        $ RBtnEditTesisRel��5L   .   $) #Resources/1425495821_edit-notes.png��5N   .   $	 Large=�5O   .   .��5P      �� $
 Vertical=�6%   .+�5Q      �� $��Center=�6(   $��
Agregar o ��6E   �� 5R      $��Center=�6(   $��Eliminar��6E   5S      5T        5U      -
   5        $ RBtnPreviewTesisRel��5U   .   $&  Resources/1425495916_preview.png��5W   .   $	 Large=�5X   .   .��5Y      �� $
 Vertical=�6%   .+�5Z      �� $��Center=�6(   $��	Ver Tesis��6E   �� 5[      $��Center=�6(   $��Relacionadas��6E   5\      5]      �� 5^      $
 Vertical=�6!   .+�5_        -   5        $��Cortar Tesis��5`   2   $	 Medium=�5a   2   $ Resources/cut_32.png��5b   2     5c      -   5        $��Copiar Tesis��5d   2   $	 Medium=�5e   2   $ Resources/copy_32.png��5f   2     5g      -   5        $��Pegar Tesis��5h   2   $	 Medium=�5i   2   $ Resources/paste_32.png��5j   2   5k        5l      -   5        $ RBtnCopiaMarc��5l   .   $ Resources/dbok_256.png��5n   .   $	 Large=�5o   .   .��5p      �� $
 Vertical=�6%   .+�5q      �� $��Center=�6(   $��Copiar tesis��6E   �� 5r      $��Center=�6(   $��Marcadas��6E   5s      5t        5u      -   5        $ RBtnTraslMarc��5u   .   $ Resources/dbgo_256.png��5w   .   $	 Large=�5x   .   .��5y      �� $
 Vertical=�6%   .+�5z      �� $��Center=�6(   $��Trasladar Tesis��6E   �� 5{      $��Center=�6(   $��Marcadas��6E   5|      5}        5~      -   5        $ RBtnDelMarc��5~   .   $ Resources/dbfail_256.png��5�   .   $	 Large=�5�   .   .��5�      �� $
 Vertical=�6%   .+�5�      �� $��Center=�6(   $��Eliminar��6E   �� 5�      $��Center=�6(   $��
Marcación��6E   5�      5�      5�        5�      $��Expresiones��6)   {�.  .@�5�        -   5        $ RBtnEjecutaExp��5�   .   $��Ejecutar��5�   .   $ Resources/question.png��5�   .   $	 Large=�5�   .   �� 5�      $
 Vertical=�6!   .+�5�        -   5        $ 
RBtnNewExp��5�   2   $��Nueva��5�   2   $	 Medium=�5�   2   $ Resources/question.png��5�   2     5�      -   5        $ RBtnEditExp��5�   2   $��	Modificar��5�   2   $	 Medium=�5�   2   $ Resources/question.png��5�   2     5�      -   5        $ 
RBtnDelExp��5�   2   $��Eliminar��5�   2   $	 Medium=�5�   2   $ Resources/question.png��5�   2   5�      5�        5�      $��Herramientas��6)      Icon$ Resources/tools_16.png��6?   .@�5�        -   5        $ RBtnNoIngresadas��5�   .   $! Resources/lupaverde_128.png��5�   .   $	 Large=�5�   .   .��5�      �� $
 Vertical=�6%   .+�5�      �� $��Center=�6(   $��Buscar Tesis��6E   �� 5�      $��Center=�6(   $��NO ingresadas��6E   5�      5�        5�      -   5        $ RBtnBuscarPorIus��5�   .   $" Resources/binoculars_128.png��5�   .   $	 Large=�5�   .   .��5�      �� $
 Vertical=�6%   .+�5�      �� $��Center=�6(   $��Buscar Relaciones��6E   �� 5�      $��Center=�6(   $��por Núm. IUS��6E   5�      5�        5�      -   5        $ RBtnStat��5�   .   $��Estadisticas��5�   .   $" Resources/statistics_128.png��5�   .   $	 Large=�5�   .   5�        5�      $��Reportes��6)   .@�5�        -   5        $ RBtnListCertifica��5�   .   {�.  $% Resources/certification_128.png��5�   .   $	 Large=�5�   .   .��5�      �� $
 Vertical=�6%   .+�5�      �� $��Center=�6(   $��Listado para��6E   �� 5�      $��Center=�6(   $��Certificación��6E   5�      5�        5�      -   5        $ RBtnExpArbolPdf��5�   .   $ Resources/pdf_128.png��5�   .   $	 Large=�5�   .    � ToolTip$R LExporta la estructura del árbol que se encuentra visible a un documento PDF��5�   .   .��5�      �� $
 Vertical=�6%   .+�5�      �� $��Center=�6(   $��
Estructura��6E   �� 5�      $��Center=�6(   $��a PDF��6E   5�      5�      �� 5�      $
 Vertical=�6!   .+�5�      /	 	 )Telerik.Windows.Controls.RadNumericUpDown	  -   5         	 Name$ 
DetailDeep��5�   L   +
  %Telerik.Windows.Controls.RadRangeBase 
  Minimum$ 1Q�63    
  Maximum$ 12Q�6?    
  Value$ 1X�6`    	  	IsInteger . 6j     5�      -   5        $ RBtnExportExcel��5�   2   $��Reporte Excel��5�   .   $	 Large=�5�   .   $ Resources/excel_48.png��6Y   5�      5�      5�      5�      ]� 5�   
   -   5         ]�Name$ 	rootPanel��5�      $��1��6'    5�      4  .AppTesauro09wpf.Converters.ForegroundConverter  foreConverter��%   &	         5�      60   5�      .+�5�      o� $	��Top=�6    o� 
IsMainMenu . 6(   � 5�      -   5         �Name$ mainGrid��5�       � ColumnDefinitions 5�      �� 5�      $
��1.6* �6'   $	��590��64   �� 5�      $��1* �6'   $	��348��62   .+�5�      �� $��0��6   $��10��5�      $��Stretch=�5�      � 5      -   5        $ mainPane��5      $��0��5     $��10q�5      5     �� 5     $��40 �6(   �� 5     $��* �6(   �� 5     $��0 �6(   �� 5     $
��Auto �6(   �� 5     $��25 �6(   �� 5	     $��25 �6(   �� 5
     $��25 �6(   .+�5     �� -   5         ��Name$ mainPaneBorder��5     ���	#FF535353$��1q�5     ��5     �� 5      �� 
StartPoint$ 1.0,0.0&�62    �� EndPoint$ 0.2,1.0&�6G   .��5  "   �  � Offset$ 0Q�6/    � Color$ 	#FF202020��6:   � 5  "   $ 1Q�6/   $ 	#FF545454��6:   5     5     �� 5     -    5        $ header��5     $��0��5     ���#25A0D9���	#FF535353$��1,1,1,0q�5      �� Padding$ 10q�5     ._�5      ]� .+�5!     �� $
��Left=�6(   $��16(�5"  (   $
��Bold"�5#  (   ���	#FFFFFFFF�� 5$  @   $�Resultado de Temas��6D   5%        UIControls.SearchTextBox  5&     -!   5          Name$ 	SearchBox��5&  7   $	��300��5'  7   $��Right=�5(  7      	LabelText$ Buscar Tema��5)  7    �� TextWrapping$
 Wrap=�5+  7   5,     5-     R� 5/     -"   5          R�Name$  treeView��5/     $��1��50     $��Stretch=�51     $��Stretch=�52     ! J� 	AllowDrop! . ��54     �� *
IdOrigen+# [ ��56     �� ��59     �� 5;     " �� 
TargetType"" Q�6$   .�5<  "   �� # �� Property# �@Q�
IsExpanded$ �� Value$ 6?   �� *
IsExpanded+% �� Mode$% TwoWay=��� 5=  "   # �@Q�
IsSelected$ 6?   �� *
IsSelected+$% TwoWay=��� 5>  "   # � ��$$ Normal"�6?   & �� Triggers& 5?  "   P� 5@  &   ' P� Property' �@Q�
IsSelected( P� Value( . . �5A  *   �� # � ��$
$ Bold"�6G   5B  '   5D     ��5F     �� 5G     ) �� ItemsSource) 67   �� *
SubTemas+.R�5H  "   �� $
 
Horizontal=�6-   .+�5I  &   �� $��Center=�60   ��5J  0   �� *
IdOrigen+# [ ��5L  0   �� *Descripcion+$% TwoWay=��� 5N  &   $��  [  ��60   �� 5O  &   ��60   �� *TesisRelacionadas+$% TwoWay=��� 5P  &   $	��  ]��60   5Q  #   5R     5T     �� 5W     -#   5        $ newExpresionHeader��5W     $��2��5X     $��2��5Y     $��Stretch=�5Z     $��Stretch=�5[     ���#25A0D9
���i��$��1,1,1,0q�5^     $ 10q�5_     �� 5`     $��3��6   ���	#FF535353$��0,1,0,0q�5c     $y�	Collapsed=�5d     ��5a     Z� ._�5e     ` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  ` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c67xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   6/   -$   5        *  Name$* dgExpresiones��5f  /   $	��150��5g  /   +   AutoCreateColumns+ .  ,   NavigationBehavior$, RowOnly=�5i  /   -   ReadOnly- . .   SelectionMode$. Single=�5k  /   $y�Visible=�5l  /   /   Columns/ 5m       Xceed.Wpf.DataGrid.Column  5n  "   0   Title$0 
Expresión��6.   1   Width8  2Xceed.Wpf.DataGrid.Converters.ColumnWidthConverter$	1 400 5o  .   2   	FieldName$2 Descripcion��5p  .     5q  "   3   CellContentTemplate#3 [ 6.   $2 Campos��6j     5r  "   #3 [ 6.   $2 Operador��6k   4   View4 5t     (  "Xceed.Wpf.DataGrid.Views.TableView  5u  "   5   ShowFixedColumnSplitter5 .  6   ShowRowSelectorPane6 .  '  !Xceed.Wpf.DataGrid.Views.ViewBase7   UseDefaultHeadersFooters7 .  8   FixedHeaders8 5x  &   �� 5y  *   .R�5z  .   )  #Xceed.Wpf.DataGrid.ColumnManagerRow  9   	AllowSort9 .  6D   5{  +   5}  #   5     5�     �� 5�     -%   5        : ��Name$: LabelIA��5�     $��4��5�     $��28��5�     
����  �$��Inclusión Ascendente:��5�     
�������5�     �� 5�     -&   5        $: LblResultadoIA��5�     $��5��5�     $��28��5�     ���	#FF6573A0; � Cursor$
; Hand��5�     $
��Bold"�5�     
�������5�     �� 5�     -'   5        $: LblTemaSelLbl��5�     $��6��5�     
�������$
��Bold"�5�     
�������.��5�     �� -(   5        < ��Name$< lblTemaSelec��5�  $   $��6��6:   �� 6H   $�Tema seleccionado:��6L   5�     5�     5�     � 5�     -)   5        $ detailsPanel��5�     $��1��5�     $��10q�5�      5�     �� 5�     $��30 �6(   �� 5�     $��* �6(   �� 5�     $��30 �6(   �� 5�     $��* �6(   �� 5�     $��* �6(   �� 5�     $��* �6(   ��5�     �� 5�     $	 0,0&�6.   $	 0,1&�6?   .��5�     � $ 1Q�6+   $ DarkGray��66   � 5�     $ 0Q�6+   $ Black��66   5�     .+�5�     � $��0��6   ���#25A0D9 5�     �� 5�     �� 5�     $	��200 �6/   .+�5�     �� $��1��6$   $��16(�5�  $   $
��Bold"�5�  $   ���	#FFFFFFFF$��
Sinónimos��5�  $   �� 5�     $��1��6%   $��0,10,0,0q�5�  %   $
 
Horizontal=�5�  %   .+�5�     �� -*   5        = ��Name$= btnSinonimoNvo��5�  %   $��60��5�  %   $��0,0,5,0q�5�  %   $��_Nuevo��5�  %   �� 5�     -+   5        $= btnSinonimoModif��5�  %   $��60��5�  %   $��0,0,5,0q�5�  %   $��
_Modificar��5�  %   �� 5�     -,   5        $= btnSinonimoElim��5�  %   $��60��5�  %   $��	_Eliminar��5�  %   5�     5�       5�     7xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   6+   --   5        $* DgSinonimos��5�  +   $��1��5�  +   $
��Auto��5�  +   + .  >   EditTriggers$
> None=�5�  +   ?   ItemScrollingBehavior$? 	Immediate=�5�  +   $, RowOnly=�5�  +   - . / 5�       5�     $0 Descripción��6*   $	1 250 5�  *   $2 Descripcion��5�  *   @   IsMainColumn@ . A   ReadOnlyA . B   TextWrapping$B WrapWithOverflow=�5�  *   4 5�       5�     C   ColumnStretchMode$
C None=�6-   5 .  7 .  8 5�  "   �� 5�  &   .R�5�  *     5�  '   5�     5�     �� 5�     $��1��6#   $��5��5�  #   $��Stretch=�5�  #   $��Bottom=�5�  #   $��Stretch=�5�  #   $	��Top=�5�  #   � 5�     $��2��6   $��2��5�     ���#25A0D9 5�     �� 5�     $��30 �6,   $��20��5�  ,   $��30��5�  ,   �� 5�      5�     �� 5�     �� 5�     .+�5�     �� $��0��6$   $��16(�5�  $   $
��Bold"�5�  $   ���	#FFFFFFFF$��Relaciones próximas��5�  $   �� 5      $��0��6%   $��1��5  %   $��0,10,0,0q�5  %   $
 
Horizontal=�5  %   .+�5     �� -.   5        $= btnRelacionespNvo��5  %   $��60��5  %   $��0,0,5,0q�5  %   $��Nuevo��5  %   �� 5	     -/   5        $= btnRelacionespModif��5	  %   $��60��5
  %   $��0,0,5,0q�5  %   $��	Modificar��5  %   �� 5     -0   5        $= btnRelacionespElim��5  %   $��60��5  %   $��Eliminar��5  %   5     5       5     7xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   6+   -1   5        $* 	DgRelProx��5  +   $��3��5  +   $��1��5  +   $
��Auto��5  +   + .  $
> None=�5  +   $? 	Immediate=�5  +   $, RowOnly=�5  +   - . 4 5        5!     5 .  7 .  8 5"  "   �� 5#  &   .R�5$  *     5%  '   5'     / 5)       5*     $0 Descripción��6*   $	1 250 5+  *   $2 Descripcion��5,  *   @ . A . $B WrapWithOverflow=�5/  *   51     �� 52     $��3��6#   $��5��53  #   $��Stretch=�54  #   $��Bottom=�55  #   $��Stretch=�56  #   $	��Top=�57  #   �� 58     $��4��6   
�������
���   �$
��Auto=�5;     .��5<     �� -2   5        $< TblNotas��5<  $   $��0,0,0,0q�5=  $   $��WrapWithOverflow=�5>  $   �� 6E   $�Notas:��6I   5?     5@     �� 5A     $��5��6   $��1��5B     
�������
���   �$
��Auto=�5E     .��5F     �� -3   5        $< TblObservaciones��5F  $   $��0,0,0,0q�5G  $   $��WrapWithOverflow=�5H  $   �� 6E   $�Observaciones:��6I   5I     5J     5K     5M     5N     5O     5P     
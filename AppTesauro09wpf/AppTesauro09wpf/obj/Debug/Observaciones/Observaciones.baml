   M S B A M L   `   `   `  ����  AppTesauro09wpf8    2AppTesauro09wpf.Observaciones.VentanaObservaciones   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Observaciones��5   	   $	��640��6   $	��640��6,    � WindowStartupLocation$ CenterScreen=�5   	    � WindowStyle$ ThreeDBorderWindow=�6.    � 
ResizeMode$ NoResize=�5   	   .��5      �� 
�������.+�5   
   �� -   5        $��TbxObservaciones��5      $	��200��6*   $	�� 10q�67   � 5	   
   -   5        $��Botones��5	      $��40��6   
������� � ColumnDefinitions 5
      �� 5      �� 5      �� 5      �� 5      .+�5      �� -   5        $��
BtnRevisar��5      $	��100��5      $	��150��6$   $��Center=�5      .��62   
_Revisar6<   �� 5      -   5        $��
BtnAtender��5      $��1��5      $	��100��5      $	��150��6$   $��Center=�5      .��62   
_Atender6<   �� 5      -   5        $��
BtnAceptar��5      $��2��5      $	��100��5      $	��150��6$   $��Center=�5      .��62   
Ace_ptar6<   �� 5      -   5        $��BtnCancelar��5      $��3��5      $	��100��5       $	��150��6$   $��Center=�5!      .��6L   	_Cancelar6W   5"      � 5#   
   -   5        $��	Historial��5#      $	��300��6    5$      �� 5%      $��0.80* �6#   �� 5&      $
��0.2* �6#   .+�5(      �� 
�������.+�5)      �� -   5        $��TblHistorial��5)   )   $	��200��6   $��10,10q�6=   
���������. $��WrapWithOverflow=�5,      $
��Auto=�5-      $
��Auto=�5.      � 5/       50      �� 51      �� 52       � RowDefinitions 54      �� 55      $��40 �6(   �� 56      $��40 �6(   .+�58      �� .��6   	Usuario6%   �� 59      -	   5        $��
LblUsuario��59   ,   $��1��6   $��5q�5:      
���   �$��1q�5<      
�������5=   1   �� 5?      $��1��6   .��6)   Fecha60   �� 5@      -
   5        $��LblFecha��5E   )   $��1��5@      
���   �$��1q�5B      $��5q�5C      
�������$��1��5E      6;   5F      5G      ` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6(  "Xceed.Wpf.DataGrid.DataGridControl  5H      ` \Xceed.Wpf.Controls, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c67xcdg*http://schemas.xceed.com/wpf/xaml/datagrid   5O   #   -   5        $��DgdHistorial��5M   #   $��1��5H   #      AutoCreateColumns .  $��Right=�5K   #   $��0,0,0,0q�5L   #   $��Stretch=�5N   #      ReadOnly .    SelectionMode$ Single=�5O   k   	   NavigationBehavior$	 RowOnly=�6�   
   Columns
 5P        Xceed.Wpf.DataGrid.Column  5Q         	FieldName$ Texto��6"      Title$ Observaciones��64      IsMainColumn . 6J     5R      $ Usuario��6"   $ Usuario��66      View 5T      (  "Xceed.Wpf.DataGrid.Views.TableView  5U         ShowFixedColumnSplitter .     ShowRowSelectorPane .  '  !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     FixedHeaders 5V      �� 5W      .R�5X   "   )  #Xceed.Wpf.DataGrid.ColumnManagerRow  5Y      5[      5]      5^      5_      5`      
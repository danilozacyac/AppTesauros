   M S B A M L   `   `   `  ����  AppTesauro09wpf,    &AppTesauro09wpf.Busqueda.BusquedaCopia   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � WindowStartupLocation$  CenterScreen=�5   	    � Title$ Temas que coinciden��5      $	��300��6!   $	��300��6.   ��5      �� 5   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 0,1&�6/   .��5	      �  � Color$ 	LightBlue��6    � Offset$	 0.0Q�6-   � 5
      $
 Gray��6   $ 1Q�6(   5      .��5      �  � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��15 �6   �� 5      �� 5      $��25 �6   .+�5   
   �� $��0,0,0,0q�6   .��5      �� 
������ .��5      �� -   5        $��lblRuta��5      $��0,0,0,0q�6+   $��WrapWithOverflow=�6<   Seleccione un tema5      5      5      �� 5   
   -   5        $��CbxTodos��5      $��1��6#   .��6G   Seleccionar _todo6Z   R� 5   
   -   5        $��TviResultado��5      $��2��6'   $
��Auto=�5      $
��Auto=�5      � 5   
   $��3��6    � ColumnDefinitions 5       �� 5!      �� 5"      �� 5#      .+�5%      �� -   5        $��	BtnCopiar��5%       �� 	IsDefault . .��6O   	_Copiar6X   �� 5&      -   5        $��
BtnIgnorar��5&      $��1��6@   .��6P   
_Ignorar6Z   �� 5'      -   5        $��BtnCancelar��5'      $��2��6B   .��6R   	C_ancelar6]   5(      5)      5*      
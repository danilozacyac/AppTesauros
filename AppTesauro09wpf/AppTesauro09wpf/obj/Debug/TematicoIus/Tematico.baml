   M S B A M L   `   `   `  ����J  FAppTesauro09wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:AppTesauro09wpf.ConvertersAppTesauro09wpf.Converters  5   	   *    $AppTesauro09wpf.TematicoIus.Tematico   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   >	converter(clr-namespace:AppTesauro09wpf.Converters     5   	   -   5        $��TematicoWnd��5   	     � Title$  Tematico��5   	   $	��727��5   	   $	��651��5   	   
����� � � 
ResizeMode$ NoResize=�5   	    � ShowInTaskbar .   � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5      4   .AppTesauro09wpf.Converters.ForegroundConverter   foreConverter��%   &	          5   
   6(   5      .��5      �  � RowDefinitions 5   
   �� 5      $��55 �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   � $��1��6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   .+�5      R� -   5         R�Name$ TreeTematico��5      $��1��5      $��Stretch=�5      $��Stretch=�5        J� 	AllowDrop . ��5"      �� *IdTemaOrigen+	 �� 	Converter#	 [  ��5$      �� *
Categories+��5&      �� 5(      
 �� 
TargetType"
 Q�6   .�5)      ��  �� Property �@Q�
IsExpanded �� Value 67   �� *
IsExpanded+ �� Mode$ TwoWay=��� 5*       �@Q�
IsSelected 67   �� *
IsSelected+$ TwoWay=��� 5+       � ��$ Normal"�67    �� Triggers 5,      P� 5-       P� Property �@Q�
IsSelected P� Value . . �5.   "   ��  � ��$
 Bold"�6?   5/      51      ��53      �� 54       �� ItemsSource 6/   �� *
SubTemas+.R�55      ��  �� Orientation$ 
Horizontal=�6%   .+�56      �� $��Center=�6(   ��57   (   �� *IdTemaOrigen+#	 [  ��59   (   �� *Descripcion+$ TwoWay=�5;      5<      5>      �� 5?      -   5        $��
textBlock1��5?      $��1��5@      $��23��5A      $��	20,23,0,0q�5B      $
��Left=�5C      $	��Top=�5D      $��14(�5E      $
��Bold"�5F      $��Descripción:��5G      �� 5H      -   5        $��TxtDescripcion��5H      $��1��5I      $	��292��5J      $��23��5K      $��	20,52,0,0q�5L      $
��Left=�5M      $	��Top=�5N      {�.  5O      �� 5P      -   5        $��
textBlock2��5P      $��1��5Q      $��23��5R      $��
20,107,0,0q�5S      $
��Left=�5T      $	��Top=�5U      $��¿Es de primer nivel?��5V      	� 5W      -   5        $��RadYes��5W      $��1��5X      $��16��5Y      $��236,107,0,0q�5Z      $
��Left=�5[      $	��Top=�5\      $	�� Si��5]       d� 	IsChecked$ FalseY�5^      	� 5_      -   5        $��RadNo��5_      $��1��5`      $��16��5a      $��284,107,0,0q�5b      $
��Left=�5c      $	��Top=�5d      $	�� No��5e      $
 TrueY�5f      �� 5g      -   5        $��
textBlock3��5g      $��1��5h      $��23��5i      $��
20,163,0,0q�5j      $
��Left=�5k      $	��Top=�5l      $��14(�5m      $
��Bold"�5n      $��Notas:��5o      �� 5p      -	   5        $��TxtNotas��5p      $��1��5q      $	��292��5r      $	��131��5s      $��
20,192,0,0q�5t      $
��Left=�5u      $	��Top=�5v      �� 5w      -
   5        $��
textBlock4��5w      $��1��5x      $��23��5y      $��
21,363,0,0q�5z      $
��Left=�5{      $	��Top=�5|      $��14(�5}      $
��Bold"�5~      $��Observaciones:��5      �� 5�      -   5        $��TxtObservaciones��5�      $��1��5�      $	��292��5�      $	��109��5�      $��
21,392,0,0q�5�      $
��Left=�5�      $	��Top=�5�      5�      �� 5�   
   -   5        $��BtnCancelar��5�      $��2��5�      $��75��5�      $��23��5�      $��	0,15,39,0q�5�      $��Right=�5�      $	��Top=�5�      $��Cancelar��5�      �� 5�   
   -   5        $��
BtnAceptar��5�      $��2��5�      $��75��5�      $��23��5�      $��
501,15,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Aceptar��5�      �� 5�   
   -   5        $��	TxtBuscar��5�      $	��178��5�      $��23��5�      $��	12,12,0,0q�5�      $
��Left=�5�      $	��Top=�5�      �� 5�   
   -   5        $��	BtnBuscar��5�      $��52��5�      $��23��5�      $��
196,11,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Buscar��5�      �� 5�   
   -   5        $��BtnRestaurar��5�      $��75��5�      $��23��5�      $��
254,11,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��	Restaurar��5�      5�      5�      
   M S B A M L   `   `   `  ����J  FAppTesauro09wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:AppTesauro09wpf.ConvertersAppTesauro09wpf.Converters  5   	   '    !AppTesauro09wpf.BuscarPorRegistro   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   >	converter(clr-namespace:AppTesauro09wpf.Converters     5   	   -   5          � Title$  BuscarPorRegistro��5   	   $	��510��5   	   $	��500��5   	   $	��510��5   	   $	��500��5	   	   $	��700��5
   	   
����� � � 	Resources 5      4   .AppTesauro09wpf.Converters.ForegroundConverter   foreConverter��%   &	          5   
   6(   5      .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   .+�5   
   �� -   5        $��TxtRegistroIus��5      $	��207��5      $��23��5      $��	12,22,0,0q�5      $
��Left=�5      $	��Top=�5      5      �� 5   
   -   5        $��
textBlock1��5      $��23��5      $��12,2,0,0q�5      $
��Left=�5      $	��Top=�5       $��Número de registro IUS:��5!      �� 5"   
   -   5        $��	BtnBuscar��5"      $��59��5#      $��23��5$      $��
225,21,0,0q�5%      $
��Left=�5&      $	��Top=�5'      $��Buscar��5)       �� 	IsDefault . 5*      � 5+   
   $��1��6    � ColumnDefinitions 5,      �� 5-      $��12 �6#   �� 5.      $��* �6#   �� 5/      $��12 �6#    51      �� 52      $��* �6    �� 53      $��10 �6    .+�55   
   R� -   5         R�Name$ IusTreeView��55      $��1��56      $��Stretch=�57      $��Stretch=�58       J� 	AllowDrop . ��5:      �� *
IdOrigen+ �� 	Converter# [  ��5<      �� ��5=      �� 5?       �� 
TargetType" Q�6   .�5@      �� 	 �� Property	 �@Q�
IsExpanded
 �� Value
 63   �� *
IsExpanded+ �� Mode$ TwoWay=��� 5A      	 �@Q�
IsSelected
 63   �� *
IsSelected+$ TwoWay=��� 5B      	 � ��$
 Normal"�63    �� Triggers 5C      P� 5D       P� Property �@Q�
IsSelected P� Value . . �5E      �� 	 � ��$

 Bold"�6;   5F      5H      ��5J      �� 5K       �� ItemsSource 6+   �� *
SubTemas+.R�5L      ��  �� Orientation$ 
Horizontal=�6!   .+�5M      �� $��Center=�6$   ��5N   $   �� *
IdOrigen+# [  ��5P   $   �� *Descripcion+$ TwoWay=��� 5R      $��  [  ��6$   �� 5S      ��6$   �� *TesisRelacionadas+$ TwoWay=��� 5T      $	��  ]��6$   5U      5V      5X      5Y      5Z      5[      
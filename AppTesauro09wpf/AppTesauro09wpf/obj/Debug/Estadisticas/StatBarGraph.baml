   M S B A M L   `   `   `  ����C  ?Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5(clr-namespace:Controls;assembly=ControlsControls  5   	   | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization 5   	   �{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5   	    AppTesauro09wpf/   )AppTesauro09wpf.Estadisticas.StatBarGraph   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   CcustomControls(clr-namespace:Controls;assembly=Controls     5   	   �DVrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   �DVC{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   -   5          � Title$   Avance de captura de tesis��5   	   $
��Auto��5   	   $
��Auto��5	   	   $	��500��5
   	   $	��350��5   	    � WindowState$ 	Maximized=�5   	    � 	Resources 5         detailsSmallTitle �� 
TargetType �� Property �� Value  detailsSmallText  
darkBorder  cityDetails � ColumnDefinitions � RowDefinitions  pieChartLabelStyle   Controls.PieChartLabel  pieChartLabelDataTemplate �� Orientation	 �� RelativeSource
 �� AncestorType �� Path��%$  &	        &	 �     &	 �     &	 �    7   7  7   7  &	 q    &	 �    �� 5   
   " ��6*   .�5      ��  � ��$
 Bold"�6+   �� 5       � ��$ Right=�64   �� 5       � ��$ 14(�6)   5      �� 5   
   " ��6)   .�5      ��  � ��$ 14(�6)   5      �� 5   
   " ��6#   .�5      ��  � ��
 �����6,   �� 5       � ��$ 1q�60   �� 5       �@��Padding$ 10q�6(   �� 5       � ��$ 	0,10,0,10q�6'   5      �� 5    
   .R�5!      �  5"      �� 5#      $
��Auto �6'   �� 5$      $��90 �6'    5&      �� 5'      �� 5(      .+�5*      �� $��0��6   $��0��5+      8��  $��Name: ��5-      �� 5.      $��0��6   $��1��5/      8�� ��51      �� *Key+�� 52      $��1��6   $��0��53      8�� $��Population: ��55      �� 56      $��1��6   $��1��57      8�� ��59      �� *Value+5:      5;      �� 5=   
   "  6+   5B      �� 5E   
   .R�5F      �� 
�������$��1q�6(   ._�5G      �� 
�������$ 
Horizontal=�60   .+�5H      �� $��5,0,5,0q�6    $��Center=�5I       ��5J       �� 	 �� "
  $ FormattedRatio��� 5L      $��- ��6    �� 5M      $��0,0,5,0q�6    ��61   �� *Key+5N      5O      5P      5R      .��5T      � 
����İ� 5U   
   �� 5V      -   5         �Name$ Barras��5V      $��* �6*   �� 5W      -   5        $	 Pie��5W      $
��Auto �6'   �� 5X      $��30 �6   .+�5Z   
   � -   5        $��barGrid��5Z      $
��Auto��6   .+�5[      >  8System.Windows.Controls.DataVisualization.Charting.Chart  -   5        $��mcChart��5[         Title$- 'Número de tesis ingresadas por abogado��5\       �� Left$ 10��5]      
 �� Top$ 80��5^      $
��Auto��5_      $
��Auto��5`      
����İ�   Series 5c      B  <System.Windows.Controls.DataVisualization.Charting.BarSeries  ?  9System.Windows.Controls.DataVisualization.Charting.Series   Title$ Tesis agregadas��6$   H  BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValueBinding 5d   $   �� $ Value�   IndependentValueBinding 5e   $   �� $	 Key�  5f      $ Tesis eliminadas��6$    5g   $   �� $ Value� 5h   $   �� $	 Key�5j      5k      � 5l   
   -   5        $��pieGrid��5l      $��1��5m      $
��Auto��5n      $y�	Collapsed=�5o      .+�5�         Controls.LabeledPieChart  -   5          Name$ labeledPieChart��5�   -   $+ %Total de tesis ingresadas por materia��5�   -   $��3��5�   -   $
��Auto��5�   -   $	��500��5�   -   
������� 5�         Controls.LabeledPieSeries  -   5          Name$ labeledPieSeries��5�   6      AnimationSequence$ FirstToLast=�6P      DependentValuePath$ Value��5�   6      IndependentValuePath$	 Key��5�   6      IsSelectionEnabled .    LabelDisplayMode$ 	Connected=�5�   6       PieChartLabelItemTemplate# [ 5�   6      PieChartLabelStyle# [ 5�   6      ItemsSource 5�   6   �� 5�   6   5�      5�      �� 5�   
   -	   5        $��BtnVerDetalle��5�      $��2��5�      $��75��5�      $��23��5�      $��0,0,12,0q�5�      $��Right=�5�      $	��Top=�5�      $��Ver Detalle��5�      �� 5�   
   -
   5        $��BtnTesisPorMateria��5�      $��2��5�      $	��109��5�      $��23��5�      $��12,0,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Tesis por materia��5�      �� 5�   
   -   5        $��BtnTesisAbogado��5�      $��2��5�      $	��111��5�      $��23��5�      $��	127,0,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Tesis por abogado��5�      5�      5�      
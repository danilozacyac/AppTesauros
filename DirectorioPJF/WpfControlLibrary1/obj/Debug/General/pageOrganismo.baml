   M S B A M L   `   `   `  ����  DirectorioPJF2    ,mx.gob.scjn.directorio.General.pageOrganismo   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5        N� Title$  pageOrganismo��5      $��721.533333333333��6   $��500.14��64   .��5      �  � ColumnDefinitions 5   
   �� 5      $��368.682* �6   �� 5      $��352.851* �6    � RowDefinitions 5
   
   �� 5      $��493.005* �6   �� 5      $��7.135* �6   .+�5   
   � -   5        $��grdXgrdLista��5   +   $��12,12,14.29,12.865q�6    5      �� 5      $��437.274* �6    �� 5      $��30.866* �6    .+�5   
   ` \Xceed.Wpf.DataGrid, Version=3.0.8453.23470, Culture=neutral, PublicKeyToken=ba83ff368b7563c6S<clr-namespace:Xceed.Wpf.DataGrid;assembly=Xceed.Wpf.DataGridXceed.Wpf.DataGrid 5   5   _Bclr-namespace:Xceed.Wpf.DataGrid.Views;assembly=Xceed.Wpf.DataGridXceed.Wpf.DataGrid.Views 5      (  "Xceed.Wpf.DataGrid.DataGridControl  5   
   Imy<clr-namespace:Xceed.Wpf.DataGrid;assembly=Xceed.Wpf.DataGrid    5   5   Pmy1Bclr-namespace:Xceed.Wpf.DataGrid.Views;assembly=Xceed.Wpf.DataGrid    5      -   5        $��grdOrganismos��5      $��0��5      $��0��6.      AutoCreateColumns .     EditTriggers$
 None=�5      $��1,1,1,1q�62      NavigationBehavior$ RowOnly=�5   3      ReadOnly .    SelectionMode$ Single=�5         Columns 5        Xceed.Wpf.DataGrid.Column  5      	   	FieldName$		 Org��6   
   View
 5      (  "Xceed.Wpf.DataGrid.Views.TableView  5         ShowFixedColumnSplitter .     ShowRowSelectorPane .  '  !Xceed.Wpf.DataGrid.Views.ViewBase   UseDefaultHeadersFooters .     Theme 5       3  -Xceed.Wpf.DataGrid.Views.AeroNormalColorTheme  5!         FixedHeaders 5#      �� 5$      .R�5%      )  #Xceed.Wpf.DataGrid.ColumnManagerRow     AllowColumnReorder .     	AllowSort .  6M   5&      5(      5*      5+      � 5-   
   -   5        $��
grdDetalle��5-   4   $��1��6   $��10,10,10,10q�6    5.      �� 5/      $��52.873* �6    �� 50      $��37.32* �6    �� 51      $
��151* �6    �� 52      $��30.009* �6    �� 53      $��201.489* �6     55      .+�5:   
   �� -   5        $��	NombreOrg��5:      $��0��6!   $��0��61   $��5,5,5,5q�6?   $��Center=�6Q   
�������$��2q�6�   $��10q�6�   $��20(�6�   $��48.14��6�   $	��Top=�6�   .��6�   !Aqui va el nombre del Organismo6  �� 5;   
   -   5        $��lblDomSPAdmin��5;      $��0��6&   $��1��66   $��5,5,5,5q�6C   $��Center=�6U   $��23.75��6y   $��Bottom=�6�   
�������$��14(�6�   .��6�   	DOMICILIO6�   �� 5<   
   -   5        $��	lblDirOrg��5<      $��0��6!   $��2��61   $��5,5,5,5q�6?   $	��150��6P   $��Center=�6]   $��Bottom=�6�   
�������$��14(�6�   .��6�   !Aquí va la dirección completa6�   �� 5=   
   -   5        $��lblAreaAdmin��5=      $��0��6$   $��3��64   $��5,5,5,5q�6A   $��30��6R   $��Center=�6^   $	��Top=�6�   
�������$��14(�6�   .��6�   	TITULAR6�   �� 5>   
   -   5        $��
lblTitular��5>      $��0��6"   $��4��62   $��5,5,5,5q�6?   
�������$��14(�6d   $��Center=�6r   .��6�   Aqui va el nombre del titular6�   5@      5B      5C      
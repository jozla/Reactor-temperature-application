   M S B A M L   `   `   `  ����>  :PZ2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:PZ2.Views	PZ2.Views  5   	   -clr-namespace:PZ2.ViewModelPZ2.ViewModel  5   	   $    PZ2.Views.MeasurementGraphView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:PZ2.Views     5   	   1	viewModelclr-namespace:PZ2.ViewModel     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � DataContext  5      -   'PZ2.ViewModel.MeasurementGraphViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� $��0��6   $��0��6#   $	��200��60   $��Center=�6<   $��Center=�6W   $��10q�6t   ��6�   �� *AvailableReactors+ �� SelectedItem 6�   �� *SelectedReactor+ �� Mode$ TwoWay=���5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $	��ID:��6$   �� 5      ��6$   �� *Id+�� 5       $��, ��6$   �� 5!      ��6$   �� *Name+5"      5#      5%      �� 5&   
   $��0��6   $��1��6   $��Center=�6.   $
��Left=�6I   $��10q�6d   $��90��6�    � ToolTip$ 
Show graph��6�   ��6p   �� *ShowCommand+��6�   ��  �� ElementName$ graphCanvas��.��5'      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ ChartScatterPlot=�6&   $��24��6>   $��24��6J   5(      �� 5*   
   -   5        	 ��Name$	 graphCanvas��5*      $��3��6&   $��2��63   $	��600��6G   $	��300��6S   
�������5+        5-   
   $
 Help=�6"   $��24��6.   $��24��6:   $��1��6E   $��2��6R   $��center=�6f   y�6�   �� *IsHelpVisible+�� 5.   
   $��20,0,0,0q�6   $��1��6&   $��2��63   $����To see the history of the states (last 5 changes) for the entity first, select the entity from the comboBox and then press the button to show it on the graph. The radius of the circle equals temperature intensity.��6G   $��15(�6$  $
��Wrap=�62  
 �� TextAlignment$
 Center=�6G  y�6^  �� *IsHelpVisible+6�    5/   
   $
 Help=�6"   $��24��6.   $��24��6:   $��2��6E   $��2��6R   $��center=�6f   y�6�   �� *IsHelpVisible+�� 50   
   $��20,0,0,0q�6   $��2��6&   $��2��63   $����If the circle is blue that means the temperature is in valid margins, otherwise, when the color is red that means the temperature is not in valid margins.��6G   $��15(�6�   $
��Wrap=�6�   $
 Center=�6  y�6#  �� *IsHelpVisible+6J  51      52      
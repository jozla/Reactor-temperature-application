   M S B A M L   `   `   `  ����>  :PZ2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:PZ2.Views	PZ2.Views  5   	   -clr-namespace:PZ2.ViewModelPZ2.ViewModel  5   	   #    PZ2.Views.NetworkEntitiesView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:PZ2.Views     5   	   1	viewModelclr-namespace:PZ2.ViewModel     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � DataContext  5      ,   &PZ2.ViewModel.NetworkEntitiesViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� $��0��6   $��0��6     �� SelectedValuePath$ Content��5      $��
30,10,0,10q�5   J    � ToolTip$ Chose the type here��6^    �� SelectedValue 6   ��  �� Path$ 
FilterType� �� Mode$ TwoWay=� ^� 	IsEnabled 6|   �� *IsToolTipVisible+.@�5       �� $
��Type��6   6,   �� 5!      $	��RTD��6   6+   �� 5"      $��
Thermopile��6   62   5#      � 5$   
   $��0��6   $��1��6    5%      �� 5&      $��1* �6#   �� 5'      $��1* �6#   �� 5(      $��1* �6#   .+�5*      �� $��Id��6   $��0��6!   $��Right=�61   $��Center=�6T   �� 5+      $��1��6   $��Center=�6)   $��Center=�6F   .+�5,      	� 	 d� 	IsChecked	 6   �� $ FilterMode[0]�$ TwoWay=�.��6T   <6Z   	� 5-      	 6   �� $ FilterMode[1]�$ TwoWay=�.��6T   =6W   	� 5.      	 6   �� $ FilterMode[2]�$ TwoWay=�.��6T   >6Z   5/      �� 50      $��2��6=   $
��Left=�6M   $��Center=�6o   $��0,0,20,0q�6�   $# Value must be a valid integer��6�   ��6   �� *
FilterId+$ TwoWay=� 6�   �� *IsToolTipVisible+51      � 52   
   $��0��6   $��2��6    53      �� 54      $��1* �6#   �� 55      $��1* �6#   .+�57      �� $��0��6   $��20q�6G   $! Press here to apply filters��6S   ��6%   �� *FilterCommand+ 6y   �� *IsToolTipVisible+.��58      '  !MaterialDesignThemes.Wpf.PackIcon  
   Kind$
 Filter=�6*   $��24��68   $��24��6D   59      �� 5:      $��1��6   $��20q�6L   $! Press here to reset filters��6X   ��6%   �� *ResetFilterCommand+ 6~   �� *IsToolTipVisible+.��5;        $
 FilterRemove=�6*   $��24��6>   $��24��6J   5<      5=      � 5@   
   $��1��6   $��3��6   $��Center=�60   $��30, 10, 30, 10q�6M    5A      �� 5B      $
��auto �6    60    5D      �� 5E      $
��auto �6#   62   �� 5F      $��* �6#   6/   .+�5I        $

 Help=�6&   $��24��62   $��24��6>   $��0��6I   $��center=�6W   y�6r   �� *IsHelpVisible+�� 5J      $��0��6   $��1��6%   $����To apply filters select the type and then type in the ID whose value will be greater/less/equal based on the radio button selection. After that, just press the filter button.��65   $��15(�6�   $
��Wrap=�6�    �� TextAlignment$ Center=�6  y�6&  �� *IsHelpVisible+6M  5K      &   System.Windows.Controls.DataGrid  5N   
   $��2��6   $��0��6    $��3��60      AutoGenerateColumns .     
IsReadOnly . $��2q�6�   
�������$��0,0,0,10q�6�   ��6D   �� *ReactorList+ �� SelectedItem 6h   �� *SelectedReactor+$ TwoWay=�   Columns 5O      0  *System.Windows.Controls.DataGridTextColumn  5P      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6%      Header$ Id��6/   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *Id+  5Q      $ * 6%   $
 Name��6/    6>   �� *Name+  5R      $ * 6%   $
 Type��6/    6>   �� *	Type.Name+  5S      $ * 6%   $ Temperature��6/    6E   �� *Temperature+4  .System.Windows.Controls.DataGridTemplateColumn  5T      $ * 6)   $ Delete��63      CellTemplate 5U      �� 5V      .R�5W      ��    MaterialDesignRaisedButton#��[  5X   !   ���#00bcd4$ Press here to delete item��6j   ��5W   %   �� $&  DataContext.DeleteReactorCommand� �� RelativeSource �� *FindAncestor+ �� AncestorType"  .��5Y   "     $
 Delete=�6:   $��24��6H   $��24��6T   5Z      5[      5]      5_      � 5b   
   $��3��6   $��0��6   $��2��6,    5c      �� 5d      $
��Auto �6    �� 5e      $
��Auto �6    �� 5f      $
��Auto �6    �� 5g      $
��Auto �6    �� 5h      $
��Auto �6     5j      �� 5k      $��1* �6#   �� 5l      $��1* �6#   �� 5m      $��1* �6#   �� 5n      $��1* �6#   .+�5q      �� $	��Id:��6   $��0��6"   $��0��6/   $��Center=�6?   $��Right=�6Z   $��0,0,20,0q�6v   �� 5r      $��0��6F   $��1��6S   $��Center=�6c   $��0,0,20,0q�6~   $# Value must be a valid integer��6�   ��6   �� *CurrentReactor.Id+$ TwoWay=� 6�   �� *IsToolTipVisible+�� 5s      $��1��6M   $��1��6Z   
���  ��$��Center=�6{   $
��Wrap=�6�   ��6   �� *%#CurrentReactor.ValidationErrors[Id]+6�   �� 5u      $��Type:��6   $��2��6$   $��0��61   $��Center=�6A   $��Right=�6\   $��0,0,20,0q�6x   �� 5v      $��2��6   $��1��6$    �� DisplayMemberPath$
 Name��5x      $
 Name��5y      $��Center=�5z   <   $��0,0,20,0q�6W   $ Chose the type here��6i   ��5w      �� $ ReactorTypes� 5z      �� $ AddType� 6�   �� *IsToolTipVisible+�� 5|      $��Name:��6   $��3��6$   $��0��61   $��Center=�6A   $��Right=�6\   $��0,0,20,0q�6x   �� 5}      $��3��6H   $��1��6U   $��Center=�6e   $��0,0,20,0q�6�   $ Enter the name here��6�   ��6   �� *CurrentReactor.Name+$ TwoWay=� 6�   �� *IsToolTipVisible+�� 5~      $��4��6O   $��1��6\   
���  ��$��Center=�6}   ��6   �� *'%CurrentReactor.ValidationErrors[Name]+6}   �� 5�      $��0��6   $��5��6!   $��2��62   $��2��6B   $	��150��6V   $	��150��6c   ��6o   �� *CurrentReactor.Type.Image+5�      �� 5�   
   $��3��6   $��2��6   $��30��6T   $��90��6`   $$ Press here to add new reactor.��5�      ��5�   .   �� *AddReactorCommand+ 5�   ;   �� *IsToolTipVisible+.��5�        $	
 Add=�6&   $��24��61   $��24��6=   5�      � 5�   
   $��4��6   $��3��6   $��Center=�60   $��30, 10, 30, 10q�6M    5�      �� 5�      $
��auto �6    60    5�      �� 5�      $
��auto �6#   62   �� 5�      $��* �6#   6/   .+�5�        $

 Help=�6&   $��24��62   $��24��6>   $��0��6I   $��center=�6V   y�6q   �� *IsHelpVisible+�� 5�      $��0��6   $��1��6%   $����To add a new item type in the ID, select the type and type in the name. After that, you will see the preview of the item based on its type on the image next to the form.��65   $��15(�6�   $
��Wrap=�6�   $ Center=�6	  y�6   �� *IsHelpVisible+6G  5�      5�      5�      
   M S B A M L   `   `   `  ����>  :PZ2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PZ2PZ2  5   	   %clr-namespace:PZ2.Views	PZ2.Views  5   	   -clr-namespace:PZ2.ViewModelPZ2.ViewModel  5   	       PZ2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:PZ2     5   	   )viewsclr-namespace:PZ2.Views     5   	   1	viewModelclr-namespace:PZ2.ViewModel     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	   -   5          � Title$  
MainWindow��5   	   $	��800��6   $
��1000��6)    � WindowStartupLocation$ CenterScreen=�5   "    � DataContext 5      '   !PZ2.ViewModel.MainWindowViewModel  5   
    � 	Resources 5       �� DataType,   &PZ2.ViewModel.NetworkEntitiesViewModel#   PZ2.Views.NetworkEntitiesView+   %PZ2.ViewModel.NetworkDisplayViewModel"   PZ2.Views.NetworkDisplayView-   'PZ2.ViewModel.MeasurementGraphViewModel$   PZ2.Views.MeasurementGraphView��%�   (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5       $��1* �6   .+�5#   
   ��  � ToolTip$ Press here to close help��6   $��50��64   $��40��6?   $
��Left=�6K   $��10, 5, 5, 5q�6g   ��6|   �� *ToggleHelpCommand+.��5$      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Help=�6*   $��24��66   $��24��6B   $��Center=�6M   5%      �� 5&   
   $( "Press here to undo the last action��6   $��50��6q   $��40��6|   $��Right=�6�   $��5, 5, 10, 5q�6�    ^� 	IsEnabled 6>   �� *IsHelpVisible+��6�   �� *UndoCommand+.��5'        $
 Undo=�6&   $��24��62   $��24��6>   $��Center=�6I   5(      ]� 5)   
   $��0��6   $��5q�6!   $��Center=�6,   .+�5*      o� $	��Top=�6   	 o� 
IsMainMenu	 . .@�5+      m� $��Network Entities��6   
 m� CommandParameter$
 networkEntities��5,       m� Command 5+   5   �� *
NavCommand+ m� Icon 5-        5.      $ CellphoneLink=�62   50      m� 51      $��Network Display��6   $
 networkDisplay��52       51   4   �� *
NavCommand+ 53        54      $ Network=�62   56      m� 57      $��Measurement graph��6   $
 measurementGraph��58       57   6   �� *
NavCommand+ 59        5:      $ ChartScatterPlot=�62   5<      5=      5>      �� 5?   
   $��1��6   $��2��6K   $��20,10,20,10q�6_   ��6&   �� *CurrentViewModel+6_   5@      5A      
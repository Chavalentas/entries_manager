   M S B A M L   `   `   `  ����G  CWPF_Frontend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WPF_Frontend.ViewWPF_Frontend.View  5   	   &     WPF_Frontend.View.AddEntryWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:WPF_Frontend.View     5   	   -   5          � Title$  	Add entry��5   	   $	��450��6   $	��800��6(    � WindowState$ 	Maximized=�64   .��5	      ��  �� Orientation$ Vertical=�6   .+�5
   
   �� $��50��6   $��50��6   $��0,10,0,0q�6)   ��6;   �� *GoBackCommand+.��5      �� .+�5      �� $ ��../Images/return_image.png��6   5      5      � 5   
   $��Center=�6   $��Stretch=�6,   $��0��6H   $��0��6U   $��	0,50,0,50q�5       � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      $��* �6#   .+�5      �� $��3��6   $��1��6)   
������� �� RadiusX$ 50��5       �� RadiusY$ 50��6!   
���   �5      �� 5      $��	Add entry��6   $��Center=�6(   $��25(�6E   $
��Bold"�6S   $��Center=�5      $��0��5      $��0��6$   � 5       $��Center=�6   $��Center=�60   $��0��5!      $��1��6#   $��	10,0,10,0q�60    5"      �� 5#      $��* �6'    5%      �� 5&      $��* �6$   �� 5'      $��* �6$   .+�5)      �� $ Vertical=�6   $��0��64   $��0��6A   .+�5*      �� $
��Left=�6   $��20(�67   .��6E   Title6L   �� 5+      
�������$	��300��61   ��6=   �� *
EntryTitle+ �� Mode$ TwoWay=�5,      �� 5-      $ Vertical=�6   $��1��64   $��0��6A   .+�5.      �� $
��Left=�6   $��20(�67   .��6E   Text6K   �� 5/      
�������$	��300��61    �� TextWrapping$
 Wrap=�6=   "	 �� VerticalScrollBarVisibility$	 Visible=�50      $	��200��51      $	��200��52      ��6*   �� *	EntryText+$ TwoWay=�53      54      �� 55      $��	0,20,0,50q�6   $	��100��6(   $��Submit��64   $��Center=�56      $��Center=�62   
�������$��2��57   (   $��0��65   ��6E   �� *SubmitCommand+58      59      5:      
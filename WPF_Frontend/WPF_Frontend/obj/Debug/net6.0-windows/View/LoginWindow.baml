   M S B A M L   `   `   `  ����G  CWPF_Frontend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WPF_FrontendWPF_Frontend  5   	   ?$clr-namespace:WPF_Frontend.ViewModelWPF_Frontend.ViewModel  5	   	   #    WPF_Frontend.View.LoginWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WPF_Frontend     5   	   3vm$clr-namespace:WPF_Frontend.ViewModel     5	   	   -   5          �Name$  loginWindow��5
   	    � Title$ Login��5   	   $	��450��6   $	��800��6$    � WindowState$ 	Maximized=�5
      .��5      � $��Center=�6   $��Stretch=�6(   $��0��6D   $��0��6Q   $��	0,50,0,50q�5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��4��6   $��1��6%   
������� �� RadiusX$ 50��5       �� RadiusY$ 50��6!   
���   �5      �� 5   
   $��Login��6   $��Center=�6    $��25(�6=   $
��Bold"�6K   $��Center=�5      $��0��5      $��0��6$   � 5   
   $��Center=�6   $��Center=�6,   $��0��5      $��1��6#   $��	10,0,10,0q�60    5      �� 5       $��* �6#    5"      �� 5#      $��* �6    �� 5$      $��* �6    .+�5&      ��  �� Orientation$ Vertical=�6   $��0��60   $��0��6=   .+�5'      �� $
��Left=�6   $��20(�63   .��6A   Username or email6T   �� 5(      
�������$	��300��6-   ��69   �� *UsernameOrEmail+ �� Mode$ TwoWay=�5)      �� 5*      $ Vertical=�6   $��1��60   $��0��6=   .+�5+      �� $
��Left=�6   $��20(�63   .��6A   
Password6K   G� 5,      -   5        
�������$	��300��5,   1   6=   5-      5.      �� 5/   
   $��	0,20,0,50q�6   $	��100��6$   $��Submit��60   $��Center=�50      $��Center=�62   
�������$��2��51   (   $��0��65   ��6E   �� *SubmitCommand+�� 52   
   $ 
Horizontal=�6   $��3��6.   $��0��6;   $��Center=�6K   $��0,0,0,10q�53      .+�54      �� $��No account yet?��6   �� 55      $�� ��6   �� 56      �� 57      	 �� Command	 6   �� *RegisterCommand+.�6@   
Register59      5:      �� 5;      $��.��6   5<      5=      5>      
   M S B A M L   `   `   `  ����   term_project!    term_project.AdminDashboard   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Admin Dashboard��$	��600��$	��800�� � 	Resources    ActionButtonStyle �� 
TargetType �� Property �� Value��%�  &	        '�� �     '�� N    �� " ��.���  � ��$ 40����  � ��$ 10q���  � �� �	#FF4C87B9��  � ��
 �������  � ��$ 14(���  � ��$ 0q���  �@�Cursor$
 Hand����  � ��$
 10,5q���  � ��$
 Left=��� " ��.���  � ��
 �������  � ��$ 10q���  �@��TabStripPlacement$
 Left=��� " ��.���  � ��$	 150����  � ��$ 60����  � ��$ 14(���  � �� �	#FF333333��  � �� �	#FFE6E6E6.���� $��10q�.@��� $��User Management��.��� .+�&   System.Windows.Controls.DataGrid  -     Name$ UsersDataGrid��$��0,0,0,40q���  �� Orientation$ 
Horizontal=�$
��Left=�$��Bottom=�.+��� -    ��Name$ AddUserButton��$��Add User��#��[  �� -   $ EditUserButton��$��	Edit User��#��[  �� -   $ DeleteUserButton��$��Delete User��#��[  �� $��System Logs��.��� .+�  -   $ LogsDataGrid��$��0,0,0,40q��� $ Vertical=�$��Bottom=�.+��� -    ��Name$ SearchLogsTextBox��$	��200��$��5q��� $ 
Horizontal=�.+��� -   $��Search���� -   $��Refresh Logs���� $��Database Backup��.��� .+��� $ Vertical=�$��Bottom=�.+��� -	   $��Backup Database��� -
   	 �Name$	 BackupProgressBar���� -   
 ��Name$
 BackupStatusTextBlock���� $��Settings��.��� .+��� $ Vertical=�$��Bottom=�.+��� -   $��Save Settings���� -   $��Revert Settings���� -   $��Reset to Defaults��
   M S B A M L   `   `   `  ����H  DBoxID_GR1_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BoxID_GR1_WPFBoxID_GR1_WPF  5   	       BoxID_GR1_WPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:BoxID_GR1_WPF     5   	   -   5          �Name$  frmBox��5   	    � Title$ BoxID��5   	   $��602.267��6   $��638.887��6(    � 
ResizeMode$ NoResize=�68   .��5	      � -   5         �Name$
 grid��5	      .+�5
   
   ]� $
��Left=�6   $	��100��6/   $��	330,5,0,0q�6<   $	��Top=�6O   $	��250��6g   ���	#FF89FF9C��.  6�   �� 5   
   -   5         ��Name$ label��5      $��
Print Date��6   $
��Left=�64   $��	13,10,0,0q�6O   $	��Top=�6b   �� 5   
   -   5        $ 
label_Copy��5      $��	Ship Date��6$   $
��Left=�68   $��	13,41,0,0q�6S   $	��Top=�6f   �� 5   
   -   5        $ label_Copy1��5      $��Ship Status��6%   $
��Left=�6;   $��
339,10,0,0q�6V   $	��Top=�6j   �� 5   
   -   5        $ label_Copy2��5      $��Ship To��6%   $
��Left=�67   $��
339,41,0,0q�6R   $	��Top=�6f   �� 5   
   -   5        $ label_Copy3��5      $��Invoice��6%   $
��Left=�67   $��
339,72,0,0q�6R   $	��Top=�6f   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ dtpPrintDate��5      $
��Left=�6+   $��	82,10,0,0q�6F   $	��Top=�6Y   $	��145��6q      FirstDayOfWeek$ Monday=�6}     5   
   -	   5        $ dtpShipDate��5      $
��Left=�6*   $��	82,43,0,0q�6E   $	��Top=�6X   $	��145��6p   �� 5   
   -
   5         ��Name$ txtShipStatus��5      $
��Left=�6)   $��23��6D   $��
407,11,0,0q�6P    �� TextWrapping$
 Wrap=�6d   $	��Top=�6x   $	��162��6�   	 �� CharacterCasing$	 Upper=�6�   �� 5   
   -   5        $ 	txtShipTo��5      $
��Left=�6%   $��23��6@   $��
407,41,0,0q�6L   $
 Wrap=�6`   $	��Top=�6t   $	��162��6�   $	 Upper=�6�   �� 5   
   -   5        $ 
txtInvoice��5      $
��Left=�6&   $��23��6A   $��
407,72,0,0q�6M   $
 Wrap=�6a   $	��Top=�6u   $	��162��6�   $	 Upper=�6�   	� 5   
   -   5        
 	�Name$
 rdPrintDate��5      $�� ��6+   $
��Left=�66   $��
232,15,0,0q�6Q   $	��Top=�6e    d� 	IsChecked$
 TrueY�6}   	� 5   
   -   5        $
 
rdShipDate��5      $�� ��6*   $
��Left=�65   $��
232,47,0,0q�6P   $	��Top=�6d   &   System.Windows.Controls.DataGrid  5   
   -   5        $��dgvBoxId��5   /      AutoGenerateColumns .  $	��403��6?   $��9,0,10.6,9.8q�6L   $��Bottom=�6b   $��Stretch=�6}      CanUserResizeColumns .     CanUserSortColumns .     CanUserReorderColumns .     
IsReadOnly . ���	#FFCEF3DA
���   � J� OpacityMask
 �  ��   GridLinesVisibility$
 None=�6]     CanUserResizeRows .     SelectionUnit$
 Cell=�6�     SelectionMode$ Single=�6�  ��6
  Z�    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Box ID��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   ��  �� Path$ boxid�  5      $ Child Model��6%    6:   �� $ child_model�  5      $
 User��6%    63   �� $ user_cd�  5      $ 
Print Date��6%    69   �� $ 	printdate�  5      $ 	Ship Date��6%    68   �� $ shipdate�4  .System.Windows.Controls.DataGridTemplateColumn  5         CellTemplate 5      �� 5       .R�5!      �� $��btnOpen��6%   -   5        .��5!   J   Open6P   5"      5$      5&      �� 5'   
   -   5         ��Name$ 	btnSearch��5'      $��Search��6$   $
��Left=�65   $��
80,118,0,0q�6P   $	��Top=�6d   $��75��6|   $��25��6�   6�   �� 5(   
   -   5        $ btnAddBoxID��5(      $��	Add BoxID��6&   $
��Left=�6:   $��182,118,0,0q�6U   $	��Top=�6j   $	��104��6�   $��25��6�   �� 5)   
   -   5        $ btnShipmentHistory��5)      $��Shipment History��6-   $
��Left=�6H   $��314,118,0,0q�6c   $	��Top=�6x   $	��130��6�   ���	#FF0523FF$
��Bold"�6�   $��25��6�   6�   �� 5*   
   -   5        $ 	btnCancel��5*      $��Cancel��6$   $
��Left=�65   $��471,118,0,0q�6P   $	��Top=�6e   $��75��6}   $��25��6�   6�   ]� 5+   
   $
��Left=�6   $	��425��6/   ��.  $��
9,0,0,-0.2q�6R   $��Bottom=�6f   $	��614��6�    J� RenderTransformOrigin$ 0.516,0.512&�6�   5,      5-      
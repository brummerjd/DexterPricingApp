   M S B A M L   `   `   `  ���� [  WJDB.Library.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d3369d1bcc76ef37Y@clr-namespace:JDB.Library.Controls;assembly=JDB.Library.ControlsJDB.Library.Controls  ]Bclr-namespace:JDB.Library.Converters;assembly=JDB.Library.ControlsJDB.Library.Converters  eFclr-namespace:JDB.Library.Controls.Icons;assembly=JDB.Library.ControlsJDB.Library.Controls.Icons   DexterPricingApp!   DexterPricingApp.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Ucontrols@clr-namespace:JDB.Library.Controls;assembly=JDB.Library.Controls     Y
convertersBclr-namespace:JDB.Library.Converters;assembly=JDB.Library.Controls     WiconFclr-namespace:JDB.Library.Controls.Icons;assembly=JDB.Library.Controls     -   $��myWindow��  � Title$"  Brad's Equipment Application��$	��650��$
��1200�����#ADD6FF � WindowStartupLocation$ CenterScreen=� � 	Resources  �� 
TargetType �� Property �� Value(   "JDB.Library.Controls.NumberTextBox7   1JDB.Library.Converters.ValueToVisibilityConverter   ValueToVisibilityConverter4   .JDB.Library.Converters.ValueToBooleanConverter  ValueToBooleanConverter+   %JDB.Library.Controls.TabSizeConverter  TabSizeConverter `� 	Converter �� RelativeSource �� Mode	 �� AncestorType
 �� Path��%  '��       '  %     &	  7     &	 <     &	 A     '�� F     7  �� " ��.���  � ��$
 Bold"��� "  .�-         �� " ��.���  � �� `� 8   .1���  �� $ FindAncestor=�"	 ���� $
 ActualWidth� �� $ FindAncestor=�"	 ��.��]� ��. .+��  � RowDefinitions �� $��60 ��� $��* �.+��� $��0��$��Magneto)�$��48(�$��10,5,0,0q�$
��Left=�$	��Top=�Brad's Equipment� $��1�� � ColumnDefinitions �� $	��300 ��� $	��325 ��� $��* �.+��� $��3��
���   �$��8��	 �� X1$ 0��	 �� Y1$ 0��	 �� Y2$ 0��	 �� X2 ��  �� ElementName$ myWindow��$
 Width�� $��0�� �� $��* ��� $	��165 �.+�� $��0��.+��� $��24(�$��	10,10,0,0q�$	��Top=�$
��Left=�	CUSTOMERS�� $��	10,55,0,0q�$
��Left=�$	��Top=� �� Orientation$ 
Horizontal=�.+��� $��16(�$��0,0,0,0q�$��Center=�	Search:�� -   $��12(�$	��150��$��10,0,0,0q�$��Center=�{��� $ myWindow��$
 EditMode� �� 	Converter# [  �� ConverterParameter$ reverse������ $ myWindow��$
 CustomerSearch��� -   $��	0,55,10,0q�$��25��$��25��$	��Top=�$��Right=�$��Center=�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse��.��,   &JDB.Library.Controls.Icons.AddCustomer  �� -    ��Name$ CustomersGrid��$��10,85,10,10q�{��� $ myWindow��$
 EditMode�# [ $ reverse������ $ myWindow��$
 	Customers� �� SelectedItem �� $ myWindow��$
 SelectedCustomer�  �� Color��%8   (��       *SystemColors.ControlBrushKey+)�� # Z�� � ContextMenu �� .@�m� -   $��Change Name��m� -   $��Delete������ .R�� $��35��.+��� $��10,0,0,0q�$
��Left=�$��Center=����� *Name+� $��1��.+��� $��14(�$��10,5,0,0q�$	��Top=�$
��Left=�Customer Notes�� $��90��$��10,19,10,49q�{��� $ myWindow��$
 EditMode�# [ $ reverse������ $ myWindow��$
 SelectedCustomer.Notes�� $��1��.+��� $��24(�$��	10,10,0,0q�$	��Top=�$
��Left=�ADD CHARGES�� $ Vertical=�$��0,61,0,0q�$	��Top=�.+��� $��0��$��	10,0,10,0q�$��Center=�$��Center=�.@��� -   $��PartTab��$
��Part��{��� $ myWindow��$
 EditMode�# [ $ reverse��.��� .+�� $��55��$	��Top=� �� $��* ��� $��* �.+��� $��0��$��Center=�$��Center=�$ 
Horizontal=�.+��� $��16(�$
��Left=�$��Center=�
Taxable:�� $��10,0,0,0q�$
��Left=�$��Center=� d� 	IsChecked �� $ myWindow��$"
 SelectedPartCharge.IsTaxable��� $��1��$��Center=�$��Center=�$ 
Horizontal=�.+��� $��16(�$��Center=�	Quantity:  $��12(�$��25��$��5,0,0,0q�$
��Left=�$��Center=����� $ myWindow��$!
 SelectedPartCharge.Quantity��� $��2,0,0,0q�$
��Left=�$��Center=�.+��� -	   $��20��$��20��$��Center=�.��$   JDB.Library.Controls.Icons.Add  �� -
   $��20��$��20��$��0,5,0,0q�$��Center=�$	��Top=�.��)   #JDB.Library.Controls.Icons.Subtract  �� $��	10,55,0,0q�$
��Left=�$	��Top=�$ 
Horizontal=�.+��� $��16(�$��Center=�	Search:�� -   $��12(�$	��100��$��10,0,0,0q�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse������ $ myWindow��$
 
PartSearch�&   System.Windows.Controls.DataGrid  -     Name$ 	PartsGrid��$��10,80,10,25q�$	��214��   AutoGenerateColumns .     SelectionMode$ Single=�   SelectionUnit$ FullRow=�   
IsReadOnly . ���� $ myWindow��$
 Parts�{��� $ myWindow��$
 EditMode�# [ $ reverse�� �� $ myWindow��$
 SelectedPart� ��%8   (��       *SystemColors.ControlBrushKey+)�� # Z�� �� .@�m� -   $
��Edit��m� -   $��Delete��   Columns 0	  *System.Windows.Controls.DataGridTextColumn	  ,
  &System.Windows.Controls.DataGridColumn  
  Header$
  Code��! 
  Width5  /System.Windows.Controls.DataGridLengthConverter$! 60 1  +System.Windows.Controls.DataGridBoundColumn"   Binding" �� $

 Code�	  $  Description��$! * " �� $
 Description�	  $  Price��$! 75 " �� $
 Price�# �� StringFormat$# C��� $��22��$��Bottom=� �� $��* ��� $��* �.+��� -   $��0��$	��100��$��20��$��10(�$
��Bold"�$��Center=�$��Center=�y��� $ myWindow��$
 EditMode�# [  $ reverse��.��Create Custom Part�� -   $��1��$	��100��$��20��$��10(�$
��Bold"�$��Center=�$��Center=�y��� $ myWindow��$
 EditMode�# [  $ reverse��.��Import Part List�� -   $��MiscTab��$��Miscellaneous��{��� $ myWindow��$
 EditMode�# [ $ reverse��.��� $	��300��$��Center=�$��Center=�.+��� $��16(�$��	15,15,0,0q�$
��Left=�$	��Top=�Date:(  "System.Windows.Controls.DatePicker  $��12(�$	��105��$��	0,12,15,0q�$	��Top=�$��Right=�$   SelectedDate$ �� $ myWindow��$
 SelectedMiscCharge.Date��� $��16(�$��	15,45,0,0q�$
��Left=�$	��Top=�Charge Amount:�� $��	0,45,15,0q�$��Right=�$	��Top=�$ 
Horizontal=�.+��� $��16(�$  $��75��$��3,0,0,0q�$��Right=����� $ myWindow��$
 SelectedMiscCharge.Amount�$# {0:F2}���� $��16(�$��	15,75,0,0q�$
��Left=�$	��Top=�Include Tax:�� $��	0,75,15,0q�$��Right=�$	��Top=� �� $ myWindow��$"
 SelectedMiscCharge.IsTaxable��� $��16(�$��
15,105,0,0q�$
��Left=�$	��Top=�Description:�� $��75��$��20,130,20,0q�$	��Top=�% �� TextWrapping$% WrapWithOverflow=����� $ myWindow��$$
 SelectedMiscCharge.Description�� $	��Top=� �� $��* ��� $��35 �.+�� $��1��$��0,0,0,0q�$��Bottom=�.+��� -   $	��100��$��30��$
��Bold"�$��Center=����#40A3EF
�������y��� $ myWindow��$
 EditMode�# [  $ reverse��.��
ADD CHARGE�  �� $��* ��� $��* �.+��� -   $��0��$��75��$��30��$
��Bold"�$��Center=����#4DDB94y��� $ myWindow��$
 EditMode�# [  .��Save�� -   $��1��$��75��$��30��$
��Bold"�$��Center=����#FF8080y��� $ myWindow��$
 EditMode�# [  .��Cancel� $��2��.+��� $��24(�$��	10,10,0,0q�$	��Top=�$
��Left=�PRINT CHARGES�� $ 
Horizontal=�$��22��$��0,40,0,0q�$��Center=�$	��Top=�.+��� $��12(�$
��Left=�$��Center=�	Select:�� -   $��12(�$
��Bold"�$��25��$��20��$��5,0,0,0q�$
��Left=�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse��.��All�� -   $��12(�$
��Bold"�$��40��$��20��$��5,0,0,0q�$
��Left=�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse��.��None�� -   $��12(�$
��Bold"�$��55��$��20��$��5,0,0,0q�$
��Left=�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse��.��	Between  -   $��BetweenDatePicker1��$��12(�$��90��$��5,0,0,0q�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse���� $��3,0,0,0q�$��Center=�and  -   $��BetweenDatePicker2��$��12(�$��90��$��3,0,0,0q�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse���� $ 
Horizontal=�$��0,65,0,0q�$	��Top=�$��Center=�.+��� -   $
��Bold"�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse�� �� $ myWindow��$
 ShowPrintedCharges�.��Show Printed Charges�� -   $
��Bold"�$��20,0,0,0q�$��Center=�{��� $ myWindow��$
 EditMode�# [ $ reverse�� �� $ myWindow��$
 ShowPartCharges�.��Show Part Charges�� -   $��20��$��10(�$
��Bold"�$��20,0,0,0q�$��Center=�.��Calculate Totals  -   $ ChargesGrid��$��10,87,10,49q� .  $ Single=����� $ myWindow��$
 Charges�{��� $ myWindow��$
 EditMode�# [ $ reverse�� �� $ myWindow��$
 SelectedCharge� �� .@�m� -   $
��Edit��m� -   $��Delete�� 4  .System.Windows.Controls.DataGridTemplateColumn  $  Include��$! 50 &   CellTemplate& �� .R��� $��Center=�$��Center=� �� $
 Include�' �� Mode$' TwoWay=�( �� UpdateSourceTrigger$( PropertyChanged=�	  $
  Date��$! 60 " �� $

 Date�$# M/d/yy��	  $  Description��$! * " �� $
 Description�	  $  Amount��$! 75 " �� $
 Total�$# C���� -    $��75��$��30��$
��Bold"�$��0,0,0,14q�$��Center=�$��Bottom=����#40A3EF
�������{��� $ myWindow��$
 EditMode�# [ $ reverse��.��PRINT
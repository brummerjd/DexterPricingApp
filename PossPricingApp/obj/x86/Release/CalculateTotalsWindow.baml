   M S B A M L   `   `   `  ����   DexterPricingApp,    &DexterPricingApp.CalculateTotalsWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $��myWindow��  � Title$  Calculate Totals��$	��265��$	��300�����#F0F0F0 � WindowStartupLocation$ CenterOwner=� � 	Resources  �� 
TargetType �� Property �� Value��%/   '��       �� " ��.���  � ��$
 Bold"�.���  � RowDefinitions �� $��50 ��� $��* ��� $��50 �.+��� $��0��$��26(�$��Center=�$��Center=�Calculate Totals� $��1��.+��� $��16(�$��	10,10,0,0q�$
��Left=�$	��Top=�Determine totals between:��  �� Orientation$ 
Horizontal=�$��0,40,0,0q�$	��Top=�$��Center=�.+�(  "System.Windows.Controls.DatePicker  -   $��BetweenDatePicker1��$��12(�$��90��$��5,0,0,0q�$��Center=��� $��14(�$��7,0,0,0q�$��Center=�and  -   $��BetweenDatePicker2��$��12(�$��90��$��7,0,0,0q�$��Center=��� $��16(�$��	10,0,0,25q�$
��Left=�$��Bottom=�Tax Charges:�� $��16(�$��	0,0,10,25q�$��Right=�$��Bottom=�����  �� ElementName$ myWindow��	 �� Path$	 	TaxAmount�
 �� StringFormat$
 C���� $��16(�$��10,0,0,0q�$
��Left=�$��Bottom=�Total Charges:�� $��16(�$��0,0,10,0q�$��Right=�$��Bottom=����� $ myWindow��$	 TotalCharges�$
 C���� -   $��2��$��30��$	��100��$��Center=�$��Center=�$
��Bold"�.��	CALCULATE
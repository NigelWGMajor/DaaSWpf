   M S B A M L   `   `   `  ���� B  >DaaSWpf, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:DaaSWpfDaaSWpf      DaaSWpf.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:DaaSWpf     -     � Title$   ��$	��600��$	��800�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ 	CanResize=����	#ff222222 � 	Resources '   !DaaSWpf.FalseToCollapsedConverter   FalseToCollapsedConverter&    DaaSWpf.TrueToCollapsedConverter  TrueToCollapsedConverter"   DaaSWpf.SizeToBrushConverter  SizeToBrushConverter ��Name 
 FadeIn �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value��%�   &	        &	      &	 
     &	            �� $ FadeIn��.��U� $ (UIElement.Opacity)�$ BigTextBlock��.\�  $ 0��$ 0Q�  $ 0:0:0.7��$ 1Q�.��]� ��. $��2q�.+��� -   $��bottom=�	 �� TextAlignment$	 Center=����	#FF0066aa���#aaaaaa
 �� Padding$
 6q�$��Italic$�$��DemiBold"�$^��XJokes are provided courtesy of I can haz dad joke api, (https://icanhazdadjoke.com/api) �� � ToolTip$% Click here to visit their site!��]� $	��Top=�$��4q�.+��� $��Right=�$��23��$��23��$��4q����#aaaaaa���	#FF0066aa$��	ExtraBold"�$ Click here to exit������ *CloseCommand+.��X�� $��10q����	#FF0066aa$
��Left=�$��32(�$��Arial)�Daas�� $��Bottom=����	#FF22aaff$
��Left=�$��12(�$��Italic$�$��Arial)�$ (what a concept!)��... Dad as a Service�� $	��Top=�
���  ��$��1q�$��2q�._��  � Orientation$ 
Horizontal=�$��2q�
���   �.+�	� $��4q�$	��7,0q�$��Center=����	#FF22aaff$? 9Select this option to show a random joke every 10 seconds�� d� 	IsChecked �� *	IsAutoRun+.��Auto-update	� $��4q�$	��7,0q�$��Center=����	#FF22aaff$; 5Select this option to enter an (optional) search term�� �� *
IsSearch+.��Use search term:�� $	��4,2q�$��2q�$	��100�����	#ff333333���	#ffcccccc$ Enter a search term here������ *
SearchTerm+�� ���	#ffcccccc���	#FF0066aa$- 'Click here to retrieve up to 30 results��$��80��$��2q����� *SearchCommand+.��Search�� $��Center=�$
 20,0,0,0q�
����·����� *ResultStatus+�� $��Bottom=�._���  �� Orientation$ 
Horizontal=�$��4q�
���  ��y��� *	ShowError+ �� 	Converter# [  .+��� 
��� ���$��4q�Error:�� 
��� ���$��4q����� *	ErrorText+�� -    ��Name$ 	MainField��$��2q����	#ff333333._�� .+��� $��Stretch=�$��Disabled=�
������ y��� *	IsAutoRun+# [ ���� *Jokes+����  �� 
TargetType" ��.���  �� Property � �� �� Value$ Stretch=����� .R��� 
����  �$��Stretch=�$��1q� �� Padding$ 4q�$��2q� �� CornerRadius$ 5��
�������._��  � ColumnDefinitions �� $
��Auto ��� $��* �.+��� $��0��$	��Top=�$��23��$��23��$
 3q�$��2q�$	 Center=�
����������� *size+���� *size+# [  �� *	sizeTip+�� $��1��$
��Wrap=�$��Stretch=�$��13(�$	
 7,4q����� *joke+ �� *wordCountText+�� -    ��Name$ BigTextBlock��$��Center=�$��Center=�$	 Center=�$��50q�$��32(�$
��Wrap=�
����·�
������ y��� *	IsAutoRun+# [  ���� *	BigJoke+
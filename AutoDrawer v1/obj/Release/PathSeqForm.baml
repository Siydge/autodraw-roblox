   M S B A M L   `   `   `  ���� E  AAutoDrawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:AutoDrawer
AutoDrawer      AutoDrawer.PathSeqForm   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:AutoDrawer       � Title$  PathSeqForm��$	��320��$	��250�� � 
ResizeMode$ CanMinimize=� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � Topmost . '  !System.Windows.Shell.WindowChrome   WindowChrome      CaptionHeight$	 320Q�%  System.Windows.SystemParameters"   WindowResizeBorderThickness   ResizeBorderThickness# Z 	 � 	Resources	    FocusVisual
 �� Property �� Value �� StrokeDashArray J� SnapsToDevicePixels  Button.Static.Background �� Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border 	 Button.Disabled.Foreground �� 
TargetType �� 
TargetType ��Name ��Name �� Triggers P� Property P� Value �� 
TargetName��%*  &	        &	 N     &	 c     &	 x     &	 �     &	 �     &	 �     &	 �     &	 �     &		 �     '��     7   7  7  7  7  7 	 �� .��� 
 � �� �� .R��� $��2q�$	 1 2R�#��� �� . $��1���� $ 	#FFDDDDDD���� $ 	#FF707070���� $ 	#FFBEE6FD���� $ 	#FF3C7FB1���� $ 	#FFC4E5F6���� $ 	#FF2C628B���� $ 	#FFF4F4F4���� $ 	#FFADB2B5���� $ 	#FF838383���� " ��.��� 
 �@�FocusVisualStyle8   �� 
 � ��8  �� 
 � ��8  �� 
 � ��# � ���� 
 � ��$ 1q��� 
 � ��$ Center=��� 
 � ��$ Center=��� 
 � ��$ 1q��� 
 � �� �� " ��.R��� $ border��#��z��#��z��#��z�� . ._��� $ contentPresenter��|�.  #��z��#��z����. # z #��z�� P�  �@��IsDefaulted . . ��� $ border��# � ��
 � ��P�  �@J�IsMouseOver . . ��� 
 �@J�Opacity$	 0.7Q�P�  � �� . . ��� 
 �@J�Opacity$	 0.5Q�P�  � {� .  . ��� $ border��8  
 � ���� $ border��8  
 � ���� $ contentPresenter��8  
 � ��.��� -    �Name$	 BKG�����	#FF2C302E.+��� -    ��Name$ label��$$��Enter custom path int sequence��$��Center=�$	��Top=�$��25��$	��250��
�������$��5,0,0,0q�$��Center=�$��Center=�$��16(�$��0,9,0,0q��� -    ��Name$
 BKG2��$
��Left=�$	��212��$��	16,70,0,0q�$	��Top=�$	��214�����	#FF3C3C3C�� -    ��Name$ 	pathInput��   IsHitTestVisibleInChrome . $
��Left=�$��	16,37,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��116��$��27��$��18(� �� TextAlignment$ Center=����	#FF2C302E �� TextDecorations Z� �� -    ��Name$ submitButton�� . $��Submit��$
��Left=�$��
137,37,0,0q�$	��Top=�$��28��$��93��
����������	#FF2C302E�� -   $ CloseBox�� . $��Close��$
��Left=�$��
16,287,0,0q�$	��Top=�$��20��$	��214��
���������Z� �� -   $ 	textOuter��$���+---+---+---+
 |      |      |      |
+---+---+---+
 |      |      |      |
+---+---+---+
 |      |      |      |
+---+---+---+��$
��Left=�$	��Top=�$	��212��$	��218��
�������$��5,0,0,0q�$��Center=�$��Center=�$��22(�$��	12,70,0,0q��� -   $ t1��$��1��$
��Left=�$��	65,95,0,0q�$	��Top=�$��35��
�������$��50��$��Center=�$��Center=�$��24(���Z� �� -	   $ t2��$��2��$��Center=�$��0,95,0,0q�$	��Top=�$��34��
�������$��50��$��Center=�$��Center=�$��24(���Z� �� -
   $ t3��$��3��$
��Left=�$��
149,95,0,0q�$	��Top=�$��34��
�������$��50��$��Center=�$��Center=�$��24(���Z� �� -   $ t4��$��4��$
��Left=�$��
65,154,0,0q�$	��Top=�$��34��
�������$��50��$��Center=�$��Center=�$��24(���Z� �� -   $ t5��$��5��$
��Left=�$��149,154,0,0q�$	��Top=�$��34��
�������$��50��$��Center=�$��Center=�$��24(���Z� �� -   $ t6��$��6��$
��Left=�$��
65,213,0,0q�$	��Top=�$��34��
�������$��50��$��Center=�$��Center=�$��24(���Z� �� -   $ t7��$��7��$��Center=�$��	0,213,0,0q�$	��Top=�$��34��
�������$��50��$��Center=�$��Center=�$��24(���Z� �� -   $ t8��$��8��$
��Left=�$��149,213,0,0q�$	��Top=�$��34��
�������$��50��$��Center=�$��Center=�$��24(���Z� 
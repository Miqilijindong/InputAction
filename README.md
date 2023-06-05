# InputAction
搓招

这个项目是将每一个招式写成一个实例(SpecialMoveItem.cs)，然后在fixedupdate里检测输入，每一个实例都要检测键鼠输入。
然后输入的格式是按照小键盘的方向键判断的，8246对应着上下左右这个样子，5是没有输入。
SpecialMoveItem.cs里Simples是简化输入的意思，一一对应着Inputs字符，0是不是简化的按键，1是可以简化的按键。




0x2329		9001		栏目权限
0x232a		9002		全局权限
0x232b		9003
0x232c		9004		用户权限
0x232d		9005		全局权限






命名规范：

1. Helper + 名子
表示此类可以独立的，提法一些方法。

2.名子 + Helper           
名子-->model   表示此类为此model的controller


数据库中，字段不可以使用enity
因为，所有bin都entiy开头的，在context query的时候会将entity去掉   line 70 ,HelperContext.cs 
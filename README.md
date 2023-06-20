# Fireboy-And-Watergirl
使用Unity3D编写客户端，.Net6+C#控制台应用编写服务端，制作2D联机小游戏森林冰火人。服务端采用状态同步方式。  
本人是Unity新手，主要参考项目：  
https://github.com/PupYuan/Online_SpaceShooter.git  
https://github.com/luopeiyu/unity_net_book.git  
美术素材来自:  
https://github.com/lv0senku/Fireboy-and-Watergirl.git
# 使用说明
## 源代码
本仓库所提供的就是源代码，直接运行的程序见“可执行程序”。  
Fireboy-And-Watergirl_Server下的文件是.Net6框架下的控制台应用，需要使用Visual Studio2022打开，  
**并需要安装两个NuGet包：** Multiverse.CSharpSDK、Newtonsoft.Json  
Fireboy-And-Watergirl_Client下的文件是Unity客户端程序，我的Unity版本是Unity 2022.2.1f1  
需要先开启服务端程序，再开启两个客户端程序，才能启动本项目。  
## 可执行程序
服务端可执行程序（.exe文件）：http://8.141.161.164/FireboyAndWatergirl/Fireboy-And-Watergirl_Server.zip  
客户端可执行程序（.exe文件）：http://8.141.161.164/FireboyAndWatergirl/Fireboy-And-Watergirl_Client.zip  

## 演示
https://github.com/github-Vge/Fireboy-And-Watergirl/assets/59076082/e5967247-9f49-443c-bb6f-5450ba02fac4

# 技术说明
1.客户端与服务端的通程使用原生Socket连接。  
2.通讯协议采用Json格式，使用Newtonsoft.Json插件对消息数据进行序列化和反序列化。  
3.粘包分包采用结束符号法。  
4.消息分发采用判空方法，NetMessage类为传输的消息类，此类中包含所有消息对象的引用，哪种消息的引用不为空，则代表是哪种消息。  
-这种方式简化了消息传输，提高了代码的可读性和可维护性，但处理效率较低（相比于反射）。  
5.服务端接收/发送消息采用单线程模式；客户端接收/发送消息采用多线程模式，多个线程使用消息队列来存储/提取消息。  
6.客户端的消息处理采用观察者模式，即C#中的event方法。  
7.服务端具有心跳机制，来周期性检测客户端是否存在。反之同理。  
8.箱子（Box）的状态同步做得不好。  









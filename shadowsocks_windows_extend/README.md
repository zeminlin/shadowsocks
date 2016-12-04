## 基于Shadowsocks for Windows V3.3.1 修改
===============

在原shadowsocks for Windows V3.3.1主界面基础上，添加使用测试账号按钮，从[iVPN] 和 [ishadowsocks] 获取实验账号，改善网络,提升效率。。。

**特别说明：**

> * 感谢 [ishadowsocks] 提供的实验账号，请多多支持商家，购买账号以畅游网络
> * 计划作为个人使用的网络测试工具，添加的代码部分，多使用硬编码，说不定哪天获取不到账号，暂不支持定时自动获取账号
> * 基于.NET Framework4.6，须使用VS2015打开，低版本的IDE可能不支持，编译后到bin目录下的`Debug/Release`文件夹获取相关文件
> * 如有侵犯相关人员权益，请联系本人（邮箱为`zemin_lynn#qq.com`替换@）删除相关资料，谢谢!






## 原shadowsocks for Windows的说明如下


Shadowsocks for Windows
=======================

[![Build Status]][Appveyor]

[中文说明]

#### Features

1. System proxy configuration
2. PAC mode and global mode
3. [GFWList] and user rules
4. Supports HTTP proxy
5. Supports server auto switching
6. Supports UDP relay (see Usage)

#### Download

Download the [latest release].

#### Basic

1. Find Shadowsocks icon in the notification tray
2. You can add multiple servers in servers menu
3. Select `Enable System Proxy` menu to enable system proxy. Please disable other
proxy addons in your browser, or set them to use system proxy
4. You can also configure your browser proxy manually if you don't want to enable
system proxy. Set Socks5 or HTTP proxy to 127.0.0.1:1080. You can change this
port in `Servers -> Edit Servers`

#### PAC

1. You can change PAC rules by editing the PAC file. When you save the PAC file
with any editor, Shadowsocks will notify browsers about the change automatically
2. You can also update PAC file from [GFWList] (maintained by 3rd party)
3. You can also use online PAC URL

#### Server Auto Switching

1. Load balance: choosing server randomly
2. High availability: choosing the best server (low latency and packet loss)
3. Choose By Total Package Loss: ping and choose. Please also enable
   `Availability Statistics` in the menu if you want to use this
4. Write your own strategy by implement IStrategy interface and send us a pull request!

#### UDP

For UDP, you need to use SocksCap or ProxyCap to force programs you want
to be proxied to tunnel over Shadowsocks

#### Multiple Instances

If you want to manage multiple servers using other tools like SwitchyOmega,
you can start multiple Shadowsocks instances. To avoid configuration conflicts,
copy Shadowsocks to a new directory and choose a different local port.

Also, make sure to use `SOCKS5` proxy in SwitchyOmega, since we have only
one HTTP proxy instance.

#### Server Configuration

Please visit [Servers] for more information.

#### Portable Mode

If you want to put all temporary files into shadowsocks/temp folder instead of
system temp folder, create a `shadowsocks_portable_mode.txt` into shadowsocks folder.

#### Develop

Visual Studio 2015 is required.

#### License

GPLv3


[Appveyor]:       https://ci.appveyor.com/project/icylogic/shadowsocks-windows-l9mwe
[Build Status]:   https://ci.appveyor.com/api/projects/status/ytllr9yjkbpc2tu2/branch/master
[latest release]: https://github.com/shadowsocks/shadowsocks-csharp/releases
[GFWList]:        https://github.com/gfwlist/gfwlist
[Servers]:        https://github.com/shadowsocks/shadowsocks/wiki/Ports-and-Clients#linux--server-side
[中文说明]:       https://github.com/shadowsocks/shadowsocks-windows/wiki/Shadowsocks-Windows-%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E

[ishadowsocks]:			http://www.ishadowsocks.org
[iVPN]:			  http://www.ifanqiang.cn/#free
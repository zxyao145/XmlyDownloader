# XmlyDownloader
喜马拉雅专辑下载器，基于NET Core 3.1 和 Avalonia UI，支持Windows、MacOS（不具备此系统，未验证）、Linux（不具备此GUI系统，未验证）。

注意：仅能够下载免费音频。

# 平台支持

1. win-x64：Wndows 64位操作系统
2. win-x86：Wndows 32位操作系统
3. osx-x64：最低 OS 版本为 macOS 10.12 Sierra
4. linux-x64：大多数桌面发行版，如 CentOS、Debian、Fedora、Ubuntu 及派生版本

平台代码详见：https://docs.microsoft.com/zh-cn/dotnet/core/rid-catalog

# 下载与使用

## 下载

请前往 [Releases](https://github.com/zxyao145/XmlyDownloader/releases) 选择对应平台进行下载。

## 使用

### 启动程序

**Windows**

运行压缩包中的**XmlyDownloader.exe**

**MacOS、Linux**

双击运行压缩包中**XmlyDownloader**

### 使用

程序启动后界面如下图1所示，在专辑地址中输入专辑的url（例如图2，专辑郭德纲单口《丑娘娘》，https://www.ximalaya.com/xiangsheng/8287357/ ），点击搜索按钮，搜索结果如图3所示，专辑中的音频将会以表格的形式呈现出来。

![image-20200620164934047](https://github.com/zxyao145/XmlyDownloader/blob/master/mdfiles/start.png)

<Center>图1 启动界面</Center>

![image-20200620165340869](https://github.com/zxyao145/XmlyDownloader/blob/master/mdfiles/专辑示例.png)

<Center>图2 专辑示例</Center>

![image-20200620165513485](https://github.com/zxyao145/XmlyDownloader/blob/master/mdfiles/搜索结果.png)

<Center>图3  专辑搜索结果</Center>

在搜索结果左侧，勾选想要下载的音频，然后点击下载。下载文件将保存在程序运行目录下 **xmlyDownloader/专辑名** 的文件夹中，此处下载结果保存在 **xmlyDownloader/【高清】郭德纲单口《丑娘娘》**文件夹中，如图4所示：

![image-20200620170346355](https://github.com/zxyao145/XmlyDownloader/blob/master/mdfiles/下载结果.png)



<Center>图4 下载结果</Center>

# 协议

本工具仅限个人学习，不用于商业等用途，所涉及的所有音视频资源版权归喜马拉雅所有。
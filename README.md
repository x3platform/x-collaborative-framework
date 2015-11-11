## x-collaborative-framework ##

### 如何编译此项目?

#### 编译环境要求
- **.NET Framework 4.0** 以上或者 **Mono 3.0** 以上
- 构建工具 [NAnt](https://github.com/nant/nant "https://github.com/nant/nant")

#### Winodws 环境
假设项目目录为D:\github\x-collaborative-framework

`cd D:\github\x-collaborative-framework`

`nant -t:net-4.0`

> 如果一切顺利可以在 dist\net-4.0\ 目录下找到编译后的文件.

#### Linux 环境

假设项目目录为 /github/x-collaborative-framework

`cd /github/x-collaborative-framework`

chmod +x build

`nant -t:mono-4.0`

> 如果一切顺利可以在 dist\mono-4.0\ 目录下找到编译后的文件.

####测试

**MSTest**
`nant -t:net-4.0 -D:testing=mstest`

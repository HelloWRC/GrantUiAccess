# UIAccess 提权

> 提权方法及核心由 Doubx690i(Dubi906w/kriastans) 提供。

本插件可以为 ClassIsland 提升 UIAccess 令牌，使 ClassIsland 可以置顶到全屏 UWP 应用和系统界面上。

> [!caution]
> **注意事项：**
>
> - 您需要以管理员身份运行 ClassIsland 才能正常使用本插件的功能。
> 
> - 本插件进行的操作涉及敏感操作，可能会触发安全软件拦截。建议将 ClassIsland 工作目录添加到安全软件的排除名单中，以避免误报。

## 截图

![图片](https://github.com/user-attachments/assets/8dcd9bc7-7b60-4584-be39-a9843d9a3c97)


## 使用方法

1. 在插件市场下载并安装插件
2. 在 ClassIsland 应用文件属性 -> 【兼容性】页面中勾选【以管理员身份运行此程序】，以在运行 ClassIsland 时自动以管理员身份运行。
   > 如果您设置了开机自启，这可能会导致每次启动系统时，都会显示 UAC 弹窗。
3. 启动 ClassIsland，如果一切没有问题，插件会自动为应用提升 UIAccess。

## 致谢

提权方法及核心由 Doubx690i(Dubi906w/kriastans) 提供。

本项目使用了以下第三方库：

- [uiaccess](https://github.com/killtimer0/uiaccess/tree/master/uiaccess)
- [ClassIsland.PluginSdk](https://github.com/ClassIsland/ClassIsland)

## 许可证

本项目基于 [GNU General Public License v3.0](https://github.com/HelloWRC/GrantUiAccess/blob/master/LICENSE.txt) 许可。

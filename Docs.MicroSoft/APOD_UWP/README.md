# UWP实例——显示NASA_APOD 发布的天文图
[MOOC地址：生成访问 Internet 门户的应用](https://docs.microsoft.com/zh-cn/learn/modules/build-internet-connected-windows10-apps/2-building-app-that-accesses-internet?pivots=uwp)

> 在本单元中，我们将：
>
> - 使用 Visual Studio 2017 创建最多三个项目（Windows 窗体、WPF 和 UWP 应用），以显示自 NASA 程序 APOD 发布以来任意一天的天文图。
> - 了解如何使用 NuGet 包管理器来安装实用的第三方 API。
> - 了解如何使用可供选择日期的 UI 日历，以及可用于显示图片的图像元素。
> - 发出 HTTP 调用来检索图像。
> - 检查图像格式是否受支持，并稳健地处理错误。
> - 保存并还原 UI 设置，以展示如何提高应用的易用性。
> - 分阶段（可测试）生成大型应用，先验证一个阶段是否正常，再进入下一阶段。
> - （可选）从问世很长时间的 Windows 窗体入手，再到 WPF，最后到 UWP，展示较新技术如何提供一些超棒的编程功能。
>
> 此为启发性应用，并不执行强力无比应用可能执行的所有操作。 在本单元中，我们不会：
>
> - 巧妙地处理每日一图是视频的情况。 而是只显示错误消息。
> - 在网络连接耗时太长的情况下显示任何内容供用户查看。

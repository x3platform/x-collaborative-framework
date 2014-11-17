添加图标到屏幕里的有两种属性值apple-touch-icon和apple-touch-icon-precomposed，区别就在于是否会应用iOS中自动给图标添加的那层高光。
由于iPhone以及iPad都有两种分辨率的设备存在，图标的尺寸就需要做4个：144×144(iPad Retina)、72×72(iPad)、114×114(iPhone Retina)、57×57(iPhone)。
可以使用sizes尺寸来告诉设备该调用哪个图标。

- iPhone        => 48 x 48
- iPhone        => 57 x 57
- iPad          => 72 x 72
- iPhone Retina => 114 x 114
- iPad Retina   => 144 x 144

在Android上的主页icon需使用precomposed icon，大小为48x48 px.

启动画面的图片尺寸并非完全等于设备的尺寸，比如iPad2的尺寸是1024×768，但它的启动画面尺寸横向是1024×748，竖向尺寸是 768×1004，因为需要减去系统状栏的高度20px，而使用的Retina屏幕的iPhone4以及iPad3则需要减去状态栏的高度40px。
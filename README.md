# Zdanmaku
A Unity3D Danmaku Engine. Unity3D 弹幕引擎。 

## 使用方法(How to use)
* 导入(Import) Zdanmaku.unitypackage
* 点击(Click) /Tool/Zdanmaku/Zdanmaku Utility Panel 菜单
* 初始化(Click init button)->设置字体(Fill 'Font' field in panel)

## 显示弹幕(Display Danmaku)
```cs
Zdanmaku.Show(Content,Color,Offset,Size,Duration)
```
## 设置透明度
```cs
Zdanmaku.Alpha = 0.5f
```
## 暂停弹幕
```cs
Zdanmaku.Pause()
```
## 恢复暂定的弹幕
```cs
Zdanmaku.Continue()
```

![preview](https://raw.githubusercontent.com/DASTUDIO/Zdanmaku/master/img/1.jpg)

![preview](https://raw.githubusercontent.com/DASTUDIO/Zdanmaku/master/img/20.jpg)

![preview](https://raw.githubusercontent.com/DASTUDIO/Zdanmaku/master/img/1.png)

插件包含两个字体，一个思源，一个站酷，都是可以免费商用的。
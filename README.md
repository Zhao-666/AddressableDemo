# Addressable Demo
本项目用于练习Unity中的Addressable功能。

## 学习资料
- [Unity官方Addressable文档](https://docs.unity.cn/Packages/com.unity.addressables@1.18/api/UnityEngine.AddressableAssets.Addressables.html)
- [Unity官方Github示例](https://github.com/Unity-Technologies/Addressables-Sample)
- [Unity官方bilibili教学](https://www.bilibili.com/video/BV1p741167kM)
- [bilibili讲解Addressable教程](https://www.bilibili.com/video/BV1p54y1p7Pd)

## 项目目标
制作出三个场景，使用Addressable分别打包成三个AssetsBundle，实现按需下载更新包功能

## 项目需求
- 制作场景（已完成，0.5h）
- 实现展示包体大小、下载进度、包体加载与卸载等功能
    - 实现场景跳转与返回（已完成，1.5h）
    - 实现展示包体大小弹窗（已完成，3h）
    - 实现下载与删除包体（已完成，2h）
- 拆分模型资源为独立包体

## 项目进度
### 2021-12-15
1. 实现三个场景间的跳换（1h）
2. 实现下载与删除包体(2h)
3. 实现展示包体大小弹窗(2h)

#### 遇到问题
1. 使用Addressables.ClearDependencyCacheAsync()进行资源卸载时会报错：
System.Exception: Unable to clear the cache.  AssetBundle's may still be loaded for the given key.

解决方法：
查到了问题是在调用Addressables.DownloadDependenciesAsync时，未释放返回的资源，
在参数中传递autoReleaseHandle=true则解决了问题。

### 2021-12-14
1. 完成三个关卡场景（0.5h）
2. 制作UI界面（1h）
3. 编写场景跳转脚本（0.5h）

#### 遇到问题
1.使用LoadSceneAsync读取新场景后，如何使用UnloadSceneAsync跳转回初始场景？

解决方法：LoadSceneAsync使用Additive模式叠加场景，就可以使用UnloadSceneAsync回到原场景。
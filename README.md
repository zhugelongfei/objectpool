# Object pool for unity(C#)

# 介绍
游戏中不停创建释放的对象，可使用对象池管理，达到减少gc的目的。
比较常见的对象池模块，对象池的对象由工厂进行生产。创建对象所需的参数，可在new工厂对象时，传递到工厂对象中。
此工程为Unity Package的Git包，可通过Unity的PackageManagerWindow导入到需要使用的项目中。

# Unity导入步骤
- 在Unity中点击菜单栏的Window->Package Manager打开PackageManager面板
- 点击Add package from git url
- 复制git工程的地址，粘贴到输入栏
- 点击Add

# 应用场景
- 射击游戏中发射的子弹，超出屏幕后，回收到对象池，再次使用时，直接从对象池拿出即可，
- ...

# 使用方法
- 实现IPoolObject接口，来管理对象Ctor（有无参数取决于工厂类），Pop，Push，和OnDestroy的处理。
- 实现IPoolObjectFactory接口，用以创建上一步的对象
- 创建ObjectPool对象，构造函数中将工厂对象传入即可。

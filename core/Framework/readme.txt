# Tips by LComplete

0. Framework 
    这个类库不能引用任何第三方组件。
    项目里面其他任何类库都可以引用本类库。

1. EF 数据分页
    分页一般带有数据筛选条件，为提升扩展性，最好定义一个对象存储筛选属性，这个对象可继承 PagingQuery 类。
    PagingDataSource.cs 文件中为 IQueryable 定义了一个扩展方法 ToPagingDataSource，传入 PagingQuery 对象即可获得，PagingDataSource 继承自 PagingModel。
    在 UI 层使用强类型 PagingDataSource 对象，即能获取数据，又能通过 Html.NumerilPager 方法传入 PagingDataSource（或PagingModel）对象得到分页链接的 UI。
    OrderPagingQuery 可以提供多字段自定义排序的能力，页面上的封装还没设计成通用的形式，因此还没放到这个框架里面，后续添加进来。

2. 缓存的使用
    对外统一使用 CacheManager 对象，一般而言，只需要调用 Get<T>(string key, Func<T> ifnotfound, int cacheMinutes) 这个方法。

3. 配置的写入、读取
    自定义配置类继承自 ISetting 接口，配置文件路径为 “custom_configs/{class_name}.config”，具体示例可以看 CacheSetting 是如何使用的。

4. 日志抽象
    日志对象接口为 ILog，默认实现为 DebugLogger，外部程序可以通过 LogManager 中暴露的 LogFactory 替换日志工厂，随意更改底层使用的日志组件。注意：LogFactory 是全局的。
    在 Web 项目下切换使用 Log4Net：LogManager.LogFactory = new Log4NetFactory(Server.MapPath("log4net.config"));

5. 异常的级别
    Exceptions 文件夹下定义了几个常用的异常，VerifyException 表示验证异常，业务逻辑层的代码在验证到提交数据异常时，可以抛出此类异常，更详细的验证异常可以继承此类。

6. 面向切面的缓存说明
    在 Services 层中的实现方法上添加 CachingAttribute 即可实现对方法调用的缓存。
    
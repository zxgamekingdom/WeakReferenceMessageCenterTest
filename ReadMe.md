# 匿名委托

匿名委托,匿名方法

委托在使用上可以类比为一个函数指针,但实际上是一个类.

匿名委托(匿名方法/Lambda/闭包)在编译时会将该委托编译为一个类,并且该委托中捕获的变量会成为该类的字段.

# 弱消息中心

消息中心为发布订阅的管理者,发布者通过消息中心发布消息,订阅者在消息中心订阅自己感兴趣的消息,并执行动作.

订阅对象可以无需手动取消订阅,并且在订阅对象应该被GC回收时顺利被GC回收,不造成内存泄露问题.

# 使用普通委托与弱引用委托产生的问题

在订阅者订阅消息时,会将收到感兴趣的消息时需要执行的逻辑(即委托)传入消息中心,消息中心在收到发布者发布的消息时会调用订阅了该消息的订阅者们注册在消息中心的委托.

发布者与订阅者均为普通的DotNet对象.

订阅者注册在消息中心的委托可能捕获了订阅者本身.

如果消息中心以强引用的方式保存委托.则需要订阅者手动取消订阅,否则消息中心一直存在,则订阅者注册的委托也会一直存在,委托一直存在,则委托捕获的变量也会一直存在.

如果消息中心以弱引用的方式保存委托,则委托可能先于订阅者被GC回收,则发布者发布了订阅者感兴趣的消息时,消息中心无法调用订阅者注册的委托,因为委托已经被GC回收,执行委托会抛空引用异常.

# ConditionalWeakTable<TKey,TValue>

[ConditionalWeakTable<TKey,TValue>的微软文档](https://docs.microsoft.com/zh-cn/dotnet/api/system.runtime.compilerservices.conditionalweaktable-2?f1url=https%3A%2F%2Fmsdn.microsoft.com%2Fquery%2Fdev16.query%3FappId%3DDev16IDEF1%26l%3DZH-CN%26k%3Dk(System.Runtime.CompilerServices.ConditionalWeakTable`2);k(DevLang-csharp)%26rd%3Dtrue&view=netcore-3.1)

该类近似一个弱引用字典类,但该类并没有继承自IDictionary接口.但可以用LINQ或者迭代遍历等等逻辑做近似字典的增删改查.

该类的Key与Value均为弱引用.

当Key没有被GC回收时,Value也不会被GC回收.

只有当Key被GC回收时,Value才可能被GC回收.

当某个Key被GC回收后,其Key/Value对会自动从该类中消失,无法被找到.

因此可以在消息中心使用此类引用订阅者及其委托.订阅者应该被GC回收时,其委托并不阻止其被回收,且其委托也会被回收.

# 声明

本项目是对弱消息中心的一次尝试与实验.
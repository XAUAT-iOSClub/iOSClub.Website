# iOS Club和你一起学C#（6）函数

> 在上一节我们讲了String的更多用法
>
> 这一节我们继续学习

我们在第二节中讲到过：

>  初始值可以是 一个值，运算表达式，**函数结果**

我们在前面也讲过了，这个函数结果一般长这样：

```csharp
string result = Console.ReadLine();
```

我们之前听过，用过，但是一直以来都是用的官方的。就像数学中我们会用到什么sin呀，cos呀这些一样。但是计算机这个东西又不是玄学，官方的函数也绝对不是胡乱变来的。或者说，我们可以定义一个自己的函数吗？说不定还会比官方的算法更快呢。

## 函数

我们在中学的时候都学过，函数我们一般会这样写：

> f(x) = x*x

这里我们能看出来，函数有三个要素：

- 函数名，也就是这里的f
- 参数，也就是这里的x
- 返回值，也就是这里的x*x

对于C#来说，函数也应该这样去描述，因为这样就非常符合常理。但是C#总有自己的特殊之处。毕竟C#要考虑的不只是数字，还有很多很多的类型。

那么我们来看看还怎么用C#来描述上面的函数：

```csharp
int f(int x) { // f(x)
    return x * x; // = x*x
}
```

在这里，我们定义了一个函数，函数名为f，参数为x，为int类型。

对于C#这样一门强类型语言，跟数学的写法的一个不同就在于C#必须要指定参数的类型。毕竟数学只要考虑数字，而C#就要考虑很多很多类型了。

那么我们现在就有了这么一个需求。我们想表达一个分段函数，就比如我们想搞一个这样的函数：

> f(x) = |x|

也就是说，当x大于0时，f(x) = x，当x小于0时，f(x) = -x

那我们该怎么写呢？

## Return语句和Void

我们来看看:

```csharp
int f(int x) {
    if(x > 0)
        return x;
    return -x;
}
```

也就是说，其实最开始讲的那个函数括号里头不是只能写一句话，而是代表着一个代码块。这跟我们之前学到的那些语句是一样的。

那么到了这里各位也都明白了，这个return才是返回东西的关键。

return语句的意思就是返回，而return后面跟的这个值就是我们要返回的东西。而当代码运行到return语句的时候，就会直接返回这个值，后面的代码是不会再执行的。

我们在这里看一下:

```csharp
int f(int x) {
    if(x > 0)
    {
        return x;
        Console.WriteLine("这里是不会被执行的吗？");
    }
    return -x;
}

Console.WriteLine(f(5));
```

这里我们发现，我们给定了一个5，这是一个大于0的数字，但是返回了5之后我们看不到任何的输出。

但是我们会发现一个事情，对于Console.WriteLine()这种函数，他居然没有返回值？

```csharp
string result = Console.WriteLine();
// 这里会报错
```

我们这里根本就接不住这个函数的值，这是为什么？

因为这个函数的返回值定义的是void。void的意思就是没有返回值。

那么我们在这里也可以定义一个:

```csharp
void f(int x) {
    if(x > 0)
        Console.WriteLine(x);
    else
        Console.WriteLine(-x);
}
f(5);
```

我们在这里直接定义了一个没有返回值的函数。那如果我现在又有个需求呢？现在我需要一个函数，要求是没有返回值，然后当他不满足这个条件的时候，直接退出，满足这个条件的时候，就输出"你正在使用这个函数"和"你满足了这个条件"。而且if语句里头只能有一句话。

那这里就得先判断是否满足这个条件，如果满足，则直接退出；如果不满足就进行输出操作。

那我们可以在void函数里头使用return语句吗？当然可以：

```csharp
void f(int x)
{
    if(x > 0)return;
    Console.WriteLine("你正在使用这个函数");
    Console.WriteLine("你满足了这个条件");
}
```

我们在这里直接使用return语句，这样当x大于0时，函数就直接退出了。

那如果我们现在有这样一个需求，我们希望可以比较两个数，如果第一个数大于第二个数，就返回第一个数，否则就返回第二个数。也就是所谓的Max函数。

## 函数一定就只能是一个参数吗

我们发现，这个函数看来是必须要用到两个参数了。可是之前教的看到的都是一个或者没有啊。那怎么办？

很简单，设置两个参数不就行了吗：

```csharp
int max(int a,int b)
{
    if(a > b)return a;
    return b;
}

Console.WriteLine(max(10,20));
// 20
```

这里我们设置了两个int类型的参数，并进行比较，如果a大于b就返回a，否则就返回b。

相应的，我们可以不设置参数，或者设置多个参数:

```csharp
void write(){
    Console.WriteLine("Hello World");
}
write();
// Hello World
```

所以我们现在可以总结一下函数的写法：

```csharp
返回值类型 函数名称(0到无数个参数){
    // 函数体
}
```

那么现在就有一种新的情况，就是我想默认一个值，就比如说我默认不运行一段代码，但是当我给定一个值的时候，就运行这段代码。

那该怎么办？

```csharp
void write(bool isWrite = false){
    if(isWrite){
        Console.WriteLine("Hello iOS Club");
    }
    Console.WriteLine("Hello World");
}

write();
write(true);
```

我们这里可以看到，我们这里使用了两次这个函数，第一次的时候，我们没有给定任何参数，所以默认的参数是false，所以不会输出Hello iOS Club，但是第二次的时候，我们给定了一个参数，所以就会输出Hello iOS Club。

那我们回到刚才的max函数，我们的产品经理又有了新的需求，就是当我传入一个double类型的参数的时候，我需要返回一个double类型的结果。

那这个时候又该怎么办？有的人说我们要不然就再写一个新的:

```csharp
double doubkeMax(double a, double b)
{
    if (a > b)return a;
    return b;
}
```


嗯，这样写也是可以的，但是这样写代码的人，会觉得有点麻烦，因为当我们需要的类型越来越多的时候，这样写代码就会越来越乱。要有没有一种办法，我们就只使用max这个方法，但是我们可以传入不同类型的值，从而返回不同类型的数据呢？

## 重载

C#中，我们可以使用**重载函数**，来达到我们的目的。

来看看这段代码：

```csharp
int max(int a, int b)
{
    return a > b ? a : b;
}

double max(double a, double b)
{
    return a > b ? a : b;
}

int a = max(1, 2);
double b = max(1.0, 2.0);

Console.WriteLine(a);
Console.WriteLine(b);
```

我们这里就使用到了函数的重载，我们只需要给定不同的参数，就可以使用不同的函数，但是函数名是一样的。

## 为什么要使用到函数

讲句实话，我们讲到现在都没有讲为什么要用到函数这个东西。

我们来举个例子吧。例如我们要在控制台输出一个 Hello,World，如果没有函数的话，我们得自己写一个控制台，然后用像素一个一个的敲，最后才能在控制台上面显现。

那么这里也和显而易见了，函数就是为了方便我们**少写点代码**。或者说一个非常专业的，就是对**逻辑的抽象和复用**。

那么有了函数，我们就可以直接使用，打印我们的Hello,World了。

## 课后作业

1. 你可以自己写一个函数，在控制台中打印出你自己的名字或者什么什么的。
2. 使用函数来写一个乘法运算，要求支持int，double，还有字符串乘以一个数
3. 使用函数写一个兔子数列，要求支持输入一个数字，然后打印出这个数字对应的值

## 上节的答案

### 1. 创建一个程序，要求用户输入一个字符串，然后输出该字符串的长度。

```csharp
string result = Console.ReadLine();
Console.WriteLine(result.Length);
```

### 2. 请使用string来写一个判断是否为回文数的程序

```csharp
string input = Console.ReadLine();
for(int i = 0; i < input.Length / 2; i++){
    if(input[i] != input[input.Length - i - 1]){
        Console.WriteLine("不是回文数");
    }
}
Console.WriteLine("是回文数");
```

### 3. 请写一个简易的指令程序，要求的指令如下:
  
  - **help** : 显示帮助信息
  - **exit** : 退出程序
  - **add** : 添加一个数字到列表中
  - **remove** : 删除一个数字
  - **list** : 列出所有数字
  - **clear** : 清空列表
  - **print** : 打印一个值
```csharp
  while (true) {
      Console.WriteLine("请输入指令:");
      string input = Console.ReadLine();
      List<int> list = new List<int>();
      string[] args = input.Split(' ');
      switch (args[0]) {
        case "exit":
            return;
        case "add":
            int num = int.Parse(args[1]);
            list.Add(num);
            Console.WriteLine("添加成功");
            break;
        case "list":
            foreach (var item in list) {
                Console.WriteLine(item);
            }
            break;
        case "remove":
            int index = int.Parse(args[1]);
            list.Remove(index);
            Console.WriteLine("删除成功");
            break;
        case "help":
            Console.WriteLine("add <num>: 添加一个数字");
            Console.WriteLine("print: 打印一个值");
            Console.WriteLine("remove <num>: 删除数字");
            Console.WriteLine("help: 显示帮助信息");
            Console.WriteLine("exit: 退出程序");
            Console.WriteLine("clear: 清空列表");
            Console.WriteLine("list: 列出所有数字");
            break;
        case "clear":
            list.Clear();
            Console.WriteLine("列表已清空");
            break;
        case "print":
            Console.WriteLine(arg[1]);
            break;
        default:
            Console.WriteLine("无效的命令");
            break;
      }
   }
```

## 结尾

我们这次学习了C#的函数，不过也有叫方法的，其实都可以。

下节课，我们将扩展原有的数据类型。

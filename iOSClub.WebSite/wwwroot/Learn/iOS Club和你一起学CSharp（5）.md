# iOS Club和你一起学C#（5）String的更多用法

> 在上一节我们讲到了数组，列表，字典这三种数据结构。
>
> 但是你有没有想过，我们之前的有一个东西他很像上面这三个，也属于一种集合
>
> 没错就是字符串，这一节我们将好好讲讲string这个数据类型

## String是char[]的包装？

我们在使用string的时候确实可以这样做:

```csharp
string s = "Hello World";
Console.WriteLine(s[0]); //读取s的第一个字符
Console.WriteLine(s.Length); // 获取s的长度
```

我们甚至可以直接使用foreach来进行使用:

```csharp
foreach(char c in s)
    Console.WriteLine(c);
```

那么我们可以直接通过索引值来更新字符吗？

```csharp
s[0] = 'H';
// 错误
// this[int] is read-only
```

错误提示我们无法修改string的元素，也就是说，string跟char[]还是不太一样的

那么string跟char[]可以互转吗？

答案是可以的：

```csharp
// char[] -> string
char[] charArray = {'H', 'e', 'l', 'l', 'o'};
string myString = new string(charArray);  // "Hello"
// 当然，我们也可以选定需要使用到哪些字符
charArray = {'H', 'e', 'l', 'l', 'o', ' ', 'W', 'o', 'r', 'l', 'd'};
string myString = new string(charArray, 0, 5);  // "Hello"

// string -> char[]
string myString = "Hello";
char[] charArray = myString.ToCharArray();
```

如上面的代码所示，我们可以通过**new string()**来创建一个string，通过**ToCharArray()**来获取char[]

那string这样的话，我进行string的操作就很难搞呀。就比如说我们想要进行字符串的更改，那就得重新创建一个string，然后接着了。

有没有更好的办法？

## String的众多方法

是这样的，因为String的不可变性，有很多的方法

### 字符串操作


1. **Length** : 获取字符串的长度。

    ```csharp
        string str = "Hello, World!";
        int length = str.Length; // 13
    ```

1. **ToUpper()** : 将字符串转换为大写。

    ```csharp
        string str = "hello";
        string upperStr = str.ToUpper(); // "HELLO"
    ```

1. **ToLower()** : 将字符串转换为小写。

    ```csharp
        string str = "HELLO";
        string lowerStr = str.ToLower(); // "hello"
    ```


1. **Substring()** : 提取子字符串。

    ```csharp
        string str = "Hello, World!";
        string subStr = str.Substring(7, 5); // "World"
    ```


1. **IndexOf()** : 查找子字符串或字符的第一次出现位置。

    ```csharp
        string str = "Hello, World!";
        int index = str.IndexOf("World"); // 7
    ```

1. **LastIndexOf()** : 查找子字符串或字符的最后一次出现位置。

    ```csharp
        string str = "Hello, World! Hello!";
        int lastIndex = str.LastIndexOf("Hello"); // 13
    ```

1. **Contains()** : 判断字符串是否包含指定的子字符串。

    ```csharp
        string str = "Hello, World!";
        bool contains = str.Contains("World"); // true
    ```


1. **Replace()** : 替换字符串中的指定子字符串。

    ```csharp
        string str = "Hello, World!";
        string newStr = str.Replace("World", "C#"); // "Hello, C#!"
    ```


1. **Split()** : 分割字符串为字符串数组。

    ```csharp
        string str = "Hello, World!";
        string[] words = str.Split(' '); // ["Hello,", "World!"]
    ```


1. **Trim()** : 去除字符串两端的空白字符。

    ```csharp
        string str = "  Hello, World!  ";
        string trimmedStr = str.Trim(); // "Hello, World!"
    ```

## StringBuilder —— 将字符串拼接起来

事实上，我们在使用string的时候，如果要拼接很多的字符串，我们通常会这样做：

```csharp
int[] numbers = Enumerable.Range(0, 10001).ToArray();
StringBuilder builder = new StringBuilder("[");
foreach (int number in numbers){
    builder.Append(number); // 如果想要换行，使用builder.AppendLine(number);
}
builder.Append("]");
Console.WriteLine(builder.ToString());
```

这样的话就可以更加快速的生成和拼接字符串了。

## 练习

1. 创建一个程序，要求用户输入一个字符串，然后输出该字符串的长度。
2. 请使用string来写一个判断是否为回文数的程序
3. 请写一个简易的指令程序，要求的指令如下:
    - **help** : 显示帮助信息
    - **exit** : 退出程序
    - **add** : 添加一个数字到列表中
    - **remove** : 删除一个数字
    - **list** : 列出所有数字
    - **clear** : 清空列表
    - **print** : 打印一个值

## 上期的答案

请使用列表实现斐波那契数列：

```csharp
List<int> fibonacci = new List<int>(){1,1};
int n = int.Parse(Console.ReadLine());
for(int i = 2; i < n; i++){
    fibonacci.Add(fibonacci[i-1] + fibonacci[i-2]);
}
Console.WriteLine(string.Join(",", fibonacci));
```

## 结尾

本节内容到此结束，我们今天讲了String的更多用法。

下节课我们来讲一个我们一直在用但是却一直不了解的东西。

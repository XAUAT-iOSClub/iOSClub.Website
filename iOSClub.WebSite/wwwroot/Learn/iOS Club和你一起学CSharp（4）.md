# iOS Club和你一起学C#（4）

> 在上一节中，我们学习了分支与循环。现在我们可以写一些复杂的东西了
>
> 这一节我们继续学习

## 数据不是一个就能解决问题的

我们在之前的学习当中学会了很多的数据类型，大致分为3种：

- 跟数字相关的
- 跟字符相关的
- 和逻辑相关的（bool类型）

那我们现在也有了这样一个需求：

> 请存储一个人的九科成绩（语数英政史地物化生）

我们在之前肯定就使用九个变量来命名了，但是这也太麻烦了吧。而且如果我们的需求变成了十九科那怎么办？

当我们遇到这种情况的时候，其实最好的办法就是使用一种集合，把我们的数据都包含进来，这样就好了。

没错，在C#当中，还真有这种东西

## 数组

数组是一种基本的数据结构，用于存储固定大小的同类型元素的集合。数组中的每个元素都可以通过索引访问，索引是从0开始的整数。

我们现在可以来简单声明一下：

```csharp
int[] scores = new int[9]; 
// 这里我们创建了一个长度为9的数组，数组中的每个元素都是int类型
int[] scores2 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; 
//我们可以给定一些初始值，当然我们也可以写成下面这种形式 
int[] scores3 = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; 
```

对于数组，我们最基本的创建格式是这样的：

> 数据类型[] 变量名 = new 数据类型[];

那我们现在已经创建好了数组，那我们应该怎么样来读取和更改呢？

我们可以通过索引来读取和修改数组中的元素，索引是从0开始的整数。

例如这样：

```csharp
int[] scores = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
Console.WriteLine(scores[0]); // 输出：1
scores[0] = 10;
Console.WriteLine(scores[0]); // 输出：10
// 当然，我们也可以循环读取数组中的元素
for (int i = 0; i < scores.Length; i++) 
{
    Console.WriteLine(scores[i]);
    // 输出：10 2 3 4 5 6 7 8 9
}
```

我们这里示范了一下，通过for循环来一次读取了数组的所有元素。

不过这里我们需要注意这几点：

- 数组的长度是固定的，我们无法修改它的长度
- 数组的索引是从0开始的整数。我们可以给出一个公式：
  索引值 = 数组的序列数 - 1
  例如：
  索引值 = 1 - 1 = 0
  第一个的序列数是1，索引值就是0

那么我们现在就可以把之前的问题解决一下了：

```csharp
int[] scores = new int[9];
Console.WriteLine("请输入9门课的成绩：");
for (int i = 0; i < scores.Length; i++) 
{
    Console.Write("请输入第" + (i + 1) + "门课的成绩：");
    scores[i] = Convert.ToInt32(Console.ReadLine());
}
Console.WriteLine("你的成绩如下：");
for (int i = 0; i < scores.Length; i++) 
{
    Console.WriteLine("第" + (i + 1) + "门课成绩为：" + scores[i]);
    // 输出：第1门课成绩为：10
}
```

这里我们先创建了一个长度为9的数组，然后我们通过for循环来更改9门课的成绩，最后再使用for循环来输出9门课的成绩。

那有没有一种语法可以帮我们简化一下呢？

## foreach语句

foreach是C#当中的一个语法，它可以用来遍历数组中的元素。

我们可以这样写：

```csharp
int[] scores = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
foreach (int score in scores) 
{
    Console.WriteLine(score); // 输出：1 2 3 4 5 6 7 8 9
}
```

对于foreach语句，我们可以总分析一下他的结构：

> foreach(类型 变量名 in 数组名)

那我们现在就可以直接改一下我们之前的代码了：

```csharp
int[] scores = new int[9];
Console.WriteLine("请输入9门课的成绩：");
foreach (int score in scores) 
{
    Console.Write("请输入第" + (score + 1) + "门课的成绩：");
    score = Convert.ToInt32(Console.ReadLine());
}
Console.WriteLine("你的成绩如下：");
foreach (int score in scores) 
{
    Console.WriteLine("第" + (score + 1) + "门课成绩为：" + score);
}
```

但是当我们运行这段代码的时候，我们会发现报错了：

> foreach循环中不能修改数组元素的值

这是因为在foreach循环当中，我们需要使用到的是一个完整的数组，如果我这个数组被破坏了，那就会造成影响。这样做是非常不安全的，因此在C#中，我们不能在foreach循环当中修改数组元素的值。

所以我们这里就必须得使用到for循环了。

最后我们来总结一下：

- foreach循环可以直接读取数组中的元素
- 但是foreach只能用于遍历数组中的元素，不能修改数组元素的值

## 列表

对于我们上面的这个需求，看似解决了，但是我们并没有真正的方便的解决这个问题。

就是如果我们需要n个科，也就是不定长的数组。那么这个时候如果使用数组也是可以的，但是我们必须得先让用户自己输入一个值才可以进行创建，这样的代码如下：

```csharp
Console.WriteLine("请输入课程数量：");
int count = Convert.ToInt32(Console.ReadLine());
int[] scores = new int[count];
Console.WriteLine("请输入课程成绩：");
for (int i = 0; i < scores.Length; i++) 
{
    Console.Write("请输入第" + (i + 1) + "门课的成绩：");
    scores[i] = Convert.ToInt32(Console.ReadLine());
}

Console.WriteLine("你的成绩如下：");

for (int i = 0; i < scores.Length; i++) 
{
    Console.WriteLine("第" + (i + 1) + "门课成绩为：" + scores[i]);
}
```

但是如果连老师都不知道有多少科的话，那就完了。

那么这个时候，我们就可以使用C#当中的列表来处理这个问题了。

列表是一种动态数组，我们可以通过Add方法来添加元素，通过Remove方法来删除元素，通过Clear方法来清空列表，通过Count属性来获取列表的长度。

我们现在试一下:

```csharp
List<int> scores = new List<int>();
```

对于列表来说，我们需要这样写：

> List<类型> 变量名 = new List<类型>();

至于为什么不使用[]而是<>，我们后面会讲到，我先埋个坑。🐕

对于列表来说，想要添加元素，我们可以使用Add方法：

```csharp
List<int> scores = new List<int>();
while (true) {
    Console.WriteLine("请输入课程成绩：");
    string input = Console.ReadLine();
    if (input == "q" || input == "quit") {
        break;
    }
    scores.Add(Convert.ToInt32(input));
}
Console.WriteLine("你的成绩如下：");
for (int i = 0; i < scores.Count; i++) {
    Console.WriteLine("第" + (i + 1) + "门课成绩为：" + scores[i]);
}
```

我们这里使用了一个while循环，当用户输入q或者quit的时候，就会退出循环。

当然我们也可以进行初始化:

```csharp
List<int> scores = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
Console.WriteLine("你的成绩如下：");
for (int i = 0; i < scores.Count; i++) {
    Console.WriteLine("第" + (i + 1) + "门课成绩为：" + scores[i]);
}
```

这里我们使用了一个初始化列表，这个列表当中的元素是1到9，然后我们使用for循环来输出列表中的元素。

我们还可以进行删除和清空操作：

```csharp
List<int> scores = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
scores.Remove(1);
Console.WriteLine("现在有" + scores.Count + "门课");

scores.Clear();
Console.WriteLine("现在有" + scores.Count + "门课");
// 输出：现在有8门课
// 输出：现在有0门课
```

我们现在来总结一下列表和数组的区别：

- 数组的长度是固定的，而列表的长度是可变的
- 数组是静态的，而列表是动态的

但是在使用前两者的时候，各位有没有发现一个问题：

> 课程和成绩之间没有关联

是的，我们之前都是假定的索引为0的时候就是语文，索引为1的时候就是数学，索引为2的时候就是英语，但是这显然是不合理的，所以我们需要一个更好的解决方案。

就比如说我们刚才的这个不定长需求，我们现在迫切的需要同时存储课程和成绩。

我们现在有这样一种方案，就是使用两个列表来解决：

```csharp
List<string> courses = new List<string>();
List<int> scores = new List<int>();
while (true) {
    Console.WriteLine("请输入课程：");
    string input = Console.ReadLine();
    if (input == "q" || input == "quit") {
        break;
    }
    courses.Add(input);
    Console.WriteLine("请输入成绩：");
    input = Console.ReadLine();
    scores.Add(Convert.ToInt32(input));
}

Console.WriteLine("你的成绩如下：");
for (int i = 0; i < courses.Count; i++) {
    Console.WriteLine("第" + (i + 1) + "门课为：" + courses[i] + "，成绩为：" + scores[i]);
}
```

这里我们使用了两个列表，一个用来存储课程，一个用来存储成绩。

但是当我们需要遍历的时候，我们发现这样写还是有点麻烦的，所以我们可以使用字典来处理这个问题。

## 字典

抱着这样的思路，聪明的C#设计师们设计了**字典**来存储课程和成绩。

字典是一种键值对的集合，我们可以通过Key来获取Value，我们可以通过Add方法来添加键值对，通过Remove方法来删除键值对，通过Clear方法来清空字典，通过Count属性来获取字典的长度。

我们现在来试一下：

```csharp
Dictionary<string, int> scores = new Dictionary<string, int>();
// 格式为：Key, Value
while (true) {
    Console.WriteLine("请输入课程：");
    string input = Console.ReadLine();
    if (input == "q" || input == "quit") {
        break;
    }
    Console.WriteLine("请输入成绩：");
    int score = Convert.ToInt32(Console.ReadLine());
    scores.Add(input, score);
}
Console.WriteLine("你的成绩如下：");
foreach (KeyValuePair<string, int> item in scores) {
    Console.WriteLine(item.Key + "课的成绩为：" + item.Value);
}
```

在字典当中，我们可以直接使用Add这个东西来进行添加，在里头输入键(Key)和值(Value)，这样就可以进行添加了。

同样的，我们在进行更改的时候，我们可以直接使用到我们的Key来更改对应的Value。

```csharp
Dictionary<string, int> scores = new Dictionary<string, int>();
scores.Add("语文", 100);
scores.Add("数学", 90);
scores["语文"] = 80;
scores.Remove("数学"); // 使用Key来删除
```

但是如果我们查不到这个键对值的时候，就会报错。所以我们需要查询是否存在这个键对值：

```csharp
    if (scores.ContainsKey("语文")) {
        Console.WriteLine("语文成绩为：" + scores["语文"]);
        scores["语文"] = 80;
    }
```

当然，我们也可以使用TryGetValue方法来查询是否存在这个键对值，如果存在，就会返回true，如果不存在，就会返回false。

```csharp
int score;
if (scores.TryGetValue("语文", out score)) {
    Console.WriteLine("语文成绩为：" + score);
}
```

这里会输出两个东西，一个是bool类型的值，一个是int类型的值(out score)。

## 上期的作业答案：

* 请使用while语句，输入一个数字，然后输出这个数字的阶乘。
* 请使用for语句，输入一个数字，然后输出这个数字的阶乘。

1. 使用while语句：
```csharp

int value = int.Parse(Console.ReadLine());

while (value > 1) {
    value *= value - 1;
    value--;
}

Console.WriteLine(value);
```

2. 使用for语句：

```csharp
int value = int.Parse(Console.ReadLine());
for (int i = value; i > 1; i--) {
    value *= i - 1;
}
Console.WriteLine(value);
```

## 课后作业

1. 请使用列表实现斐波那契数列

## 结尾

本节内容到此结束，我们今天讲了数组，列表，字典这三种数据结构。

下节课我们返璞归真。
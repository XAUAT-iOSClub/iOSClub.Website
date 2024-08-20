# iOS Club和你一起学C#（7）结构体，枚举，类

>  在上一节我们学习了函数
>
> 这一节我们继续来学习

我们在第二节的时候学习了基础的变量，后面又学到了数组，列表，字典这些。

但是函数的确是把我们的思路给打开了：我们为什么不能自己写一个数据类型呢？就像我们可以自己写一个函数一样的。

但是咱们也不能按照自己的想法去做事对吧，不如来看看有什么需求，毕竟开发项目讲究的就是一个需求。

> 小王想看一下学生的各科成绩，还有学生的学号，性别，姓名，班级

嘶......这个该怎么写呢？

不如我们试试，用List写一个?

```csharp
List<string> nameList = new List<string>();
List<string> sexList = new List<string>();
List<string> classList = new List<string>();
List<int> idList = new List<int>();
List<List<int>> scoreList = new List<int[]>();

while (true){
    Console.WriteLine("请输入学生姓名：");
    string name = Console.ReadLine();
    if(name == "quit")break;
    nameList.Add(name);
    Console.WriteLine("请输入学生性别：");
    sexList.Add(Console.ReadLine());
    Console.WriteLine("请输入学生班级：");
    classList.Add(Console.ReadLine());
    Console.WriteLine("请输入学生学号：");
    idList.Add(int.Parse(Console.ReadLine()));
    Console.WriteLine("请输入学生成绩：");
    var score = Console.ReadLine().Split(",");
    var scoreInt = new List<int>();
    foreach (var item in score) {
        scoreInt.Add(int.Parse(item));
    }
    scoreList.Add(scoreInt.ToArray());
}

Console.WriteLine("姓名\t性别\t班级\t学号\t成绩");
for (int i = 0; i < nameList.Count; i++) {
    Console.WriteLine($"{nameList[i]}\t{sexList[i]}\t{classList[i]}\t{idList[i]}\t{string.Join(",",scoreList[i])}");
}

```

我们在这里使用了List来存储数据，但是问题来了，我们每次添加一个学生，都需要添加5个List，这样是不是有点麻烦了？

或者说，如果我这边写了一个业务，那我想把数据传给对方。别人一看：

> 哇塞，5个数列

那为什么不能咱们自己创建一个全新的数据类型，然后存我们想要存的东西呢？

## 结构体

没错，C#还真有这种方案。我们可以将我们需要的数据合在一起，然后对外就专门使用这个数据类型。

我们可以来试一下:

```csharp
struct Student{
    public string name;
    public string sex;
    public string className; // 这里不能使用class当名字，因为class是关键字
    public string id;
    public List<int> score;
}

List<Student> studentList = new List<Student>();
while (true){
    Console.WriteLine("请输入学生姓名：");
    string name = Console.ReadLine();
    if(name == "quit")break;
    Student student = new Student();
    student.name = name;
    Console.WriteLine("请输入学生性别：");
    student.sex = Console.ReadLine();
    Console.WriteLine("请输入学生班级：");
    student.className = Console.ReadLine();
    Console.WriteLine("请输入学生学号：");
    student.id = Console.ReadLine();
    Console.WriteLine("请输入学生成绩：");
    var score = Console.ReadLine().Split(",");
    student.score = new List<int>();
    foreach (var item in score) {
        student.score.Add(int.Parse(item));
    }
    studentList.Add(student);
}

Console.WriteLine("姓名\t性别\t班级\t学号\t成绩");
foreach (var student in studentList) {
    Console.WriteLine($"{student.name}\t{student.sex}\t{student.className}\t{student.id}\t{string.Join(",",student.score)}");
}

```

我们在这里定义了一个结构体，里面包含了我们需要的所有数据，但是这里请一定要记住，我们在定义的时候不要忘记加入public。

public代表着这个变量的访问权限。一般来说我们会碰到两种：

1. public：表示这个变量是公开的，也就是可以在外面进行变量访问。

2. private：表示这个变量是私有的，也就是只能在类里头进行变量访问。如果我们不写访问权限，默认是private的。

我们来试一下：

```csharp
struct Student{
    public string name;
    public string sex;
    public string className;
    string id;
    public List<int> score;
}

Student student = new Student();
student.name = "小王";
student.sex = "男";
student.className = "1班";
student.id = "123456"; // 这里会报错，因为id是私有的
student.score = new List<int>();
Console.WriteLine(student.name);
Console.WriteLine(student.id); // 这里会报错，因为id是私有的，外界无法访问
Console.WriteLine(student.sex);
Console.WriteLine(student.className);
Console.WriteLine(student.score);
```

这里我们会发现，id是直接标红的，这也就意味着我们无法在外界访问这个变量。

那现在就有一个问题了，我们现在有一个需求，我们需要给定一个变量，算出来所有数据，但是我们要重复的调用。

按照我们以前的方法，我们这里是肯定要使用函数的。

这里给个需求吧：我们的学号是一个十位的数字，其格式如下：

> 1，2是年份
>
> 3，4是学院代号
>
> 5，6是专业代号
>
> 7，8是班级代号
>
> 9，10是班级内的序号

那我们用函数来试一下：

```csharp
string[] Get(string id){
    string[] result = new string[5];
    result[0] = id.Substring(0,2);
    result[1] = id.Substring(2,2);
    result[2] = id.Substring(4,2);
    result[3] = id.Substring(6,2);
    result[4] = id.Substring(8,2);
    return result;
}
string[] id = Get("2020123456");
foreach (var item in id) {
    Console.WriteLine(item);
}
```

但是我们如果我们是一直在使用一个数据
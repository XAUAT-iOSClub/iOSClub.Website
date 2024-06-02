using iOSClub.Data;
using iOSClub.Data.DataModels;

var proj = new ProjectModel() { Title = "1", Description = "1", DepartmentName = "1" };
Console.WriteLine(proj.ToString());
Console.WriteLine(DataTool.StringToHash(proj.ToString()));
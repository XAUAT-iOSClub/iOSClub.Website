using iOSClub.Data.DataModels;

namespace iOSClub.WebSite.Models;

[Serializable]
public class AllDataModel
{
    public List<StudentModel> Students { get; init; } = [];
    public List<StaffModel> Presidents { get; init; } = [];
    public List<TaskModel> Tasks { get; init; } = [];
    public List<ProjectModel> Projects { get; init; } = [];
    public List<ResourceModel> Resources { get; init; } = [];
    public List<DepartmentModel> Departments { get; init; } = [];
    public List<TodoModel> Todos { get; init; } = [];
    public List<ArticleModel> Articles { get; init; } = [];
}
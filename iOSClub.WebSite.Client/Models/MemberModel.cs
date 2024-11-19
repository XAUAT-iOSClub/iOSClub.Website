using System.Text;
using iOSClub.Data.DataModels;

namespace iOSClub.WebSite.Client.Models;

public class MemberModel : StudentModel
{
    /// <summary>
    /// Founder : 创始人
    /// President : 社长,副社长,秘书长
    /// Minister : 部长/副部长
    /// Department : 部员成员
    /// Member : 普通成员
    /// </summary>
    public string Identity { get; set; } = "Member";

    public static readonly Dictionary<string, string> IdentityDictionary = new()
    {
        { "Founder", "创始人" },
        { "President", "社长/团支书" },
        { "Department", "部员" },
        { "Minister", "部长/副部长" },
        { "Member", "普通成员" }
    };

    public static TChild AutoCopy<TParent, TChild>(TParent parent) where TChild : TParent, new()
    {
        var child = new TChild();
        var ParentType = typeof(TParent);
        var Properties = ParentType.GetProperties();
        foreach (var property in Properties)
        {
            //循环遍历属性
            if (property is { CanRead: true, CanWrite: true })
            {
                //进行属性拷贝
                property.SetValue(child, property.GetValue(parent, null), null);
            }
        }

        return child;
    }
    
    public bool IsAdmin()
    {
        return Identity is "Founder" or "President" or "Minister";
    }

    public static MemberModel CopyFrom(StudentModel model)
    {
        return AutoCopy<StudentModel, MemberModel>(model);
    }

    public static string GetCsv(List<MemberModel> models)
    {
        var builder = new StringBuilder("姓名,学号,性别,学院,政治面貌,专业班级,电话号码,身份");
        foreach (var model in models)
            builder.Append("\n" + model);

        return builder.ToString();
    }

    public override string ToString()
    {
        return $"{UserName},{UserId},{Gender},{Academy},{PoliticalLandscape},{ClassName},{PhoneNum},{Identity}";
    }
}
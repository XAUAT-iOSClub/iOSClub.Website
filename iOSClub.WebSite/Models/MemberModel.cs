using iOSClub.Data.DataModels;

namespace iOSClub.WebSite.Models;

public class MemberModel : StudentModel
{
    /// <summary>
    /// Founder : 创始人
    /// President : 社长,副社长,秘书长
    /// TechnologyMinister : 科技部部长/副部长
    /// PracticalMinister : 实践交流部部长/副部长
    /// PracticalMember : 实践交流部成员
    /// NewMediaMinister : 新媒体部部长/副部长
    /// NewMediaMember : 新媒体部成员
    /// TechnologyMember : 科技部成员
    /// Member : 普通成员
    /// </summary>
    public string Identity { get; set; } = "Member";

    public static readonly Dictionary<string, string> IdentityDictionary = new()
    {
        { "Founder", "创始人" },
        { "President", "社长,副社长,秘书长" },
        { "TechnologyMinister", "科技部部长/副部长" },
        { "PracticalMinister", "实践交流部部长/副部长" },
        { "PracticalMember", "实践交流部成员" },
        { "NewMediaMinister", "新媒体部部长/副部长" },
        { "NewMediaMember", "新媒体部成员" },
        { "TechnologyMember", "科技部成员" },
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
}
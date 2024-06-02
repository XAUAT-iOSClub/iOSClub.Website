using iOSClub.Data.DataModels;

namespace iOSClub.WebSite.Models;

[Serializable]
public class OldSignModel
{
    public string UserName { get; set; } = "";
    public string Id { get; set; } = "";
    public string Academy { get; set; } = "";
    public string PoliticalLandscape { get; set; } = "";
    public string Gender { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string PhoneNum { get; set; } = "";

    public StudentModel To()
        => new()
        {
            UserName = UserName,
            Academy = Academy,
            ClassName = ClassName,
            Gender = Gender,
            PhoneNum = PhoneNum,
            PoliticalLandscape = PoliticalLandscape,
            UserId = Id
        };
}
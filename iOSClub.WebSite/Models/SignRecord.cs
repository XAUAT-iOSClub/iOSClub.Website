﻿using AntDesign;

namespace iOSClub.WebSite.Models;

public static class SignRecord
{
    public static readonly OrgSignRecord ios = new(
        "mqqapi://card/show_pslcard?src_type=internal&version=1&uin=952954710&card_type=group&source=external",
        "https://qm.qq.com/cgi-bin/qm/qr?authKey=MUNgIj%2B1gnkiI175qAQla6EcR44Fa0APCv%2FLo1a7YIlYgpeg76Q%2BGYMoedb8gGHU&k=HvhhArSc7tzuySOhXsnmZ6RgLcWkzXgu&noverify=0");

    public static string GetIcon(this string url)
        => $"https://{url.Replace("https://", "").Replace("http://", "").Split('/').First()}/favicon.ico";

    #region Table

    public static readonly string[] Academies =
    [
        "信息与控制工程学院",
        "理学院",
        "机电工程学院",
        "管理学院",
        "土木工程学院",
        "环境与市政工程学院",
        "建筑设备科学与工程学院",
        "材料科学与工程学院",
        "冶金工程学院",
        "资源工程学院",
        "城市发展与现代交通学院",
        "文学院",
        "艺术学院",
        "建筑学院",
        "马克思主义学院",
        "公共管理学院",
        "化学与化工学院",
        "体育学院",
        "安德学院",
        "未来技术学院",
        "国际教育学院"
    ];

    public static readonly string[] PoliticalLandscapes =
    [
        "群众",
        "共青团员",
        "中共党员",
        "中共预备党员"
    ];

    public static readonly string[] Genders = ["男", "女"];

    public static FormValidationRule[] UserNameRules =>
    [
        new()
            // ReSharper disable once StringLiteralTypo
            { Pattern = @"^[\u4E00-\u9FA5A-Za-z\s]+(·[\u4E00-\u9FA5A-Za-z]+)*$", Message = "姓名出错!", Required = true }
    ];

    public static FormValidationRule[] IdRules =>
    [
        new() { Len = 10, Message = "学号出错!", Pattern = "(19|20|21|22|23|24)([0-9]{8})", Required = true }
    ];

    public static FormValidationRule[] PhoneNumRules =>
    [
        new() { Len = 11, Pattern = "^1\\d{10}$", Message = "手机号不正确!", Required = true }
    ];

    public static FormValidationRule[] ClassNameRules =>
    [
        new() { Pattern = @"[\u4e00-\u9fa5|(|)|（|）]+[0-9]{4}(.*)", Message = "班级姓名出错!", Required = true }
    ];

    public static FormValidationRule[] GenderRules =>
    [
        new() { Required = true, Message = "性别没填!" }
    ];

    public static FormValidationRule[] AcademyRules =>
    [
        new() { Required = true, Message = "学院没填!" }
    ];

    public static FormValidationRule[] PoliticalLandscapeRules =>
    [
        new() { Required = true, Message = "政治面貌没填!" }
    ];

    #endregion

    #region Project

    public static FormValidationRule[] ProjectNameRules =>
    [
        new() { Len = 20, Message = "项目名称太长了", Required = true }
    ];

    public static FormValidationRule[] ProjectDescriptionRules =>
    [
        new() { Len = 512, Message = "项目描述太长了", Required = true }
    ];

    public static Dictionary<string, string> DepartmentDictionary => new()
    {
        ["All"] = "所有",
        ["Technology"] = "科技部",
        ["NewMedia"] = "新媒体部",
        ["Practical"] = "交流实践部",
        [""] = "其他",
        ["Other"] = "其他"
    };

    #endregion

    #region Resourse

    public static FormValidationRule[] ResourceNameRules =>
    [
        new() { Len = 20, Message = "项目名称太长了", Required = true }
    ];

    public static FormValidationRule[] ResourceDescriptionRules =>
    [
        new() { Len = 512, Message = "项目描述太长了", Required = true }
    ];

    #endregion

    #region Task

    public static FormValidationRule[] TaskTitleRules =>
    [
        new() { Len = 20, Message = "项目名称太长了", Required = true }
    ];

    public static FormValidationRule[] TaskDescriptionRules =>
    [
        new() { Len = 200, Message = "项目描述太长了", Required = true }
    ];

    #endregion
}

public record OrgSignRecord(string qqApi, string https);
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iOSClub.Data.DataModels;

public class ArticleModel
{
    /// <summary>
    /// 路径
    /// </summary>
    [Key]
    [Column(TypeName = "varchar(128)")]
    public string Path { get; set; } = "";

    /// <summary>
    /// 标题
    /// </summary>
    [Column(TypeName = "varchar(32)")]
    public string Title { get; set; } = "";

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = "";

    /// <summary>
    /// 最后修改时间
    /// </summary>
    [Column(TypeName = "DATE")]
    public DateTime LastWriteTime { get; set; } = DateTime.Now;
}
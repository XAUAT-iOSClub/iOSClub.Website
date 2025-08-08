# iOSClub.WebSite

这是一个使用 ASP.NET Core 和 Entity Framework Core 构建的网站项目，旨在为 iOS Club 提供全面的管理与展示功能。

## 📌 功能概览

- **用户管理**：支持注册、登录及权限验证（JWT），并提供不同角色（如创始人、主席、部长）的权限控制。
- **部门管理**：提供部门信息的展示与编辑功能，支持部门成员与项目的管理。
- **项目与任务管理**：支持项目创建、更新与删除，以及任务的加入与退出。
- **资源与待办事项**：提供资源列表与待办事项管理。
- **数据可视化**：支持学生信息的多维度统计展示，包括学院分布、年级、性别等。
- **文档管理**：支持文章和 RSS 内容的展示与管理。

## 🛠️ 技术栈

- **后端**：ASP.NET Core (.NET 9.0)
- **前端**：Blazor
- **数据库**：Entity Framework Core (使用 SQLite 或其他支持的数据库)
- **身份验证**：JWT 与自定义权限控制
- **部署**：Docker 镜像构建与部署

## 📦 项目依赖

- `iOSClub.Data`：数据模型与数据库上下文。
- `iOSContext`：数据库上下文，管理所有数据实体。
- `JwtHelper`：用于生成和验证用户 token。

## 📄 主要模型

- `StudentModel`：学生信息模型，包含姓名、学号、学院、政治面貌、性别、班级、手机号等。
- `StaffModel`：工作人员模型，关联部门并管理项目与任务。
- `DepartmentModel`：部门模型，包含部门名称、Key、描述及关联的工作人员与项目。
- `ProjectModel`：项目模型，包含标题、描述、时间范围及关联的部门与任务。
- `TaskModel` 与 `TodoModel`：任务模型，记录任务标题、描述、状态及负责人。
- `ResourceModel`：资源模型，用于管理学习资源或工具。
- `ArticleModel`：文章模型，用于存储和展示内容。

## 🚀 快速启动

### 1. 构建 Docker 镜像

```bash
docker build -t iosclub/web-site .
```

### 2. 运行容器

```bash
docker run -d -p 8080:8080 -p 8081:8081 --name iosclub-web-site iosclub/web-site
```

### 3. 本地开发启动

```bash
dotnet run --project iOSClub.WebSite
```

## 📚 API 文档

- `/api/Member/Login`：用户登录接口，返回 JWT token。
- `/api/Member/SignUp`：学生注册接口。
- `/api/Member/GetTodos`：获取当前用户的待办事项。
- `/api/Project/GetAllData`：获取所有项目数据。
- `/api/Project/CreateOrUpdateProject`：创建或更新项目。
- `/api/Department/{name}`：获取部门信息。
- `/api/Department/UpdateDepartment`：更新部门信息。

## 🧩 主要组件

- **Markdown 渲染**：支持 Markdown 文档的渲染展示。
- **数据表格**：支持学生信息的表格展示与筛选。
- **图表展示**：使用饼图、柱状图等展示学院、年级、政治面貌等数据分布。
- **文件导入/导出**：支持数据以 CSV 或 JSON 格式下载和上传。

## 📝 项目结构

- `Controllers/`：包含 API 控制器，如 `MemberController.cs`、`ProjectController.cs`。
- `Components/`：Blazor 组件，如布局、首页、展示页等。
- `Models/`：业务模型，如 `MemberModel.cs`、`SignRecord.cs`。
- `AppComponents/`：应用内的公共组件，如导航菜单、返回按钮等。
- `CentreToolPages/`：管理工具页面，包含部门、项目、成员等管理页面。

## 📜 许可证

本项目使用 MIT 许可证。详情请查看 [LICENSE](LICENSE) 文件。

## 🤝 贡献

欢迎提交 PR 与 Issue。请遵循项目代码规范并提供清晰的变更说明。

## 📬 联系

如有问题或建议，请在 Gitee 上提交 issue 或联系项目维护者。
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class createDefalutData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

GO
SET IDENTITY_INSERT [dbo].[Actions] ON 

INSERT [dbo].[Actions] ([Id], [NameAr], [NameEn], [Code], [IsMaster], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (1, N'عرض', N'View', N'view', NULL, NULL, CAST(N'2022-08-23T15:46:26.2833333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Actions] ([Id], [NameAr], [NameEn], [Code], [IsMaster], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (2, N'إضافة', N'Add', N'add', NULL, NULL, CAST(N'2022-08-23T15:46:26.2866667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Actions] ([Id], [NameAr], [NameEn], [Code], [IsMaster], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (3, N'تعديل', N'Update', N'update', NULL, NULL, CAST(N'2022-08-23T15:46:26.2866667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Actions] ([Id], [NameAr], [NameEn], [Code], [IsMaster], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (4, N'حذف', N'Delete', N'delete', NULL, NULL, CAST(N'2022-08-23T15:46:26.2866667' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[Actions] OFF
GO
SET IDENTITY_INSERT [dbo].[Pages] ON 

INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (1, N'إدارة المستخدمين', N'Users Management', N'User', NULL, 1, N'/User', 1, 1, NULL, NULL, 0, NULL, CAST(N'2021-12-28T17:24:43.5400000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (2, N'إدارة مجموعات المستخدمين', N'Users Groups Management', N'Role', NULL, 2, N'/Role', 1, 1, NULL, NULL, 0, NULL, CAST(N'2021-12-28T17:24:43.5400000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (3, N'حقيبة المشاريع', N'Projects Portfolio', N'ProjectsPortfolio', NULL, 1, N'/ProjectsPortfolio', 3, 1, NULL, NULL, 0, NULL, CAST(N'2021-12-28T17:24:43.5400000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (4, N'مشاريعي', N'My Projects', N'Project', NULL, 3, N'/Project', 3, 1, NULL, NULL, 0, NULL, CAST(N'2021-12-28T17:24:43.5400000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (5, N'طلباتي', N'My Requests', N'ProjectRequests', NULL, 4, N'/ProjectRequests', 3, 1, NULL, NULL, 0, NULL, CAST(N'2021-12-28T17:24:43.5400000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (6, N'العملاء', N'Clients', N'Client', N' ', 3, N'/Client', 2, 1, NULL, NULL, 1, NULL, CAST(N'2022-08-24T16:10:57.3866667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (7, N'إدارة الرخص', N'License Management', N'LicenseManagement', NULL, 3, N'/LicenseManagement', 2, 1, NULL, NULL, 0, NULL, CAST(N'2021-12-28T17:24:43.5400000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (8, N'جميع المشاريع', N'All Project', N'AllProject', N' ', 3, N'/AllProject', 3, 1, NULL, NULL, 1, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (9, N'جميع طلبات المشاريع', N'All Project Requests', N'AllProjectRequests', N' ', 3, N'/AllProjectRequests', 3, 1, NULL, NULL, 1, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (10, N'أيام العطل', N'Holidays Settings', N'HolidaysSettings', N' ', 3, N'/HolidaysSettings', 2, 1, NULL, NULL, 1, NULL, CAST(N'2022-08-28T12:53:43.1133333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (11, N'الأجازات الرسمية', N'Official Vacations', N'OfficialVacation', N' ', 4, N'/OfficialVacation', 2, 1, NULL, NULL, 1, NULL, CAST(N'2022-08-28T12:53:43.1133333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (12, N'الحقيبة', N'Portfolio', N'Portfolio', N' ', 6, N'/Portfolio', 3, 1, NULL, NULL, 1, NULL, CAST(N'2022-08-28T13:18:20.2933333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (13, N'قائمة الخطط', N'Plans List', N'PlansList', N'aside-icon--plans', 2, N'/PlansList', 4, 1, NULL, NULL, 1, NULL, CAST(N'2022-09-27T17:56:40.9100000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (14, N'ملخص', N'Overview', N'Overview', N'aside-icon--overview', 1, N'/Overview', 4, 1, NULL, NULL, 1, NULL, CAST(N'2022-09-27T17:56:40.9100000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[Pages] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [PageCategoryId], [HasDataPermissions], [HasWorkflow], [IsWFEnabled], [IsAdmin], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (15, N'التقارير', N'Reports', N'Reports', N'aside-icon--reports', 3, N'/Reports', 4, 1, NULL, NULL, 1, NULL, CAST(N'2022-09-27T17:56:40.9100000' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[Pages] OFF
GO
SET IDENTITY_INSERT [dbo].[PageActions] ON 

INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (1, 1, 1, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (2, 1, 2, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (3, 1, 3, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (4, 1, 4, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (5, 2, 1, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (6, 2, 2, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (7, 2, 3, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (8, 2, 4, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (9, 3, 1, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (10, 3, 3, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (11, 4, 1, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (12, 4, 2, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (13, 4, 3, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (14, 4, 4, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (15, 5, 1, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (16, 5, 2, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (17, 5, 3, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (18, 5, 4, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (19, 6, 1, NULL, CAST(N'2022-08-24T16:10:57.3900000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (20, 6, 2, NULL, CAST(N'2022-08-24T16:10:57.3900000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (21, 6, 3, NULL, CAST(N'2022-08-24T16:10:57.3900000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (22, 6, 4, NULL, CAST(N'2022-08-24T16:10:57.3900000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (23, 7, 1, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (24, 7, 2, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (25, 7, 3, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (26, 7, 4, NULL, CAST(N'2021-11-28T17:57:08.3433333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (27, 8, 1, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (28, 8, 2, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (29, 8, 3, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (30, 8, 4, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (31, 9, 1, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (32, 9, 2, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (33, 9, 3, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (34, 9, 4, NULL, CAST(N'2022-08-25T14:33:00.9966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (35, 10, 1, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (36, 10, 2, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (37, 10, 3, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (38, 10, 4, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (39, 11, 1, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (40, 11, 2, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (41, 11, 3, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (42, 11, 4, NULL, CAST(N'2022-08-28T12:53:43.1166667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (43, 12, 1, NULL, CAST(N'2022-08-28T13:18:20.2966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (44, 12, 2, NULL, CAST(N'2022-08-28T13:18:20.2966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (45, 12, 3, NULL, CAST(N'2022-08-28T13:18:20.2966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (46, 12, 4, NULL, CAST(N'2022-08-28T13:18:20.2966667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (47, 13, 1, NULL, CAST(N'2022-09-27T17:56:40.9133333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (48, 13, 2, NULL, CAST(N'2022-09-27T17:56:40.9133333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (49, 13, 3, NULL, CAST(N'2022-09-27T17:56:40.9133333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (50, 13, 4, NULL, CAST(N'2022-09-27T17:56:40.9133333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (51, 14, 1, NULL, CAST(N'2022-09-27T17:56:40.9133333' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageActions] ([Id], [PageId], [ActionId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (52, 15, 2, NULL, CAST(N'2022-09-27T17:56:40.9133333' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[PageActions] OFF
GO
SET IDENTITY_INSERT [dbo].[AppUsers] ON
INSERT [dbo].[AppUsers] ([Id], [CreatedBy],[CreatedDate],[ModifiedBy], [ModifiedDate], [IsActive],[IsDeleted], [UserName],[FullName],[VerificationCode],[IsEnabled], [IsConfirmed], [Token], [IsSuperAdmin],[resetPasswordCode], [resetPasswordCodeTimeOut],[UserType],[UserCategory],[UserId], [NormalizedUserName],[Email], [NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp], [ConcurrencyStamp],[PhoneNumber],  [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd],  [LockoutEnabled],[AccessFailedCount])
VALUES (1,         0, 		CAST(N'2022-03-29T17:54:24.8900000' AS DateTime2), 		11,		CAST(N'2023-01-23T09:17:12.3527961' AS DateTime2), 		1,		0, 		N'admin', 		N'admin', 		N'True', 		1, 		1, 		N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwiVXNlcklkIjoiMTEiLCJVc2VyTmFtZSI6ImFkbWluIiwiVG9rZW5UeXBlIjoiMSIsIm5iZiI6MTY3NDQ2NTQzMiwiZXhwIjoxNjc0NDY3MjMyLCJpYXQiOjE2NzQ0NjU0MzJ9.wc6485jjPlJVpZJKqBru7b13kZZFUmuoce1pfcCVdFo', 		1, 		N'55',		NULL, 		2,		1, 		N'1',			N'msayed@futureface.sa', 		N'admin@FUTUREFACE.SA', 		N'True', 		1, 		N'AQAAAAEAACcQAAAAEPEMTAFKPN0eHJEDWhC+9Cd+z7bFoBqTuLJwZN/hD3Dg4V1McSEL2wLdXoEPUAIXzg==',		N'eb1f63b3-3eac-49d0-be8b-d5edca5ca0e6', 		N'1', 		N'True', 		0, 		0, 		NULL,		0, 		0)
SET IDENTITY_INSERT [dbo].[AppUsers] OFF

SET IDENTITY_INSERT [dbo].[APIPermissions] ON 

INSERT [dbo].[APIPermissions] ([Id], [ControllerName], [ActionName], [PageActionId], [IsPublic], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (1, N'*', N'*', 1, 1, 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[APIPermissions] OFF
GO
SET IDENTITY_INSERT [dbo].[AttachmentExtension] ON 

INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (1, N'PDF', N'25 50 44 46', N'application/pdf', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (2, N'DOCX', N'50 4B 03 04', N'application/vnd.openxmlformats-officedocument.wordprocessingml.document', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (3, N'XLSX', N'50 4B 03 04', N'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (4, N'GIF', N'47 49 46 38', N'image/gif', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (5, N'JPG', N'FF D8 FF E0', N'image/jpeg', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (6, N'JPG', N'FF D8 FF E1', N'image/jpeg', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (7, N'PNG', N'89 50 4E 47', N'image/png', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (8, N'MP4', N'00 00 00 20', N'video/mp4', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (9, N'AVI', N'52 49 46 46', N'video/avi', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (10, N'MOV', N'00 00 00 14', N'video/quicktime', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (11, N'WMV', N'30 26 B2 75', N'video/x-ms-wmv', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (12, N'JPEG', N'FF D8 FF E0', N'image/jpeg', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (13, N'MP4', N'00 00 00 18', N'video/mp4', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (14, N'PDF', N'38 42 50 53', N'application/pdf', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (15, N'MP4', N'00 00 00 14', N'video/mp4', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[AttachmentExtension] OFF
GO
SET IDENTITY_INSERT [dbo].[PageCategories] ON 

INSERT [dbo].[PageCategories] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [ShowType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (1, N'إدراة النظام', N'System Management', N'SystemManagement', N'aside-icon--users', 1, N' ', N' ', NULL, CAST(N'2022-08-23T15:46:26.2866667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageCategories] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [ShowType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (2, N'إدارة البيانات الأساسية', N'Data and Informaton Management', N'DataandInformatonManagement', N'aside-icon--setting', 2, N' ', N' ', NULL, CAST(N'2022-08-23T15:46:26.2866667' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageCategories] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [ShowType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (3, N'المشاريع', N'Projects', N'Portfolio', N'aside-icon--home', 3, N' ', N' ', NULL, CAST(N'2022-08-24T10:58:30.8000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[PageCategories] ([Id], [NameAr], [NameEn], [Code], [Icon], [Index], [Path], [ShowType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (4, N'إدارة الإستراتيجيات', N'Strategy Management', N'StrategyManagement', N'aside-icon--strategy', 4, N'', NULL, NULL, CAST(N'2022-09-27T17:56:40.9066667' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[PageCategories] OFF
GO


/****** Object:  StoredProcedure [dbo].[CheckApiPermission]    Script Date: 2/6/2023 10:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or ALTER      PROCEDURE [dbo].[CheckApiPermission]
                @actionname nvarchar(max),
                @controllername nvarchar(max),
                @userId int = null
                as
                declare @ReturnValue AS INT
                begin

                select @ReturnValue = count(APIPermissions.Id) from APIPermissions
                left join PageActions on APIPermissions.PageActionId = PageActions.Id
                left
                                                               join Permissions on PageActions.Id = Permissions.PageActionId
                                                          left
                                                               join UserRoles on UserRoles.RoleId = Permissions.RoleId
                                                          --left join AppUsers on UserRoles.UserId = AppUsers.Id
                                                        --left join Roles
                where
                not exists(select top 1 id from APIPermissions where ControllerName = @controllername and actionName = @actionname)--الcontroller وال action  مش مسجلين
                or(ControllerName = @controllername and actionName = @actionname and ispublic = 1)
                or(ControllerName = @controllername and(actionName = @actionname or actionName = '*') and UserRoles.UserId = @userId and  UserRoles.UserId is not null and Permissions.Id is not null)
                or exists(select * from AppUsers where AppUsers.Id = @userId and appusers.issuperadmin = 1)
                or EXISTS(select top 1 * from roles join UserRoles on roles.id = UserRoles.RoleId and UserRoles.UserId = @userId where NormalizedName = 'admin')
                print  @ReturnValue
                return @ReturnValue
                end
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAuthorizeActions]    Script Date: 2/6/2023 10:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or ALTER           PROCEDURE[dbo].[sp_GetAuthorizeActions] 
                @userId int,
                @pagecode nvarchar(max)
                AS
                BEGIN
                select distinct Actions.* from
                Pages join PageActions on Pages.Id = PageActions.PageId
                join Actions on Actions.Id = PageActions.ActionId and isnull(Actions.IsMaster,0)=0
                left join Permissions on Permissions.PageActionId = PageActions.Id
                left join Roles as Roles1 on Roles1.Id = Permissions.RoleId
                left join UserRoles as UserRoles1 on Roles1.Id = UserRoles1.RoleId
                left join appusers as appusers1 on appusers1.Id = UserRoles1.UserId

                where Pages.Code = @pagecode and ((appusers1.Id = @userId and Permissions.Id is not null)or exists(select * from AppUsers where AppUsers.Id = @userId and appusers.issuperadmin = 1) or EXISTS(select top 1 * from roles join UserRoles on roles.id = UserRoles.RoleId and UserRoles.UserId = @userId where NormalizedName = 'admin'))
               

				union 
				 select distinct Actions.* from
                Pages join PageActions on Pages.Id = PageActions.PageId
                join Actions on Actions.Id = PageActions.ActionId
				join Permissions on Permissions.PageActionId = PageActions.Id			
				where Pages.Code = @pagecode             

                END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAuthorizePages]    Script Date: 2/6/2023 10:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                create or ALTER             PROCEDURE[dbo].[sp_GetAuthorizePages] --[sp_GetAuthorizePages] 11
                @userId int
                AS
                BEGIN
                select distinct Pages.* from Pages
                join PageActions on Pages.Id = PageActions.PageId
                left join Permissions on Permissions.PageActionId = PageActions.Id
                left join Roles on Roles.Id = Permissions.RoleId
				left join UserRoles on Roles.Id = UserRoles.RoleId
                left join AppUsers on AppUsers.Id = UserRoles.UserId
                where ((AppUsers.Id = @userId and Permissions.Id is not null) or exists 
				(select * from AppUsers where AppUsers.Id=@userId and appusers.issuperadmin=1) or EXISTS 
				(select top 1 * from roles join UserRoles on roles.id=UserRoles.RoleId and UserRoles.UserId=@userId where NormalizedName='admin') )and
				Pages.isdeleted = 0 and pages.isactive = 1

                --left join UserRoles on Roles.Id = UserRoles.RoleId
                --left join AppUsers on AppUsers.UserType & cast(Roles.NormalizedName as int) >0
                --where ((AppUsers.Id = @userId and Permissions.Id is not null) or exists (select * from AppUsers where AppUsers.Id=@userId and (appusers.IsSuperAdmin = 1  or appusers.UserType = 4 /*UserType : SystemAdmin*/)) or EXISTS (select top 1 * from roles join UserRoles on roles.id=UserRoles.RoleId and UserRoles.UserId=@userId where NormalizedName='admin') )and Pages.isdeleted = 0 and pages.isactive = 1
                --order by pages.[index]		
				
				union 
				 select distinct Pages.* from Pages
                join PageActions on Pages.Id = PageActions.PageId
				join Permissions on Permissions.PageActionId = PageActions.Id
				order by pages.[index]

                END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAuthorizePagesWithAction]    Script Date: 2/6/2023 10:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or ALTER           PROCEDURE[dbo].[sp_GetAuthorizePagesWithAction] 
                @userId int
                AS
                BEGIN
                select pages.[index],pages.PageCategoryId,PageActions.Id as PageActionId, CONCAT(Actions.NameEn, ' ', Pages.NameEn) AS NameEn , CONCAT(Actions.NameAr, ' ', Pages.NameAr) AS NameAr from Pages
                join PageActions on Pages.Id = PageActions.PageId
                join Actions on Actions.Id = PageActions.ActionId
                left join Permissions on Permissions.PageActionId = PageActions.Id
                left join Roles on Roles.Id = Permissions.RoleId
				left join UserRoles on Roles.Id = UserRoles.RoleId
                left join AppUsers on AppUsers.Id = UserRoles.UserId
                where ((AppUsers.Id = @userId and Permissions.Id is not null) or exists 
				(select * from AppUsers where AppUsers.Id=@userId and appusers.issuperadmin=1) or EXISTS 
				(select top 1 * from roles join UserRoles on roles.id=UserRoles.RoleId and UserRoles.UserId=@userId where NormalizedName='admin') )and
				Pages.isdeleted = 0 and pages.isactive = 1 and ActionId != 1

                --left join UserRoles on Roles.Id = UserRoles.RoleId
                --left join AppUsers on AppUsers.UserType & cast(Roles.NormalizedName as int) >0
                --where ((AppUsers.Id = @userId and Permissions.Id is not null) or exists (select * from AppUsers where AppUsers.Id=@userId and (appusers.IsSuperAdmin = 1  or appusers.UserType = 4 /*UserType : SystemAdmin*/)) or EXISTS (select top 1 * from roles join UserRoles on roles.id=UserRoles.RoleId and UserRoles.UserId=@userId where NormalizedName='admin') )and Pages.isdeleted = 0 and pages.isactive = 1 and ActionId != 1
                --order by pages.[index]				
				union 
                select pages.[index],pages.PageCategoryId,PageActions.Id as PageActionId, CONCAT(Actions.NameEn, ' ', Pages.NameEn) AS NameEn , CONCAT(Actions.NameAr, ' ', Pages.NameAr) AS NameAr from Pages
                join PageActions on Pages.Id = PageActions.PageId
                join Actions on Actions.Id = PageActions.ActionId
				join Permissions on Permissions.PageActionId = PageActions.Id
				 
				where ActionId != 1 

				order by pages.[index]

                END                
            
            
GO


           ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

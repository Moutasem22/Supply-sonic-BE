using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class seedSysSeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
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
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (15, N'doc', N'D0 CF 11 E0', N'application/doc', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (16, N'xls', N'D0 CF 11 E0', N'application/xls', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[AttachmentExtension] ([Id], [Extension], [MagicNo], [ExtFileType], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (17, N'mpp', N'D0 CF 11 E0', N'application/mpp', NULL, CAST(N'2022-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[AttachmentExtension] OFF


GO
SET IDENTITY_INSERT [dbo].[SysSettings] ON 

INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (1, N'AdminURL', N'http://localhost:8081', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (2, N'AccessTokenTimeout', N'30', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (3, N'OTPTimeOut', N'2', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (4, N'AdminAccessTokenTimeout', N'30', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (5, N'ExternalAccessTokenTimeout', N'30', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (6, N'RefreshTokenTimeout', N'120', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (7, N'LongOTPExpire', N'5', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (8, N'PageCount', N'5', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (9, N'ForgetPasswordTokenTimeout', N'60', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (10, N'NewsLetterEmailSubject', N'Impactor Newsletter', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (11, N'NewUserEmailSubject', N'Impactor New User', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (12, N'ForgetPasswordEmailSubject', N'Impactor Reset Password', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (13, N'AttachmentMaxFileName', N'100', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (14, N'AttachmentMaxFileSize', N'10', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
INSERT [dbo].[SysSettings] ([Id], [SysKey], [SysValue], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsActive], [IsDeleted]) VALUES (15, N'SetPasswordTokenTimeout', N'43200', 0, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[SysSettings] OFF
GO


             ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

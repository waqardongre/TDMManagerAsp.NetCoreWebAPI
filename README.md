## 3D Models Manager– Full Stack (Angular, ASP.NET Web API, Dapper, SQL) CRUD Web Application - Web Development - October 2022.

### This is a Full stack Web application which can be used to upload (create), view, update and delete 3D Models (multiple files at a time, only with .glb extension for now).
### Here you can find some .glb 3d models to test: https://drive.google.com/drive/folders/1EHvxYOY2-KPupqPvGLPmR9yKjgbHnTbN
### Both Frontend and Backend developed using best practices. Code is clean and understandable.
### Beautiful and responsive UI/UX in pure CSS3.
### This application is in development using these technologies: Angular 14, ASP.Net Core 6 Web API and SQL.
### Used ThreeJS to read and render 3D Models.
### Used Twillio SendGrid for email confirmations to register/sign up user.
### Used Visual Studio Code, MS SSMS 18 IDEs.

### Functionalities: CRUD operations, Responsive for mobile/desktop, simplified and beautiful design, User management with JWT Authentication, Angular reactive forms input validations on frontend, email confirmation with OTP on user registration/sign up using Twillio Sendgrid.

### Web application running on local host demo video: https://www.youtube.com/watch?v=EtNOF_makew

### Frontend- Angular UI git rep.: https://github.com/waqardongre/TDMManagerAngularUI.git

### Live Angular UI: https://tdm-manager-angular-ui.vercel.app/

### Live Asp.net core 6 web API: https://tdm20221108224831.azurewebsites.net/



## SQL - 
### To register a user you need to confirm your email id via OTP.
### Add 2 records in UserTypeInfo with UserTypeName: admin and user, UserTypeId = 1 and 2
### Any user registered will be user, to make some one admin, you have to change UserTypeId = 1 in UserInfo table for that user.
### Admin can see everyones uploaded models and can do CRUD op's.
### All 3D model files will be stored in bit format in SQL DB.

## Scripts -

    USE [tdmodelsdb]
    GO

    /****** Object:  Table [dbo].[UserTypeInfo]    Script Date: 09-Nov-22 4:39:09 PM ******/
    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO

    CREATE TABLE [dbo].[UserTypeInfo](
        [UserTypeId] [bigint] NOT NULL,
        [UserTypeName] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_UserTypeInfo] PRIMARY KEY CLUSTERED 
    (
        [UserTypeId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

    USE [tdmodelsdb]
    GO

    /****** Object:  Table [dbo].[UserInfo]    Script Date: 09-Nov-22 4:36:31 PM ******/
    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO

    CREATE TABLE [dbo].[UserInfo](
        [UserId] [bigint] IDENTITY(1,1) NOT NULL,
        [DisplayName] [nvarchar](max) NOT NULL,
        [UserName] [nvarchar](max) NOT NULL,
        [Email] [nvarchar](max) NOT NULL,
        [Password] [nvarchar](max) NOT NULL,
        [CreatedDate] [datetime2](7) NOT NULL,
        [UpdatedDate] [datetime2](7) NOT NULL,
        [IsActive] [bit] NOT NULL,
        [UserTypeId] [bigint] NOT NULL,
    PRIMARY KEY CLUSTERED 
    (
        [UserId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

    ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_IsActive]  DEFAULT ((1)) FOR [IsActive]
    GO

    ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_UserTypeId]  DEFAULT ((2)) FOR [UserTypeId]
    GO

    ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserTypeId_UserInfo] FOREIGN KEY([UserTypeId])
    REFERENCES [dbo].[UserTypeInfo] ([UserTypeId])
    GO

    ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserTypeId_UserInfo]
    GO

    USE [tdmodelsdb]
    GO

    /****** Object:  Table [dbo].[RefreshToken]    Script Date: 09-Nov-22 4:39:35 PM ******/
    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO

    CREATE TABLE [dbo].[RefreshToken](
        [RefreshTokenId] [bigint] IDENTITY(1,1) NOT NULL,
        [UserId] [bigint] NOT NULL,
        [RefreshToken] [nvarchar](max) NOT NULL,
        [Token] [nvarchar](max) NOT NULL,
        [IsActive] [bit] NOT NULL,
    PRIMARY KEY CLUSTERED 
    (
        [RefreshTokenId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

    ALTER TABLE [dbo].[RefreshToken] ADD  CONSTRAINT [DF_RefreshToken_IsActive]  DEFAULT ((1)) FOR [IsActive]
    GO

    ALTER TABLE [dbo].[RefreshToken]  WITH CHECK ADD  CONSTRAINT [FK_RefreshToken_UserInfo] FOREIGN KEY([UserId])
    REFERENCES [dbo].[UserInfo] ([UserId])
    GO

    ALTER TABLE [dbo].[RefreshToken] CHECK CONSTRAINT [FK_RefreshToken_UserInfo]
    GO

    USE [tdmodelsdb]
    GO

    /****** Object:  Table [dbo].[TDModels]    Script Date: 09-Nov-22 4:39:48 PM ******/
    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO

    CREATE TABLE [dbo].[TDModels](
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [UserId] [bigint] NOT NULL,
        [Email] [nvarchar](max) NOT NULL,
        [ModelName] [nvarchar](max) NOT NULL,
        [Model] [varbinary](max) NOT NULL,
        [CreatedDate] [datetime2](7) NOT NULL,
        [UpdatedDate] [datetime2](7) NOT NULL,
    CONSTRAINT [PK_TDModels] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

    ALTER TABLE [dbo].[TDModels]  WITH CHECK ADD  CONSTRAINT [FK_TDModels_UserInfo] FOREIGN KEY([UserId])
    REFERENCES [dbo].[UserInfo] ([UserId])
    GO

    ALTER TABLE [dbo].[TDModels] CHECK CONSTRAINT [FK_TDModels_UserInfo]
    GO

    USE [tdmodelsdb]
    GO

    /****** Object:  StoredProcedure [dbo].[PR_GETTDMODELS]    Script Date: 09-Nov-22 5:16:34 PM ******/
    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO



    CREATE PROCEDURE [dbo].[PR_GETTDMODELS] @UserId bigint
    AS
    DECLARE @UserTypeId bigint
    SET @UserTypeId = 2
    select @UserTypeId = UserTypeId from UserInfo where UserId = @UserId
    IF @UserTypeId=1
    BEGIN
        SELECT TD.*, UI.EMAIL FROM USERINFO UI INNER JOIN TDMODELS TD ON UI.USERID = TD.USERID
    END
    ELSE
    BEGIN
        SELECT TD.*, UI.EMAIL FROM USERINFO UI INNER JOIN TDMODELS TD ON UI.USERID = TD.USERID WHERE UI.USERID = @userId
    END
    GO



    USE [tdmodelsdb]
    GO

    /****** Object:  StoredProcedure [dbo].[PR_getUserRefreshToken]    Script Date: 09-Nov-22 5:16:53 PM ******/
    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO



    CREATE PROCEDURE [dbo].[PR_getUserRefreshToken] @UserId bigint, @RefreshToken nvarchar(Max)
    AS
        SELECT top 1 ui.*, rt.refreshtoken FROM UserInfo ui INNER JOIN refreshtoken rt ON ui.userid = rt.userid where rt.refreshtoken = @RefreshToken
    GO
USE [SMGPA_SCHEMA]
GO
/****** Object:  Table [dbo].[Activity]    Script Date: 08-11-2016 2:05:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activity](
	[idActivity] [uniqueidentifier] NOT NULL,
	[state] [int] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[idProcess] [uniqueidentifier] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_dbo.Activity] PRIMARY KEY CLUSTERED 
(
	[idActivity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Career]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Career](
	[Nombre] [nvarchar](50) NOT NULL,
	[idCareer] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Descripcion] [nvarchar](max) NOT NULL DEFAULT (''),
	[Activa] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.Career] PRIMARY KEY CLUSTERED 
(
	[idCareer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Document]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[idDocument] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Document] PRIMARY KEY CLUSTERED 
(
	[idDocument] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Entities]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entities](
	[idEntities] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Nombre] [nvarchar](30) NOT NULL,
	[Activo] [bit] NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_dbo.Entities] PRIMARY KEY CLUSTERED 
(
	[idEntities] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EntitiesFunctionary]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntitiesFunctionary](
	[Entities_idEntities] [uniqueidentifier] NOT NULL,
	[Functionary_idUser] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.EntitiesFunctionary] PRIMARY KEY CLUSTERED 
(
	[Entities_idEntities] ASC,
	[Functionary_idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Observation]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Observation](
	[idObservation] [uniqueidentifier] NOT NULL,
	[FechaComentario] [datetime] NOT NULL,
	[Comentario] [nvarchar](200) NOT NULL,
	[ValidacionEstatus] [int] NOT NULL,
	[Funcionario_idUser] [uniqueidentifier] NULL,
	[Tarea_idTask] [uniqueidentifier] NULL,
 CONSTRAINT [PK_dbo.Observation] PRIMARY KEY CLUSTERED 
(
	[idObservation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Operation]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operation](
	[idOperation] [uniqueidentifier] NOT NULL,
	[Nombre] [nvarchar](30) NOT NULL,
	[Descripcion] [nvarchar](200) NOT NULL,
	[Type] [int] NOT NULL,
	[idProcess] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Operation] PRIMARY KEY CLUSTERED 
(
	[idOperation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permission]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[idPermission] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[TextLink] [nvarchar](max) NOT NULL,
	[Controller] [nvarchar](max) NOT NULL,
	[ActionResult] [nvarchar](max) NOT NULL,
	[ActiveMenu] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Permission] PRIMARY KEY CLUSTERED 
(
	[idPermission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionRole]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionRole](
	[Permission_idPermission] [uniqueidentifier] NOT NULL,
	[Role_idRole] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.PermissionRole] PRIMARY KEY CLUSTERED 
(
	[Permission_idPermission] ASC,
	[Role_idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Process]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Process](
	[idProcess] [uniqueidentifier] NOT NULL,
	[Criterio] [nvarchar](20) NOT NULL,
	[Descripcion] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_dbo.Process] PRIMARY KEY CLUSTERED 
(
	[idProcess] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[idRole] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Nombre] [nvarchar](20) NOT NULL,
	[Descripcion] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_dbo.Role] PRIMARY KEY CLUSTERED 
(
	[idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[idTask] [uniqueidentifier] NOT NULL,
	[fechaInicio] [datetime] NULL,
	[fechaFin] [datetime] NULL,
	[TiempoInactividad] [float] NOT NULL,
	[Estado] [int] NOT NULL,
	[idFunctionary] [uniqueidentifier] NOT NULL,
	[idEntities] [uniqueidentifier] NOT NULL,
	[Nombre] [nvarchar](max) NULL,
	[Descripcion] [nvarchar](max) NULL,
	[DesplazamientoHoras] [float] NOT NULL,
	[DesplazamientoDias] [float] NOT NULL,
	[Activity_idActivity] [uniqueidentifier] NULL,
	[idDocument] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Tasks] PRIMARY KEY CLUSTERED 
(
	[idTask] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 08-11-2016 2:05:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[idUser] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Rut] [nvarchar](14) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Apellido] [nvarchar](max) NOT NULL,
	[MailInstitucional] [nvarchar](50) NOT NULL,
	[Contrasena] [nvarchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	[NumeroTelefono] [nvarchar](15) NULL,
	[CorreoPersonal] [nvarchar](50) NULL,
	[idCareer] [uniqueidentifier] NULL,
	[idRole] [uniqueidentifier] NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Activity] ADD  DEFAULT (newsequentialid()) FOR [idActivity]
GO
ALTER TABLE [dbo].[Document] ADD  DEFAULT (newsequentialid()) FOR [idDocument]
GO
ALTER TABLE [dbo].[Observation] ADD  DEFAULT (newsequentialid()) FOR [idObservation]
GO
ALTER TABLE [dbo].[Operation] ADD  DEFAULT (newsequentialid()) FOR [idOperation]
GO
ALTER TABLE [dbo].[Process] ADD  DEFAULT (newsequentialid()) FOR [idProcess]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (newsequentialid()) FOR [idTask]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ((0)) FOR [DesplazamientoHoras]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ((0)) FOR [DesplazamientoDias]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [idDocument]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Activity_dbo.Process_idProcess] FOREIGN KEY([idProcess])
REFERENCES [dbo].[Process] ([idProcess])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_dbo.Activity_dbo.Process_idProcess]
GO
ALTER TABLE [dbo].[EntitiesFunctionary]  WITH CHECK ADD  CONSTRAINT [FK_dbo.EntitiesFunctionary_dbo.Entities_Entities_idEntities] FOREIGN KEY([Entities_idEntities])
REFERENCES [dbo].[Entities] ([idEntities])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EntitiesFunctionary] CHECK CONSTRAINT [FK_dbo.EntitiesFunctionary_dbo.Entities_Entities_idEntities]
GO
ALTER TABLE [dbo].[EntitiesFunctionary]  WITH CHECK ADD  CONSTRAINT [FK_dbo.EntitiesFunctionary_dbo.User_Functionary_idUser] FOREIGN KEY([Functionary_idUser])
REFERENCES [dbo].[User] ([idUser])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EntitiesFunctionary] CHECK CONSTRAINT [FK_dbo.EntitiesFunctionary_dbo.User_Functionary_idUser]
GO
ALTER TABLE [dbo].[Observation]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Observation_dbo.Tasks_Tarea_idTask] FOREIGN KEY([Tarea_idTask])
REFERENCES [dbo].[Tasks] ([idTask])
GO
ALTER TABLE [dbo].[Observation] CHECK CONSTRAINT [FK_dbo.Observation_dbo.Tasks_Tarea_idTask]
GO
ALTER TABLE [dbo].[Observation]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Observation_dbo.User_Funcionario_idUser] FOREIGN KEY([Funcionario_idUser])
REFERENCES [dbo].[User] ([idUser])
GO
ALTER TABLE [dbo].[Observation] CHECK CONSTRAINT [FK_dbo.Observation_dbo.User_Funcionario_idUser]
GO
ALTER TABLE [dbo].[Operation]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Operation_dbo.Process_idProcess] FOREIGN KEY([idProcess])
REFERENCES [dbo].[Process] ([idProcess])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Operation] CHECK CONSTRAINT [FK_dbo.Operation_dbo.Process_idProcess]
GO
ALTER TABLE [dbo].[PermissionRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PermissionRole_dbo.Permission_Permission_idPermission] FOREIGN KEY([Permission_idPermission])
REFERENCES [dbo].[Permission] ([idPermission])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionRole] CHECK CONSTRAINT [FK_dbo.PermissionRole_dbo.Permission_Permission_idPermission]
GO
ALTER TABLE [dbo].[PermissionRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PermissionRole_dbo.Role_Role_idRole] FOREIGN KEY([Role_idRole])
REFERENCES [dbo].[Role] ([idRole])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionRole] CHECK CONSTRAINT [FK_dbo.PermissionRole_dbo.Role_Role_idRole]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Tasks_dbo.Activity_Activity_idActivity] FOREIGN KEY([Activity_idActivity])
REFERENCES [dbo].[Activity] ([idActivity])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_dbo.Tasks_dbo.Activity_Activity_idActivity]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Tasks_dbo.Document_idDocument] FOREIGN KEY([idDocument])
REFERENCES [dbo].[Document] ([idDocument])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_dbo.Tasks_dbo.Document_idDocument]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Tasks_dbo.Entities_idEntities] FOREIGN KEY([idEntities])
REFERENCES [dbo].[Entities] ([idEntities])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_dbo.Tasks_dbo.Entities_idEntities]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Tasks_dbo.User_idFunctionary] FOREIGN KEY([idFunctionary])
REFERENCES [dbo].[User] ([idUser])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_dbo.Tasks_dbo.User_idFunctionary]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_dbo.Career_idCareer] FOREIGN KEY([idCareer])
REFERENCES [dbo].[Career] ([idCareer])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_dbo.User_dbo.Career_idCareer]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_dbo.Role_idRole] FOREIGN KEY([idRole])
REFERENCES [dbo].[Role] ([idRole])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_dbo.User_dbo.Role_idRole]
GO

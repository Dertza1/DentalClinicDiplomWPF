USE [DentalClinicDB]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentID] [int] IDENTITY(1,1) NOT NULL,
	[BeginTimeAppointment] [time](7) NOT NULL,
	[EndTimeAppointment] [time](7) NOT NULL,
	[DateAppointment] [datetime2](7) NOT NULL,
	[DoctorID] [int] NULL,
	[StatusAppointmentID] [int] NULL,
	[ClientID] [int] NULL,
	[ServiceID] [int] NULL,
 CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
(
	[AppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[MiddleName] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[DayOfBirth] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctors]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctors](
	[DoctorID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[MiddleName] [nvarchar](max) NULL,
	[WorkPhone] [nvarchar](max) NULL,
	[PersonalPhone] [nvarchar](max) NULL,
	[CabinetNumber] [nvarchar](max) NULL,
	[WorkExperience] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[SpecializationID] [int] NULL,
 CONSTRAINT [PK_Doctors] PRIMARY KEY CLUSTERED 
(
	[DoctorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistoryAppointment]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistoryAppointment](
	[HistoryAppointmentID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NULL,
	[AppointmentsAppointmentID] [int] NULL,
 CONSTRAINT [PK_HistoryAppointment] PRIMARY KEY CLUSTERED 
(
	[HistoryAppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Specializations]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specializations](
	[SpecializationID] [int] IDENTITY(1,1) NOT NULL,
	[SpecializationName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Specializations] PRIMARY KEY CLUSTERED 
(
	[SpecializationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusAppointment]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusAppointment](
	[StatusAppointmentID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](max) NULL,
 CONSTRAINT [PK_StatusAppointment] PRIMARY KEY CLUSTERED 
(
	[StatusAppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkSchedules]    Script Date: 19.05.2023 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkSchedules](
	[WorkScheduleID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[BeginWorkDay] [time](7) NOT NULL,
	[EndWorkDay] [time](7) NOT NULL,
	[DoctorID] [int] NULL,
 CONSTRAINT [PK_WorkSchedules] PRIMARY KEY CLUSTERED 
(
	[WorkScheduleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Appointments] ON 

INSERT [dbo].[Appointments] ([AppointmentID], [BeginTimeAppointment], [EndTimeAppointment], [DateAppointment], [DoctorID], [StatusAppointmentID], [ClientID], [ServiceID]) VALUES (11, CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), CAST(N'2023-05-22T00:00:00.0000000' AS DateTime2), 2, 3, 2, 2)
INSERT [dbo].[Appointments] ([AppointmentID], [BeginTimeAppointment], [EndTimeAppointment], [DateAppointment], [DoctorID], [StatusAppointmentID], [ClientID], [ServiceID]) VALUES (12, CAST(N'18:00:00' AS Time), CAST(N'19:00:00' AS Time), CAST(N'2023-05-19T00:00:00.0000000' AS DateTime2), 1, 1, 7, 3)
SET IDENTITY_INSERT [dbo].[Appointments] OFF
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (1, N'Фролов', N'Александр', N'Сергеевич', N'8915312499', CAST(N'2004-01-13T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (2, N'Краснов', N'Игорь', N'Олегович', N'89776541233', CAST(N'2005-02-15T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (3, N'Павлюченко', N'Антон', N'Сергеевич', N'89771992577', CAST(N'1997-06-25T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (4, N'Черных', N'Руслан', N'Павлович', N'89771234568', CAST(N'2000-01-11T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (5, N'Яшнов', N'Андрей', N'Сергеевич', N'87112233544', CAST(N'2000-01-12T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (6, N'Ширяев', N'Павел', N'Григорьевич', N'12344568784', CAST(N'2000-01-15T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (7, N'Фролов', N'Даниил', N'Сергеевич', N'89771542354', CAST(N'2004-01-13T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Clients] ([ClientID], [LastName], [FirstName], [MiddleName], [Phone], [DayOfBirth]) VALUES (8, N'Иванов', N'Иван', N'Васильевич', N'89798456112', CAST(N'2023-05-13T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
SET IDENTITY_INSERT [dbo].[Doctors] ON 

INSERT [dbo].[Doctors] ([DoctorID], [LastName], [FirstName], [MiddleName], [WorkPhone], [PersonalPhone], [CabinetNumber], [WorkExperience], [Photo], [SpecializationID]) VALUES (1, N'Горин', N'Павел', N'Павлович', N'822', N'89771994432', N'11', N'822', N'', 1)
INSERT [dbo].[Doctors] ([DoctorID], [LastName], [FirstName], [MiddleName], [WorkPhone], [PersonalPhone], [CabinetNumber], [WorkExperience], [Photo], [SpecializationID]) VALUES (2, N'Горов', N'Данил', N'Александрович', N'433', N'222111344', N'8', N'4', NULL, 2)
INSERT [dbo].[Doctors] ([DoctorID], [LastName], [FirstName], [MiddleName], [WorkPhone], [PersonalPhone], [CabinetNumber], [WorkExperience], [Photo], [SpecializationID]) VALUES (3, N'Кухтинова', N'Татьяна', N'Анатольевна', N'45', N'89714551212', N'12', N'6', N'', 1)
INSERT [dbo].[Doctors] ([DoctorID], [LastName], [FirstName], [MiddleName], [WorkPhone], [PersonalPhone], [CabinetNumber], [WorkExperience], [Photo], [SpecializationID]) VALUES (4, N'Кухтинова', N'Татьяна', N'Анатольевна', N'45', N'89714551212', N'12', N'6', N'\PhotoDoctors\3642b290-0e98-4d2b-b67b-e49d23b22d95.jpeg', 1)
INSERT [dbo].[Doctors] ([DoctorID], [LastName], [FirstName], [MiddleName], [WorkPhone], [PersonalPhone], [CabinetNumber], [WorkExperience], [Photo], [SpecializationID]) VALUES (5, N'Дубко', N'Максим', N'Олегович', N'232', N'22233', N'234', N'2', N'\PhotoDoctors\eaea4a1c-8752-4742-b8dd-57348abe04ab.jpeg', 3)
INSERT [dbo].[Doctors] ([DoctorID], [LastName], [FirstName], [MiddleName], [WorkPhone], [PersonalPhone], [CabinetNumber], [WorkExperience], [Photo], [SpecializationID]) VALUES (6, N'Козелков', N'Александр', N'Сергеевич', N'452', N'89771992577', N'312', N'452', N'\PhotoDoctors\ea406bbf-cddb-481a-ae82-bab4ef367b0f.jpeg', 2)
SET IDENTITY_INSERT [dbo].[Doctors] OFF
GO
SET IDENTITY_INSERT [dbo].[HistoryAppointment] ON 

INSERT [dbo].[HistoryAppointment] ([HistoryAppointmentID], [ClientID], [AppointmentsAppointmentID]) VALUES (11, 2, 11)
INSERT [dbo].[HistoryAppointment] ([HistoryAppointmentID], [ClientID], [AppointmentsAppointmentID]) VALUES (12, 7, 12)
INSERT [dbo].[HistoryAppointment] ([HistoryAppointmentID], [ClientID], [AppointmentsAppointmentID]) VALUES (13, 7, 12)
SET IDENTITY_INSERT [dbo].[HistoryAppointment] OFF
GO
SET IDENTITY_INSERT [dbo].[Services] ON 

INSERT [dbo].[Services] ([ServiceID], [ServiceName]) VALUES (1, N'Чистка зубов')
INSERT [dbo].[Services] ([ServiceID], [ServiceName]) VALUES (2, N'Удаление')
INSERT [dbo].[Services] ([ServiceID], [ServiceName]) VALUES (3, N'Консультация')
SET IDENTITY_INSERT [dbo].[Services] OFF
GO
SET IDENTITY_INSERT [dbo].[Specializations] ON 

INSERT [dbo].[Specializations] ([SpecializationID], [SpecializationName]) VALUES (1, N'Стоматолог')
INSERT [dbo].[Specializations] ([SpecializationID], [SpecializationName]) VALUES (2, N'Детский стоматолог')
INSERT [dbo].[Specializations] ([SpecializationID], [SpecializationName]) VALUES (3, N'Стоматолог Хирург')
SET IDENTITY_INSERT [dbo].[Specializations] OFF
GO
SET IDENTITY_INSERT [dbo].[StatusAppointment] ON 

INSERT [dbo].[StatusAppointment] ([StatusAppointmentID], [StatusName]) VALUES (1, N'Принят')
INSERT [dbo].[StatusAppointment] ([StatusAppointmentID], [StatusName]) VALUES (2, N'Не явился')
INSERT [dbo].[StatusAppointment] ([StatusAppointmentID], [StatusName]) VALUES (3, N'Записан')
SET IDENTITY_INSERT [dbo].[StatusAppointment] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkSchedules] ON 

INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [Date], [BeginWorkDay], [EndWorkDay], [DoctorID]) VALUES (52, CAST(N'2023-05-17T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 1)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [Date], [BeginWorkDay], [EndWorkDay], [DoctorID]) VALUES (53, CAST(N'2023-05-18T00:00:00.0000000' AS DateTime2), CAST(N'09:00:00' AS Time), CAST(N'15:00:00' AS Time), 1)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [Date], [BeginWorkDay], [EndWorkDay], [DoctorID]) VALUES (54, CAST(N'2023-05-19T00:00:00.0000000' AS DateTime2), CAST(N'13:00:00' AS Time), CAST(N'19:00:00' AS Time), 1)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [Date], [BeginWorkDay], [EndWorkDay], [DoctorID]) VALUES (55, CAST(N'2023-05-22T00:00:00.0000000' AS DateTime2), CAST(N'09:00:00' AS Time), CAST(N'12:00:00' AS Time), 2)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [Date], [BeginWorkDay], [EndWorkDay], [DoctorID]) VALUES (56, CAST(N'2023-05-23T00:00:00.0000000' AS DateTime2), CAST(N'09:00:00' AS Time), CAST(N'13:00:00' AS Time), 2)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [Date], [BeginWorkDay], [EndWorkDay], [DoctorID]) VALUES (57, CAST(N'2023-05-24T00:00:00.0000000' AS DateTime2), CAST(N'09:00:00' AS Time), CAST(N'14:00:00' AS Time), 2)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [Date], [BeginWorkDay], [EndWorkDay], [DoctorID]) VALUES (58, CAST(N'2023-05-25T00:00:00.0000000' AS DateTime2), CAST(N'09:00:00' AS Time), CAST(N'12:00:00' AS Time), 2)
SET IDENTITY_INSERT [dbo].[WorkSchedules] OFF
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Clients_ClientID] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Clients] ([ClientID])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Clients_ClientID]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Doctors_DoctorID] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Doctors] ([DoctorID])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Doctors_DoctorID]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Services_ServiceID] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Services_ServiceID]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_StatusAppointment_StatusAppointmentID] FOREIGN KEY([StatusAppointmentID])
REFERENCES [dbo].[StatusAppointment] ([StatusAppointmentID])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_StatusAppointment_StatusAppointmentID]
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD  CONSTRAINT [FK_Doctors_Specializations_SpecializationID] FOREIGN KEY([SpecializationID])
REFERENCES [dbo].[Specializations] ([SpecializationID])
GO
ALTER TABLE [dbo].[Doctors] CHECK CONSTRAINT [FK_Doctors_Specializations_SpecializationID]
GO
ALTER TABLE [dbo].[HistoryAppointment]  WITH CHECK ADD  CONSTRAINT [FK_HistoryAppointment_Appointments_AppointmentsAppointmentID] FOREIGN KEY([AppointmentsAppointmentID])
REFERENCES [dbo].[Appointments] ([AppointmentID])
GO
ALTER TABLE [dbo].[HistoryAppointment] CHECK CONSTRAINT [FK_HistoryAppointment_Appointments_AppointmentsAppointmentID]
GO
ALTER TABLE [dbo].[HistoryAppointment]  WITH CHECK ADD  CONSTRAINT [FK_HistoryAppointment_Clients_ClientID] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Clients] ([ClientID])
GO
ALTER TABLE [dbo].[HistoryAppointment] CHECK CONSTRAINT [FK_HistoryAppointment_Clients_ClientID]
GO
ALTER TABLE [dbo].[WorkSchedules]  WITH CHECK ADD  CONSTRAINT [FK_WorkSchedules_Doctors_DoctorID] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Doctors] ([DoctorID])
GO
ALTER TABLE [dbo].[WorkSchedules] CHECK CONSTRAINT [FK_WorkSchedules_Doctors_DoctorID]
GO

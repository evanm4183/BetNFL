CREATE TABLE [UserProfile] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserTypeId] int NOT NULL,
  [Email] nvarchar(255) NOT NULL,
  [Username] nvarchar(255),
  [FirebaseUserId] nvarchar(255) NOT NULL,
  [AvailableFunds] decimal NOT NULL,
  [IsApproved] bit NOT NULL
)
GO

CREATE TABLE [UserType] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [Team] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [LocationName] nvarchar(255) NOT NULL,
  [TeamName] nvarchar(255) NOT NULL,
  [Abbreviation] nvarchar(255) NOT NULL,
  [LogoUrl] nvarchar(255)
)
GO

CREATE TABLE [Game] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [HomeTeamId] int NOT NULL,
  [AwayTeamId] int NOT NULL,
  [HomeTeamScore] int,
  [AwayTeamScore] int,
  [KickoffTime] datetime NOT NULL,
  [Week] int NOT NULL,
  [Year] int NOT NULL
)
GO

CREATE TABLE [Bet] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserProfileId] int NOT NULL,
  [GameId] int NOT NULL,
  [BetTypeId] int NOT NULL,
  [Line] float,
  [AwayTeamOdds] int,
  [HomeTeamOdds] int,
  [CreateDateTime] datetime NOT NULL,
  [isLive] bit NOT NULL
)
GO

CREATE TABLE [BetType] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [UserProfileBet] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserProfileId] int NOT NULL,
  [BetId] int NOT NULL,
  [WinnerId] int,
  [Side] int NOT NULL,
  [BetAmount] decimal NOT NULL,
  [CreateDateTime] datetime NOT NULL,
  [ProcessedDateTime] datetime
)
GO

CREATE TABLE [SiteTime] (
  [CurrentYear] int,
  [CurrentWeek] int
)
GO

ALTER TABLE [Game] ADD FOREIGN KEY ([HomeTeamId]) REFERENCES [Team] ([Id])
GO

ALTER TABLE [Game] ADD FOREIGN KEY ([AwayTeamId]) REFERENCES [Team] ([Id])
GO

ALTER TABLE [Bet] ADD FOREIGN KEY ([GameId]) REFERENCES [Game] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Bet] ADD FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
GO

ALTER TABLE [Bet] ADD FOREIGN KEY ([BetTypeId]) REFERENCES [BetType] ([Id])
GO

ALTER TABLE [UserProfileBet] ADD FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
GO

ALTER TABLE [UserProfileBet] ADD FOREIGN KEY ([BetId]) REFERENCES [Bet] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [UserProfileBet] ADD FOREIGN KEY ([WinnerId]) REFERENCES [UserProfile] ([Id])
GO

ALTER TABLE [UserProfile] ADD FOREIGN KEY ([UserTypeId]) REFERENCES [UserType] ([Id])
GO

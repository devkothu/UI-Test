CREATE TABLE [dbo].[ShortUrls]
(
    [Code] NVARCHAR(20) NOT NULL,
    [OriginalUrl] NVARCHAR(2048) NOT NULL,
    [CreatedAtUtc] DATETIME2 NOT NULL CONSTRAINT [DF_ShortUrls_CreatedAtUtc] DEFAULT (GETUTCDATE()),
    [VisitCount] INT NOT NULL CONSTRAINT [DF_ShortUrls_VisitCount] DEFAULT ((0)),
    CONSTRAINT [PK_ShortUrls] PRIMARY KEY CLUSTERED ([Code] ASC)
);
GO

CREATE INDEX [IX_ShortUrls_CreatedAtUtc]
ON [dbo].[ShortUrls]([CreatedAtUtc] DESC);
GO

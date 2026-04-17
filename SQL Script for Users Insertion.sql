Use ArtAuction

-- 1. Insert the Admin
INSERT INTO [AspNetUsers] 
([UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [SecurityStamp], [ConcurrencyStamp], [UserType])
VALUES 
('admin_user', 'ADMIN_USER', 'admin@art.com', 'ADMIN@ART.COM', 1, 0, 0, 1, 0, NEWID(), NEWID(), 'Admin');

-- Get the Admin ID to link to Artists
DECLARE @AdminId INT = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserType = 'Admin' ORDER BY Id DESC);

-- 2. Insert 2 Artists
INSERT INTO [AspNetUsers] 
([UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [SecurityStamp], [ConcurrencyStamp], [UserType], [City], [Country], [PhoneNumber], [HireDate], [AdminId])
VALUES 
('artist_1', 'ARTIST_1', 'a1@art.com', 'A1@ART.COM', 1, 0, 0, 1, 0, NEWID(), NEWID(), 'Artist', 'Cairo', 'Egypt', '0101', GETDATE(), @AdminId),
('artist_2', 'ARTIST_2', 'a2@art.com', 'A2@ART.COM', 1, 0, 0, 1, 0, NEWID(), NEWID(), 'Artist', 'Alex', 'Egypt', '0102', GETDATE(), @AdminId);

-- 3. Insert 3 Buyers
INSERT INTO [AspNetUsers] 
([UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [SecurityStamp], [ConcurrencyStamp], [UserType], [City], [Country], [PhoneNumber], [Address])
VALUES 
('buyer_1', 'BUYER_1', 'b1@mail.com', 'B1@MAIL.COM', 1, 0, 0, 1, 0, NEWID(), NEWID(), 'Buyer', 'Giza', 'Egypt', '0121', 'Nile St'),
('buyer_2', 'BUYER_2', 'b2@mail.com', 'B2@MAIL.COM', 1, 0, 0, 1, 0, NEWID(), NEWID(), 'Buyer', 'Cairo', 'Egypt', '0122', 'Cairo St'),
('buyer_3', 'BUYER_3', 'b3@mail.com', 'B3@MAIL.COM', 1, 0, 0, 1, 0, NEWID(), NEWID(), 'Buyer', 'Mansoura', 'Egypt', '0123', 'Mansoura St');

--TODO mby rename 
-- 1-to-N 
CREATE TABLE User
(
    id       INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    username NVARCHAR(255)                  NOT NULL,
    admin    bit              NOT NULL,
    password NVARCHAR(255)                  NOT NULL
);
--
-- 1-to-N

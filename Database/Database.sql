--TODO mby rename 
-- 1-to-N 
CREATE TABLE users
(
    id       INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    guid     UNIQUEIDENTIFIER NOT NULL,
    username NVARCHAR(255)                  NOT NULL,
    admin    bit              NOT NULL,
    password NVARCHAR(255)                  NOT NULL
);

-- 1-to-N 
create table pictures
(
    id           int identity (1,1) primary key not null,
    guid         UNIQUEIDENTIFIER NOT NULL,
    name         NVARCHAR(255)                  not null,
    description  NVARCHAR(255)                  not null,
    photographer NVARCHAR(255)                  not null,
    userId       int              not null,
    foreign key (userId) references users (id),
);
-- 1-to-1
create table pictureBytes
(
    pictureId int primary key not null,
    data      varbinary(max)  not null,
    foreign key (pictureId) references pictures (id) on delete cascade 
);
-- M-to-N
create table tags
(
    id   int identity (1,1) primary key not null,
    guid UNIQUEIDENTIFIER NOT NULL,
    name NVARCHAR(255)                  not null
);

-- M-to-N-bridge
create table pictureTags
(
    id        int identity (1,1) primary key not null,
    pictureId int not null,
    tagId     int not null,
    foreign key (pictureId) references pictures (id),
    foreign key (tagId) references tags (id)
);

-- 1-to-N 
create table downloads
(
    id        int identity (1,1) primary key not null,
    date      datetime not null,
    pictureId int      not null,
    userId    int      not null,
    foreign key (pictureId) references pictures (id) on delete cascade,
    foreign key (userId) references users (id) on delete cascade
);
GO

-- 1-to-N
create table logs
(
    id      int identity (1,1) primary key not null,
    date    datetime not null,
    message NVARCHAR(max)                  not null,
    Lvl     int      not null
);
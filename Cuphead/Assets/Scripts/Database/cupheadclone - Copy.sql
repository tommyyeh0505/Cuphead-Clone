DROP DATABASE  IF EXISTS CUPHEADCLONE;
CREATE DATABASE CUPHEADCLONE;
USE CUPHEADCLONE;
DROP TABLE IF EXISTS Users;
CREATE TABLE Users(
 userID INT NOT NULL AUTO_INCREMENT, 
 username VARCHAR(255), 
 PRIMARY KEY (userID)
 );
INSERT INTO Users(username) VALUES("tommy1");
INSERT INTO Users(username) VALUES("tommy2");
INSERT INTO Users(username) VALUES("tommy3");
INSERT INTO Users(username) VALUES("tommy4");
DROP TABLE IF EXISTS Score;
CREATE TABLE Score(
 userID int,
 levelID INT,
 clearTime DECIMAL(10,2),
 PRIMARY KEY (userID, levelID)
);
INSERT INTO Score(userID, levelID, clearTime) VALUES(1, 1, 1.234);
INSERT INTO Score(userID, levelID, clearTime) VALUES(1, 2, 2.234);
INSERT INTO Score(userID, levelID, clearTime) VALUES(1, 3, 3.234);
INSERT INTO Score(userID, levelID, clearTime) VALUES(1, 4, 4.234);
INSERT INTO Score(userID, levelID, clearTime) VALUES(2, 4, 5.234);

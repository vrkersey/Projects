Victor Kersey, Nick Steiner
12/08/2016

Database/Highscore
------------------------------------------
We built a static class that used MySQLConnections to talk to our database. 
We set up standard methods for inserting an entry and pulling scores. The 
Highscore methods can take in some custom perameters and safely send those 
commands to the MySql Server. We then built a miny webserver for listening 
for communication on port 11100 and uses the Network Controller. 

Our sql server only has one table in it, highscores. This table has 4 fields;
Name, Duration, Score, GameId
We decided to go with only one table because this table won't contain an
useless, duplicate data that should be in another table.

SQL Commands used:
------------------------------------------
INSERT INTO `cs3500_u0536910`.`SnakeGame_Reg_HiScore` (`GameID`, `PlayerName`, `Duration`, `Score`) VALUES(null, @name, '" + duration + "', '" + length + "');
select * from `cs3500_u0536910`.`SnakeGame_Reg_HiScore`"
select * from `cs3500_u0536910`.`SnakeGame_Reg_HiScore` where PlayerName = @name
select * from `cs3500_u0536910`.`SnakeGame_Reg_HiScore` where GameID = @gameID


TODO's
------------------------------------------
Unit Tests

Additional Game Mode
------------------------------------------
Shrink mode - player starts off large and has to eat food to shrink down.

Note for TA's
------------------------------------------
We ran into some major bugs a day before assignment was due. After fixing the 
bugs we were out of time and unable to get to unit tests.
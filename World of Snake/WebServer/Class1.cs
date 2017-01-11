using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Network_Controller;
using System.Text.RegularExpressions;

namespace WebServer
{
    public static class HighScore
    {
        public const string connectionString = "server=atr.eng.utah.edu;database=cs3500_u0536910;uid=cs3500_u0536910;password=LookIChangedIt";

        public static void addEntry(string name, string duration, int length)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "INSERT INTO `cs3500_u0536910`.`SnakeGame_Reg_HiScore` (`GameID`, `PlayerName`, `Duration`, `Score`)" +
                                                                                    "VALUES(null, @name, '" + duration + "', '" + length + "');";

                    command.Parameters.AddWithValue("@name", name);

                    using (MySqlDataReader reader = command.ExecuteReader()) { }
                }
                catch
                {

                }
            }
        }

        public static string lookupScores()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("PlayerName" + "\t");
            sb.Append("Score" + "\t");
            sb.Append("Duration" + "\t");
            sb.Append("GameID" + "\t");
            sb.Append("\n");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "select * from `cs3500_u0536910`.`SnakeGame_Reg_HiScore`";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sb.Append(reader["PlayerName"] + "\t");
                            sb.Append(reader["Score"] + "\t");
                            sb.Append(reader["Duration"] + "\t");
                            sb.Append(reader["GameID"] + "\t");
                            sb.Append("\n");
                        }
                    }
                }
                catch
                {

                }

                return sb.ToString();
            }
        }
        public static string lookupPlayer(string name)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("PlayerName" + "\t");
            sb.Append("Score" + "\t");
            sb.Append("Duration" + "\t");
            sb.Append("GameID" + "\t");
            sb.Append("\n");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "select * from `cs3500_u0536910`.`SnakeGame_Reg_HiScore` where PlayerName = @name";

                    command.Parameters.AddWithValue("@name", name);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sb.Append(reader["PlayerName"] + "\t");
                            sb.Append(reader["Score"] + "\t");
                            sb.Append(reader["Duration"] + "\t");
                            sb.Append(reader["GameID"] + "\t");
                            sb.Append("\n");
                        }
                    }
                }
                catch
                {

                }

                return sb.ToString();
            }
        }

        public static string lookupGame(int gameID)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("PlayerName" + "\t");
            sb.Append("Score" + "\t");
            sb.Append("Duration" + "\t");
            sb.Append("GameID" + "\t");
            sb.Append("\n");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "select * from `cs3500_u0536910`.`SnakeGame_Reg_HiScore` where GameID = @gameID";

                    command.Parameters.AddWithValue("@gameID", gameID);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sb.Append(reader["PlayerName"] + "\t");
                            sb.Append(reader["Score"] + "\t");
                            sb.Append(reader["Duration"] + "\t");
                            sb.Append(reader["GameID"] + "\t");
                            sb.Append("\n");
                        }
                    }
                }
                catch
                {

                }

                return sb.ToString();
            }
        }
    }

    public class Server
    {

        private const string HTMLHeader = "HTTP/1.1 200 OK\r\nConnection:close\r\nContent-Type: text/html; charset=UTF-8\t\n\t\n";

        public Server()
        {
            Networking.ServerAwaitingClientLoop(HandleNewClient, 11100);

        }

        private void HandleNewClient(SocketState s)
        {
            s.CallMe = recieveMessage;
            Networking.GetData(s);
        }

        private void recieveMessage(SocketState s)
        {
            //"GET / HTTP/1.1\r\n
            //Host: localhost:11100\r\n
            //Connection: keep-alive\r\n
            //Upgrade -Insecure-Requests: 1\r\n
            //User -Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36\r\n
            //Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n
            //Accept -Encoding: gzip, deflate, sdch, br\r\n
            //Accept -Language: en-US,en;q=0.8\r\n
            //\r\n"

            string str = s.sb.ToString().Substring(0, s.sb.ToString().IndexOf('\r'));

            string query;
            string title;

            switch (str)
            {
                case "GET /scores HTTP/1.1":
                    query = HighScore.lookupScores();
                    title = "Scores";
                    break;
                case "GET /games?player=Joe HTTP/1.1":
                    string name = "Joe";
                    query = HighScore.lookupPlayer(name);
                    title = "Scores for Player: " + name;
                    break;
                case "GET /game?id=35 HTTP/1.1":
                    string sid = "35";
                    int id;
                    int.TryParse(sid, out id);
                    query = HighScore.lookupGame(id);
                    title = "Scores for Game " + id;
                    break;
                default:
                    title = "Error";
                    query = null;
                    break;
            }


            StringBuilder sb = new StringBuilder();
            sb.Append(HTMLHeader);
            //sb.Append("<!DOCTYPE html>");
            //sb.Append("<html>");
            //sb.Append("<head>");
            sb.Append("<h1>" + title + "</h1>");
            //sb.Append("</head");
            //sb.Append("<body>");
            
            if (query != null)
            {
                sb.Append("<table border=\"1\">");

                string[] rows = Regex.Split(query, @"(?<=[\n])");

                foreach (string r in rows)
                {
                    if (r != "")
                    {
                        sb.Append("<tr>");
                        string[] cells = Regex.Split(r.Replace("\n", ""), @"(?<=[\t])");
                        foreach (string c in cells)
                        {
                            if (c != "")
                            {
                                sb.Append("<td>");
                                sb.Append(c.Replace("\t", ""));
                                sb.Append("</td>");
                            }
                        }
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }
            //sb.Append("</body>");
            //sb.Append("</html>");

            Networking.SendMessage(s, sb.ToString(), true);
        }
    }
}

using SnakeGame;
using Model;
using Network_Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Timers;
using WebServer;

namespace SnakeServer
{
    class SnakeServer
    {
        //server settings
        private int boardWidth, boardHeight, MSPerFrame, foodDensity;
        private float snakeRecycleRate;
        private bool shrinkMode = false;

        private World world;

        //active clients
        private List<SocketState> clients;
        private int clientID = 0;

        static void Main(string[] args)
        {
            SnakeServer server = new SnakeServer();
            while (true) {} ;
        }

        /// <summary>
        /// Snake Server constuctor. Initializes a new world and then starts accepting new clients
        /// </summary>
        public SnakeServer()
        {
            clients = new List<SocketState>();
            setParameters(); //read in the settings file
            world = new World(boardWidth, boardHeight, foodDensity, snakeRecycleRate, shrinkMode);
            Server webserver = new Server();

            Timer timer = new Timer();

            Console.WriteLine("Waiting for first connection...");
            timer.Interval = MSPerFrame;
            timer.Elapsed += Timer_Tick;
            timer.Enabled = true;

            Networking.ServerAwaitingClientLoop(HandleNewClient, 11000);
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            world.advanceWorld();
            try
            {
                lock (this)
                    foreach (SocketState ss in clients)
                    {
                        Networking.SendMessage(ss, world.ToString(), false);
                    }
            }
            catch
            {}

            // debugging by printing the 2d world to the console
            //Console.WriteLine(world.getWorld); 
            //Console.WriteLine();
            //Console.WriteLine();
        }

        /// <summary>
        /// Delegate for what to do when a new connection is made
        /// </summary>
        /// <param name="s">socket the connection was made on</param>
        private void HandleNewClient(SocketState s)
        {
            Console.WriteLine("A new client has contacted the Server");
            s.CallMe = RecieveName;
            Networking.GetData(s);
        }

        /// <summary>
        /// Delegate for what to do when the first message is sent by a new client
        /// </summary>
        /// <param name="s"></param>
        private void RecieveName(SocketState s)
        {
            lock (world)
            {
                clients.Add(s);
            }

            string name = s.sb.ToString().Replace("\n","");
            s.sb.Clear();

            int id = clientID++;
            lock (world)
            world.addSnake(new Snake(id, name));
            Networking.SendMessage(s, id + "\n" + boardWidth + "\n" + boardHeight, false);

            s.CallMe = ProcessMessage;
            Networking.GetData(s);
        }

        /// <summary>
        /// Delegate for handline all other messages sent from the client
        /// </summary>
        /// <param name="sender"></param>
        private void ProcessMessage(SocketState sender)
        {
            // Client disconnected
            if (sender.sb.Length == 0)
            {
                Console.WriteLine("ClientID " + sender.ID + " disconnected.");
                lock (world)
                    clients.Remove(sender);

                Networking.Disconnect(sender);

            }
            // Client sent movement commands
            else
            {
                if (sender.sb.ToString()[0] == '\n')
                    sender.sb.Remove(0, 1);
                if (sender.sb.ToString()[sender.sb.Length - 1] == '\n')
                {
                    string totalData = sender.sb.ToString().Replace("\n", "").Replace("(", "").Replace(")", "");
                    int direction;
                    int.TryParse(totalData[totalData.Length - 1].ToString(), out direction);
                    world.changeDirection(sender.ID, direction);
                    sender.sb.Remove(0, sender.sb.ToString().IndexOf('\n'));
                }
                Networking.GetData(sender);
            }
        }

        // Healper method for reading in the settings files
        private void setParameters()
        {
            using (XmlReader reader = XmlReader.Create("..\\..\\..\\Resources\\settings.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "SnakeSettings":
                                continue;

                            case "BoardWidth":
                                boardWidth = reader.ReadElementContentAsInt();
                                continue;

                            case "BoardHeight":
                                boardHeight = reader.ReadElementContentAsInt();
                                continue;

                            case "MSPerFrame":
                                MSPerFrame = reader.ReadElementContentAsInt();
                                continue;

                            case "FoodDensity":
                                foodDensity = reader.ReadElementContentAsInt();
                                continue;

                            case "SnakeRecycleRate":
                                snakeRecycleRate = reader.ReadElementContentAsFloat();
                                continue;

                            case "ShrinkMode":
                                shrinkMode = reader.ReadElementContentAsBoolean();
                                continue;
                        }
                    }
                }
            }
        }
    }
}
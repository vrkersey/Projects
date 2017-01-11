using Network_Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.Read();
        }

        private SocketState theServer;
        public Program()
        {
            System.Threading.Thread.Sleep(5000);
            theServer = Networking.ConnectToServer(FirstContact, Networking.IPResolve("localhost"));

        }
        // Connected to server, sends Name and waits
        private void FirstContact(SocketState s)
        {
            theServer.connectionEstablished = true;

            Networking.SendMessage(s, "Victor");
            s.CallMe = ReceiveStartup;
        }

        // receive startup data from the server
        private void ReceiveStartup(SocketState s)
        {
            //recieves your snake ID and dimentions of the world
            string startupData = s.sb.ToString();
            Console.WriteLine(startupData);
            s.sb.Remove(0, startupData.Length); //removing the startup data from the socketstate string builder
            s.CallMe = ReceiveWorld;
            Networking.GetData(s);
        }

        // receive World data from the server
        private void ReceiveWorld(SocketState s)
        {
            //if (s.sb.ToString().ToCharArray()[s.sb.Length - 1] == '\n') //only process data when a complete message was transmitted
            //{ processWorld(s.sb); }
            Console.WriteLine(s.sb.ToString());
            s.sb.Remove(0, s.sb.Length);
            Networking.GetData(s);
        }
    }
}

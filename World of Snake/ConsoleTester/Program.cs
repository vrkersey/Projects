using Network_Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleTester
{

#if false
    /// <summary>
    /// A simple server for receiving simple text messages from multiple clients
    /// </summary>
    class ChatServer
    {
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();
            server.StartServer();

            // Sleep to prevent the program from closing,
            // since all the real work is done in separate threads
            // StartServer is non-blocking
            Console.Read();
        }

        // A list of clients that are connected.
        private List<SocketState> clients;

        private TcpListener listener;

        public ChatServer()
        {
            listener = new TcpListener(IPAddress.Any, Networking.DEFAULT_PORT);

            clients = new List<SocketState>();
        }

        /// <summary>
        /// Start accepting Tcp sockets connections from clients
        /// </summary>
        public void StartServer()
        {
            Console.WriteLine("Server waiting for client");

            listener.Start();

            // This begins an "event loop".
            // ConnectionRequested will be invoked when the first connection arrives.
            listener.BeginAcceptSocket(ConnectionRequested, null);
        }

        /// <summary>
        /// A callback for invoking when a socket connection is accepted
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectionRequested(IAsyncResult ar)
        {
            Console.WriteLine("Contact from client");

            // Get the socket
            Socket s = listener.EndAcceptSocket(ar);

            // Save the socket in a SocketState, 
            // so we can pass it to the receive callback, so we know which client we are dealing with.
            SocketState newClient = new SocketState(s, clients.Count);

            // Can't have the server modifying the clients list if it's braodcasting a message.
            lock (clients)
            {
                clients.Add(newClient);
            }

            // Start listening for a message
            // When a message arrives, handle it on a new thread with ReceiveCallback
            //                                  the buffer          buffer offset        max bytes to receive                         method to call when data arrives    "state" object representing the socket
            newClient.theSocket.BeginReceive(newClient.messageBuffer, 0, newClient.messageBuffer.Length, SocketFlags.None, ReceiveCallback, newClient);

            // Continue the "event loop" that was started on line 53
            // Start listening for the next client, on a new thread
            listener.BeginAcceptSocket(ConnectionRequested, null);

        }

        /// <summary>
        /// Callback method for when data is received (started from line 80)
        /// </summary>
        /// <param name="ar">The result that includes the "state" parameter from BeginReceive</param>
        private void ReceiveCallback(IAsyncResult ar)
        {

            // Get the socket state out of the AsyncState
            // This is the object that we passed to BeginReceive that represents the socket
            SocketState sender = (SocketState)ar.AsyncState;

            int bytesRead = sender.theSocket.EndReceive(ar);

            // If the socket is still open
            if (bytesRead > 0)
            {
                string theMessage = Encoding.UTF8.GetString(sender.messageBuffer, 0, bytesRead);
                // Append the received data to the growable buffer.
                // It may be an incomplete message, so we need to start building it up piece by piece
                sender.sb.Append(theMessage);

                // TODO: If we had an "EventProcessor" delagate associated with the socket state,
                //       We could call that here, instead of hard-coding this method to call.
                ProcessMessage(sender);
            }

            // Continue the "event loop" that was started on line 80.
            // Start listening for more parts of a message, or more new messages
            sender.theSocket.BeginReceive(sender.messageBuffer, 0, sender.messageBuffer.Length, SocketFlags.None, ReceiveCallback, sender);

        }

        /// <summary>
        /// Given the data that has arrived so far, 
        /// potentially from multiple receive operations, 
        /// determine if we have enough to make a complete message,
        /// and process it (print it).
        /// </summary>
        /// <param name="sender">The SocketState that represents the client</param>
        private void ProcessMessage(SocketState sender)
        {
            string totalData = sender.sb.ToString();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.
            foreach (string p in parts)
            {

                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                Console.WriteLine("received message: \"" + p + "\"");

                byte[] messageBytes = Encoding.UTF8.GetBytes(p);

                // Remove it from the SocketState's growable buffer
                sender.sb.Remove(0, p.Length);

                // Broadcast the message
                // Can't have new connections popping up while looping through the clients list.
                lock (clients)
                {
                    foreach (SocketState client in clients)
                        client.theSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, client);

                }

            }

        }

        private void SendCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            ss.theSocket.EndSend(ar);
        }
    }
#endif
    class client
    {
        SocketState theServer;
        HashSet<Food> foods;
        List<Snake> snakes;

        static void Main(string[] args)
        {
            string s = "This is a sentence";
            Console.WriteLine(s.IndexOf("\n"));
            //string ip = "localHost";
            //client c = new client();
            //c.Connect(ip);
            //Food f = new Food(15, new Point(15, 30));

            //string s = JsonConvert.SerializeObject(f);
            //c.processWorld(s);

            // Sleep to prevent the program from closing,
            // since all the real work is done in separate threads
            // StartServer is non-blocking
            Console.Read();
        }
#if false
        private void Connect(string ip)
        {
            try
            {

                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;
                IPAddress ipAddress = IPAddress.None;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(ip);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + ip);
                        return;
                    }
                }
                catch (Exception e1)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(ip);
                }
                
                theServer = Networking.ConnectToServer(FirstContact, ipAddress);
            }
            catch (Exception e)
            {

            }
        }

        private void FirstContact(SocketState s)
        {
            Console.WriteLine("Connected");
            Networking.SendMessage(s,"Nick\n");
            s.CallMe = ReceiveStartup;
        }

        private void ReceiveStartup(SocketState s)
        {
            //recieves your snake ID and dimentions of the world
            string startupData = s.sb.ToString();
            Console.WriteLine(startupData);
            s.sb.Remove(0, startupData.Length);
            s.CallMe = ReceiveWorld;
            Networking.GetData(s);
        }

        bool first = true;
        private void ReceiveWorld(SocketState s)
        {
            //recieve world info
            string worldData = s.sb.ToString();
            //if (first)
                //Console.WriteLine(worldData);
    
            processWorld(s.sb.ToString());
            s.sb.Remove(0, worldData.Length);
            Console.WriteLine();
            Console.WriteLine();
            first = false;
            Networking.GetData(s);
        }

        private void processWorld(string data)
        {
            string[] parts = Regex.Split(data, @"(?<=[\n])");
            //Console.WriteLine(parts[5]);
            foreach (string s in parts)
            {
                if (s.Length > 0)
                {
                    Console.WriteLine(s);
                    JObject obj = JObject.Parse(s.Replace("\n",""));
                    JToken snakeProp = obj["vertices"];
                    JToken foodProp = obj["loc"];

                    if (snakeProp != null)
                    {
                        Snake snake = JsonConvert.DeserializeObject<Snake>(s.Replace("\n", ""));
                        Console.WriteLine(snake);
                    }

                    if (foodProp != null)
                    {
                        object food = JsonConvert.DeserializeObject<Food>(s.Replace("\n", ""));
                        Console.WriteLine(food);
                    }
                }
               
            }
        }
#endif
    }
}


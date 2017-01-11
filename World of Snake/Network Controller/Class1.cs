using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network_Controller
{
    /// <summary>
    /// Custom Object that contains all the socket information needed.
    /// </summary>
    public class SocketState
    {
        public Socket theSocket { get; set; }
        public int ID { get; set; }
        public Networking.callMe CallMe { get; set; }
        public bool connectionEstablished { get; set; }

        public byte[] messageBuffer = new byte[2048];

        public StringBuilder sb = new StringBuilder();

        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
            connectionEstablished = false;
        }
    }

    public class ConnectionState
    {
        public TcpListener listener;
        public Networking.callMe CallMe;
    }

    /// <summary>
    /// Networking Class used to connect to servers or clients
    /// </summary>
    public static class Networking
    {
        /// <summary>
        /// The default port used by our clients and server
        /// </summary>
        //public const int DEFAULT_PORT = 11000;

        private static int clientID = 0;

        /// <summary>
        /// callback function delegate
        /// </summary>
        /// <param name="s"></param>
        public delegate void callMe(SocketState s);

        #region Client
        /// <summary>
        /// Method used to convert string web addresses to IPAddress
        /// </summary>
        /// <param name="host">web address</param>
        /// <returns>IPAddress</returns>
        public static IPAddress IPResolve(string host)
        {
            // Establish the remote endpoint for the socket.
            IPHostEntry ipHostInfo;
            IPAddress ipAddress = IPAddress.None;

            // Determine if the server address is a URL or an IP
            try
            {
                ipHostInfo = Dns.GetHostEntry(host);
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
                    System.Diagnostics.Debug.WriteLine("Invalid addres: " + host);
                }
            }
            catch
            {
                // see if host name is actually an ipaddress, i.e., 155.99.123.456
                System.Diagnostics.Debug.WriteLine("using IP");
                ipAddress = IPAddress.Parse(host);
            }

            return ipAddress;
        }

        /// <summary>
        /// gracefully disconnect from a specific socket
        /// </summary>
        /// <param name="s"></param>
        public static void Disconnect(SocketState s)
        {
            s.theSocket.Disconnect(false);
            s.theSocket.Shutdown(SocketShutdown.Both);
            s.theSocket.Close();
        }

        /// <summary>
        /// Attempt to connect to a server
        /// </summary>
        /// <param name="action">callback for when connection is made with server</param>
        /// <param name="ip">ip address of the server</param>
        /// <returns></returns>
        public static SocketState ConnectToServer(callMe action, IPAddress ip, int port)
        {
            SocketState theServer;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

            theServer = new SocketState(s, -1);
            theServer.CallMe = action;
            s.BeginConnect(ip, port, ConnectedToServer, theServer);

            return theServer;
        }

        // Ends the attempt to connect to server and fires the callback then begins to receive message
        private static void ConnectedToServer(IAsyncResult ar)
        {
            
            SocketState state = (SocketState)ar.AsyncState;
            try
            {
                state.theSocket.EndConnect(ar);
                state.CallMe(state);
                state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, RecieveCallback, state);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Unable to connect to Server");
            }
        }
        #endregion
        // receives a message so end receive, add to stringbuilder then fire the callback
        private static void RecieveCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;
            try
            {
                int bytesRead = state.theSocket.EndReceive(ar);
                if (bytesRead == 0)
                {
                    state.sb.Clear();
                }
                else
                {
                    state.sb.Append(Encoding.UTF8.GetString(state.messageBuffer, 0, bytesRead));
                }
                state.CallMe(state);
            }
            catch { }

        }

        /// <summary>
        /// used to retrieve data sent by socketstate
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, RecieveCallback, state);
        }

        /// <summary>
        /// used to send a message to a socketstate
        /// </summary>
        /// <param name="state"></param>
        /// <param name="Message"></param>
        public static void SendMessage(SocketState state, string Message, bool close)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(Message + "\n");
            try
            {
                state.theSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, (ar) => SendCallBack(ar,close), state);
            }
            catch { }

        }

        // end the sendMessage callback
        private static void SendCallBack(IAsyncResult ar, bool close)
        {
            ((SocketState)ar.AsyncState).theSocket.EndSend(ar);

            if (close)
                Networking.Disconnect(((SocketState)ar.AsyncState));
        }

        /// <summary>
        /// wait for a new connection, after a new connection start listening again
        /// </summary>
        /// <param name="cm">call back delegate</param>
        public static void ServerAwaitingClientLoop(callMe cm, int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            ConnectionState cs = new ConnectionState();
            cs.listener = listener;
            cs.CallMe = cm;
            listener.BeginAcceptSocket(AcceptNewClient, cs);
        }

        /// <summary>
        /// Accept the new client then invoke the call back
        /// </summary>
        /// <param name="ar"></param>
        private static void AcceptNewClient(IAsyncResult ar)
        {
            ConnectionState cs = (ConnectionState)ar.AsyncState;
            Socket socket = cs.listener.EndAcceptSocket(ar);
            SocketState ss = new SocketState(socket, clientID++);
            ss.theSocket = socket;
            ss.CallMe = cs.CallMe;
            ss.CallMe(ss);
            cs.listener.BeginAcceptSocket(AcceptNewClient, cs);
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Utils;
using System.Threading;

namespace SMSPortalModemLibrary.Network
{
    public class NetworkManager
    {

        public static NetworkManager getInstance(OnDataRecievedListener _listener, int port)
        {
            if (instance != null)
                return instance;
            else
                return new NetworkManager(_listener, port);

        }

        public bool isConnected()
        {

            return socket.Connected;
        }

        private static NetworkManager instance;

        private int port;


        private OnDataRecievedListener listener;

        private Socket socket;

        BlockingCollection<byte[]> recievedQueue = new BlockingCollection<byte[]>();



        private NetworkManager(OnDataRecievedListener form, int port)
        {
            this.listener = form;
            this.port = port;
            instance = this;




            Task.Run(() =>
             {
                 while (true)
                 {
                     try
                     {
                         if (!recievedQueue.IsCompleted)
                         {

                             PacketHandler.getInstance().parseRecievedPacket(form, recievedQueue.Take());
                         }
                     }
                     catch (Exception e)
                     {
                         Console.WriteLine("Task Error => {0}", e.Message.ToString());
                     }
                 }
             });



        }


        public static string GetLocalIP()
        {
            //IPHostEntry host;
            //host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            //return "127.0.0.1";
            return "192.168.1.2";
        }


        public void StartListening()
        {
            if (socket == null || !socket.Connected)
            {
                byte[] bytes = new Byte[1024];


                IPAddress ipAddress = IPAddress.Parse(GetLocalIP());

                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);



                try
                {

                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(localEndPoint);
                    socket.Listen(100);

                    socket.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        socket);


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    throw e;

                }

            }
            else
            {
                Console.WriteLine("Modem is aready conected");
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {

            socket = (Socket)ar.AsyncState;
            socket = socket.EndAccept(ar);

            listener.onConnectionAcceted();

            StateObject state = new StateObject();

            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                StateObject state = (StateObject)ar.AsyncState;

                if (socket.Connected)
                {
                    int bytesRead = socket.EndReceive(ar);

                    if (bytesRead > 0)
                    {

                        try
                        {

                            foreach (byte[] data in PacketUtility.getDataPackets(state.buffer))
                            {
                                recievedQueue.Add(data);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Read CallBack Error => {0}", e.Message.ToString());
                        }

                    }
                    state = new StateObject();
                    socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error => " + e.Message);
                shutDown();
            } 

        }

        public void Send(byte[] data)
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    socket.BeginSend(data, 0, data.Length, 0,
                        new AsyncCallback(SendCallback), socket);
                }
                else
                {
                    Console.WriteLine("Modem is not connected. Must be reset... ");
                    throw new Exception("Modem is not connected. Must be reset... ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                throw e;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message.ToString());
            }
        }


        public void shutDown()
        {
            try
            {
                if (socket != null)
                {

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot shutdown socket => " + e.Message);
            }
        }



    }

    // State object for reading client data asynchronously
    public class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }


}

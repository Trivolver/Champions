using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualBasic;

namespace Champions
{
    static class MultiplayerManager
    {
        static Socket m_sendSocket;

        static int m_serverPort = 8999;
        static int m_recvPort = 8998;

        static IPAddress m_exchange;
        static IPEndPoint m_sendEndpoint;
        static IPEndPoint m_clientEndpoint;
        static UdpClient m_listener;
        static bool m_isConnected = false;
        static bool m_isServer = false;
        static bool m_isMulti = false;



        static MultiplayerManager()
        {
            m_sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public static void StartMultiplayer()
        {
            string choice1 = Microsoft.VisualBasic.Interaction.InputBox("Type 1 to host a game, type 2 to join a game.");
            if (choice1.Trim() == "1")
            {
                string choice2 = Microsoft.VisualBasic.Interaction.InputBox("Enter your IPAdress.");
                SetupServer(choice2.Trim());
                SetupServerReadClient();
            }
            else if (choice1.Trim() == "2")
            {
                choice1 = Microsoft.VisualBasic.Interaction.InputBox("");
            }
            else
            {
                Chat.AddMessage("Invalid choice; multiplayer not started.");
            }
        }

        public static void SetupServer(string a_IpAddress)
        {
            if (IPAddress.TryParse(a_IpAddress, out m_exchange))
            {
                Chat.AddMessage("IpAddress Valid.");
                m_sendEndpoint = new IPEndPoint(m_exchange, m_serverPort);

                m_isConnected = true;
                m_isServer = true;
                m_isMulti = true;


            }
            else
            {
                Chat.AddMessage("IpAddress invalid! Try again.");
            }
        }

        public static void SetupServerReadClient()
        {

        }

        public static void SetupClient()
        {
            m_listener = new UdpClient(m_serverPort);
            m_clientEndpoint = new IPEndPoint(IPAddress.Any, m_serverPort);

            m_isMulti = true;
            m_isConnected = true;
        }

        public static void SendDebug()
        {
            string message = "Hello! I'm connected.";
            byte[] buffer = Encoding.ASCII.GetBytes(message);

            try
            {
                m_sendSocket.SendTo(buffer, m_sendEndpoint);
                Chat.AddMessage("Message Sent. Waiting for response.");
            }
            catch (Exception messageBad)
            {
                Chat.AddMessage("There was a problem sending the message: ");
                Chat.AddMessage(messageBad.ToString());
            }
        }

        public static void ListenForServerMessages()
        {
            m_listener.BeginReceive(new AsyncCallback(RecieveMessage), null);
        }

        public static void ClientToServerMessages()
        {
            m_listener.BeginReceive(new AsyncCallback(RecieveMessage), null);
        }

        public static void RecieveMessage(IAsyncResult a_res)
        {
            byte[] data = m_listener.EndReceive(a_res, ref m_clientEndpoint);
            string stringdata = Encoding.ASCII.GetString(data, 0, data.Length);

            Chat.AddMessage(stringdata);
        }

        public static bool IsSetup()
        {
            return m_isConnected;
        }

        public static bool IsServer()
        {
            return m_isServer;
        }

        public static bool IsMulti()
        {
            return m_isMulti;
        }
    }
}

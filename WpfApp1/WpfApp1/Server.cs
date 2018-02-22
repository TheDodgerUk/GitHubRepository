using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Security.Permissions;

namespace ConsoleLogOutput
{
    class TcpServer
    {
        private Mutex m_Mutex;
        private TcpListener m_Server;
        private Messages m_Messages;
        Thread m_Thread;
        public TcpServer(ref Messages lMessages)
        {
            m_Mutex = new Mutex();
            m_Messages = lMessages;
            m_Server = new TcpListener(IPAddress.Any, 5555);
            m_Server.Start();
            LoopClients();
        }

        public void StopClass()
        {
            //m_Thread.Join();
            m_Thread.Abort();
        }


        public void LoopClients()
        {
            m_Thread = new Thread(new ParameterizedThreadStart(HandleClient));
            m_Thread.Start();

           // m_Messages.AddMessage("thread??14:00:47  00000??c:\\users\\antdrt\\progtest95\\progtest95.cpp (191): wWinMain");
           // m_Messages.AddMessage("thread??14:00:47  111111??c:\\users\\antdrt\\progtest95\\progtest95.cpp (191): wWinMain");

           // m_Messages.AddMessage("thread1??14:00:47  2222??c:\\users\\antdrt\\progtest95\\progtest95.cpp (191): wWinMain");
           // m_Messages.AddMessage("thread1??14:00:47  3333??c:\\users\\antdrt\\progtest95\\progtest95.cpp (191): wWinMain");
        }

        public void HandleClient(object obj)
        {
            Boolean bClientConnected = true;
            while (true)
            {
                while (!m_Server.Pending())
                {
                    Thread.Sleep(0);
                }
                TcpClient newClient = m_Server.AcceptTcpClient();
                NetworkStream networkStream = newClient.GetStream();
                networkStream.ReadTimeout = 100;

                String lData = string.Empty;
                bClientConnected = true;
                Byte[] lBytes;
                while (bClientConnected)
                {
                    if (newClient.ReceiveBufferSize > 0)
                    {
                        lBytes = new byte[newClient.ReceiveBufferSize];
                        try
                        {
                            networkStream.Read(lBytes, 0, newClient.ReceiveBufferSize);
                            lData = Encoding.ASCII.GetString(lBytes);
                            m_Mutex.WaitOne();
                            m_Messages.AddMessage(lData);
                            m_Mutex.ReleaseMutex();
                        }
                        catch
                        {
                            bClientConnected = false; // connection lost 
                        }
                    }
                    lData = string.Empty;
                }
            }
        }
    }
}
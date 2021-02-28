using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AlphaFlashSelectClient.stomp
{
    class StompConnection
    {

        private readonly string host;
        private readonly int port;

        private TcpClient tcpClient;

        public StompConnection(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public StompMessage connect(string accessToken)
        {

            this.tcpClient = new TcpClient(host, port);


            StompMessage connectMessage = new StompMessage("CONNECT");

            connectMessage.addHeader("passcode", accessToken);
            connectMessage.addHeader("heart-beat", "0,30000");

            WriteMessage(connectMessage);

            StompMessage connectResult = readMessage();

            return connectResult;


        } 

        public void Subscribe(string destination)
        {
            StompMessage connectMessage = new StompMessage("SUBSCRIBE");

            connectMessage.addHeader("destination", destination);
        
            WriteMessage(connectMessage);
        }

        public StompMessage readMessage()
        {
            byte[] bytes;

            while ((bytes = readFrame()).Length == 0)
                Console.WriteLine("Heartbeart");

            return new StompMessage(bytes);
        }

        public void WriteMessage(StompMessage stompMessage)
        {
            tcpClient.GetStream().Write(stompMessage.ToBytes());
            tcpClient.GetStream().Write(new byte[] { 0 });
        }

        byte[] readFrame()
        {
            //TODO - this should use the bulk read call for better performance
            int i;
            MemoryStream memoryStream = new MemoryStream();

            byte[] buffer = new byte[1024];


            while ((i = tcpClient.GetStream().ReadByte()) != -1)
            {

                if (i == 0 || (memoryStream.Length == 0 && i == '\n'))
                    return memoryStream.ToArray();

                byte b = System.Convert.ToByte(i);

                memoryStream.WriteByte(b);
            }

            throw new Exception("Unexpected end of stream");
        }
    }
}

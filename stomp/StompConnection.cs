using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;

namespace AlphaFlash.Select.Stomp
{
    class StompConnection
    {

        private readonly string host;
        private readonly int port;
        private readonly bool useSSL;
        private Stream stream;
        private TcpClient tcpClient;

        public StompConnection(string host, int port, bool ssl)
        {
            this.host = host;
            this.port = port;
            this.useSSL = ssl;
        }

        public StompMessage connect(string accessToken)
        {

            this.tcpClient = new TcpClient(host, port);

            if (useSSL){
                SslStream sslStream = new SslStream(tcpClient.GetStream(), false);

                sslStream.AuthenticateAsClient(host);

                this.stream = sslStream;
                
            }else
                this.stream = tcpClient.GetStream();


            StompMessage connectMessage = new StompMessage("CONNECT");

            connectMessage.addHeader("passcode", accessToken);
            connectMessage.addHeader("heart-beat", "0,30000");
            connectMessage.addHeader("accept-version", "1.0,1.1,1.2");

            WriteMessage(connectMessage);

            StompMessage connectResult = readMessage();

            return connectResult;


        } 

        public void Subscribe(string destination)
        {
            StompMessage connectMessage = new StompMessage("SUBSCRIBE");

            connectMessage.addHeader("destination", destination);
            connectMessage.addHeader("id", destination);
        
            WriteMessage(connectMessage);
        }

        public StompMessage readMessage()
        {
            Stream bytes;

            while ((bytes = readFrame()).Length == 0)
                ;

            bytes.Position = 0;

            return new StompMessage(bytes);
        }

        public void WriteMessage(StompMessage stompMessage)
        {
            stream.Write(stompMessage.ToBytes());
            stream.Write(new byte[] { 0 });
        }

        Stream readFrame()
        {
            //TODO - this should use the bulk read call for better performance
            int i;
            MemoryStream memoryStream = new MemoryStream();

            byte[] buffer = new byte[1024];


            while ((i = stream.ReadByte()) != -1)
            {

                if (i == 0 || (memoryStream.Length == 0 && i == '\n'))
                    return memoryStream;

                byte b = System.Convert.ToByte(i);

                memoryStream.WriteByte(b);
            }

            throw new Exception("Unexpected end of stream");
        }

        public void Close(){
            this.tcpClient.Close();
        }
    }
}

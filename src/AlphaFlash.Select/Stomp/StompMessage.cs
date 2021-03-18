using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlphaFlash.Select.Stomp
{
    class StompMessage
    {
        private readonly string messageType;
        private readonly Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>();
        private readonly string body = "";
        private static readonly byte[] lineSeperator = new byte[] { (byte)'\n' };
        private static readonly byte[] headerSeperator = new byte[] { (byte)':' };


        public string MessageType
        {
            get { return messageType; }
        }

        public string Body
        {
            get { return body; }
        }

        public StompMessage(string type)
        {
            this.messageType = type;
        }

        public StompMessage(Stream bytes)
        {
            TextReader reader = new StreamReader(bytes, Encoding.UTF8);

            this.messageType = reader.ReadLine();

            string header =  reader.ReadLine();

            while (header != null && !header.Equals(""))
            {
                string[] tokens = header.Split(":");

                addHeader(tokens[0], tokens[1]);

                header = reader.ReadLine();
            }

            this.body = reader.ReadToEnd();
        }

        public byte[] ToBytes()
        {
            MemoryStream memoryStream = new MemoryStream();

            memoryStream.Write(Encoding.UTF8.GetBytes(this.messageType));
            memoryStream.Write(lineSeperator);

            foreach (string s in headers.Keys)
            {
                foreach (string v in headers[s])
                {
                    memoryStream.Write(Encoding.UTF8.GetBytes(s));
                    memoryStream.Write(headerSeperator);
                    memoryStream.Write(Encoding.UTF8.GetBytes(v));
                    memoryStream.Write(lineSeperator);

                }
            }

            memoryStream.Write(lineSeperator);
            memoryStream.Write(Encoding.UTF8.GetBytes(this.body));

            return memoryStream.ToArray();

        }

        public void addHeader(string key, string value)
        {
            if (!this.headers.ContainsKey(key))
                this.headers.Add(key, new List<string>());

            this.headers[key].Add(value);
        }

        public string GetFirstHeader(string key){
            if (this.headers.ContainsKey(key) && this.headers[key].Count > 0)
                return this.headers[key][0];

            return null;
        }

        public override string ToString(){
            string result = this.MessageType + "\n";

            foreach (string s in headers.Keys)
            {
                foreach (string v in headers[s])
                {
                    result+=$"{s}:{v}\n";
                }
            }

            result+="\n";
            result+=this.Body;

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlphaFlashSelectClient.stomp
{
    class StompMessage
    {
        private readonly string messageType;
        private readonly Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>();
        private string body = "";
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

        private int countUntilNextOccurence(byte[] content, byte target, int start)
        {

            int count = 0;
            for (int i = start; i < content.Length; i++)
            {


                if (content[i] == target)
                    return count;

                count++;
            }

            return count;
        }

        public StompMessage(byte[] bytes)
        {
            int chunkStart = 0;
            int count = countUntilNextOccurence(bytes, (byte)'\n', chunkStart);

            this.messageType = System.Text.Encoding.UTF8.GetString(bytes, chunkStart, count);

            Console.WriteLine(this.messageType);

            chunkStart += count + 1;


            while ((count = countUntilNextOccurence(bytes, (byte)'\n', chunkStart)) != 0)
            {
                string header = System.Text.Encoding.UTF8.GetString(bytes, chunkStart, count);

                Console.WriteLine(header);

                string[] tokens = header.Split(":");

                addHeader(tokens[0], tokens[1]);

                chunkStart += count + 1;

            }

            this.body = System.Text.Encoding.UTF8.GetString(bytes, chunkStart, bytes.Length - chunkStart);
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
    }
}

using System;
using System.IO;
using System.Text;
using System.Xml;
namespace fodt2txt
{
    class Program
    {
        static void Main(string[] args)
        {
            string stdin = null;
            if (Console.IsInputRedirected)
            {
                Console.WriteLine("Console is redirected");
                using (StreamReader reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
                {
                    stdin = reader.ReadToEnd();
                }
            }
            else {
                 Console.WriteLine("Console is not really redirected");
            }
            if (stdin != null) 
            {
                byte[] bytes = Encoding.UTF8.GetBytes(stdin);  
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (XmlTextReader reader = new XmlTextReader(ms))
                    {
                    Console.WriteLine("In the XmlTextReader"); 
                    while (reader.Read()){
                        Console.WriteLine("{0}, {1}", reader.NodeType, reader.Name);
                        /// Console.WriteLine(reader.NodeType);
                        if (reader.NodeType == XmlNodeType.Text){
                            //Console.WriteLine("Text node found");
                            //Console.WriteLine(reader.Name);
                         }

                    }
                    };
                }
            }
        }
    }
}

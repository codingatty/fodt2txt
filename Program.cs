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
            string fodt_contents = null;
            XmlDocument doc = null;

            fodt_contents = getXMLStdinAsString();
            
            if (fodt_contents != null) 
            {
                doc = makeXMLDoc(fodt_contents);
            }

            if (doc != null)
            {
                Console.WriteLine("Yo: {0}", doc.InnerText);
            }
            else
            {
                Console.WriteLine("Yo, makeXMLDoc failed");
            }


            string getXMLStdinAsString(){
                string stdin = null;
                if (Console.IsInputRedirected)
                {
                    Console.WriteLine("Console is redirected");
                    using (StreamReader reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
                    {
                        stdin = reader.ReadToEnd();
                    }
                }
                else
                {
                    Console.WriteLine("Console is not really redirected");
                }
                return(stdin);
            }

            XmlDocument makeXMLDoc(string XMLString)
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(XMLString);
                }
                catch (System.Xml.XmlException){
                    doc = null;
                }
                return doc;
            }
            void firstTry(string XMLString)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(XMLString);  
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (XmlTextReader reader = new XmlTextReader(ms))
                    {
                        Console.WriteLine("In the XmlTextReader"); 
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}, {1}, {2}", reader.NodeType, reader.Name);
                            /// Console.WriteLine(reader.NodeType);
                            if (reader.NodeType == XmlNodeType.Text)
                            {
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

using System;
using System.IO;
using System.Text;
using System.Xml;
namespace fodt2txt

/*

Copyright 2022 Terry Carroll

Simple program to read a LibreOffice .fodt and dump out its text analogous to odt2txt for .odt

I wrote this primarily to be able to get reasonable diffs of .fodt files from a git diff command
without displaying extraneous XML tags. It's not perfect, but is good enough for my needs.

This program is licensed under Apache License, version 2.0 (January 2004);
see http://www.apache.org/licenses/LICENSE-2.0
SPX-License-Identifier: Apache-2.0

usage:

   fodt2txt <document.fodt

Any text found inside <text:p> tags is simply dumped to stdout.

There is no stdout if:
   there is no stdin;
   stdin doesn't parse as XML; 
   or the XML has no <text:p> tags

*/

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
                // Console.WriteLine("makeXMLDoc worked");
                
                XmlNodeList nodes2 = doc.GetElementsByTagName("*");
                foreach (XmlNode node in nodes2)
                {
                    // Console.WriteLine("in foreach"); 
                    // Console.WriteLine("{0}", node.Name);
                    if (node.Name == "text:p")
                    {
                        // Console.WriteLine("{0}", node.InnerText);
                        Console.WriteLine($"{node.InnerText}");

                    }
                }
            }

            string getXMLStdinAsString(){
                string stdin = null;
                if (Console.IsInputRedirected)
                {
                    // Console.WriteLine("Console is redirected");
                    using (StreamReader reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
                    {
                        stdin = reader.ReadToEnd();
                    }
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
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Xml;

/*

Copyright 2022 Terry Carroll

Simple program to read a LibreOffice .fodt file and dump out its text,
analogous to what odt2txt does for .odt

I wrote this primarily to be able to get reasonable diffs of .fodt files from a git diff command
without displaying extraneous XML tags. It's not perfect, but is good enough for my needs.

This program is licensed under Apache License, version 2.0 (January 2004);
see http://www.apache.org/licenses/LICENSE-2.0
SPX-License-Identifier: Apache-2.0

usage:

   fodt2txt document.fodt

Any text found inside <text:p> tags is simply dumped to stdout.

Bare-bones, no diagnostic messages. It just lets exceptions occur, otherwise
it would look like the error message text is the file content.

Deficiencies:
 Does not catch format changes like italics or bold; nor things like tab characters.
 I may look into doing this sometime in the future

*/

namespace fodt2txt
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathname = null;
            switch (args.Length)
            {
                case 0:
                    throw new ArgumentException(string.Format("No arguments provided: exactly one argument is required to indicate pathname of file to process"));
                
                case 1:
                    pathname = args[0];
                    break;
                
                default:
                    throw new ArgumentException(string.Format("Too many arguments: exactly one argument required to indicate pathname of file to process"));
            }

            var xmldoc = new XmlDocument();
            xmldoc.Load(pathname);
            XmlNodeList nodes2 = xmldoc.GetElementsByTagName("*");
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
    }
}
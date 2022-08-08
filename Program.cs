using System;
using System.Collections.Generic;
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
            int LINE_LENGTH = 65;  // 0: do not split lines; any other number split at max indicated length (rounding up to word boundary)

            switch (args.Length)
            {
                case 0:
                    Console.Error.WriteLine("No arguments provided: exactly one argument is required to indicate pathname of file to process");
                    return;
                
                case 1:
                    pathname = args[0];
                    break;
                
                default:
                    Console.Error.WriteLine("Too many arguments: exactly one argument required to indicate pathname of file to process");
                    return;
            }

            var xmldoc = new XmlDocument();
            xmldoc.Load(pathname);
            XmlNodeList nodes2 = xmldoc.GetElementsByTagName("*");
            foreach (XmlNode node in nodes2)
            {
                if (node.Name == "text:p")
                {
                    List<string> split_lines = split_up(node.InnerText, LINE_LENGTH);

                    foreach (string line in split_lines)
                    {
                        Console.WriteLine($"{line}");
                    }                   
                }
            }

            List<string> split_up(string text, int fragment_length)
            {
                List<string> returned_list = new List<string>();
                if ((fragment_length == 0) || (text.Length <= fragment_length))
                {
                    returned_list.Add(text);
                }
                else
                {
                    string remaining_text = text;
                    do
                    {
                    int splitpoint = get_splitpoint(remaining_text, fragment_length);
                    string fragment = remaining_text.Substring(0, splitpoint);
                    returned_list.Add(fragment);
                    remaining_text = remaining_text.Substring(splitpoint);
                    } while (remaining_text != "");
                }
                return returned_list;
            }

            int get_splitpoint(string text, int max_length)
            {
                // find the place to split into a max_length chunk
                // if the string is shorter than max_length, use the entire string
                // otherwise:
                //   use position after the last blank in the first max_length characters;
                //   if there are no blanks in that porion, use the first blank occurring *after* max_length;
                //   if there are no blanks at all, use the entire string
                int splitpoint;
                if (text.Length < max_length) splitpoint = text.Length;
                else 
                {
                    var check = text.Substring(0, max_length).LastIndexOf(" ");
                    if (check != -1) splitpoint = check + 1; // "+1" to split *after* the blank
                    else
                    {
                        check = text.IndexOf(" ", max_length);
                        if (check != -1) splitpoint = check + 1;
                        else splitpoint = text.Length;
                    }
                }
                return splitpoint;
            }
        }
    }
}
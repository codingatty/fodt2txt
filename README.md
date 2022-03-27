# fodt2txt
Simple program to read a LibreOffice .fodt file and dump out its text, analogous to what [odt2txt](https://github.com/dstosberg/odt2txt) does for .odt.

I wrote this primarily to be able to get reasonable diffs of .fodt files from a git diff command
without displaying extraneous XML tags. It's not perfect, but is good enough for my needs.

This program is licensed under the [Apache Software License, version 2.0](http://www.apache.org/licenses/LICENSE-2.0).

usage (command line):

   `fodt2txt document.fodt`

To use in `git`:

1. add the following line to the `.gitattributes` file:

````
*.fodt diff=fodt
````
    
2. add the following lines to the `git/config` file:

````
[diff "fodt"]
    textconv = fodt2txt
````

Any text found inside `<text:p>` tags is simply dumped to stdout.

Bare-bones, no diagnostic messages. It just lets exceptions occur, otherwise
it would look like the error message text is the file content.

Deficiencies:

Does not catch format changes like italics or bold; nor things like tab characters.
I may look into doing this sometime in the future.

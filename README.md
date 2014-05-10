Freaky-Sources
==============

Collection of freaky sources written on C# (mostly quines in different forms).

##Sources
* Single Line Comment Palindrome Quine.
* Multi Line Comment Palindrome Quine.
* Asciimationa Quine (Used data from http://asciimation.co.nz/ by Simon Jansen © 1997 - 2011).

![Star wars in source code](http://habrastorage.org/getpro/habr/post_images/e08/393/bed/e08393bed8a7a465bccbd5b8a9bc9eec.png)


##Implemention detail
Quine generation consists of several steps:
* Code generation
* Data generation
* Code minification
* Code formatting
* Quine genration

At first next quine template is used:
```csharp
using System;
using System.Text;
using System.Collections.Generic;

namespace Asciimation_1_3
{
    class Program
    {
        /*#Asciimation_1_3*/
        /*Asciimation_1_3#*/

        /*#HuffmanTree*/
        /*HuffmanTree#*/

        /*#HuffmanRleDecode2*/
        /*HuffmanRleDecode2#*/

        /*#Enums*/
        /*Enums#*/

        /*#Utils*/
        /*Utils#*/

        static string Data = /*%Data_1_3*/""/*Data_1_3%*/;
        static int CurrentFrame = /*$CurrentFrame*/0/*CurrentFrame$*/;

        static void Main()
        {
            var output = Decompress_v_1_3(Data, CurrentFrame++);
            if (CurrentFrame == 3591)
                CurrentFrame = 3590;
            /*@*/
        }
    }
}
/*$Output_1_3$*/
```

Comments with special char combinations named as "markers". Marker is used for code extraction or generation from different sources.
Marker can have one of the following types:

* Code copying from files for code blocks marked as ```/*#...*/… /*...#*/```.
* Data generation for data blocks marked ```/*%...*/… /*...%*/```.
* Quine parameters (introns) marked as ```/*$...*/… /*...$*/```.
* Place, where quine should be printed marked as ```/*@*/```.

##Compilation

One can generate quine via developed GUI. But prepared asciimation code is available here:   [AsciimationQuine_1_3.7z](https://github.com/KvanTTT/Freaky-Sources/releases/download/1.3/AsciimationQuine_1_3.7z)

It's possible to compile one frame or entire animation with following batch commands (.NET only tested):

```
echo off
SET /a i=0
:LOOP
IF %i%==3591 GOTO END
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" Asciimation.cs && (Asciimation > Asciimation.cs) && Asciimation
SET /a i=%i%+1
GOTO LOOP
:END
pause
```

##Licence

* Code for code, data, quine generation under the Apache 2.0 License.
* Generated source codes under the MIT License.

More detail description is available on [Russian](http://habrahabr.ru/post/190616/)

Enjoy!
```
     ___     _____   _____   ^   ^   _    _     ___    _____   ^   _____   __  _ 
    / _ \   |  ___| |  ___| | | | | | \  / |   / _ \  |_   _| | | |  _  | |  \| |
   / /_\ \  |___  | | |___  | | | | |  \/  |  / /_\ \   | |   | | | |_| | | |\  |
  /_______\ |_____| |_____| |_| |_| |_|\/|_| /_______\  |_|   |_| |_____| |_| \_|
                                                                        
   _____    _   _   ^   __  _   _____                                
  |  _  |  | | | | | | |  \| | |  ___|                               
  | |_| |  | |_| | | | | |\  | |  __|              
  |_____ \ |_____| |_| |_| \_| |_____|                               
        \/                                        
```

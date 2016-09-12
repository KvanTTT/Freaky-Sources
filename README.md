Freaky-Sources
==============

Collection of freaky sources written on C# (mostly quines in different forms).

More detail description is available on Russian:
* [С днем программиста !! атсиммаргорп менд С (Квайн-палиндром)](https://habrahabr.ru/post/189192/).
* [Звездные войны в исходном коде](http://habrahabr.ru/post/190616/).
* [Интересности из мира квайнов (100 языков, радиационно-устойчивый, часы, змейка)](https://habrahabr.ru/post/232781/).
* [Пишем квайн-полиглот-палиндромы в честь дня 2^2^3](https://habrahabr.ru/company/pt/blog/309702/).

## Sources
#### Single Line Comment Palindrome Quine.

#### Multi Line Comment Palindrome Quine.

#### Asciimation Quine
Used data from http://asciimation.co.nz/ by Simon Jansen © 1997 - 2011. One compilation per one frame.

![Star wars in source code](http://habrastorage.org/getpro/habr/post_images/57d/e67/628/57de6762827e000614ac24b327dbf6a7.png)

#### Quine Clock.
One compilation per one second.

![Quine Clock](https://habrastorage.org/files/6e4/831/3d3/6e48313d31e046ffb47410f829697e48.png)

#### Quine Snake.
Use arrow keys to change direction and move snake. Game can be either completed or failed :)

![Quine Snake](https://habrastorage.org/files/b52/2fd/6d8/b522fd6d824542dcb6c318b7c4b62161.png)

#### Polyglot Quine.
The quine that compilied both in C# and Java.

#### Palindrome Polyglot Quine
The quine that compilied both in C# and Java and have a palindromic format (PalindromePolyglotQuine.cs.java):

```CSharp
/**///\u000A\u002F\u002A
using System;//\u002A\u002F
class Program{public static void//\u000A\u002F\u002A
Main//\u002A\u002Fmain
(String[]z){String s="`**?`@#_^using System;?_#^class Program{public static void?@#_^Main?_#main^(String[]z){String s=!$!,t=s;int i;int[]a=new int[]{33,94,38,64,35,95,96,63,36};String[]b=new String[]{!&!!,!&n!,!&&!,!&@!,!&#!,!&_!,!`!,!?!,s};for(i=0;i<9;i++)t=t.?@#_^Replace?_#replace^(!!+(char)a[i],b[i]);t+='*';for(i=872;i>=0;i--)t=t+t?@#_^[i];Console.Write?_#.charAt(i);System.out.printf^(t);}}/",t=s;int i;int[]a=new int[]{33,94,38,64,35,95,96,63,36};String[]b=new String[]{"\"","\n","\\","\\u000A","\\u002F","\\u002A","/","//",s};for(i=0;i<9;i++)t=t.//\u000A\u002F\u002A
Replace//\u002A\u002Freplace
(""+(char)a[i],b[i]);t+='*';for(i=872;i>=0;i--)t=t+t//\u000A\u002F\u002A
[i];Console.Write//\u002A\u002F.charAt(i);System.out.printf
(t);}}/*/}};)t(
ftnirp.tuo.metsyS;)i(tArahc.F200u\A200u\//etirW.elosnoC;]i[
A200u\F200u\A000u\//t+t=t)--i;0=>i;278=i(rof;'*'=+t;)]i[b,]i[a)rahc(+""(
ecalperF200u\A200u\//ecalpeR
A200u\F200u\A000u\//.t=t)++i;9<i;0=i(rof;}s,"//","/","A200u\\","F200u\\","A000u\\","\\","n\",""\"{][gnirtS wen=b][gnirtS;}63,36,69,59,53,46,83,49,33{][tni wen=a][tni;i tni;s=t,"/}};)t(^ftnirp.tuo.metsyS;)i(tArahc.#_?etirW.elosnoC;]i[^_#@?t+t=t)--i;0=>i;278=i(rof;'*'=+t;)]i[b,]i[a)rahc(+!!(^ecalper#_?ecalpeR^_#@?.t=t)++i;9<i;0=i(rof;}s,!?!,!`!,!_&!,!#&!,!@&!,!&&!,!n&!,!!&!{][gnirtS wen=b][gnirtS;}63,36,69,59,53,46,83,49,33{][tni wen=a][tni;i tni;s=t,!$!=s gnirtS{)z][gnirtS(^niam#_?niaM^_#@?diov citats cilbup{margorP ssalc^#_?;metsyS gnisu^_#@`?**`"=s gnirtS{)z][gnirtS(
niamF200u\A200u\//niaM
A200u\F200u\A000u\//diov citats cilbup{margorP ssalc
F200u\A200u\//;metsyS gnisu
A200u\F200u\A000u\///**/
```

## Implemention detail
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

## Compilation

One can generate quine via developed GUI. But prepared asciimation code is available here:   [AsciimationQuine_1_3.7z](https://github.com/KvanTTT/Freaky-Sources/releases/download/1.3/AsciimationQuine_1_3.7z). It's possible to compile one frame or entire animation with the following scripts:

### Windows command line (bat):
```batch
echo off

:LOOP
    "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" "Asciimation_1_3.cs"
    "Asciimation_1_3.exe" > "Asciimation_1_3.cs"
    type "Asciimation_1_3.cs"
goto LOOP

:END
```

### Powershell:
```powershell
while ($true) {
    &"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" "Asciimation_1_3.cs"
    ./"Asciimation_1_3.exe" > "Asciimation_1_3.cs"
    type "Asciimation_1_3.cs"
}
```

### Linux with mono
```shell
while :
do
    mcs "Asciimation_1_3.cs"
    mono "Asciimation_1_3.exe" > "Asciimation_1_3.cs"
    cat "Asciimation_1_3.cs"
done
```

## Tests

Quines, Palidrome, Polyglot and other program tests available in FreakySource.Tests project. Polyglot quine tests require installed Java.

## Licence

* Code for code, data, quine generation under the Apache 2.0 License.
* Generated source codes under the MIT License.

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

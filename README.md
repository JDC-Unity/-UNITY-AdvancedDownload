# -UNITY-AdvancedDownload
Download several files and directories with ASYNC method 
# How It is work?
here is a example of download some files and sorting by folders:

```javascript
using UnityEngine;
using AdvancedDownload;
using System.Threading.Tasks;

public class example : MonoBehaviour
{
    AdvRequest todo; // field
    void Start()
    {
        todo = new AdvRequest(new string[] { "https://somethin.com/folder/folder2/file.txt", "https://somethin.com/folder/homework.amv" }, "https://somethin.com/");
        todo.Begin(); // Start downloading
        todo.OnDownload += sendWhenDownload; //adding function what will be executed after download file (non async example: todo.OnDownload += justPrintSomeThing;)
    }

    void justPrintSomeThing()
    {
        print("was downloaded lol");
    }

    async void sendWhenDownload()
    {
        print("file downloaded: "+todo.fileName+", total download time: "+todo.timePassed);

        todo.Pause(); //Pauses the download after downloading file (Pause is no longer BETA yay!)

        await Task.Delay(1000);

        todo.Resume(); //Resume
    }
}

```
Result of this code will be:

- (CREATED) UrApplicationPath/folder
- (CREATED) UrApplicationPath/folder/folder2
- (CREATED) UrApplicationPath/folder/folder2/file.txt
--------
- (Printed) file downloaded: file.txt, total download time: 00m:00s.78ms
--------
- (Action) Stop the download for a second
--------
- (Created) UrApplicationPath/folder/homework.amv
--------
- (Printed) file downloaded: homework.amv, total download time: 23m:06s.00ms
--------
- (Action) Stop the download for a second
- END

## What if I dont want to sort the files?
If u dont want to sort files by folders specified in link, do this:

```javascript
using UnityEngine;
using AdvancedDownload;

public class example : MonoBehaviour
{
    void Start()
    {
        AdvRequest todo = new AdvRequest(new string[]{ "https://somethin.com/folder/folder2/file.txt", "https://somethin.com/folder/homework.amv" }); //removed second (root) argument
        
        todo.Begin(); //begin download
    }
}
```
Result of this code will be:

- UrApplicationPath/file.txt

- UrApplicationPath/homework.amv

## Variables,Functions
from "AdvRequest todo" we can get:
- todo.progress - float variable, contains progress of current downloading from 0 to 100;
- todo.fileName - string variable, contains name of file downloanding now
- todo.timePassed - string variable with "{0:00m}:{1:00s}.{2:00ms}" format, contains time passed from downloading past file
----------------
- todo.Begin() - starting download
- todo.Pause() - Download last file and then stop
- todo.Resume() - Resume download

## (NEW) Event

todo.OnDownload - downloadingHandler (custom) event type, calls when file was download

## Installing
Download AdvRequest File and put in your unity project

then by "using" include AdvancedDownload

and that all XD

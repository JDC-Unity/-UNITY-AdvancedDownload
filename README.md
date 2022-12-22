# -UNITY-AdvancedDownload
Download several files and directories with ASYNC method 
# How It is work?
here is a example of download some files and sorting by folders:

```javascript
using UnityEngine;
using AdvancedDownload;

public class example : MonoBehaviour
{
    void Start()
    {
        AdvRequest todo = new AdvRequest(new string[]{ "https://somethin.com/folder/folder2/file.txt", "https://somethin.com/folder/homework.amv" }, "https://somethin.com/");
        
        todo.Begin(); //begin download
    }
}
```
Result of this code will be:

- UrApplicationPath/folder/folder2/file.txt

- UrApplicationPath/folder/homework.amv

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
- todo.Progress - float variable, contains progress of current downloading from 0 to 100;
- todo.FileName - string variable, contains name of file downloanding now
- todo.TimePassed - string variable with "{0:00m}:{1:00s}.{2:00ms}" format, contains time passed from downloading past file

- todo.Begin() - starting download
## (BETA functions! MAY NOT WORK)
- todo.BETA_Pause() - Download last file and then stop
- todo.BETA_Resume() - Resume download

## Installing
Download AdvRequest File and put in your unity project

then by Using include AdvancedDownload

and that all XD

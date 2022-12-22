using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AdvancedDownload
{
    public class AdvRequest
    {
        internal float Progress = 0.0f;
        internal string[] _Links;
        internal bool _Downloading = false;
        internal string _path = Application.dataPath;
        internal string _root = null; //https://somethin.com/ - ROOT, then it cut by link https://somethin.com/folder/file.txt and result will be like this: 1- create folder 'folder' and put file.txt in this
        internal string FileName;
        internal string TimePassed;
        DownloadHandlerFile loader; 
        public AdvRequest() { }
        public AdvRequest(IEnumerable<string> urls, string root) // with root it will cut by /folder/file.som
        {
            List<string> converting = new List<string>();
            foreach (string i in urls) { converting.Add(i); }
            _Links = converting.ToArray();
            _root = root;
        }
        public AdvRequest(IEnumerable<string> urls) // just download all files in dataPath
        {
            List<string> converting = new List<string>();
            foreach (string i in urls) { converting.Add(i); }
            _Links = converting.ToArray();
        }
        async public void Begin()
        {
            _Downloading = true;
            UnityWebRequest r;
            UnityWebRequestAsyncOperation op;
            string[] foldersToDo;
            for (int index = 0; index != _Links.Length; index++)
            {
                check:
                if (_Downloading)
                {
                    if (!(_root == null))
                    {
                        foldersToDo = (_Links[index].Replace(_root, "")).Split('/');
                        if (foldersToDo.Length > 0) {
                            string pathtodo=_path;
                            for (int folderNum=0;folderNum != foldersToDo.Length-1;folderNum++)
                            {
                                pathtodo = pathtodo+"/"+foldersToDo[folderNum];
                                if (!Directory.Exists(pathtodo))
                                {
                                    Directory.CreateDirectory(pathtodo);
                                }
                            }
                            loader = new DownloadHandlerFile(pathtodo + "/" + Path.GetFileName(_Links[index])); loader.removeFileOnAbort = true;
                            r = new UnityWebRequest(_Links[index]); r.downloadHandler = loader; r.disposeDownloadHandlerOnDispose = true;
                            op = r.SendWebRequest();
                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();
                            while (!op.isDone)
                            {
                                
                                Progress = r.downloadProgress * 100;
                                if (op.isDone)
                                {
                                    break;
                                }
                                await Task.Delay(100);
                            }
                            stopWatch.Stop();
                            TimePassed = String.Format("{0:00m}:{1:00s}.{2:00ms}", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds,stopWatch.Elapsed.Milliseconds / 10);
                        }
                    }
                    else
                    {
                        loader = new DownloadHandlerFile(_path + "/" + Path.GetFileName(_Links[index])); loader.removeFileOnAbort = true;
                        r = new UnityWebRequest(_Links[index]); r.downloadHandler = loader; r.disposeDownloadHandlerOnDispose = true;
                        op = r.SendWebRequest();
                        FileName = Path.GetFileName(_Links[index]);
                        while (!op.isDone)
                        {

                            Progress = r.downloadProgress * 100;
                            if (op.isDone)
                            {
                                break;
                            }
                            await Task.Delay(100);
                        }
                    }
                }else{
                    await Task.Delay(500);
                    goto check;
                }
            }
            _Downloading = false;
        }
        
        public void BETA_Pause() // download last file and stop 
        {
            _Downloading = false;
        }
        public void BETA_Resume()
        {
            _Downloading = true;
        }
    }
}

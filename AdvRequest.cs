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
        //1.7
        public delegate void downloadingHandler();
        public event downloadingHandler OnDownload = null;
        internal float progress = 0.0f;
        internal string[] _links = { "" };
        internal bool _downloading = false;
        internal string _path = Application.dataPath;
        internal string _root = null; //https://somethin.com/ - ROOT, then it cut by link https://somethin.com/folder/file.txt and result will be like this: 1- create folder 'folder' and put file.txt in this
        internal string fileName;
        internal string timePassed;
        DownloadHandlerFile loader;
        public AdvRequest() { }
        public AdvRequest(string url, string path)
        {
            _links[0] = url;
            _path = path;
        }
        public AdvRequest(IEnumerable<string> urls, string root) // with root it will cut by /folder/file.som
        {
            List<string> converting = new List<string>();
            foreach (string i in urls) { converting.Add(i); }
            _links = converting.ToArray();
            _root = root;
        }
        public AdvRequest(IEnumerable<string> urls) // just download all files in dataPath
        {
            List<string> converting = new List<string>();
            foreach (string i in urls) { converting.Add(i); }
            _links = converting.ToArray();
        }
        private void InvokeEvent() { if (OnDownload != null) { OnDownload.Invoke(); } }
        async public void Begin()
        {
            _downloading = true;
            UnityWebRequest r;
            UnityWebRequestAsyncOperation op;
            string pathtodo = _path;
            string[] foldersToDo;
            for (int index = 0; index != _links.Length; index++)
            {
                while (!_downloading)
                {
                    await Task.Delay(500);
                }

                if (_root != null)
                {
                    foldersToDo = (_links[index].Replace(_root, "")).Split('/');
                    if (foldersToDo.Length > 0)
                    {
                        for (int folderNum = 0; folderNum != foldersToDo.Length - 1; folderNum++)
                        {
                            pathtodo = pathtodo + "/" + foldersToDo[folderNum];
                            if (!Directory.Exists(pathtodo))
                            {
                                Directory.CreateDirectory(pathtodo);
                            }
                        }
                    }
                }

                loader = new DownloadHandlerFile(pathtodo + "/" + Path.GetFileName(_links[index])); loader.removeFileOnAbort = true;
                r = new UnityWebRequest(_links[index]); r.downloadHandler = loader; r.disposeDownloadHandlerOnDispose = true;
                op = r.SendWebRequest();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                fileName = Path.GetFileName(_links[index]);
                while (!op.isDone)
                {
                    progress = r.downloadProgress * 100;
                    if (op.isDone)
                    {
                        break;
                    }
                    await Task.Delay(100);
                }
                stopWatch.Stop();
                timePassed = String.Format("{0:00m}:{1:00s}.{2:00ms}", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10);
                InvokeEvent();
            }
            _downloading = false;
        }

        public void Pause() // download last file and stop 
        {
            _downloading = false;
        }
        public void Resume()
        {
            _downloading = true;
        }
    }
}

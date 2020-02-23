using System;
using System.Collections.Generic;
using System.Text;
using Plugins.XAsset;
using System.IO;
using UnityEngine;

namespace DestroyViruses
{
    public class AssetsUpdate
    {
        public enum State
        {
            Init,
            Wait,
            Checking,
            Downloading,
            Completed,
            Error,
        }

        public State state;

        public Action completed;

        public Action<string, float> onProgress;

        public Action<string> onError;
        public int downloadIndex => _downloadIndex;
        public int downloadCount => _downloads.Count;
        public string downloadUrl => _downloads[_downloadIndex].url;
        public float progress => _progress;

        private Dictionary<string, string> _versions = new Dictionary<string, string>();
        private Dictionary<string, string> _serverVersions = new Dictionary<string, string>();
        private readonly List<Download> _downloads = new List<Download>();
        private int _downloadIndex;
        private float _progress;

        private string versionsTxt = "versions.txt";

        private void OnError(string e)
        {
            if (onError != null)
            {
                onError(e);
            }

            message = e;
            state = State.Error;
        }

        public string message { get; private set; } = "click Check to start.";

        void OnProgress(string arg1, float arg2)
        {
            _progress = (_downloadIndex + arg2) / _downloads.Count;
            message = string.Format("{0:F0}%:{1}({2}/{3})", arg2 * 100, arg1, _downloadIndex, _downloads.Count);
        }

        void Clear()
        {
            var dir = Path.GetDirectoryName(Utility.updatePath);
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            _downloads.Clear();
            _downloadIndex = 0;
            _versions.Clear();
            _serverVersions.Clear();
            message = "click Check to start.";
            state = State.Wait;

            Versions.Clear();

            var path = Utility.updatePath + Versions.versionFile;
            if (File.Exists(path))
                File.Delete(path);
        }

        public void Init()
        {
            if (!Utility.assetBundleMode)
            {
                state = State.Completed;
                return;
            }
            state = State.Init;
            Versions.Load();
            Assets.Initialize(() =>
            {
                state = State.Wait;
            }, OnError);
            onProgress += OnProgress;
        }

        public void Check()
        {
            var path = Utility.GetRelativePath4Update(versionsTxt);
            if (!File.Exists(path))
            {
                var asset = Assets.LoadAsync(Utility.GetWebUrlFromDataPath(versionsTxt), typeof(TextAsset));
                asset.completed += delegate
                {
                    if (asset.error != null)
                    {
                        OnError(asset.error);
                        return;
                    }

                    var dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    File.WriteAllText(path, asset.text);
                    LoadVersions(asset.text);
                    asset.Release();
                };
            }
            else
            {
                LoadVersions(File.ReadAllText(path));
            }

            state = State.Checking;
        }

        public void Update()
        {
            if (state == State.Downloading)
            {
                if (_downloadIndex < _downloads.Count)
                {
                    var download = _downloads[_downloadIndex];
                    download.Update();
                    if (download.isDone)
                    {
                        _downloadIndex = _downloadIndex + 1;
                        if (_downloadIndex == _downloads.Count)
                        {
                            Complete();
                        }
                        else
                        {
                            _downloads[_downloadIndex].Start();
                        }
                    }
                    else
                    {
                        if (onProgress != null)
                        {
                            onProgress.Invoke(download.url, download.progress);
                        }
                    }
                }
            }
        }

        private void Complete()
        {
            Versions.Save();

            if (_downloads.Count > 0)
            {
                for (int i = 0; i < _downloads.Count; i++)
                {
                    var item = _downloads[i];
                    if (!item.isDone)
                    {
                        break;
                    }
                    else
                    {
                        if (_serverVersions.ContainsKey(item.path))
                        {
                            _versions[item.path] = _serverVersions[item.path];
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (var item in _versions)
                {
                    sb.AppendLine(string.Format("{0}:{1}", item.Key, item.Value));
                }

                var path = Utility.GetRelativePath4Update(versionsTxt);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                File.WriteAllText(path, sb.ToString());
                Assets.Initialize(()=>
                {
                    if (completed != null)
                    {
                        completed();
                    }
                }, OnError);
                state = State.Completed;

                message = string.Format("{0} files has update.", _downloads.Count);
                return;
            }

            if (completed != null)
            {
                completed();
            }

            message = "nothing to update.";
            state = State.Completed;
        }

        private void Download()
        {
            _downloadIndex = 0;
            _downloads[_downloadIndex].Start();
            state = State.Downloading;
        }

        private void LoadVersions(string text)
        {
            LoadText2Map(text, ref _versions);
            var asset = Assets.LoadAsync(Utility.GetDownloadURL(versionsTxt), typeof(TextAsset));
            asset.completed += delegate
            {
                if (asset.error != null)
                {
                    OnError(asset.error);
                    return;
                }

                LoadText2Map(asset.text, ref _serverVersions);
                foreach (var item in _serverVersions)
                {
                    string ver;
                    if (!_versions.TryGetValue(item.Key, out ver) || !ver.Equals(item.Value))
                    {
                        var downloader = new Download();
                        downloader.url = Utility.GetDownloadURL(item.Key);
                        downloader.path = item.Key;
                        downloader.version = item.Value;
                        downloader.savePath = Utility.GetRelativePath4Update(item.Key);
                        _downloads.Add(downloader);
                    }
                }

                if (_downloads.Count == 0)
                {
                    Complete();
                }
                else
                {
                    var downloader = new Download();
                    downloader.url = Utility.GetDownloadURL(Utility.GetPlatform());
                    downloader.path = Utility.GetPlatform();
                    downloader.savePath = Utility.GetRelativePath4Update(Utility.GetPlatform());
                    _downloads.Add(downloader);
                    Download();
                }
            };
        }

        private static void LoadText2Map(string text, ref Dictionary<string, string> map)
        {
            map.Clear();
            using (var reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(':');
                    if (fields.Length > 1)
                    {
                        map.Add(fields[0], fields[1]);
                    }
                }
            }
        }
    }
}

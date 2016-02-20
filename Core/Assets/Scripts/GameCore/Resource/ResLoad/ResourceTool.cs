using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FuckGame.Res
{
    public class ResourceTool
    {
        public delegate void FinishReadNotify(bool isSuccess, WWW resource);

        public delegate void FinishDownloadNotify(string path);

        public struct DownloadCommand
        {
            public WWW resource;

            public bool isCache;

            public ResourceTool.FinishReadNotify notify;
        }

        public class DownloadFileToLocal
        {
            public ResourceTool.FinishDownloadNotify mNotify;

            public string mPath;

            public DownloadFileToLocal(string path, ResourceTool.FinishDownloadNotify notify)
            {
                this.mPath = path;
                this.mNotify = notify;
            }

            public void FinishDownLoad(bool isSuccess, WWW resource)
            {
                if (isSuccess)
                {
                    ResourceTool.WriteLocalFile(this.mPath, resource.bytes);
                    this.mNotify(this.mPath);
                }
            }
        }

        protected static List<ResourceTool.DownloadCommand> msDownloadCommands = new List<ResourceTool.DownloadCommand>();

        protected static Dictionary<int, WWW> msCache = new Dictionary<int, WWW>();

        public static string GetDefaultDownloadPath()
        {
            return Application.persistentDataPath + "/";
        }

        public static void ReadLocalFile(string path, ResourceTool.FinishReadNotify notify)
        {
            ResourceTool.DownloadCommand item = default(ResourceTool.DownloadCommand);
            string str = "file:///";
            item.resource = new WWW(str + path);
            Debug.Log("url       =" + item.resource.url);
            item.notify = notify;
            ResourceTool.msDownloadCommands.Add(item);
        }

        public static WWW ReadNetFile(string path, bool isCache, ResourceTool.FinishReadNotify notify)
        {
            WWW wWW = null;
            HashName hashName = new HashName(path);
            if (ResourceTool.msCache.TryGetValue(hashName.GetId(), out wWW))
            {
                notify(true, wWW);
                return wWW;
            }
            ResourceTool.DownloadCommand item = default(ResourceTool.DownloadCommand);
            item.resource = new WWW(path);
            item.isCache = isCache;
            item.notify = notify;
            ResourceTool.msDownloadCommands.Add(item);
            return item.resource;
        }

        public static void WriteLocalFile(string path, byte[] bytes)
        {
            if (ResourceTool.IsLocalFileExists(path))
            {
                File.Delete(path);
                Debug.Log("detele file");
            }
            int num = path.LastIndexOf('/');
            if (num < 0)
            {
                num = 0;
            }
            string path2 = path.Substring(0, num);
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }
            if (File.Exists(path))
            {
                return;
            }
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();
        }

        public static bool IsLocalFileExists(string path)
        {
            return File.Exists(path);
        }

        public static void DeleteFolder(string dir)
        {
            string[] fileSystemEntries = Directory.GetFileSystemEntries(dir);
            for (int i = 0; i < fileSystemEntries.Length; i++)
            {
                string text = fileSystemEntries[i];
                if (File.Exists(text))
                {
                    FileInfo fileInfo = new FileInfo(text);
                    if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    {
                        fileInfo.Attributes = FileAttributes.Normal;
                    }
                    File.Delete(text);
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(text);
                    if (directoryInfo.GetFiles().Length != 0)
                    {
                        ResourceTool.DeleteFolder(directoryInfo.FullName);
                    }
                    Directory.Delete(text, true);
                }
            }
        }

        public static void DownloadFile(string fromPath, string toPath, ResourceTool.FinishDownloadNotify notify)
        {
            ResourceTool.DownloadFileToLocal @object = new ResourceTool.DownloadFileToLocal(toPath, notify);
            ResourceTool.ReadNetFile(fromPath, false, new ResourceTool.FinishReadNotify(@object.FinishDownLoad));
        }

        public static void Tick()
        {
            foreach (ResourceTool.DownloadCommand current in ResourceTool.msDownloadCommands)
            {
                if (current.resource.isDone)
                {
                    Debug.Log(current.resource.url + "resource finished =" + current.resource.error);
                    ResourceTool.msDownloadCommands.Remove(current);
                    if (current.resource.error == null)
                    {
                        HashName hashName = new HashName(current.resource.url);
                        if (current.isCache && !ResourceTool.msCache.ContainsKey(hashName.GetId()))
                        {
                            ResourceTool.msCache.Add(hashName.GetId(), current.resource);
                        }
                        Debug.Log(string.Concat(new object[]
						{
							"finish read ",
							current.resource.url,
							" ",
							Time.time
						}));
                        current.notify(true, current.resource);
                    }
                    else
                    {
                        Debug.LogWarning(current.resource.error);
                        current.notify(false, current.resource);
                    }
                    break;
                }
            }
        }
    }
}

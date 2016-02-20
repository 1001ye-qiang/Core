
using System;
using System.Collections.Generic;
using System.Text;

namespace FuckGame.Res
{
    public interface IPackageAdapt
    {
        string GetBaseURL(HashName assetName);

        string GetLocalURL(HashName assetName);

        string GetOuterURL(HashName assetName);

        bool IsLocalVersion(HashName assetName);

        bool IsOuterVersion(HashName assetName);

        void WriteToLocal(HashName assetName, byte[] bytes);

        void SynchroVersion(HashName assetName);

        void RefreshLocalVersion();
    }

    public class BasePackageAdapt : IPackageAdapt
    {
        protected string mOuterURL;

        protected string mLocalURL;

        protected string mLocalPath;

        protected string mBaseURL;

        protected Dictionary<int, string> mOuterVersion;

        protected Dictionary<int, string> mLocalVersion;

        protected Dictionary<int, string> mBaseVersion;

        public BasePackageAdapt(string outerURL, string localURL, string localPath, string baseURL, Dictionary<int, string> outerVersion, Dictionary<int, string> localVersion, Dictionary<int, string> baseVersion)
        {
            this.mOuterURL = outerURL;
            this.mLocalURL = localURL;
            this.mLocalPath = localPath;
            this.mBaseURL = baseURL;
            this.mOuterVersion = outerVersion;
            this.mLocalVersion = localVersion;
            this.mBaseVersion = baseVersion;
        }

        private static string a(HashName A_0)
        {
            string[] array = A_0.GetName().Split(new char[]
			{
				'@'
			});
            string str = array[array.Length - 1];
            for (int i = array.Length - 2; i >= 0; i--)
            {
                str = str + "/" + array[i];
            }
            return str + ".assetbundle";
        }

        public string GetBaseURL(HashName assetName)
        {
            return this.mBaseURL + BasePackageAdapt.a(assetName);
        }

        public string GetLocalURL(HashName assetName)
        {
            return this.mLocalURL + BasePackageAdapt.a(assetName);
        }

        public string GetOuterURL(HashName assetName)
        {
            return this.mOuterURL + BasePackageAdapt.a(assetName);
        }

        public bool IsLocalVersion(HashName assetName)
        {
            return this.mLocalVersion != null && (!this.mBaseVersion.ContainsKey(assetName.GetId()) || this.mLocalVersion[assetName.GetId()] != this.mBaseVersion[assetName.GetId()]);
        }

        public bool IsOuterVersion(HashName assetName)
        {
            return this.mOuterVersion != null && (!this.mLocalVersion.ContainsKey(assetName.GetId()) || this.mOuterVersion[assetName.GetId()] != this.mLocalVersion[assetName.GetId()]);
        }

        public void WriteToLocal(HashName assetName, byte[] bytes)
        {
            string path = this.mLocalPath + BasePackageAdapt.a(assetName);
            ResourceTool.WriteLocalFile(path, bytes);
        }

        public void SynchroVersion(HashName assetName)
        {
            if (this.mLocalVersion.ContainsKey(assetName.GetId()))
            {
                this.mLocalVersion[assetName.GetId()] = this.mOuterVersion[assetName.GetId()];
                return;
            }
            this.mLocalVersion.Add(assetName.GetId(), this.mOuterVersion[assetName.GetId()]);
        }

        public void RefreshLocalVersion()
        {
            string text = "";
            foreach (KeyValuePair<int, string> current in this.mLocalVersion)
            {
                if (text.Length > 0)
                {
                    text += "\r\n";
                }
                text = text + HashName.ToName(current.Key) + ":" + current.Value;
            }
            string path = this.mLocalPath + "version.txt";
            ResourceTool.WriteLocalFile(path, Encoding.UTF8.GetBytes(text));
        }
    }
}

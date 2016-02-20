using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace FuckGame.Res
{
    public class VersionManager
    {
        public delegate void OnDownload(bool isSuccess, Dictionary<int, string> outerVersion, Dictionary<int, string> localVersion, Dictionary<int, string> baseVersion);

        public class VersionLink
        {
            public WWW outerVersion;

            public WWW localVersion;

            public WWW baseVersion;
        }

        [CompilerGenerated]
        private sealed class aClass
        {
            public VersionManager.VersionLink a;

            public string b;

            public string c;

            public VersionManager.OnDownload d;

            public void e(bool A_0, WWW A_1)
            {
                ResourceTool.FinishReadNotify finishReadNotify = null;
                if (A_0)
                {
                    VersionManager.VersionLink arg_32_0 = this.a;
                    string arg_2D_0 = this.b + "version.txt";
                    bool arg_2D_1 = false;
                    if (finishReadNotify == null)
                    {
                        finishReadNotify = new ResourceTool.FinishReadNotify(this.f);
                    }
                    arg_32_0.localVersion = ResourceTool.ReadNetFile(arg_2D_0, arg_2D_1, finishReadNotify);
                    return;
                }
                this.d(false, null, null, null);
            }

            public void f(bool A_0, WWW A_1)
            {
                if (!A_0)
                {
                    this.a.localVersion = null;
                }
                this.a.baseVersion = ResourceTool.ReadNetFile(this.c + "version.txt", false, new ResourceTool.FinishReadNotify(this.g));
            }

            public void g(bool A_0, WWW A_1)
            {
                if (A_0)
                {
                    string @string = Encoding.UTF8.GetString(this.a.outerVersion.bytes);
                    byte[] bytes = (this.a.localVersion == null) ? this.a.baseVersion.bytes : this.a.localVersion.bytes;
                    string string2 = Encoding.UTF8.GetString(bytes);
                    string string3 = Encoding.UTF8.GetString(this.a.baseVersion.bytes);
                    Debug.Log("out:" + @string);
                    Debug.Log("local:" + string2);
                    Debug.Log("base:" + string3);
                    this.d(true, VersionManager.a(@string), VersionManager.a(string2), VersionManager.a(string3));
                }
                else
                {
                    this.d(false, null, null, null);
                }
                VersionManager.a(this.a.outerVersion);
                VersionManager.a(this.a.localVersion);
                VersionManager.a(this.a.baseVersion);
            }
        }

        public static VersionManager.VersionLink DownloadVersion(string outerVersionURL, string resourceLocalURL, string resourceBaseURL, VersionManager.OnDownload callback)
        {
            VersionManager.aClass a = new VersionManager.aClass();
            a.b = resourceLocalURL;
            a.c = resourceBaseURL;
            a.d = callback;
            a.a = new VersionManager.VersionLink();
            a.a.outerVersion = ResourceTool.ReadNetFile(outerVersionURL, false, new ResourceTool.FinishReadNotify(a.e));
            return a.a;
        }

        private static Dictionary<int, string> a(string A_0)
        {
            StringReader stringReader = new StringReader(A_0);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (string text = stringReader.ReadLine(); text != null; text = stringReader.ReadLine())
            {
                string name = text.Split(new char[]
				{
					':'
				})[0];
                string value = text.Split(new char[]
				{
					':'
				})[1];
                dictionary.Add(new HashName(name).GetId(), value);
            }
            return dictionary;
        }

        private static void a(WWW A_0)
        {
            if (A_0 != null && A_0.assetBundle != null)
            {
                A_0.assetBundle.Unload(true);
            }
        }
    }
}

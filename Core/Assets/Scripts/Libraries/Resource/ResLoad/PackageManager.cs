using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace FuckGame.Res
{
    public class PackageManager
    {
        public delegate void OnSynchroFinish(bool isSuccess);

        protected class AssetReferenceBoxShell
        {
            public delegate void OnShellTick();

            public delegate void OnShellRelease();

            protected object box;

            protected PackageManager.AssetReferenceBoxShell.OnShellRelease OnRelease;

            protected PackageManager.AssetReferenceBoxShell.OnShellTick OnTick;

            public AssetReferenceBoxShell(object box, PackageManager.AssetReferenceBoxShell.OnShellTick onTick, PackageManager.AssetReferenceBoxShell.OnShellRelease onRelease)
            {
                this.box = box;
                this.OnRelease = onRelease;
                this.OnTick = onTick;
            }

            public void Tick()
            {
                if (this.OnTick != null)
                {
                    this.OnTick();
                }
            }

            public void Release()
            {
                if (this.OnRelease != null)
                {
                    this.OnRelease();
                }
            }

            public AssetReferenceBox<T> GetBox<T>() where T : Object
            {
                return (AssetReferenceBox<T>)this.box;
            }
        }

        [CompilerGenerated]
        private sealed class bClass
        {
            public List<IAssetPackage> a;

            public GameObject b;

            public PackageManager.OnSynchroFinish c;

            public void d()
            {
                if (this.a.Count == 0)
                {
                    this.c(true);
                    Object.Destroy(this.b);
                    return;
                }
                foreach (IAssetPackage current in this.a)
                {
                    if (current.IsFail())
                    {
                        Object.Destroy(this.b);
                        this.c(false);
                        break;
                    }
                    if (current.IsLoaded())
                    {
                        current.WriteToLocal();
                        current.Release();
                        this.a.Remove(current);
                        break;
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class d<T> where T : Object
        {
            public AssetReferenceBox<T>.OnLoadAsset a;

            public void b(T[] A_0)
            {
                if (this.a != null)
                {
                    this.a(A_0[0]);
                }
            }
        }

        [CompilerGenerated]
        private sealed class e<T> where T : Object
        {
            public PackageManager a;

            public AssetReferenceBox<T>.OnLoadAssets b;
        }

        [CompilerGenerated]
        private sealed class cClass<T> where T : Object
        {
            public PackageManager.e<T> a;

            public AssetReferenceBox<T> b;

            public void c(T[] A_0)
            {
                if (this.a.b != null)
                {
                    this.a.b(A_0);
                }
                this.a.a.OnTick -= new System.Action(this.b.Tick);
                this.b.Release();
            }
        }

        [CompilerGenerated]
        private sealed class aClass<T> where T : Object
        {
            public AssetReferenceBox<T>.OnLoadAsset a;

            public void b(T[] A_0)
            {
                if (this.a != null)
                {
                    this.a(A_0[0]);
                }
            }
        }

        [CompilerGenerated]
        private sealed class f<T> where T : Object
        {
            public AssetReferenceBox<T> a;

            public ResAttach b;

            public AssetReferenceBox<T>.OnLoadAssets c;

            public void d(T[] A_0)
            {
                if (this.c != null)
                {
                    this.c(A_0);
                }
                ResAttach expr_1A = this.b;
                expr_1A.onResUpdate = (ResAttach.OnResUpdate)System.Delegate.Remove(expr_1A.onResUpdate, new ResAttach.OnResUpdate(this.a.Tick));
            }
        }

        protected static PackageManager msInstance;

        private bool a;

        protected IPackageAdapt packageAtapt;

        private Queue<PackageManager.AssetReferenceBoxShell> b = new Queue<PackageManager.AssetReferenceBoxShell>();

        private System.Action c;

        protected Dictionary<string, Dictionary<System.Type, PackageManager.AssetReferenceBoxShell>> assetCache = new Dictionary<string, Dictionary<System.Type, PackageManager.AssetReferenceBoxShell>>();

        protected Dictionary<string, Object> bindAssets = new Dictionary<string, Object>();

        protected event System.Action OnTick
        {
            add
            {
                System.Action action = this.c;
                System.Action action2;
                do
                {
                    action2 = action;
                    System.Action value2 = (System.Action)System.Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange<System.Action>(ref this.c, value2, action2);
                }
                while (action != action2);
            }
            remove
            {
                System.Action action = this.c;
                System.Action action2;
                do
                {
                    action2 = action;
                    System.Action value2 = (System.Action)System.Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange<System.Action>(ref this.c, value2, action2);
                }
                while (action != action2);
            }
        }

        public static PackageManager Instance
        {
            get
            {
                if (PackageManager.msInstance == null)
                {
                    PackageManager.msInstance = new PackageManager();
                }
                return PackageManager.msInstance;
            }
        }

        protected void PluginPackageAdapt(IPackageAdapt pkgAdapt)
        {
            this.packageAtapt = pkgAdapt;
        }

        public IPackageAdapt GetPackageAdapt()
        {
            return this.packageAtapt;
        }

        public VersionManager.VersionLink DownloadVersion(string outerVersionURL, string resourceLocalURL, string resourceBaseURL, VersionManager.OnDownload callback)
        {
            return VersionManager.DownloadVersion(outerVersionURL, resourceLocalURL, resourceBaseURL, callback);
        }

        public void Initialize(string outerURL, string resourceLocalURL, string resourceLocalPath, string resourceBaseURL, Dictionary<int, string> outerVersion, Dictionary<int, string> localVersion, Dictionary<int, string> baseVersion)
        {
            this.PluginPackageAdapt(new BasePackageAdapt(outerURL, resourceLocalURL, resourceLocalPath, resourceBaseURL, outerVersion, localVersion, baseVersion));
        }

        public void Initialize(string resourceLocalURL, string resourceLocalPath, string resourceBaseURL)
        {
            this.PluginPackageAdapt(new BasePackageAdapt("", resourceLocalURL, resourceLocalPath, resourceBaseURL, null, null, null));
        }

        public IAssetPackage[] SynchroPackages(Dictionary<int, string> outVersion, Dictionary<int, string> localVersion, PackageManager.OnSynchroFinish callback)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<int, string> current in outVersion)
            {
                if (!localVersion.ContainsKey(current.Key) || current.Value != localVersion[current.Key])
                {
                    list.Add(HashName.ToName(current.Key));
                }
            }
            return this.SynchroPackages(list.ToArray(), callback);
        }

        public IAssetPackage[] SynchroPackages(string[] assetName, PackageManager.OnSynchroFinish callback)
        {
            PackageManager.bClass b = new PackageManager.bClass();
            b.c = callback;
            b.a = new List<IAssetPackage>();
            for (int i = 0; i < assetName.Length; i++)
            {
                string name = assetName[i];
                b.a.Add(GamePackageCache.Instance.GetAssetPackage(new HashName(name)));
            }
            IAssetPackage[] result = b.a.ToArray();
            b.b = new GameObject("SynchroPackages");
            b.b.AddComponent<ResAttach>().onResUpdate = new ResAttach.OnResUpdate(b.d);
            return result;
        }

        public void BindAsset(string assetName, Object asset)
        {
            if (!this.bindAssets.ContainsKey(assetName))
            {
                this.bindAssets.Add(assetName, asset);
            }
        }

        public void UnbindAsset(string assetName = null)
        {
            if (assetName == null)
            {
                this.bindAssets.Clear();
                return;
            }
            this.bindAssets.Remove(assetName);
        }

        public void Tick(float deltaTime)
        {
            ResourceTool.Tick();
            GamePackageCache.Instance.Tick(deltaTime);
            using (Dictionary<string, Dictionary<System.Type, PackageManager.AssetReferenceBoxShell>>.Enumerator enumerator = this.assetCache.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, Dictionary<System.Type, PackageManager.AssetReferenceBoxShell>> current = enumerator.Current;
                    foreach (KeyValuePair<System.Type, PackageManager.AssetReferenceBoxShell> current2 in current.Value)
                    {
                        this.b.Enqueue(current2.Value);
                    }
                }
                goto IL_91;
            }
        IL_81:
            this.b.Dequeue().Tick();
        IL_91:
            if (this.b.Count <= 0)
            {
                if (this.c != null)
                {
                    this.c();
                }
                return;
            }
            goto IL_81;
        }

        public void CacheAsset<T>(string assetName, string cacheName) where T : Object
        {
            this.CacheAssets<T>(new string[]
			{
				assetName
			}, cacheName);
        }

        public void CacheAssets<T>(string[] assetNames, string cacheName) where T : Object
        {
            this.aFun<T>(cacheName).CacheAsset(assetNames);
        }

        /// <summary>
        /// Must be initialized before call those fun;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="OnLoadAsset"></param>
        /// <param name="cacheName"></param>
        public void LoadAsset<T>(string assetName, AssetReferenceBox<T>.OnLoadAsset OnLoadAsset, string cacheName = null) where T : Object
        {
            PackageManager.d<T> d = new PackageManager.d<T>();
            d.a = OnLoadAsset;
            Object @object = null;
            if (this.bindAssets.TryGetValue(assetName, out @object))
            {
                d.a((T)((object)@object));
                return;
            }
            this.LoadAssets<T>(new string[]
			{
				assetName
			}, new AssetReferenceBox<T>.OnLoadAssets(d.b), cacheName);
        }

        public void LoadAssets<T>(string[] assetNames, AssetReferenceBox<T>.OnLoadAssets OnLoadAssets, string cacheName = null) where T : Object
        {
            PackageManager.e<T> e = new PackageManager.e<T>();
            e.b = OnLoadAssets;
            e.a = this;
            if (cacheName == null)
            {try{
                PackageManager.cClass<T> c = new PackageManager.cClass<T>();
                c.a = e;
                c.b = new AssetReferenceBox<T>(GamePackageCache.Instance, false);
                this.OnTick += new System.Action(c.b.Tick);
                c.b.LoadAsset(assetNames, new AssetReferenceBox<T>.OnLoadAssets(c.c));
            }
                catch(System.Exception ex){
                    Debug.LogError(ex.ToString());
                }
                return;
            }
            this.aFun<T>(cacheName).LoadAsset(assetNames, e.b);
        }

        private AssetReferenceBox<T> aFun<T>(string A_0) where T : Object
        {
            Dictionary<System.Type, PackageManager.AssetReferenceBoxShell> dictionary = null;
            System.Type typeFromHandle = typeof(T);
            AssetReferenceBox<T> assetReferenceBox;
            if (this.assetCache.TryGetValue(A_0, out dictionary))
            {
                PackageManager.AssetReferenceBoxShell assetReferenceBoxShell = null;
                if (dictionary.TryGetValue(typeFromHandle, out assetReferenceBoxShell))
                {
                    assetReferenceBox = assetReferenceBoxShell.GetBox<T>();
                }
                else
                {
                    assetReferenceBox = new AssetReferenceBox<T>(GamePackageCache.Instance, false);
                    dictionary.Add(typeFromHandle, new PackageManager.AssetReferenceBoxShell(assetReferenceBox, new PackageManager.AssetReferenceBoxShell.OnShellTick(assetReferenceBox.Tick), new PackageManager.AssetReferenceBoxShell.OnShellRelease(assetReferenceBox.Release)));
                }
            }
            else
            {
                dictionary = new Dictionary<System.Type, PackageManager.AssetReferenceBoxShell>();
                assetReferenceBox = new AssetReferenceBox<T>(GamePackageCache.Instance, false);
                dictionary.Add(typeFromHandle, new PackageManager.AssetReferenceBoxShell(assetReferenceBox, new PackageManager.AssetReferenceBoxShell.OnShellTick(assetReferenceBox.Tick), new PackageManager.AssetReferenceBoxShell.OnShellRelease(assetReferenceBox.Release)));
                this.assetCache.Add(A_0, dictionary);
            }
            return assetReferenceBox;
        }

        public void LoadAsset<T>(string assetName, AssetReferenceBox<T>.OnLoadAsset OnLoadAsset, GameObject cacheGameobject) where T : Object
        {
            PackageManager.aClass<T> a = new PackageManager.aClass<T>();
            a.a = OnLoadAsset;
            Debug.Log(assetName);
            if (this.bindAssets.ContainsKey(assetName))
            {
                a.a((T)((object)this.bindAssets[assetName]));
                return;
            }
            this.LoadAssets<T>(new string[]
			{
				assetName
			}, new AssetReferenceBox<T>.OnLoadAssets(a.b), cacheGameobject);
        }

        public void LoadAssets<T>(string[] assetNames, AssetReferenceBox<T>.OnLoadAssets OnLoadAssets, GameObject cacheGameobject) where T : Object
        {
            PackageManager.f<T> f = new PackageManager.f<T>();
            f.c = OnLoadAssets;
            f.a = new AssetReferenceBox<T>(GamePackageCache.Instance, false);
            f.b = cacheGameobject.GetComponent<ResAttach>();
            if (f.b == null)
            {
                f.b = cacheGameobject.AddComponent<ResAttach>();
            }
            ResAttach expr_4A = f.b;
            expr_4A.onResUpdate = (ResAttach.OnResUpdate)System.Delegate.Combine(expr_4A.onResUpdate, new ResAttach.OnResUpdate(f.a.Tick));
            ResAttach expr_76 = f.b;
            expr_76.onResDestroy = (ResAttach.OnResDestroy)System.Delegate.Combine(expr_76.onResDestroy, new ResAttach.OnResDestroy(f.a.Release));
            f.a.LoadAsset(assetNames, new AssetReferenceBox<T>.OnLoadAssets(f.d));
        }

        public void Release(string cacheName = null)
        {
            if (cacheName == null)
            {
                foreach (KeyValuePair<string, Dictionary<System.Type, PackageManager.AssetReferenceBoxShell>> current in this.assetCache)
                {
                    foreach (KeyValuePair<System.Type, PackageManager.AssetReferenceBoxShell> current2 in current.Value)
                    {
                        current2.Value.Release();
                    }
                }
                this.assetCache.Clear();
                return;
            }
            Dictionary<System.Type, PackageManager.AssetReferenceBoxShell> dictionary = null;
            if (this.assetCache.TryGetValue(cacheName, out dictionary))
            {
                foreach (KeyValuePair<System.Type, PackageManager.AssetReferenceBoxShell> current3 in dictionary)
                {
                    current3.Value.Release();
                }
                this.assetCache.Remove(cacheName);
            }
        }

        public void GC(bool isForce = false)
        {
            GamePackageCache.Instance.GC(isForce);
        }
    }
}

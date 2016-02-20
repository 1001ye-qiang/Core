
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FuckGame.Res
{
    public class AssetReferenceBox<T> where T : Object
    {
        public delegate void OnLoadAssets(T[] asset);

        public delegate void OnLoadAsset(T asset);

        protected class AssetReference
        {
            private bool bUsed;

            protected string mFileName;

            protected HashName mAssetName;

            protected IAssetPackageCache mPackageCache;

            private bool b;

            protected AssetReferenceBox<T>.AssetReference mPreAssetReference;

            protected bool isPreAssetLoad
            {
                get
                {
                    return this.mPreAssetReference == null || (this.mPreAssetReference.bUsed && this.mPreAssetReference.isPreAssetLoad && this.mPreAssetReference.GetAssetPackage().IsLoaded());
                }
            }

            public AssetReference(IAssetPackageCache packageCache, HashName assetName, string fileName = null, bool isSynchro = false, AssetReferenceBox<T>.AssetReference preAssetReference = null)
            {
                this.mPackageCache = packageCache;
                this.mFileName = fileName;
                this.mAssetName = assetName;
                this.b = isSynchro;
                this.mPreAssetReference = preAssetReference;
                if (this.isPreAssetLoad && !this.bUsed)
                {
                    this.a();
                }
            }

            public T TryToGetAsset<T>() where T : Object
            {
                if (this.isPreAssetLoad)
                {
                    if (!this.bUsed)
                    {
                        this.a();
                    }
                    return this.GetAssetPackage().GetAsset<T>(this.mFileName, this.b);
                }
                return default(T);
            }

            protected IAssetPackage GetAssetPackage()
            {
                return this.mPackageCache.GetAssetPackage(this.mAssetName);
            }

            public bool IsFail()
            {
                if (this.mPreAssetReference != null)
                {
                    if (this.mPreAssetReference.IsFail())
                    {
                        return true;
                    }
                    if (!this.mPreAssetReference.bUsed || !this.mPreAssetReference.GetAssetPackage().IsLoaded())
                    {
                        return false;
                    }
                }
                if (!this.bUsed)
                {
                    this.a();
                }
                IAssetPackage assetPackage = this.mPackageCache.GetAssetPackage(this.mAssetName);
                if (!assetPackage.IsLoaded())
                {
                    return this.mPackageCache.GetAssetPackage(this.mAssetName).IsFail();
                }
                if (this.mFileName == null)
                {
                    return !assetPackage.Contains(this.mAssetName.GetName().Split(new char[]
					{
						'@'
					})[0]);
                }
                return !assetPackage.Contains(this.mFileName);
            }

            private void a()
            {
                Debug.Log("init " + this.mAssetName.GetName());
                IAssetPackage assetPackage = this.GetAssetPackage();
                if (assetPackage != null)
                {
                    assetPackage.AddUser();
                }
                this.bUsed = true;
            }

            public void Release()
            {
                if (this.bUsed && this.mPackageCache.HasPackage(this.mAssetName))
                {
                    this.GetAssetPackage().ReduceUser();
                }
                if (this.mPreAssetReference != null)
                {
                    this.mPreAssetReference.Release();
                }
                this.bUsed = false;
            }
        }

        protected class AssetSetupDes
        {
            protected AssetReferenceBox<T>.OnLoadAssets OnSetup;

            protected AssetReferenceBox<T>.AssetReference[] assetRefs;

            public AssetSetupDes(AssetReferenceBox<T>.AssetReference[] refs, AssetReferenceBox<T>.OnLoadAssets onSetup)
            {
                this.assetRefs = refs;
                this.OnSetup = onSetup;
            }

            public void Release()
            {
                AssetReferenceBox<T>.AssetReference[] array = this.assetRefs;
                for (int i = 0; i < array.Length; i++)
                {
                    AssetReferenceBox<T>.AssetReference assetReference = array[i];
                    assetReference.Release();
                }
            }

            public T[] TryToGetAssets()
            {
                int num = 0;
                AssetReferenceBox<T>.AssetReference[] array = this.assetRefs;
                for (int i = 0; i < array.Length; i++)
                {
                    AssetReferenceBox<T>.AssetReference assetReference = array[i];
                    if (assetReference.IsFail())
                    {
                        num++;
                    }
                    else if (assetReference.TryToGetAsset<T>() != null)
                    {
                        num++;
                    }
                }
                if (num != this.assetRefs.Length)
                {
                    return null;
                }
                List<T> list = new List<T>();
                AssetReferenceBox<T>.AssetReference[] array2 = this.assetRefs;
                for (int j = 0; j < array2.Length; j++)
                {
                    AssetReferenceBox<T>.AssetReference assetReference2 = array2[j];
                    list.Add(assetReference2.TryToGetAsset<T>());
                }
                return list.ToArray();
            }

            public void NotifyFinishLoadAssets(T[] assets)
            {
                if (this.OnSetup != null)
                {
                    this.OnSetup(assets);
                }
            }
        }

        [CompilerGenerated]
        private sealed class a
        {
            public AssetReferenceBox<T>.OnLoadAsset cb;

            public void b(T[] A_0)
            {
                if (this.cb != null)
                {
                    this.cb(A_0[0]);
                }
            }
        }

        private Queue<AssetReferenceBox<T>.AssetSetupDes> queA = new Queue<AssetReferenceBox<T>.AssetSetupDes>();

        private Queue<AssetReferenceBox<T>.AssetSetupDes> b = new Queue<AssetReferenceBox<T>.AssetSetupDes>();

        protected bool mIsSynchro;

        protected IAssetPackageCache mPackageCache;

        public AssetReferenceBox(IAssetPackageCache packageCache, bool isSynchro = false)
        {
            this.mPackageCache = packageCache;
            this.mIsSynchro = isSynchro;
        }

        public void CacheAsset(string[] assetName)
        {
            this.b.Enqueue(new AssetReferenceBox<T>.AssetSetupDes(this.GetAssetRef(assetName), null));
        }

        public void LoadAsset(string[] assetName, AssetReferenceBox<T>.OnLoadAssets onLoad)
        {
            this.queA.Enqueue(new AssetReferenceBox<T>.AssetSetupDes(this.GetAssetRef(assetName), onLoad));
        }

        protected AssetReferenceBox<T>.AssetReference[] GetAssetRef(string[] assetName)
        {
            List<AssetReferenceBox<T>.AssetReference> list = new List<AssetReferenceBox<T>.AssetReference>();
            for (int i = 0; i < assetName.Length; i++)
            {
                string text = assetName[i];
                string[] array = text.Split(new char[]
				{
					':'
				});
                string fileName = null;
                string text2;
                if (array.Length == 2)
                {
                    fileName = array[0];
                    text2 = array[1];
                }
                else
                {
                    text2 = text;
                }
                AssetReferenceBox<T>.AssetReference preAssetReference = null;
                string[] array2 = text2.Split(new char[]
				{
					'@'
				});
                for (int j = array2.Length - 2; j >= 1; j--)
                {
                    string text3 = array2[j];
                    for (int k = j + 1; k < array2.Length; k++)
                    {
                        text3 = text3 + "@" + array2[k];
                    }
                    preAssetReference = new AssetReferenceBox<T>.AssetReference(this.mPackageCache, new HashName(text3), null, this.mIsSynchro, preAssetReference);
                }
                AssetReferenceBox<T>.AssetReference item = new AssetReferenceBox<T>.AssetReference(this.mPackageCache, new HashName(text2), fileName, this.mIsSynchro, preAssetReference);
                list.Add(item);
            }
            return list.ToArray();
        }

        public void LoadAsset(string assetName, AssetReferenceBox<T>.OnLoadAsset onLoad)
        {
            AssetReferenceBox<T>.a a = new AssetReferenceBox<T>.a();
            a.cb = onLoad;
            this.LoadAsset(new string[]
			{
				assetName
			}, new AssetReferenceBox<T>.OnLoadAssets(a.b));
        }

        public void Release()
        {
            while (this.queA.Count > 0)
            {
                this.queA.Dequeue().Release();
            }
            while (this.b.Count > 0)
            {
                this.b.Dequeue().Release();
            }
        }

        public void Tick()
        {
            if (this.queA.Count > 0)
            {
                T[] array = this.queA.Peek().TryToGetAssets();
                if (array != null)
                {
                    AssetReferenceBox<T>.AssetSetupDes assetSetupDes = this.queA.Dequeue();
                    assetSetupDes.NotifyFinishLoadAssets(array);
                    this.b.Enqueue(assetSetupDes);
                }
            }
        }
    }
}

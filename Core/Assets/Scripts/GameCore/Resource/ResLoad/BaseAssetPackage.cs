using System.Collections.Generic;
using UnityEngine;

namespace FuckGame.Res
{
    public interface IAssetPackage
    {
        int GetUserNum();
        void AddUser();
        void ReduceUser();

        bool IsLoaded();
        bool IsFail();
        bool Contains(string fileName);

        T GetAsset<T>(string fileName = null, bool isSynchro = false) where T : Object;

        void Release();
        bool IsTimeOut();
        void TimeOut();
        bool IsOuterVersion();

        bool IsLocalVersion();
        string GetBaseURL();
        string GetLocalURL();
        string GetOuterURL();

        void WriteToLocal();
        float GetBytesDownloaded();
        float GetProgress();
    }

    public class BaseAssetPackage : IAssetPackage
    {
        public HashName mAssetName;

        protected bool isLocal;

        protected bool isBase;

        protected bool isFail;

        protected AssetPackageCache mPackageCache;

        protected float mRemainTime;

        protected int mUserNum;

        protected WWW mAllAsset;

        protected Dictionary<string, AssetBundleRequest> mAssetBundleRequest = new Dictionary<string, AssetBundleRequest>();

        public BaseAssetPackage(HashName assetName, AssetPackageCache packageCache)
        {
            this.mAssetName = assetName;
            this.mPackageCache = packageCache;
            packageCache.cachePackage.Add(assetName.GetId(), this);
        }

        public int GetUserNum()
        {
            return this.mUserNum;
        }

        public void AddUser()
        {
            this.mUserNum++;
        }

        public void ReduceUser()
        {
            this.mUserNum--;
        }

        public virtual bool IsLoaded()
        {
            if (this.IsFail())
            {
                return false;
            }
            if (this.mAllAsset == null)
            {
                if (!this.mPackageCache.loadingPackage.ContainsKey(this.mAssetName.GetId()))
                {
                    if (this.mPackageCache.loadingPackage.Count >= this.mPackageCache.maxLoadingPkgNum)
                    {
                        return false;
                    }
                    this.mPackageCache.loadingPackage.Add(this.mAssetName.GetId(), this);
                }
                this.isLocal = !this.IsOuterVersion();
                if (this.isLocal)
                {
                    this.isBase = !this.IsLocalVersion();
                    if (this.isBase)
                    {
                        this.mAllAsset = new WWW(this.GetBaseURL());
                    }
                    else
                    {
                        this.mAllAsset = new WWW(this.GetLocalURL());
                    }
                }
                else
                {
                    this.mAllAsset = new WWW(this.GetOuterURL());
                }
                return false;
            }
            if (this.mAllAsset.isDone && this.mAllAsset.error != null)
            {
                Debug.LogWarning(this.mAllAsset.url + " " + this.mAllAsset.error);
                this.isFail = true;
                return false;
            }
            return this.mAllAsset.isDone && this.mAllAsset.error == null;
        }

        public bool IsFail()
        {
            return this.isFail;
        }

        public bool Contains(string fileName)
        {
            return this.mAllAsset.assetBundle.Contains(fileName);
        }

        public T GetAsset<T>(string fileName = null, bool isSynchro = false) where T : Object
        {
            if (!this.IsLoaded())
            {
                return default(T);
            }
            if (!this.isLocal)
            {
                this.isLocal = true;
                this.WriteToLocal();
            }
            if (fileName == null)
            {
                fileName = this.mAssetName.GetName().Split(new char[]
				{
					'@'
				})[0];
            }
            if (isSynchro)
            {
                return (T)((object)this.mAllAsset.assetBundle.LoadAsset(fileName, typeof(T)));
            }
            AssetBundleRequest assetBundleRequest = null;
            if (!this.mAssetBundleRequest.TryGetValue(fileName, out assetBundleRequest))
            {
                assetBundleRequest = this.a<T>(fileName);
                if (assetBundleRequest != null)
                {
                    this.mAssetBundleRequest.Add(fileName, assetBundleRequest);
                }
            }
            if (assetBundleRequest != null && assetBundleRequest.isDone)
            {
                return (T)((object)assetBundleRequest.asset);
            }
            return default(T);
        }

        public void Release()
        {
            Debug.LogError("Release " + this.GetBaseURL());
            this.mAssetBundleRequest.Clear();
            if (this.mAllAsset != null && this.mAllAsset.assetBundle != null)
            {
                this.mAllAsset.assetBundle.Unload(true);
            }
            this.mAllAsset = null;
            this.mPackageCache.loadingPackage.Remove(this.mAssetName.GetId());
            this.mPackageCache.cachePackage.Remove(this.mAssetName.GetId());
        }

        public bool IsTimeOut()
        {
            return this.mRemainTime < 0f && this.GetUserNum() <= 0;
        }

        public void TimeOut()
        {
            this.mRemainTime = -1f;
        }

        public void SetRemainTime(float remainTime)
        {
            this.mRemainTime = remainTime;
        }

        public void ReduceRemainTime(float deltaTime)
        {
            this.mRemainTime -= deltaTime;
        }

        public virtual bool IsOuterVersion()
        {
            return false;
        }

        public virtual bool IsLocalVersion()
        {
            return false;
        }

        public virtual string GetBaseURL()
        {
            return null;
        }

        public virtual string GetLocalURL()
        {
            return null;
        }

        public virtual string GetOuterURL()
        {
            return null;
        }

        public virtual void WriteToLocal()
        {
        }

        private AssetBundleRequest a<a>(string A_0) where a : Object
        {
            return this.mAllAsset.assetBundle.LoadAssetAsync(A_0, typeof(a));
        }

        public virtual float GetBytesDownloaded()
        {
            if (this.mAllAsset == null)
            {
                return 0f;
            }
            return (float)this.mAllAsset.bytesDownloaded;
        }

        public virtual float GetProgress()
        {
            if (this.mAllAsset == null)
            {
                return 0f;
            }
            return this.mAllAsset.progress;
        }
    }


    public class GameAssetPackage : BaseAssetPackage
    {
        public GameAssetPackage(HashName assetName, AssetPackageCache packageCache)
            : base(assetName, packageCache)
        {
        }

        public override bool IsLoaded()
        {
            return PackageManager.Instance.GetPackageAdapt() != null && base.IsLoaded();
        }

        public override string GetBaseURL()
        {
            return PackageManager.Instance.GetPackageAdapt().GetBaseURL(this.mAssetName);
        }

        public override string GetLocalURL()
        {
            return PackageManager.Instance.GetPackageAdapt().GetLocalURL(this.mAssetName);
        }

        public override string GetOuterURL()
        {
            return PackageManager.Instance.GetPackageAdapt().GetOuterURL(this.mAssetName);
        }

        public override bool IsLocalVersion()
        {
            return PackageManager.Instance.GetPackageAdapt().IsLocalVersion(this.mAssetName);
        }

        public override bool IsOuterVersion()
        {
            return PackageManager.Instance.GetPackageAdapt().IsOuterVersion(this.mAssetName);
        }

        public override void WriteToLocal()
        {
            if (!this.IsLoaded())
            {
                return;
            }
            Debug.Log("Save :" + this.mAssetName.GetName());
            PackageManager.Instance.GetPackageAdapt().WriteToLocal(this.mAssetName, this.mAllAsset.bytes);
            PackageManager.Instance.GetPackageAdapt().SynchroVersion(this.mAssetName);
            PackageManager.Instance.GetPackageAdapt().RefreshLocalVersion();
        }
    }
}

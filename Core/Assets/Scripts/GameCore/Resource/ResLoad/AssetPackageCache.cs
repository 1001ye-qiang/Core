using System.Collections.Generic;
using UnityEngine;

namespace FuckGame.Res
{
    public interface IAssetPackageCache
    {
        IAssetPackage GetAssetPackage(HashName assetName);

        bool HasPackage(HashName assetName);

        void Reflush(float deltaTime);

        void Release();
    }

    public class AssetPackageCache : IAssetPackageCache
    {
        private bool a = true;

        public float remainTime = 10f;

        public Dictionary<int, IAssetPackage> cachePackage = new Dictionary<int, IAssetPackage>();

        public int maxLoadingPkgNum = 10;

        public Dictionary<int, IAssetPackage> loadingPackage = new Dictionary<int, IAssetPackage>();

        public void IsAutoReleasePackage(bool isAutoRelease)
        {
            this.a = isAutoRelease;
        }

        public void ReleaseAllNoUsePackage()
        {
            Debug.Log("ReleaseAllNoUsePackage");
            foreach (KeyValuePair<int, IAssetPackage> current in this.cachePackage)
            {
                if (current.Value.GetUserNum() == 0)
                {
                    current.Value.TimeOut();
                }
            }
            this.Reflush(0f);
        }

        public void Tick(float deltaTime)
        {
            if (this.a)
            {
                this.Reflush(deltaTime);
            }
            foreach (KeyValuePair<int, IAssetPackage> current in this.loadingPackage)
            {
                if (current.Value.IsLoaded())
                {
                    this.loadingPackage.Remove(current.Key);
                    break;
                }
            }
        }

        public IAssetPackage GetAssetPackage(HashName packageName)
        {
            IAssetPackage result = null;
            if (this.cachePackage.TryGetValue(packageName.GetId(), out result))
            {
                return result;
            }
            result = this.CreatePackage(packageName);
            return result;
        }

        protected virtual BaseAssetPackage CreatePackage(HashName packageName)
        {
            return new BaseAssetPackage(packageName, this);
        }

        public bool HasPackage(HashName packageName)
        {
            return this.cachePackage.ContainsKey(packageName.GetId());
        }

        public void Reflush(float deltaTime)
        {
            foreach (KeyValuePair<int, IAssetPackage> current in this.cachePackage)
            {
                if (current.Value.GetUserNum() == 0)
                {
                    ((BaseAssetPackage)current.Value).ReduceRemainTime(deltaTime);
                }
                else
                {
                    ((BaseAssetPackage)current.Value).SetRemainTime(this.remainTime);
                }
                if (current.Value.IsTimeOut())
                {
                    current.Value.Release();
                    break;
                }
            }
        }

        public void Release()
        {
            foreach (KeyValuePair<int, IAssetPackage> current in this.cachePackage)
            {
                current.Value.Release();
            }
            this.cachePackage.Clear();
            this.loadingPackage.Clear();
        }

        public void GC(bool isForce = false)
        {
            if (isForce)
            {
                this.Release();
                return;
            }
            Queue<BaseAssetPackage> queue = new Queue<BaseAssetPackage>();
            using (Dictionary<int, IAssetPackage>.Enumerator enumerator = this.cachePackage.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<int, IAssetPackage> current = enumerator.Current;
                    if (current.Value.GetUserNum() == 0)
                    {
                        queue.Enqueue((BaseAssetPackage)current.Value);
                    }
                }
                goto IL_6A;
            }
        IL_5F:
            queue.Dequeue().Release();
        IL_6A:
            if (queue.Count <= 0)
            {
                return;
            }
            goto IL_5F;
        }
    }

    public class GamePackageCache : AssetPackageCache
    {
        protected static GamePackageCache instance;

        public static GamePackageCache Instance
        {
            get
            {
                if (GamePackageCache.instance == null)
                {
                    GamePackageCache.instance = new GamePackageCache();
                }
                return GamePackageCache.instance;
            }
        }

        protected override BaseAssetPackage CreatePackage(HashName packageName)
        {
            return new GameAssetPackage(packageName, this);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.FSM
{
    public class EntityManager<TEntity> : IEnumerable where TEntity : BaseGameEntity
    {
        private IDictionary<int, BaseGameEntity> mEntityMap;

        private int mCurId;

        public int Count
        {
            get
            {
                return this.mEntityMap.Count;
            }
        }

        public BaseGameEntity this[int key]
        {
            get
            {
                return this.GetEntityFromID(key);
            }
        }

        private EntityManager()
        {
            this.mEntityMap = new Dictionary<int, BaseGameEntity>();
        }

        public static EntityManager<TEntity> CreateEntityManager()
        {
            return new EntityManager<TEntity>();
        }

        public void RegisterEntity(TEntity newEntity)
        {
            this.mEntityMap.Add(newEntity.ID, newEntity);
        }

        public void RemoveEntity(TEntity pEntity)
        {
            this.mEntityMap.Remove(pEntity.ID);
        }

        public TEntity GetEntityFromID(int id)
        {
            BaseGameEntity baseGameEntity = null;
            if (!this.mEntityMap.TryGetValue(id, out baseGameEntity))
            {
                Debug.Log("id " + id + "is not exit in mEntityMap !");
            }
            return (TEntity)((object)baseGameEntity);
        }

        public bool HasEntity(int id)
        {
            return this.mEntityMap.ContainsKey(id);
        }

        public IEnumerator GetEnumerator()
        {
            return new EntityManagerEnum(this.mEntityMap.GetEnumerator());
        }

        public void Reset()
        {
            this.mEntityMap.Clear();
        }

        public int CreateID()
        {
            return ++this.mCurId;
        }
    }

    public class EntityManagerEnum : IEnumerator
    {
        private IEnumerator<KeyValuePair<int, BaseGameEntity>> entityEnum;

        public object Current
        {
            get
            {
                object value;
                try
                {
                    KeyValuePair<int, BaseGameEntity> current = this.entityEnum.Current;
                    value = current.Value;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
                return value;
            }
        }

        public EntityManagerEnum(IEnumerator<KeyValuePair<int, BaseGameEntity>> entityEnum)
        {
            this.entityEnum = entityEnum;
        }

        public bool MoveNext()
        {
            return this.entityEnum.MoveNext();
        }

        public void Reset()
        {
            this.entityEnum.Reset();
        }
    }
}

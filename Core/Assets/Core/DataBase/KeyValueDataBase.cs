using System;
using System.Collections.Generic;

namespace GameCore.Core
{
    public class KeyValueDataBase
    {
        private Dictionary<string, object> a = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                return this.a[key];
            }
            set
            {
                if (this.a.ContainsKey(key))
                {
                    this.a[key] = value;
                    return;
                }
                this.a.Add(key, value);
            }
        }

        public static KeyValueDataBase Create(string key, object value)
        {
            return new KeyValueDataBase().Push(key, value);
        }

        public KeyValueDataBase Push(string key, object value)
        {
            this.a[key] = value;
            return this;
        }

        public static T GetValue<T>(object data, string name)
        {
            return ((KeyValueDataBase)data).GetValue<T>(name);
        }

        public T GetValue<T>(string key)
        {
            return (T)((object)this.a[key]);
        }

        public void Remove(string key)
        {
            this.a.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return this.a.ContainsKey(key);
        }
    }
}

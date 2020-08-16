﻿using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.CL.Repositories
{
    public class CacheRepository<T> where T : Entity
    {
        private int Minutes = int.Parse(ConfigurationManager.AppSettings["CachingTimeInMinutes"]);

        public T Get(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key) as T;
        }

        public List<T> GetSeveral(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key) as List<T>;
        }

        public bool Add(T item, string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, item, DateTime.Now.AddMinutes(Minutes));
        }

        public bool Add(List<T> items, string key)
        {
            MemoryCache memotyCache = MemoryCache.Default;
            return memotyCache.Add(key, items, DateTime.Now.AddMinutes(Minutes));
        }

        public void Update(T item, string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(key, item, DateTime.Now.AddMinutes(Minutes));
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Remove(key);
        }
    }
}
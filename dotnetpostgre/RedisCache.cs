﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetpostgre
{
    public class ETagCache
    {
        private readonly IDistributedCache _cache;
        private readonly HttpContext _httpContext;
        public ETagCache(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public T GetCachedObject<T>(string cacheKeyPrefix)
        {
            string requestETag = GetRequestedETag();

            if (!string.IsNullOrEmpty(requestETag))
            {
                // Construct the key for the cache 
                string cacheKey = $"{cacheKeyPrefix}-{requestETag}";

                // Get the cached item
                string cachedObjectJson = _cache.GetString(cacheKey);

                // If there was a cached item then deserialise this 
                if (!string.IsNullOrEmpty(cachedObjectJson))
                {
                    T cachedObject = JsonConvert.DeserializeObject<T>(cachedObjectJson);
                    return cachedObject;
                }
            }

            return default(T);
        }

        public bool SetCachedObject(string cacheKeyPrefix, dynamic objectToCache)
        {
            if (!IsCacheable(objectToCache))
            {
                return true;
            }

            string requestETag = GetRequestedETag();
            string responseETag = Convert.ToBase64String(objectToCache.RowVersion);

            // Add the contact to the cache for 30 mins if not already in the cache
            if (objectToCache != null && responseETag != null)
            {
                string cacheKey = $"{cacheKeyPrefix}-{responseETag}";
                string serializedObjectToCache = JsonConvert.SerializeObject(objectToCache);
                _cache.SetString(cacheKey, serializedObjectToCache, new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTime.Now.AddMinutes(30) });
            }

            // Add the current ETag to the HTTP header
            _httpContext.Response.Headers.Add("ETag", responseETag);

            bool IsModified = !(_httpContext.Request.Headers.ContainsKey("If-None-Match") && responseETag == requestETag);
            return IsModified;
        }

        private string GetRequestedETag()
        {
            if (_httpContext.Request.Headers.ContainsKey("If-None-Match"))
            {
                return _httpContext.Request.Headers["If-None-Match"].First();
            }
            return "";
        }

        private bool IsCacheable(dynamic objectToCache)
        {
            var type = objectToCache.GetType();
            return type.GetProperty("RowVersion") != null;
        }
    }
}

using System;
using System.Web.Caching;
using System.Collections.Generic;
using GMS.Framework.Utility;

namespace GMS.Core.Config
{
    public class CachedConfigContext : ConfigContext
    {
        /// <summary>
        /// 重写基类的取配置，加入缓存机制
        /// </summary>
        public override T Get<T>(string index = null)
        {
            var fileName = this.GetConfigFileName<T>(index);
            var key = "ConfigFile_" +fileName;
            var content = Caching.Get(key);
            if (content != null)
                return (T)content;

            var value = base.Get<T>(index);
            Caching.Set(key, value, new CacheDependency(ConfigService.GetFilePath(fileName)));
            return value;
        }

        public static CachedConfigContext Current = new CachedConfigContext();

        

        public CacheConfig CacheConfig
        {
            get
            {
                return this.Get<CacheConfig>();
            }
        }

               
        public UrlConfig UrlConfig
        {
            get
            {   
                return this.Get<UrlConfig>();
            }
        }

    }
}

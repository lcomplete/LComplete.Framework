﻿using System;
using System.Reflection;
using Castle.DynamicProxy;
using LComplete.Framework.Cache;
using Snap;

namespace LComplete.Framework.CacheHandler
{
    public class CachingInterceptor : MethodInterceptor
    {

        private ICacheKeyGenerator _keyGenerator;

        public CachingInterceptor()
        {
            _keyGenerator = new DefaultCacheKeyGenerator();
        }

        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            CachingAttribute cachingAttribute = attribute as CachingAttribute;
            if (cachingAttribute == null)
            {
                invocation.Proceed();
            }
            else
            {
                string cacheKey = _keyGenerator.CreateCacheKey(method, invocation.Arguments);
                object cachedResult = CacheManager.Get(cacheKey);
                if(cachedResult==null)
                {
                    lock (method)
                    {
                        cachedResult = CacheManager.Get(cacheKey);
                        if (cachedResult == null)
                        {
                            invocation.Proceed();
                            cachedResult = invocation.ReturnValue;
                            CacheManager.Set(cacheKey, cachedResult, cachingAttribute.CacheTimeSpan);
                        }
                    }
                }

                invocation.ReturnValue = cachedResult;
            }

        }

    }
}

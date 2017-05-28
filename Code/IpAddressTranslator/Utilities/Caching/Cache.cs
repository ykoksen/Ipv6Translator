using System;
using System.Runtime.Caching;
using JetBrains.Annotations;

namespace Utilities.Caching
{
	public class Cache
	{
		private readonly MemoryCache _cache;
		private readonly CacheItemPolicy _policy;
		private readonly CacheItemPolicy _policyQuick;

		private Cache()
		{
			_cache = MemoryCache.Default;
			_policy = new CacheItemPolicy();
			_policy.SlidingExpiration = new TimeSpan(24, 0, 0);

			_policyQuick = new CacheItemPolicy();
			_policyQuick.SlidingExpiration = new TimeSpan(0, 2, 0);
		}

		private static Cache _global;

		[NotNull]
		public static Cache GlobalCache
		{
			get
			{
				if (_global == null)
				{
					_global = new Cache();
				}

				return _global;
			}
		}

		/// <summary>
		/// Will not overwrite existing item if exists
		/// </summary>
		/// <param name="key"></param>
		/// <param name="obj"></param>
		/// <param name="quickExpire"></param>
		public bool Add<T>([NotNull] string key, [NotNull] NullItem<T> obj, bool quickExpire = false)
		{
			return _cache.Add(key, obj, quickExpire ? _policyQuick : _policy);
		}

		/// <summary>
		/// Will overwrite existing item if exists
		/// </summary>
		/// <param name="key"></param>
		/// <param name="obj"></param>
		public void Put<T>([NotNull] string key, [NotNull] NullItem<T> obj)
		{
			_cache.Set(key, obj, _policy);
		}

		public void Put<T>([NotNull] string key, [CanBeNull] T obj) where T:class
		{ 
			_cache.Set(key, new NullItem<T>(obj, obj == null), _policy);
		}

		[CanBeNull]
		public NullItem<T> Get<T>([NotNull] string key) where T : class
		{
			return _cache.Get(key) as NullItem<T>;
		}

		public bool Remove([NotNull] string key)
		{
			return _cache.Remove(key) != null;
		}
	}

	public class NullItem<T>
	{
		public T Value { get; }

		public bool IsNull { get; }

		public NullItem([CanBeNull] T value, bool isNull)
		{
			if (value == null && !isNull)
			{
				throw new ArgumentException($"Cannot be null when {nameof(isNull)} is false", nameof(value));
			}

			Value = value;
			IsNull = isNull;
		}

		public NullItem([CanBeNull] T value)
		{
			IsNull = value == null;
			Value = value;
		}

		public NullItem(bool isNull)
		{
			IsNull = isNull;
		}
	}
}

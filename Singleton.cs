using System;

namespace NK.yaSingleton
{
    /// <summary>
    ///     Singleton class. It'll be initialized before the Awake method of all other MonoBehaviours.
    ///     Inherit by passing the inherited type (e.g. class GameManager : Singleton&lt;GameManager&gt;)
    /// </summary>
    /// <typeparam name="TSingleton">The Inherited Singleton's Type</typeparam>
    [Serializable]
	public abstract class Singleton<TSingleton> : BaseSingleton where TSingleton : BaseSingleton
	{
		public static TSingleton instance { get; private set; }

		internal override void CreateInstance()
		{
			if (instance != null) {
				return;
			}

			instance = GetOrCreate<TSingleton>();
		}
	}
}
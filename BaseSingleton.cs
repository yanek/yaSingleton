using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NK.yaSingleton.Helpers;
using NK.yaSingleton.Utility;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace NK.yaSingleton
{
	/// <summary>
	///     Base class for singletons. Contains method stubs and the Create method. Use this to create custom Singleton
	///     flavors.
	///     If you're looking to create a singleton, inherit Singleton or LazySingleton.
	/// </summary>
	public abstract class BaseSingleton : PreloadedScriptableObject
	{
		protected static SingletonUpdater updater => SingletonUpdater.updater;

		internal abstract void CreateInstance();

		// Reduce the visibility of OnEnable; Inheritors should override Initialize instead.
		private new void OnEnable()
		{
			base.OnEnable();
			#if UNITY_EDITOR
			if (!EditorApplication.isPlayingOrWillChangePlaymode) {
				return;
			}
			#endif
			allSingletons.Add(this);
		}

		// Reduce the visibility of OnDisable; Inheritors should override Deinitialize instead.
		private new void OnDisable()
		{
			base.OnDisable();
		}

		protected virtual void Initialize()
		{
			updater.destroyEvent += Deinitialize;

			updater.fixedUpdateEvent += OnFixedUpdate;
			updater.updateEvent += OnUpdate;
			updater.lateUpdateEvent += OnLateUpdate;

			updater.applicationFocusEvent += OnApplicationFocus;
			updater.applicationPauseEvent += OnApplicationPause;
			updater.applicationQuitEvent += OnApplicationQuit;

			updater.drawGizmosEvent += OnDrawGizmos;
			updater.postRenderEvent += OnPostRender;
			updater.preCullEvent += OnPreCull;
			updater.preRenderEvent += OnPreRender;
		}

		protected virtual void Deinitialize() { }

		#region UnityEvents

		public virtual void OnFixedUpdate() { }
		public virtual void OnUpdate() { }
		public virtual void OnLateUpdate() { }
		public virtual void OnApplicationFocus() { }
		public virtual void OnApplicationPause() { }
		public virtual void OnApplicationQuit() { }
		public virtual void OnDrawGizmos() { }
		public virtual void OnPostRender() { }
		public virtual void OnPreCull() { }
		public virtual void OnPreRender() { }

		#endregion

		#region Coroutines

		/// <summary>
		///     <para>Starts a coroutine.</para>
		/// </summary>
		/// <param name="routine">The coroutine</param>
		protected Coroutine StartCoroutine(IEnumerator routine)
		{
			return updater.StartCoroutine(routine);
		}

		/// <summary>
		///     <para>Starts a coroutine named methodName.</para>
		/// </summary>
		/// <param name="methodName">Name of coroutine.</param>
		protected Coroutine StartCoroutine(string methodName)
		{
			return updater.StartCoroutine(methodName);
		}

		/// <summary>
		///     <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
		/// </summary>
		/// <param name="routine">Name of the function in code, including coroutines.</param>
		protected void StopCoroutine(Coroutine routine)
		{
			updater.StopCoroutine(routine);
		}

		/// <summary>
		///     <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
		/// </summary>
		/// <param name="routine">Name of the function in code, including coroutines.</param>
		protected void StopCoroutine(IEnumerator routine)
		{
			updater.StopCoroutine(routine);
		}

		/// <summary>
		///     <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
		/// </summary>
		/// <param name="methodName">Name of coroutine.</param>
		protected void StopCoroutine(string methodName)
		{
			updater.StopCoroutine(methodName);
		}

		/// <summary>
		///     <para>Stops all coroutines running on this behaviour.</para>
		/// </summary>
		protected void StopAllCoroutines()
		{
			updater.StopAllCoroutines();
		}

		#endregion

		public static readonly List<BaseSingleton> allSingletons = new List<BaseSingleton>();

		protected static T GetOrCreate<T>() where T : BaseSingleton
		{
			T instance = allSingletons.FirstOrDefault(s => s.GetType() == typeof(T)) as T;

			instance = instance ? instance : CreateInstance<T>();
			Debug.Assert(instance != null);
			instance.Initialize();

			return instance;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeSingletons()
		{
			foreach (BaseSingleton singleton in allSingletons) {
				singleton.CreateInstance();
			}
		}
	}
}
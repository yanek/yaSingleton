using System;
using UnityEngine;

namespace NK.yaSingleton.Utility
{
	/// <summary>
	///     Executor behaviour that can be used as a proxy to call Unity events.
	/// </summary>
	public class ExecutorBehavior : MonoBehaviour
	{
		[SerializeField, HideInInspector]
		private bool m_DontDestroyOnLoad;

		public event Action startEvent;
		public event Action enableEvent;
		public event Action disableEvent;
		public event Action destroyEvent;

		public event Action fixedUpdateEvent;
		public event Action updateEvent;
		public event Action lateUpdateEvent;

		public event Action applicationFocusEvent;
		public event Action applicationPauseEvent;
		public event Action applicationQuitEvent;

		public event Action drawGizmosEvent;
		public event Action guiEvent;
		public event Action postRenderEvent;
		public event Action preCullEvent;
		public event Action preRenderEvent;

		public bool dontDestroyOnLoad
		{
			get => m_DontDestroyOnLoad;
			set {
				if (value) {
					DontDestroyOnLoad(gameObject);
				}

				if (!value && m_DontDestroyOnLoad) {
					Debug.Log("Object already set to not destroy on load. " +
					          "This cannot be undone (you can destroy the object instead).");
					return;
				}

				m_DontDestroyOnLoad = value;
			}
		}

		#region Lifecycle Events

		private void Start()
		{
			startEvent?.Invoke();
		}

		private void OnEnable()
		{
			enableEvent?.Invoke();
		}

		private void OnDisable()
		{
			disableEvent?.Invoke();
		}

		private void OnDestroy()
		{
			destroyEvent?.Invoke();
		}

		#endregion

		#region UpdateEvents

		private void FixedUpdate()
		{
			fixedUpdateEvent?.Invoke();
		}

		private void Update()
		{
			updateEvent?.Invoke();
		}

		private void LateUpdate()
		{
			lateUpdateEvent?.Invoke();
		}

		#endregion

		#region Application Events

		private void OnApplicationFocus(bool hasFocus)
		{
			applicationFocusEvent?.Invoke();
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			applicationPauseEvent?.Invoke();
		}

		private void OnApplicationQuit()
		{
			applicationQuitEvent?.Invoke();
		}

		#endregion

		#region Other Events

		private void OnDrawGizmos()
		{
			drawGizmosEvent?.Invoke();
		}


		private void OnGUI()
		{
			guiEvent?.Invoke();
		}

		private void OnPostRender()
		{
			postRenderEvent?.Invoke();
		}

		private void OnPreCull()
		{
			preCullEvent?.Invoke();
		}

		private void OnPreRender()
		{
			preRenderEvent?.Invoke();
		}

		#endregion

		public static TExecutor Create<TExecutor>(string name = "Executor", bool dontDestroyOnLoad = false,
			HideFlags hideFlags = HideFlags.None) where TExecutor : ExecutorBehavior
		{
			GameObject go = new GameObject(name) {
				hideFlags = hideFlags
			};

			TExecutor executor = go.AddComponent<TExecutor>();
			executor.dontDestroyOnLoad = dontDestroyOnLoad;
			return executor;
		}
	}
}
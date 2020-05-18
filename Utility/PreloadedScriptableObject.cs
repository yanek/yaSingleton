using NK.yaSingleton.Helpers;
using UnityEditor;
using UnityEngine;

namespace NK.yaSingleton.Utility
{
	/// <inheritdoc />
	/// <summary>
	///     ScriptableObject that automagically adds itself to Unity's preloaded assets.
	/// </summary>
	public abstract class PreloadedScriptableObject : ScriptableObject
	{
		protected virtual void OnEnable()
		{
			#if UNITY_EDITOR
			if (EditorApplication.isPlayingOrWillChangePlaymode) {
				return;
			}
			#endif

			this.AddToPreloadedAssets();
		}

		protected virtual void OnDisable()
		{
			#if UNITY_EDITOR
			if (EditorApplication.isPlayingOrWillChangePlaymode) {
				return;
			}
			#endif

			ScriptableObjectExtensions.RemoveEmptyPreloadedAssets();
		}
	}
}
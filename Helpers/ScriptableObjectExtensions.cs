using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NK.yaSingleton.Helpers
{
	public static class ScriptableObjectExtensions
	{
		[Conditional("UNITY_EDITOR")]
		public static void AddToPreloadedAssets(this ScriptableObject scriptableObject)
		{
			#if UNITY_EDITOR
			var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();

			if (preloadedAssets.Any(preloadedAsset =>
				preloadedAsset && preloadedAsset.GetInstanceID() == scriptableObject.GetInstanceID())) {
				// Already being preloaded
				return;
			}

			preloadedAssets.Add(scriptableObject);

			PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
			#endif
		}

		[Conditional("UNITY_EDITOR")]
		public static void RemoveEmptyPreloadedAssets()
		{
			#if UNITY_EDITOR
			var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();

			var nonEmptyPreloadedAssets = preloadedAssets.Where(asset => asset).ToArray();

			PlayerSettings.SetPreloadedAssets(nonEmptyPreloadedAssets);
			#endif
		}
	}
}
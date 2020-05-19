using System.Collections.Generic;
using NK.yaSingleton.Helpers;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace NK.yaSingleton.Editor
{
	[CustomEditor(typeof(SingletonUpdater), true)]
	public class SingletonUpdaterEditor : OdinEditor
	{
		private Dictionary<int, UnityEditor.Editor> m_Editors;
		private Dictionary<int, bool>               m_Foldouts;

		public static GUIStyle boldLabel { get; } = new GUIStyle("BoldLabel");

		protected override void OnEnable()
		{
			base.OnEnable();
			m_Editors = new Dictionary<int, UnityEditor.Editor>();
			m_Foldouts = new Dictionary<int, bool>();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.LabelField("Singletons", boldLabel);

			for (var i = 0; i < BaseSingleton.allSingletons.Count; ++i) {
				if (!m_Editors.ContainsKey(i)) {
					m_Editors.Add(i, null);
				}

				if (!m_Foldouts.ContainsKey(i)) {
					m_Foldouts.Add(i, true);
				}

				BaseSingleton singleton = BaseSingleton.allSingletons[i];

				m_Foldouts[i] = EditorGUILayout.InspectorTitlebar(m_Foldouts[i], singleton);

				if (!m_Foldouts[i]) {
					continue;
				}

				UnityEditor.Editor editor = m_Editors[i];
				CreateCachedEditor(singleton, null, ref editor);

				m_Editors[i] = editor;

				EditorGUI.indentLevel += 1;
				editor.OnInspectorGUI();

				EditorGUILayout.Space();

				if (AssetDatabase.Contains(singleton)) {
					EditorGUILayout.ObjectField(
						"Reference",
						singleton,
						singleton.GetType(),
						false
					);
				}
				else {
					EditorGUILayout.HelpBox("Runtime generated", MessageType.Info);
				}

				EditorGUI.indentLevel -= 1;
			}
		}
	}
}
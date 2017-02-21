using UnityEngine;
using UnityEditor;

namespace MelsomTMX {
	[CustomEditor(typeof(TiledMap))]
	public class TiledMapEditor : Editor {
		public string path = "";
		public bool fileNotFound = false;

		public string fullPath { get { return Application.dataPath + path; } }

		public override void OnInspectorGUI() {
			TiledMap target = (TiledMap)this.target;

			path = EditorGUILayout.TextField("Path", path);
			try {
				target.data = TiledMapData.Load(fullPath);
				EditorGUILayout.HelpBox("Using file at: " + fullPath, MessageType.Info);
			} catch (System.IO.FileNotFoundException) {
				EditorGUILayout.HelpBox("Can't find file at: " + fullPath, MessageType.Error);
			} catch (System.UnauthorizedAccessException) {
				EditorGUILayout.HelpBox("Can't acces file at: " + fullPath, MessageType.Error);
			}

			base.OnInspectorGUI();
		}
	}
}

using UnityEngine;
using UnityEditor;

namespace MelsomTMX {
	[CustomEditor(typeof(TiledMap))]
	public class TiledMapEditor : Editor {
		new public TiledMap target { get { return (TiledMap)base.target; } }
		public string fullPath { get { return target.fullPath; } }

		Rect fileSelectRect;

		public override void OnInspectorGUI() {

			// Try loading mapdata from file
			try { 
				// If everything is successful
				target.data = TiledMapData.Load(fullPath);
				EditorGUILayout.HelpBox("Using file at: " + fullPath, MessageType.Info);
			} catch (System.IO.FileNotFoundException) { 
				// If the file can't be found
				EditorGUILayout.HelpBox("Can't find file at: " + fullPath, MessageType.Error);
			} catch (System.UnauthorizedAccessException) { 
				// If the file can't be accessed
				EditorGUILayout.HelpBox("Can't acces file at: " + fullPath, MessageType.Error);
			}
			if (GUILayout.Button("Select File")) {
				// Lambda expression to avoid (redundant?) functions
				TiledMapSelectorWindow.SelectMap(x => target.relativePath = x);
			}

			EditorGUILayout.LabelField("Default Inspector");
			base.OnInspectorGUI();
		}
	}
}

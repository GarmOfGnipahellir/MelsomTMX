using UnityEngine;
using UnityEditor;
using System.IO;

namespace TiledMaps {
	[CustomEditor(typeof(TiledMap))]
	public class TiledMapEditor : Editor {
		new public TiledMap target { get { return (TiledMap)base.target; } }
		public TiledMapData data { get { return target.data; } }
		public string fullPath { get { return target.fullPath; } }

		bool[] showTilesets;
		bool defaultInspector = false;

		public override void OnInspectorGUI() {
			if (DrawMapSelection()) {
				if (data != null) {
					DrawTilesetEditing();
				}
			}

			defaultInspector = EditorGUILayout.Foldout(defaultInspector, "Default Inspector");
			if (defaultInspector) {
				EditorGUI.indentLevel++;
				base.OnInspectorGUI();
				EditorGUI.indentLevel--;
			}

			target.UpdateTiles();
		}

		void OnSceneGUI() {
		}

		bool DrawMapSelection() {
			bool foundFile = false;

			// Try loading mapdata from file
			try {
				// If everything is successful
				target.data = TiledMapData.Load(fullPath);
				EditorGUILayout.HelpBox("Using file: " + fullPath, MessageType.Info);
				foundFile = true;
			} catch (System.Exception) {
				// If the file can't be found
				EditorGUILayout.HelpBox("Can't find file: " + fullPath, MessageType.Error);
			}
			if (GUILayout.Button("Select File")) {
				// Lambda expression to avoid (redundant?) functions
				TiledMapSelectorWindow.SelectMap(x => target.relativePath = x);
			}

			return foundFile;
		}

		void DrawTilesetEditing() {
			if (target.tilesets == null || target.tilesets.Length != data.tilesets.Length) {
				target.tilesets = new TiledTileset[data.tilesets.Length];
			}
			if (showTilesets == null || showTilesets.Length != target.tilesets.Length) {
				showTilesets = new bool[target.tilesets.Length];
			}
			for (int i = 0; i < target.tilesets.Length; i++) {
				TiledTilesetData tilesetData = data.tilesets[i];

				if (target.tilesets[i] == null) {
					target.tilesets[i] = CreateInstance<TiledTileset>();
				}
				target.tilesets[i].Init(tilesetData, fullPath);

				if (!(showTilesets[i] = EditorGUILayout.Foldout(showTilesets[i], tilesetData.name))) continue;

				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical();
				EditorGUI.indentLevel--;

				target.tilesets[i] = EditorGUILayout.ObjectField("Tileset Asset",target.tilesets[i], typeof(TiledTileset), true) as TiledTileset;

				for (int col = 0; col < tilesetData.columns; col++) {
					EditorGUILayout.BeginHorizontal();
					for (int row = 0; row < tilesetData.tilecount / tilesetData.columns; row++) {
						int lid = row + col * tilesetData.columns;
						EditorGUILayout.BeginVertical();

						if (GUILayout.Button(target.tilesets[i].tiles[lid].texture, GUILayout.ExpandWidth(true))) {
							TiledTileWindow.EditTile(target.tilesets[i], lid);
						}

						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndHorizontal();
				}

				if (GUILayout.Button("Save Tileset")) {
					string path = EditorUtility.SaveFilePanelInProject("Save Tileset", tilesetData.name, "asset", "Message");
					if (path.Length > 0) {
						AssetDatabase.CreateAsset(target.tilesets[i], path);
					}
				}

				EditorGUILayout.EndVertical();
			}
		}
	}
}

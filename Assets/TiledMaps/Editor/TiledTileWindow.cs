using UnityEngine;
using UnityEditor;
using System;

namespace TiledMaps {
	public class TiledTileWindow : EditorWindow {
		public TiledTileset tileset;
		public int lid;

		public static void EditTile(TiledTileset tileset, int lid) {
			TiledTileWindow window = CreateInstance<TiledTileWindow>();
			window.tileset = tileset;
			window.lid = lid;
			window.ShowUtility();
		}

		void OnGUI() {
			TiledTile tile = tileset.tiles[lid];
			tile.gameObject = EditorGUILayout.ObjectField("Prefab", tile.gameObject, typeof(GameObject), false) as GameObject;
			tile.rotation.eulerAngles = EditorGUILayout.Vector3Field("Rotation", tile.rotation.eulerAngles);
			tileset.tiles[lid] = tile;

			if (GUILayout.Button("Done")) {
				Close();
			}
		}
	} 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TiledMaps {
	public class TiledMap : MonoBehaviour {
		public string relativePath;
		public TiledMapData data;
		public TiledTileset[] tilesets;

		public float tileWidth = 3.0f, tileHeight = 3.0f;
		public GameObject[] tiles;

		public string fullPath { get { return string.Format("{0}/{1}", Application.dataPath, relativePath); } }

		public void UpdateTiles() {
			if (tiles != null) {
				foreach (GameObject tile in tiles) {
					DestroyImmediate(tile);
				}
			}
			tiles = new GameObject[data.width * data.height];

			for (int y = 0; y < data.height; y++) {
				for (int x = 0; x < data.width; x++) {
					int i = x + y * data.width;

					int gid = data.GetGID(x, y, "Arch");
					if (gid == -1) {
						print("Bad GID");
						continue;
					}
					int tilesetIndex = data.GetTileset(gid);
					if (tilesetIndex == -1) {
						print("Bad Tileset Index");
						continue;
					}
					int lid = data.tilesets[tilesetIndex].GetLID(gid);
					if (lid == -1) {
						print("Bad LID");
						continue;
					}
					if (tilesets[tilesetIndex] == null) {
						print("Bad Tileset");
						continue;
					}

					TiledTile tile = tilesets[tilesetIndex].tiles[lid];
					if (tile.gameObject) {
						tiles[i] = Instantiate(tile.gameObject, new Vector3(x * tileWidth, 0, -y * tileHeight), tile.rotation, transform);
					} else {
						print("TILE");
					}
				}
			}
		}
	}
}

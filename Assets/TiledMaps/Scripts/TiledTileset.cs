using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TiledMaps {
	public class TiledTileset : ScriptableObject {
		public TiledTile[] tiles; 

		public void Init(TiledTilesetData tilesetData, string mapPath) {
			if (tiles == null || tiles.Length != tilesetData.tilecount) {
				tiles = new TiledTile[tilesetData.tilecount];
			}

			Texture2D tilesetTexture = tilesetData.GetTexture2D(mapPath);
			if (tilesetTexture != null) {
				for (int lid = 0; lid < tilesetData.tilecount; lid++) {
					tiles[lid].texture = tilesetTexture.GetTile(tilesetData, lid);
				}
			}
		}
	}

	[System.Serializable]
	public struct TiledTile {
		public Texture2D texture;
		public GameObject gameObject;
		public Quaternion rotation;
	}
}

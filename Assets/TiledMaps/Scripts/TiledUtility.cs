using UnityEngine;
using System.IO;

namespace TiledMaps {
	public static class TiledUtility {
		public static Texture2D GetTexture2D(this TiledTilesetData tilesetData, string mapPath) {
			Texture2D texture = null;
			string imagePath = tilesetData.AbsoluteImagePath(mapPath);
			if (File.Exists(imagePath)) {
				texture = new Texture2D(2, 2);
				texture.LoadImage(File.ReadAllBytes(imagePath));
			}
			return texture;
		}

		public static Texture2D GetTile(this Texture2D texture, TiledTilesetData tilesetData, int lid) {
			int x = (lid % tilesetData.columns) * tilesetData.tilewidth;
			int y = (tilesetData.columns - Mathf.FloorToInt(lid / tilesetData.columns) - 1) * tilesetData.tileheight;

			Texture2D tileTexture = new Texture2D(tilesetData.tilewidth, tilesetData.tileheight);
			tileTexture.SetPixels(texture.GetPixels(x, y, tilesetData.tilewidth, tilesetData.tileheight));
			tileTexture.Apply();
			return tileTexture;
		}
	} 
}

using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace TiledMaps {
	/// <summary>
	/// Root storage class of tiled map data.
	/// Also handles deserialization.
	/// Contains all relevant data in the tiled structure.
	/// </summary>
	[Serializable, XmlRoot("map")]
	public class TiledMapData {
		[XmlAttribute] public string version;
		[XmlAttribute] public string orientation;
		[XmlAttribute] public string renderorder;
		[XmlAttribute] public int width;
		[XmlAttribute] public int height;
		[XmlAttribute] public int tilewidth;
		[XmlAttribute] public int tileheight;
		[XmlAttribute] public int nextobjectid;

		[XmlElement("tileset")] public TiledTilesetData[] tilesets;
		[XmlElement("layer")] public TiledLayerData[] layers;

		/// <summary>
		/// Creates a new <see cref="TiledMapData"/> instance with data from a xml file.
		/// </summary>
		/// <param name="path">Absolute path to file</param>
		/// <returns>A populated <see cref="TiledMapData"/></returns>
		public static TiledMapData Load(string path) {
			// Declare a new serializer with our type
			XmlSerializer serializer = new XmlSerializer(typeof(TiledMapData));
			// Open file for reading
			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				// Create reader, set settings and deserialize
				XmlReader reader = XmlReader.Create(stream);
				reader.Settings.IgnoreWhitespace = true;
				TiledMapData result = serializer.Deserialize(reader) as TiledMapData;

				// Check each tileset and read it from file if needed
				int count = 0;
				foreach (TiledTilesetData tileset in result.tilesets) {
					if (Path.GetExtension(tileset.source) != ".tsx") continue;

					result.tilesets[count] = TiledTilesetData.Load(Path.GetFullPath(path + "\\..\\" + tileset.source));
					result.tilesets[count].firstgid = tileset.firstgid;
					result.tilesets[count].source = tileset.source;
					count++;
				}

				// Convert csv data of each layer to an array
				foreach (TiledLayerData layer in result.layers) {
					if (layer.data.encoding != "csv") continue;

					string[] tokens = layer.data.text.Split(',');
					layer.data.gids = Array.ConvertAll(tokens, int.Parse);
				}

				return result;
			}
		}

		public int GetGID(int x, int y, string ln) {
			foreach (TiledLayerData layer in layers) {
				if (layer.name == ln) {
					return layer.data.gids[x + y * width];
				}
			}
			return -1;
		}

		public int GetTileset(int gid) {
			for (int i = 0; i < tilesets.Length; i++) {
				TiledTilesetData tileset = tilesets[i];

				if (gid >= tileset.firstgid && gid < tileset.firstgid + tileset.tilecount) {
					return i;
				}
			}
			return -1;
		}
	}

	[Serializable, XmlRoot("tileset")]
	public class TiledTilesetData {
		[XmlAttribute] public string name;
		[XmlAttribute] public int firstgid;
		[XmlAttribute] public string source;
		[XmlAttribute] public int tilewidth;
		[XmlAttribute] public int tileheight;
		[XmlAttribute] public int tilecount;
		[XmlAttribute] public int columns;

		[XmlElement] public Image image;

		/// <summary>
		/// Creates a new <see cref="TiledTilesetData"/> instance with data from a xml file.
		/// </summary>
		/// <param name="path">Absolute path to file</param>
		/// <returns>A populated <see cref="TiledTilesetData"/></returns>
		public static TiledTilesetData Load(string path) {
			// Declare a new serializer with our type
			XmlSerializer serializer = new XmlSerializer(typeof(TiledTilesetData));
			// Open file for reading
			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				// Create reader, set settings and deserialize
				XmlReader reader = XmlReader.Create(stream);
				reader.Settings.IgnoreWhitespace = true;
				return serializer.Deserialize(reader) as TiledTilesetData;
			}
		}

		public string AbsoluteImagePath(string mapPath) {
			return Path.GetFullPath(mapPath + "/../" + image.source);
		}

		public int GetLID(int gid) {
			return gid - firstgid;
		}

		[Serializable]
		public class Image {
			[XmlAttribute] public string source;
			[XmlAttribute] public int width;
			[XmlAttribute] public int height;
		}
	}

	[Serializable]
	public class TiledLayerData {
		[XmlAttribute] public string name;
		[XmlAttribute] public int width;
		[XmlAttribute] public int height;

		[XmlElement] public Data data;	

		[Serializable]
		public class Data {
			[XmlText] public string text;

			[XmlAttribute] public string encoding;

			[XmlIgnore] public int[] gids;
		}
	}
}

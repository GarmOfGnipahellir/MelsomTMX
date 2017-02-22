using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MelsomTMX {
	[XmlRoot("map")]
	[System.Serializable]
	public class TiledMapData {
		[XmlAttribute]
		public int width;
		[XmlAttribute]
		public int height;

		[XmlElement("tileset")]
		public List<TiledTilesetData> tilesets;

		[XmlElement("layer")]
		public List<TiledLayerData> layers;

		public static TiledMapData Load(string path) {
			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				XmlReader reader = XmlReader.Create(stream);
				XmlSerializer serializer = new XmlSerializer(typeof(TiledMapData));
				reader.Settings.IgnoreWhitespace = true;
				TiledMapData result = serializer.Deserialize(reader) as TiledMapData;

				int count = 0;
				foreach (TiledTilesetData tileset in result.tilesets) {
					if (Path.GetExtension(tileset.source) != ".tsx") continue;

					result.tilesets[count] = TiledTilesetData.Load(Path.GetFullPath(path + "\\..\\" + tileset.source));
					result.tilesets[count].firstgid = tileset.firstgid;
					result.tilesets[count].source = tileset.source;
					count++;
				}

				foreach (TiledLayerData layer in result.layers) {
					if (layer.data.encoding != "csv") continue;

					string[] tokens = layer.data.text.Split(',');
					layer.data.gids = System.Array.ConvertAll(tokens, int.Parse);
				}

				return result;
			}
		}
	}

	[System.Serializable, XmlRoot("tileset")]
	public class TiledTilesetData {
		[XmlAttribute]
		public string name;
		[XmlAttribute]
		public int firstgid;
		[XmlAttribute]
		public string source;
		[XmlAttribute]
		public int tilewidth;
		[XmlAttribute]
		public int tileheight;
		[XmlAttribute]
		public int tilecount;
		[XmlAttribute]
		public int columns;

		[XmlElement]
		public Image image;

		public static TiledTilesetData Load(string path) {
			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				XmlReader reader = XmlReader.Create(stream);
				XmlSerializer serializer = new XmlSerializer(typeof(TiledTilesetData));
				reader.Settings.IgnoreWhitespace = true;
				return serializer.Deserialize(reader) as TiledTilesetData;
			}
		}

		[System.Serializable]
		public class Image {
			[XmlAttribute]
			public string source;
			[XmlAttribute]
			public int width;
			[XmlAttribute]
			public int height;
		}
	}

	[System.Serializable]
	public class TiledLayerData {
		[XmlAttribute]
		public string name;
		[XmlAttribute]
		public int width;
		[XmlAttribute]
		public int height;

		[XmlElement]
		public Data data;	

		[System.Serializable]
		public class Data {
			[XmlText]
			public string text;
			[XmlAttribute]
			public string encoding;
			[XmlIgnore]
			public int[] gids;
		}
	}
}

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MelsomTMX {
	[XmlRoot("map")]
	[System.Serializable]
	public class TiledMapData {
		[XmlAttribute("width", typeof(int))]
		public int width;
		[XmlAttribute("height", typeof(int))]
		public int height;

		[XmlElement("layer")]
		public List<TiledLayerData> layers;

		public static TiledMapData Load(string path) {
			XmlSerializer serializer = new XmlSerializer(typeof(TiledMapData));
			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				return serializer.Deserialize(stream) as TiledMapData;
			}
		}

		public static TiledMapData LoadFromText(string text) {
			XmlSerializer serializer = new XmlSerializer(typeof(TiledMapData));
			return serializer.Deserialize(new StringReader(text)) as TiledMapData;
		}
	}

	[System.Serializable]
	public class TiledLayerData {
		[XmlAttribute("name")]
		public string name;
		[XmlAttribute("width", typeof(int))]
		public int width;
		[XmlAttribute("height", typeof(int))]
		public int height;

		public string data;
	}
}

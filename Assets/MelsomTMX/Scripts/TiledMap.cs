using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MelsomTMX {
	public class TiledMap : MonoBehaviour {
		public string relativePath;
		public TiledMapData data;

		public string fullPath { get { return string.Format("{0}/{1}", Application.dataPath, relativePath); } }
	}
}

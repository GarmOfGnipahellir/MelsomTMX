using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TiledMaps {
	public class TiledMapSelectorWindow : EditorWindow {
		Action<string> returnAction;

		public static void SelectMap(Action<string> returnAction) {
			TiledMapSelectorWindow window = CreateInstance<TiledMapSelectorWindow>();
			window.returnAction = returnAction;
			window.ShowUtility();
		}

		void OnGUI() {
			string[] tmxFiles = Directory.GetFiles(Application.dataPath, "*.tmx", SearchOption.AllDirectories);
			foreach (string tmxFile in tmxFiles) {
				string relativePath = Regex.Replace(tmxFile.Replace('\\', '/'), "^("+Application.dataPath+"/)", "", RegexOptions.IgnoreCase);

				if (GUILayout.Button(relativePath)) {
					returnAction(relativePath);
					Close();
				}
			}
		}

		void OnLostFocus() {
			Close();
		}
	}
}

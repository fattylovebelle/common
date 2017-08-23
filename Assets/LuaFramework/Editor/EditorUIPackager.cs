using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

public class EditorUIPackager
{
	

	static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();

	[MenuItem("Package/macOS/ui/package ui")]
	public static void packageUIForMacOS() {
		maps.Clear();
		BuildAllUI ();
		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneOSXIntel64, maps);


		maps.Clear();
		maps = BasePackager.BuildNormalAB("Assets/IGSoft_Resources/Projects/UI");
		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneOSXIntel64, maps);
	}


	[MenuItem("Package/WindowsStandard/ui/package ui")]
	public static void packageUIForWindows() {
		maps.Clear();
		BuildAllUI ();
		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows, maps);


		maps.Clear();
		maps = BasePackager.BuildNormalAB("Assets/IGSoft_Resources/Projects/UI");
		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows, maps);
	}


	[MenuItem("Package/WindowsStandard64/ui/package ui")]
	public static void packageUIForWindows64() {
		maps.Clear();
		BuildAllUI ();
		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows64, maps);


		maps.Clear();
		maps = BasePackager.BuildNormalAB("Assets/IGSoft_Resources/Projects/UI");
		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows64, maps);
	}

	[MenuItem("Package/android/ui/package ui")]
	public static void packageUIForAndroid() {
		maps.Clear();
		BuildAllUI ();
		BasePackager.BaseBuildAssetResource (BuildTarget.Android, maps);


		maps.Clear();
		maps = BasePackager.BuildNormalAB("Assets/IGSoft_Resources/Projects/UI");
		BasePackager.BaseBuildAssetResource (BuildTarget.Android, maps);
	}


	/// <summary>
	/// 构建UI AB
	/// </summary>
	static void BuildAllUI()
	{
		string path = "Assets/Res/UI";
		if (!Directory.Exists (path)) {
			return;
		}
		string[] files = Directory.GetFiles(path);
		foreach (var item in files)
		{
			string head = Path.GetFileNameWithoutExtension(item);
			if (head.IndexOf(".") > -1 || head.IndexOf("@") > -1 || item.EndsWith(".meta") || item.Contains(".DS_Store") || item.Contains("说明") || item.ToLower().Contains("readme") )
				continue;
			AddBuildUIABOnMap(head);
		}
	}



	#region ui
	/// Fui资源打包 FairyGUI.UIPackage.AddPackage(ab_desc, ab_res) => byte , @sprite.byte + png + alets
	static void AddBuildUIABOnMap(string moduleName)
	{
		_BuildUIABOnMap(moduleName, true);
		_BuildUIABOnMap(moduleName, false);
	}
	static void _BuildUIABOnMap (string moduleName, bool isData)
	{
		string uiPath = "Assets/Res/UI";
		AssetBundleBuild build = new AssetBundleBuild ();
		string[] sources = Directory.GetFiles (uiPath);
		List<string> list = new List<string> ();
		if (isData) {
			foreach (var item in sources) {
				if (item.EndsWith(".meta") || item.Contains(".DS_Store")) continue;
				if (item.IndexOf (moduleName + ".bytes") != -1) {
					list.Add (item.Replace ('\\', '/'));
					break;
				}
			}
			build.assetBundleName = "ui/"+moduleName.ToLower() + AppConst.uiDataSubfix;
		} else {
			foreach (var item in sources) {
				if (item.EndsWith(".meta") || item.Contains(".DS_Store")) continue;
				if (item.IndexOf (moduleName + "@sprites.bytes") != -1 || item.IndexOf(moduleName + "@atlas") != -1) {
					list.Add (item.Replace ('\\', '/'));
					continue;
				}
			}
			build.assetBundleName = "ui/"+moduleName.ToLower() + AppConst.uiResSubfix;
		}
		if(list.Count == 0)return;
		build.assetNames = list.ToArray();
		maps.Add(build);
	}
	#endregion

}


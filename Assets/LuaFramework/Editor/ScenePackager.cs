using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

/**
 * 场景资源打包
 */
public class ScenePackager
{
	static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();


	[MenuItem("Package/macOS/Scene/package Scene")]
	public static void packageUIForMacOS() {
		PackageMap (BuildTarget.StandaloneOSXIntel64);
	}


	[MenuItem("Package/WindowsStandard/Scene/package Scene")]
	public static void packageUIForWindows() {
		PackageMap (BuildTarget.StandaloneWindows);
	}


	[MenuItem("Package/WindowsStandard64/Scene/package Scene")]
	public static void packageUIForWindows64() {
		PackageMap (BuildTarget.StandaloneWindows64);
	}

	[MenuItem("Package/android/Scene/package Scene")]
	public static void packageUIForAndroid() {
		PackageMap (BuildTarget.Android);
	}


	private static void PackageMap(BuildTarget buildTarget) {
		List<string> results = new List<string> ();
		BasePackager.Recursive (BasePackager.AppDataPath + "/scene", ".unity", results);

		if (results.Count > 100) {
			return;
		}
		string currentPath = null;
		for (int index = 0; index < results.Count; index++) {
			maps.Clear ();

			currentPath = results[index];
			AddSceneABMap(Path.GetFileNameWithoutExtension (currentPath));

			BasePackager.BaseBuildAssetResource (buildTarget, maps);
		}
	}


	#region 场景资源
	static void AddSceneABMap(string sceneID)
	{
		string[] files = { string.Format("Assets/scene/{0}.unity", sceneID) };
		if (files.Length == 0) {
			return;
		}

		for (int i = 0; i < files.Length; i++) {
			files[i] = files[i].Replace('\\', '/');
		}
		AssetBundleBuild build = new AssetBundleBuild();
		build.assetBundleName = string.Format("scene/{0}", sceneID) + AppConst.ExtName;
		build.assetNames = files;
		maps.Add(build);
		//string abName = string.Format("scene/{0}", sceneID);//string.Format("scenes/{0}", sceneID);
		//string path = string.Format("Assets/scene/{0}", sceneID); //"Assets/scene";//string.Format("Assets/Res/Scenes/{0}", sceneID);
		//AddBuildMap(abName + AppConst.ExtName, "*.unity", path);
	}
	#endregion

}

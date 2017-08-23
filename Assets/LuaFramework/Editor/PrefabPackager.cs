using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

public class PrefabPackager
{

	[MenuItem("Package/macOS/Prefabs/package Prefabs")]
	public static void packageUIForMacOS() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Prefabs");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneOSXIntel64, results);
	}


	[MenuItem("Package/WindowsStandard/Prefabs/package Prefabs")]
	public static void packageUIForWindows() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Prefabs");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows, results);
	}


	[MenuItem("Package/WindowsStandard64/Prefabs/package Prefabs")]
	public static void packageUIForWindows64() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Prefabs");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows64, results);
	}

	[MenuItem("Package/android/Prefabs/package Prefabs")]
	public static void packageUIForAndroid() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Prefabs");

		BasePackager.BaseBuildAssetResource (BuildTarget.Android, results);
	}
}


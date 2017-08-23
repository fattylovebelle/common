using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;


/**
 * 声音资源打包工具
 */
public class AudioPackager
{   
	
	[MenuItem("Package/macOS/Audio/package Audio")]
	public static void packageUIForMacOS() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Audio");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneOSXIntel64, results);
	}


	[MenuItem("Package/WindowsStandard/Audio/package Audio")]
	public static void packageUIForWindows() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Audio");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows, results);
	}


	[MenuItem("Package/WindowsStandard64/Audio/package Audio")]
	public static void packageUIForWindows64() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Audio");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows64, results);
	}

	[MenuItem("Package/android/Audio/package Audio")]
	public static void packageUIForAndroid() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Audio");

		BasePackager.BaseBuildAssetResource (BuildTarget.Android, results);
	}
}


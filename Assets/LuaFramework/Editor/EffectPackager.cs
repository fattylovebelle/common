using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

public class EffectPackager
{

	[MenuItem("Package/macOS/Effect/package Effect")]
	public static void packageUIForMacOS() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Effect");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneOSXIntel64, results);
	}


	[MenuItem("Package/WindowsStandard/Effect/package Effect")]
	public static void packageUIForWindows() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Effect");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows, results);
	}


	[MenuItem("Package/WindowsStandard64/Effect/package Effect")]
	public static void packageUIForWindows64() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Effect");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows64, results);
	}

	[MenuItem("Package/android/Effect/package Effect")]
	public static void packageUIForAndroid() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/Res/Effect");

		BasePackager.BaseBuildAssetResource (BuildTarget.Android, results);
	}
}

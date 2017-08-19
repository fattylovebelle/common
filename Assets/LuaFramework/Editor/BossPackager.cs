using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;



public class BossPackager
{

	[MenuItem("Package/macOS/Boss/package Boss")]
	public static void packageUIForMacOS() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Boss");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneOSXIntel64, results);
	}


	[MenuItem("Package/WindowsStandard/Boss/package Boss")]
	public static void packageUIForWindows() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Boss");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows, results);
	}


	[MenuItem("Package/WindowsStandard64/Boss/package Boss")]
	public static void packageUIForWindows64() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Boss");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows64, results);
	}

	[MenuItem("Package/android/Audio/package Audio")]
	public static void packageUIForAndroid() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Boss");

		BasePackager.BaseBuildAssetResource (BuildTarget.Android, results);
	}
}


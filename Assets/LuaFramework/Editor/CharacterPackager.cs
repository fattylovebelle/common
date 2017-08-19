using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

public class CharacterPackager
{
	[MenuItem("Package/macOS/Character/package Character")]
	public static void packageUIForMacOS() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Character");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneOSXIntel64, results);
	}


	[MenuItem("Package/WindowsStandard/Character/package Character")]
	public static void packageUIForWindows() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Character");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows, results);
	}


	[MenuItem("Package/WindowsStandard64/Character/package Character")]
	public static void packageUIForWindows64() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Character");

		BasePackager.BaseBuildAssetResource (BuildTarget.StandaloneWindows64, results);
	}

	[MenuItem("Package/android/Character/package Character")]
	public static void packageUIForAndroid() {
		List<AssetBundleBuild> results = BasePackager.BuildNormalAB ("Assets/IGSoft_Resources/Projects/Character");

		BasePackager.BaseBuildAssetResource (BuildTarget.Android, results);
	}
}

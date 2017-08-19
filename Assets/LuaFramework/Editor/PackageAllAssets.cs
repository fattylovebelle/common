using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;


public class PackageAllAssets
{		
	

	[MenuItem("Package/macOS/PackageAllAssets")]
	public static void packageUIForMacOS() {

		packageEffect ();

		AudioPackager.packageUIForMacOS ();
		BossPackager.packageUIForMacOS ();
		CharacterPackager.packageUIForMacOS ();
		EffectPackager.packageUIForMacOS ();
		FightOtherPackager.packageUIForMacOS ();
		IconPackager.packageUIForMacOS ();
		PrefabPackager.packageUIForMacOS ();
		ScenePackager.packageUIForMacOS ();
		EditorUIPackager.packageUIForMacOS ();
		LuaPackager.packageUIForMacOS ();

		FileMD5Packager.packageUIForMacOS ();
		AssetDatabase.Refresh();
	}


	[MenuItem("Package/WindowsStandard/PackageAllAssets")]
	public static void packageUIForWindows() {
		packageEffect ();

		AudioPackager.packageUIForWindows ();
		BossPackager.packageUIForWindows ();
		CharacterPackager.packageUIForWindows ();
		EffectPackager.packageUIForWindows ();
		FightOtherPackager.packageUIForWindows ();
		IconPackager.packageUIForWindows ();
		PrefabPackager.packageUIForWindows ();
		ScenePackager.packageUIForWindows ();
		EditorUIPackager.packageUIForWindows ();
		LuaPackager.packageUIForWindows ();

		FileMD5Packager.packageUIForMacOS ();
		AssetDatabase.Refresh();
	}


	[MenuItem("Package/WindowsStandard64/PackageAllAssets")]
	public static void packageUIForWindows64() {
		packageEffect ();

		AudioPackager.packageUIForWindows64 ();
		BossPackager.packageUIForWindows64 ();
		CharacterPackager.packageUIForWindows64 ();
		EffectPackager.packageUIForWindows64 ();
		FightOtherPackager.packageUIForWindows64 ();
		IconPackager.packageUIForWindows64 ();
		PrefabPackager.packageUIForWindows64 ();
		ScenePackager.packageUIForWindows64 ();
		EditorUIPackager.packageUIForWindows64 ();
		LuaPackager.packageUIForWindows64 ();

		FileMD5Packager.packageUIForMacOS ();
		AssetDatabase.Refresh();
	}

	[MenuItem("Package/android/PackageAllAssets2")]
	public static void packageUIForAndroid() {
		packageEffect ();

		AudioPackager.packageUIForAndroid ();
		BossPackager.packageUIForAndroid ();
		CharacterPackager.packageUIForAndroid ();
		EffectPackager.packageUIForAndroid ();
		FightOtherPackager.packageUIForAndroid ();
		IconPackager.packageUIForAndroid ();
		PrefabPackager.packageUIForAndroid ();
		ScenePackager.packageUIForAndroid ();
		EditorUIPackager.packageUIForAndroid ();
		LuaPackager.packageUIForAndroid ();

		FileMD5Packager.packageUIForMacOS ();
		AssetDatabase.Refresh();
	}

	public static void packageEffect() {
		if (Directory.Exists(Util.DataPath)) {
			Directory.Delete(Util.DataPath, true);
		}
		string streamPath = Application.streamingAssetsPath;
		if (Directory.Exists(streamPath)) {
			Directory.Delete(streamPath, true);
		}
		Directory.CreateDirectory(streamPath);
	}
}

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

public class BasePackager
{

	#region 遍历指定目录下的所有文件生成单个AB
	/// 普通AB 单个生成
	public static List<AssetBundleBuild> BuildNormalAB(string path)
	{
		List<AssetBundleBuild> results = new List<AssetBundleBuild> ();

		string root = path.Replace('\\', '/');

		List<string> tmp = new List<string>();
		tmp.Add(path);
		GetAllDirList(path, tmp);
		string head = Path.GetFileNameWithoutExtension(root);
		foreach (var item in tmp)
		{
			if (!Directory.Exists (item)) {
				continue;
			}
			string[] files = Directory.GetFiles(item);
			foreach (string f in files)
			{
				string p = f.Replace('\\', '/');
				if (p.EndsWith(".meta") || p.Contains(".DS_Store") || p.Contains("说明") || p.ToLower().Contains("readme") )continue;

				string assetName = p.Substring(p.LastIndexOf(root));
				string bundleName = head + "/" + p.Substring(p.LastIndexOf(root)).Replace(root + "/", "").Replace(Path.GetExtension(p), "") + AppConst.ExtName;
				//Debugger.Log(assetName + " || " + bundleName + "  " + head + "  " + root);
				AddSingleBuildMap(bundleName, assetName, results);
			}
		}

		return results;
	}
	//获取所有目录及子目录
	static void GetAllDirList(string strBaseDir, List<string> list)
	{
		if (list == null) {
			return;
		}
		if (!Directory.Exists (strBaseDir)) {
			return;
		}
		DirectoryInfo di = new DirectoryInfo(strBaseDir);
		DirectoryInfo[] diA = di.GetDirectories();
		for (int i = 0; i < diA.Length; i++)
		{
			list.Add(diA[i].FullName); //diA[i].FullName是某个子目录的绝对地址，把它记录在List<string>中
			GetAllDirList(diA[i].FullName, list);
		}
	}
	//生成单个AB文件
	static void AddSingleBuildMap(string bundleName, string assetName, List<AssetBundleBuild> maps)
	{
		assetName = assetName.Replace('\\', '/');
		AssetBundleBuild build = new AssetBundleBuild();
		build.assetBundleName = bundleName.ToLower();
		build.assetNames = new string[] { assetName };
		maps.Add(build);
	}
	#endregion



	/// <summary>
	/// 生成绑定素材
	/// </summary>
	public static void BaseBuildAssetResource(BuildTarget target, List<AssetBundleBuild> maps) {
		BaseBuildAssetResource (target, maps, "Assets/" + AppConst.AssetDir);
	}



	/// <summary>
	/// 生成绑定素材
	/// </summary>
	public static void BaseBuildAssetResource(BuildTarget target, List<AssetBundleBuild> maps, string resPath) {

		BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle;
		try {
			BuildPipeline.BuildAssetBundles(resPath, maps.ToArray(), options, target);
		} catch (System.Exception e) {
			UnityEngine.Debug.LogError(e);
			return;
		}
		string streamDir = Application.dataPath + "/" + AppConst.LuaTempDir;
		if (Directory.Exists(streamDir)) Directory.Delete(streamDir, true);
	}


	/// <summary>
	/// 生成识别资源路径
	/// </summary>
	public static void BuildFileIndex() {
		string resPath = AppDataPath + "/StreamingAssets/";
		///----------------------创建文件列表-----------------------
		string newFilePath = resPath + "/files.txt";
		if (File.Exists(newFilePath)) File.Delete(newFilePath);

		List<string> files = new List<string> ();
		Recursive(resPath, files);

		FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
		StreamWriter sw = new StreamWriter(fs);

		for (int i = 0; i < files.Count; i++) {
			string file = files[i];
			if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.Contains(".svn")) continue;

			string md5 = Util.md5file(file);
			string value = file.Replace(resPath, string.Empty);
			sw.WriteLine(value + "|" + md5);
		}
		sw.Close(); fs.Close();
	}


	/// <summary>
	/// 遍历目录及其子目录
	/// </summary>
	public static void Recursive(string path, List<string> files) {
		string[] names = Directory.GetFiles(path);
		string[] dirs = Directory.GetDirectories(path);
		foreach (string filename in names) {
			string ext = Path.GetExtension(filename);
			if (ext.Equals (".meta")) {
				continue;
			}
			files.Add(filename.Replace('\\', '/'));
		}
		foreach (string dir in dirs) {
			Recursive(dir, files);
		}
	}


	/// <summary>
	/// 遍历目录及其子目录
	/// </summary>
	public static void Recursive(string path, string extention, List<string> files) {
		string[] names = Directory.GetFiles(path);
		string[] dirs = Directory.GetDirectories(path);
		foreach (string filename in names) {
			string ext = Path.GetExtension(filename);
			if (!ext.Equals (extention)) {
				continue;
			}
			files.Add(filename.Replace('\\', '/'));
		}
		foreach (string dir in dirs) {
			Recursive(dir, extention, files);
		}
	}

	/// <summary>
	/// 数据目录
	/// </summary>
	public static string AppDataPath {
		get { return Application.dataPath.ToLower(); }
	}
}

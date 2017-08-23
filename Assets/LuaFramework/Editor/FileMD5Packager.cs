using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;

public class FileMD5Packager
{

	[MenuItem("Package/FileMd5")]
	public static void packageUIForMacOS() {
		BasePackager.BuildFileIndex ();
	}


}

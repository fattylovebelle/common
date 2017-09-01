using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 单例
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	/// <summary>
	/// 单例引用
	/// </summary>
	private static T instance;

	/// <summary>
	/// 线程锁
	/// </summary>
	private static readonly object singltonLock = new object();

	public static T Instance {
		get { 
			if (instance != null) {
				return instance;
			}
			lock (singltonLock) {
				if (instance != null) {
					return instance;
				}

				instance = FindObjectOfType<T> ();
				if (instance != null) {
					return instance;
				}

				GameObject scriptObject = new GameObject ();
				scriptObject.name = typeof(T).Name + "_Singleton";
				instance = scriptObject.AddComponent<T> ();
			}
			return instance;
		}
	}
}

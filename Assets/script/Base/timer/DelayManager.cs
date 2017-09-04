using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 延迟执行，而且只延迟一次，不会重复
/// </summary>
public class DelayManager : MonoBehaviour{

	/// <summary>
	/// 单例引用
	/// </summary>
	private static DelayManager instance;

	/// <summary>
	/// 线程锁
	/// </summary>
	private static readonly object singltonLock = new object();

	/// <summary>
	/// 延迟执行回调
	/// </summary>
	public delegate void OnDelay();

	/// <summary>
	/// 延迟执行
	/// </summary>
	/// <param name="onDelay">On delay.</param>
	/// <param name="param">Parameter.</param>
	public void Delay(float delayTime, OnDelay onDelay) {
		StartCoroutine (DelayRun (delayTime, onDelay));
	}


	/// <summary>
	/// 延迟执行
	/// </summary>
	/// <returns>The run.</returns>
	/// <param name="delayTime">Delay time.</param>
	/// <param name="onDelay">On delay.</param>
	IEnumerator DelayRun(float delayTime, OnDelay onDelay) {
		if (onDelay == null) {
			yield break;
		}
		yield return new WaitForSeconds (delayTime);
		onDelay.Invoke ();
	}


	/// <summary>
	/// 单例方法
	/// </summary>
	/// <value>The instance.</value>
	public static DelayManager Instance {
		get { 
			if (instance != null) {
				return instance;
			}
			lock (singltonLock) {
				if (instance != null) {
					return instance;
				}

				instance = FindObjectOfType<DelayManager> ();
				if (instance != null) {
					return instance;
				}

				GameObject scriptObject = new GameObject ();
				scriptObject.name = typeof(DelayManager).Name + "_Singleton";
				DontDestroyOnLoad (scriptObject);
				instance = scriptObject.AddComponent<DelayManager> ();
			}
			return instance;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 全局事件管理器
/// </summary>
public sealed class GEventDispatcher : MonoBehaviour {

	/// <summary>
	/// 事件临听者
	/// </summary>
	public delegate void Listner(object o);

	/// <summary>
	/// 事件监听者列表
	/// </summary>
	private Dictionary<string, List<Listner>> listners = new Dictionary<string, List<Listner>>();

	/// <summary>
	/// 单例引用
	/// </summary>
	private static GEventDispatcher instance;

	/// <summary>
	/// 线程锁
	/// </summary>
	private static readonly object singltonLock = new object();


	/// <summary>
	/// 添加事件监听
	/// </summary>
	/// <param name="eventType">Event type.</param>
	/// <param name="listner">Listner.</param>
	public void AddEventListner(string eventType, Listner listner) {
		if (string.IsNullOrEmpty (eventType)) {
			return;
		}
		if (listner == null) {
			return;
		}

		// 不存在事件
		List<Listner> locals = null;
		if (!listners.ContainsKey (eventType)) {
			locals = new List<Listner> ();
			listners.Add (eventType, locals);
			locals.Add (listner);
			return;
		}

		// 判断列表有没有listner
		locals = listners [eventType];
		if (locals.IndexOf (listner) >= 0) {
			return;
		}
		locals.Add (listner);
	}

	/// <summary>
	/// 移除事件监听
	/// </summary>
	/// <param name="eventType">Event type.</param>
	/// <param name="listner">Listner.</param>
	public void RemoveEventListner(string eventType, Listner listner) {
		if (!listners.ContainsKey (eventType)) {
			return;
		}

		List<Listner> localListners = listners[eventType];
		if (localListners == null) {
			return;
		}

		if (localListners.IndexOf (listner) < 0) {
			return;
		}
		localListners.Remove (listner);

		if (localListners.Count > 0) {
			return;
		}
		listners.Remove (eventType);
	}

	/// <summary>
	/// 发送事件
	/// </summary>
	/// <param name="eventType">Event type.</param>
	/// <param name="o">O.</param>
	public void DispatchEvent(string eventType, object o) {
		StartCoroutine (DispatchEventAsy (eventType, o));
	}

	/// <summary>
	/// 延时发送事件
	/// </summary>
	/// <param name="eventType">Event type.</param>
	/// <param name="o">O.</param>
	/// <param name="delayNum">Delay number.</param>
	public void DelayDispatchEvent(string eventType, object o, float delayNum) {
		StartCoroutine (DispatchEventAsy (eventType, o, delayNum));
	}

	/// <summary>
	/// 延时执行事件
	/// </summary>
	/// <returns>The event asy.</returns>
	/// <param name="eventType">Event type.</param>
	/// <param name="o">O.</param>
	/// <param name="delayNum">Delay number.</param>
	IEnumerator DispatchEventAsy(string eventType, object o, float delayNum) {
		if (delayNum <= 0) {
			delayNum = 0;
		}
		yield return new WaitForSeconds(delayNum);
		StartCoroutine (DispatchEventAsy(eventType, o));
	}

	/// <summary>
	/// 异步唤起事件
	/// </summary>
	/// <returns>The event asy.</returns>
	/// <param name="eventType">Event type.</param>
	/// <param name="o">O.</param>
	IEnumerator DispatchEventAsy(string eventType, object o) {
		if (!listners.ContainsKey (eventType)) {
			yield break;
		}
		List<Listner> localListners = listners[eventType];
		if (localListners == null) {
			yield break;
		}

		Listner currentListner;
		for(int index = 0; index < localListners.Count; index ++) {
			currentListner = localListners [index];
			if (currentListner == null) {
				continue;
			}
			currentListner.Invoke (o);
		}
	}


	/// <summary>
	/// 单例方法
	/// </summary>
	/// <value>The instance.</value>
	public static GEventDispatcher Instance {
		get { 
			if (instance != null) {
				return instance;
			}
			lock (singltonLock) {
				if (instance != null) {
					return instance;
				}

				instance = FindObjectOfType<GEventDispatcher> ();
				if (instance != null) {
					return instance;
				}

				GameObject scriptObject = new GameObject ();
				scriptObject.name = typeof(GEventDispatcher).Name + "_Singleton";
				DontDestroyOnLoad (scriptObject);
				instance = scriptObject.AddComponent<GEventDispatcher> ();
			}
			return instance;
		}
	}
}

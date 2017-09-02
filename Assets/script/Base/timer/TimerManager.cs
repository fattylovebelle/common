using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

/// <summary>
/// 统一定时器
/// </summary>
public class TimerManager : Manager {

	/// <summary>
	///宝时器回调
	/// </summary>
	public delegate void OnTimer(int time);

	/// <summary>
	/// 定时器列表
	/// </summary>
	private Dictionary<OnTimer, TimerInfo> timers = new Dictionary<OnTimer, TimerInfo> ();

	/// <summary>
	/// 执行完成需要删除的数据
	/// </summary>
	private List<OnTimer> deleteTimers = new List<OnTimer> ();

	/// <summary>
	/// 单例引用
	/// </summary>
	private static TimerManager instance;

	/// <summary>
	/// 线程锁
	/// </summary>
	private static readonly object singltonLock = new object();

	void Start() {
		InvokeRepeating ("OnTimerTick", 0, 0.01f);
	}

	/// <summary>
	/// 添加统一定时器
	/// </summary>
	/// <param name="totalTime">.运行总时间</param>
	/// <param name="onTimer">运行回调</param>
	/// <param name="interval">每隔多长时间运行一次，单位是毫秒，默认是1秒</param>
	public void AddTimer(int totalTime, OnTimer onTimer, int interval = 1000) {
		if (timers.ContainsKey (onTimer)) {
			return;
		}
		TimerInfo timerInfo = new TimerInfo ();
		timerInfo.onTimer = onTimer;
		timerInfo.interval = interval;
		timerInfo.totalTime = totalTime;
		timers.Add (onTimer, timerInfo);
	}

	/// <summary>
	/// 移除定时器
	/// </summary>
	/// <param name="onTimer">On timer.</param>
	public void RemoveTimer(OnTimer onTimer) {
		if (!timers.ContainsKey (onTimer)) {
			return;
		}
		timers.Remove (onTimer);
	}

	/// <summary>
	/// 在FixedUpdate中执行定时器
	/// </summary>
	void OnTimerTick() {
		if (timers.Count <= 0) {
			return;
		}
		RefreshTime ();

		RemoveFinishTimers ();
	}

	/// <summary>
	/// 执行定时器
	/// </summary>
	private void RefreshTime() {
		TimerInfo timerInfo;
		long currentMillisecon = TimeUtil.CurrentMillisecon();
		foreach (var item in timers) {
			timerInfo = item.Value;
			if (timerInfo == null) {
				continue;
			}

			// 最后一秒
			if (timerInfo.totalTime <= 0) {
				deleteTimers.Add (timerInfo.onTimer);
				StartCoroutine (CallUpCallBack (0, timerInfo.onTimer));
				continue;
			}

			// 没有回调过，先设置时间
			if (timerInfo.pRunTime <= 0) {
				timerInfo.pRunTime = currentMillisecon;
				continue;
			}

			// 相隔时间还没有到，不能运行
			if (currentMillisecon - timerInfo.pRunTime< timerInfo.interval) {
				continue;
			}

			// 重新设置时间
			timerInfo.totalTime = timerInfo.totalTime + (int)(timerInfo.pRunTime - currentMillisecon);
			if (timerInfo.totalTime <= 0) {
				timerInfo.totalTime = 0;
				deleteTimers.Add (timerInfo.onTimer);
			}
			timerInfo.pRunTime = currentMillisecon;
			StartCoroutine (CallUpCallBack (timerInfo.totalTime, timerInfo.onTimer));
		}
	}

	/// <summary>
	/// 删除己经完成的定时器
	/// </summary>
	private void RemoveFinishTimers() {
		OnTimer removedTimer;
		while (deleteTimers.Count > 0) {
			removedTimer = deleteTimers[0];
			deleteTimers.RemoveAt (0);
			timers.Remove (removedTimer);
		}
	}

	/// <summary>
	/// 唤起回调
	/// </summary>
	/// <returns>The up call back.</returns>
	/// <param name="time">Time.</param>
	/// <param name="onTimer">On timer.</param>
	/// <param name="o">O.</param>
	IEnumerator CallUpCallBack(int time, OnTimer onTimer) {
		if (onTimer == null) {
			yield break;
		}
		yield return new WaitForEndOfFrame ();
		onTimer.Invoke (time);
	}
	/// <summary>
	/// 单例方法
	/// </summary>
	/// <value>The instance.</value>
	public static TimerManager Instance {
		get { 
			if (instance != null) {
				return instance;
			}
			lock (singltonLock) {
				if (instance != null) {
					return instance;
				}

				instance = FindObjectOfType<TimerManager> ();
				if (instance != null) {
					return instance;
				}

				GameObject scriptObject = new GameObject ();
				scriptObject.name = typeof(TimerManager).Name + "_Singleton";
				DontDestroyOnLoad (scriptObject);
				instance = scriptObject.AddComponent<TimerManager> ();
			}
			return instance;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class TimerInfo {

		/// <summary>-
		/// 运行总时间
		/// </summary>
		public int totalTime;

		/// <summary>
		/// 时间回调
		/// </summary>
		public OnTimer onTimer;

		/// <summary>
		/// 定时器每隔多长时间运行一次
		/// </summary>
		public int interval;

		/// <summary>
		/// 前一次回调时间
		/// </summary>
		public long pRunTime;
	}
}


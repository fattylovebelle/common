using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;


public class TestDelegateTimer: MonoBehaviour
{
    private string script =
    @"
        function addTimer()
			TimerManager.Instance:AddTimer(100000, startTimer, 100)
        end

        function startTimer(time)
			print('current time', time)
        end

        function removeTimer()
			TimerManager.Instance:RemoveTimer(startTimer)
        end
		
		function delayRun() 
			DelayManager.Instance:delay(10, test)
		end

		function test()
			print('test delay........')
		end
    ";

    LuaState state = null;
	TestEventListenerTimer listener = null;

    LuaFunction addTimer = null;
	LuaFunction removeTimer = null;

	LuaFunction delayRun = null;
   
    //需要删除的转LuaFunction为委托，不需要删除的直接加或者等于即可
    void Awake()
    {
#if UNITY_5 || UNITY_2017
        Application.logMessageReceived += ShowTips;
#else
        Application.RegisterLogCallback(ShowTips);
#endif
        new LuaResLoader();
        state = new LuaState();
        state.Start();
        LuaBinder.Bind(state);
        Bind(state);

        state.LogGC = true;
        state.DoString(script);
        GameObject go = new GameObject("TestGo");
		listener = (TestEventListenerTimer)go.AddComponent(typeof(TestEventListenerTimer));

		addTimer = state.GetFunction("addTimer");
		removeTimer = state.GetFunction ("removeTimer");
		delayRun = state.GetFunction ("delayRun");
    }

    void Bind(LuaState L)
    {
        L.BeginModule(null);
		TestEventListenerTimerWrap.Register(state);
		//TimerManagerWrap.Register (state);
		//DelayManagerWrap.Register (state);

        L.EndModule();

		DelegateFactory.dict.Add(typeof(TestEventListenerTimer.OnClick), TestEventListener_OnClick);
		DelegateFactory.dict.Add(typeof(TestEventListenerTimer.VoidDelegate), TestEventListener_VoidDelegate);

		DelegateTraits<TestEventListenerTimer.OnClick>.Init(TestEventListener_OnClick);
		DelegateTraits<TestEventListenerTimer.VoidDelegate>.Init(TestEventListener_VoidDelegate);


		DelegateFactory.dict.Add (typeof(TimerManager.OnTimer), TimerManager_OnTimer);
		DelegateTraits<TimerManager.OnTimer>.Init(TimerManager_OnTimer);

		DelegateFactory.dict.Add (typeof(DelayManager.OnDelay), DelayManager_OnDelay);
		DelegateTraits<DelayManager.OnDelay>.Init(DelayManager_OnDelay);
	}

	public TimerManager.OnTimer TimerManager_OnTimer(LuaFunction func, LuaTable self, bool flag)
	{
		if (func == null)
		{
			TimerManager.OnTimer fn = delegate(int param0) { };
			return fn;
		}

		if(!flag)
		{
			TimerManager_OnTimer_Event target = new TimerManager_OnTimer_Event(func);
			TimerManager.OnTimer d = target.Call;
			target.method = d.Method;
			return d;
		}
		else
		{
			TimerManager_OnTimer_Event target = new TimerManager_OnTimer_Event(func, self);
			TimerManager.OnTimer d = target.CallWithSelf;
			target.method = d.Method;
			return d;
		}
	}

	class TimerManager_OnTimer_Event : LuaDelegate
	{
		public TimerManager_OnTimer_Event(LuaFunction func) : base(func) { }
		public TimerManager_OnTimer_Event(LuaFunction func, LuaTable self) : base(func, self) { }

		public void Call(int param0)
		{
			func.BeginPCall();
			func.Push(param0);
			func.PCall();
			func.EndPCall();
		}

		public void CallWithSelf(int param0)
		{
			func.BeginPCall();
			func.Push(self);
			func.Push(param0);
			func.PCall();
			func.EndPCall();
		}
	}


	class DelayManager_OnDelay_Event : LuaDelegate
	{
		public DelayManager_OnDelay_Event(LuaFunction func) : base(func) { }
		public DelayManager_OnDelay_Event(LuaFunction func, LuaTable self) : base(func, self) { }

		public void Call()
		{
			func.Call();
		}

		public void CallWithSelf()
		{
			func.BeginPCall();
			func.Push(self);
			func.PCall();
			func.EndPCall();
		}
	}

	public DelayManager.OnDelay DelayManager_OnDelay(LuaFunction func, LuaTable self, bool flag)
	{
		if (func == null)
		{
			DelayManager.OnDelay fn = delegate() { };
			return fn;
		}

		if(!flag)
		{
			DelayManager_OnDelay_Event target = new DelayManager_OnDelay_Event(func);
			DelayManager.OnDelay d = target.Call;
			target.method = d.Method;
			return d;
		}
		else
		{
			DelayManager_OnDelay_Event target = new DelayManager_OnDelay_Event(func, self);
			DelayManager.OnDelay d = target.CallWithSelf;
			target.method = d.Method;
			return d;
		}
	}

    void CallLuaFunction(LuaFunction func)
    {
        tips = "";
        func.BeginPCall();
        func.Push(listener);
        func.PCall();
        func.EndPCall();                
    }

    //自动生成代码后拷贝过来
    class TestEventListener_OnClick_Event : LuaDelegate
    {
        public TestEventListener_OnClick_Event(LuaFunction func) : base(func) { }

        public void Call(UnityEngine.GameObject param0)
        {
            func.BeginPCall();
            func.Push(param0);
            func.PCall();
            func.EndPCall();
        }
    }

	public static TestEventListenerTimer.OnClick TestEventListener_OnClick(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
			TestEventListenerTimer.OnClick fn = delegate { };
            return fn;
        }

        TestEventListener_OnClick_Event target = new TestEventListener_OnClick_Event(func);
		TestEventListenerTimer.OnClick d = target.Call;
        target.method = d.Method;
        return d;
    }

    class TestEventListener_VoidDelegate_Event : LuaDelegate
    {
        public TestEventListener_VoidDelegate_Event(LuaFunction func) : base(func) { }

        public void Call(UnityEngine.GameObject param0)
        {
            func.BeginPCall();
            func.Push(param0);
            func.PCall();
            func.EndPCall();
        }
    }

	public static TestEventListenerTimer.VoidDelegate TestEventListener_VoidDelegate(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
			TestEventListenerTimer.VoidDelegate fn = delegate { };
            return fn;
        }

        TestEventListener_VoidDelegate_Event target = new TestEventListener_VoidDelegate_Event(func);
		TestEventListenerTimer.VoidDelegate d = target.Call;
        target.method = d.Method;
        return d;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400), tips);

        if (GUI.Button(new Rect(10, 60, 120, 40), "add Timer"))
        {
			CallLuaFunction(addTimer);
        }
        else if (GUI.Button(new Rect(10, 110, 120, 40), "remove Timer"))
        {
			CallLuaFunction (removeTimer);
		}
		else if (GUI.Button(new Rect(10, 10, 120, 40), "delay run"))
		{
			CallLuaFunction (delayRun);
		}
    }

    void Update()
    {
        state.Collect();
        state.CheckTop();        
    }

    void SafeRelease(ref LuaFunction luaRef)
    {
        if (luaRef != null)
        {
            luaRef.Dispose();
            luaRef = null;
        }
    }

    string tips = "";    

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

    void OnApplicationQuit()
    {
        SafeRelease(ref addTimer);
		SafeRelease (ref removeTimer);
		SafeRelease (ref delayRun);
        state.Dispose();
        state = null;
#if UNITY_5 || UNITY_2017
        Application.logMessageReceived -= ShowTips;
#else
        Application.RegisterLogCallback(null);
#endif    
    }
}

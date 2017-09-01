using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;


public class TestDelegate: MonoBehaviour
{
    private string script =
    @"                              
            function DoClick1(go)                
                print('click1 gameObject is '..go.name)                    
            end

            function DoClick2(go)                
                print('click2 gameObject is '..go.name)                              
            end                       

            function AddClick1(listener)       
                if listener.onClick then
                    listener.onClick = listener.onClick + DoClick1                                                    
                else
                    listener.onClick = DoClick1                      
                end                
            end

            function AddClick2(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick + DoClick2                      
                else
                    listener.onClick = DoClick2
                end                
            end

			function test111111(a)
				print(a)
			end

            function SetClick1(listener)
                if listener.onClick then
                    listener.onClick:Destroy()
                end

                listener.onClick = DoClick1
				GEventDispatcher.Instance:dispatcherEvent('aaaaa', 'a')
				GEventDispatcher.Instance:addEventListner('bbbbb', test111111)         
            end

            function RemoveClick1(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick - DoClick1      
                else
                    print('empty delegate')
                end
				GEventDispatcher.Instance:dispatcherEvent('bbbbb', 'aaa')
            end

            function RemoveClick2(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick - DoClick2    
                else
                    print('empty delegate')                                
                end
            end

            --测试重载问题
            function TestOverride(listener)
                listener:SetOnFinished(TestEventListener.OnClick(DoClick1))
                listener:SetOnFinished(TestEventListener.VoidDelegate(DoClick2))

				GEventDispatcher.Instance:removeEventListner('bbbbb', test111111)
            end

            function TestEvent()
                print('this is a event')
            end

            function AddEvent(listener)
                listener.onClickEvent = listener.onClickEvent + TestEvent
            end

            function RemoveEvent(listener)
                listener.onClickEvent = listener.onClickEvent - TestEvent
            end

            local t = {name = 'byself'}

            function t:TestSelffunc()
                print('callback with self: '..self.name)
            end       

            function AddSelfClick(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick + TestEventListener.OnClick(t.TestSelffunc, t)
                else
                    listener.onClick = TestEventListener.OnClick(t.TestSelffunc, t)
                end   
            end     

            function RemoveSelfClick(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick - TestEventListener.OnClick(t.TestSelffunc, t)
                else
                    print('empty delegate')
                end   
            end
    ";

    LuaState state = null;
    TestEventListener listener = null;

    LuaFunction SetClick1 = null;
    LuaFunction AddClick1 = null;
    LuaFunction AddClick2 = null;
    LuaFunction RemoveClick1 = null;
    LuaFunction RemoveClick2 = null;
    LuaFunction TestOverride = null;
    LuaFunction RemoveEvent = null;
    LuaFunction AddEvent = null;
    LuaFunction AddSelfClick = null;
    LuaFunction RemoveSelfClick = null;
   
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
        listener = (TestEventListener)go.AddComponent(typeof(TestEventListener));

        SetClick1 = state.GetFunction("SetClick1");
        AddClick1 = state.GetFunction("AddClick1");
        AddClick2 = state.GetFunction("AddClick2");
        RemoveClick1 = state.GetFunction("RemoveClick1");
        RemoveClick2 = state.GetFunction("RemoveClick2");
        TestOverride = state.GetFunction("TestOverride");
        AddEvent = state.GetFunction("AddEvent");
        RemoveEvent = state.GetFunction("RemoveEvent");

        AddSelfClick = state.GetFunction("AddSelfClick");
        RemoveSelfClick = state.GetFunction("RemoveSelfClick");
    }

    void Bind(LuaState L)
    {
        L.BeginModule(null);
        TestEventListenerWrap.Register(state);

		//
		GEventDispatcherWrap.Register(state);

        L.EndModule();

        DelegateFactory.dict.Add(typeof(TestEventListener.OnClick), TestEventListener_OnClick);
        DelegateFactory.dict.Add(typeof(TestEventListener.VoidDelegate), TestEventListener_VoidDelegate);

        DelegateTraits<TestEventListener.OnClick>.Init(TestEventListener_OnClick);
        DelegateTraits<TestEventListener.VoidDelegate>.Init(TestEventListener_VoidDelegate);

		DelegateFactory.dict.Add (typeof(GEventDispatcher.Listner), EngineGEventDispatcher_Listner);
		DelegateTraits<GEventDispatcher.Listner>.Init(EngineGEventDispatcher_Listner);

		GEventDispatcher.Instance.addEventListner ("aaaaa", test);
	}

	public GEventDispatcher.Listner EngineGEventDispatcher_Listner(LuaFunction func, LuaTable self, bool flag)
	{
		if (func == null)
		{
			GEventDispatcher.Listner fn = delegate(object param0) { };
			return fn;
		}

		if(!flag)
		{
			EngineGEventDispatcher_Listner_Event target = new EngineGEventDispatcher_Listner_Event(func);
			GEventDispatcher.Listner d = target.Call;
			target.method = d.Method;
			return d;
		}
		else
		{
			EngineGEventDispatcher_Listner_Event target = new EngineGEventDispatcher_Listner_Event(func, self);
			GEventDispatcher.Listner d = target.CallWithSelf;
			target.method = d.Method;
			return d;
		}
	}

	class EngineGEventDispatcher_Listner_Event : LuaDelegate
	{
		public EngineGEventDispatcher_Listner_Event(LuaFunction func) : base(func) { }
		public EngineGEventDispatcher_Listner_Event(LuaFunction func, LuaTable self) : base(func, self) { }

		public void Call(object param0)
		{
			func.BeginPCall();
			func.Push(param0);
			func.PCall();
			func.EndPCall();
		}

		public void CallWithSelf(object param0)
		{
			func.BeginPCall();
			func.Push(self);
			func.Push(param0);
			func.PCall();
			func.EndPCall();
		}
	}

	public void test(object o) {
		Debug.Log ("test...................");
		Debug.Log ("test...................");
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

    public static TestEventListener.OnClick TestEventListener_OnClick(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
            TestEventListener.OnClick fn = delegate { };
            return fn;
        }

        TestEventListener_OnClick_Event target = new TestEventListener_OnClick_Event(func);
        TestEventListener.OnClick d = target.Call;
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

    public static TestEventListener.VoidDelegate TestEventListener_VoidDelegate(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
            TestEventListener.VoidDelegate fn = delegate { };
            return fn;
        }

        TestEventListener_VoidDelegate_Event target = new TestEventListener_VoidDelegate_Event(func);
        TestEventListener.VoidDelegate d = target.Call;
        target.method = d.Method;
        return d;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400), tips);

        if (GUI.Button(new Rect(10, 10, 120, 40), " = OnClick1"))
        {
            CallLuaFunction(SetClick1);
        }
        else if (GUI.Button(new Rect(10, 60, 120, 40), " + Click1"))
        {
            CallLuaFunction(AddClick1);
        }
        else if (GUI.Button(new Rect(10, 110, 120, 40), " + Click2"))
        {
            CallLuaFunction(AddClick2);
        }
        else if (GUI.Button(new Rect(10, 160, 120, 40), " - Click1"))
        {
            CallLuaFunction(RemoveClick1);
        }
        else if (GUI.Button(new Rect(10, 210, 120, 40), " - Click2"))
        {
            CallLuaFunction(RemoveClick2);
        }
        else if (GUI.Button(new Rect(10, 260, 120, 40), "+ Click1 in C#"))
        {
            tips = "";
            LuaFunction func = state.GetFunction("DoClick1");
            TestEventListener.OnClick onClick = (TestEventListener.OnClick)DelegateTraits<TestEventListener.OnClick>.Create(func);
            listener.onClick += onClick;
        }        
        else if (GUI.Button(new Rect(10, 310, 120, 40), " - Click1 in C#"))
        {
            tips = "";
            LuaFunction func = state.GetFunction("DoClick1");
            listener.onClick = (TestEventListener.OnClick)DelegateFactory.RemoveDelegate(listener.onClick, func);
            func.Dispose();
            func = null;
        }
        else if (GUI.Button(new Rect(10, 360, 120, 40), "OnClick"))
        {
            if (listener.onClick != null)
            {
                listener.onClick(gameObject);
            }
            else
            {
                Debug.Log("empty delegate!!");
            }
			GEventDispatcher.Instance.dispatcherEvent ("bbbbb", "a------>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        }
        else if (GUI.Button(new Rect(10, 410, 120, 40), "Override"))
        {
            CallLuaFunction(TestOverride);
        }
        else if (GUI.Button(new Rect(10, 460, 120, 40), "Force GC"))
        {
            //自动gc log: collect lua reference name , id xxx in thread 
            state.LuaGC(LuaGCOptions.LUA_GCCOLLECT, 0);
            GC.Collect();
        }
        else if (GUI.Button(new Rect(10, 510, 120, 40), "event +"))
        {
            CallLuaFunction(AddEvent);
        }
        else if (GUI.Button(new Rect(10, 560, 120, 40), "event -"))
        {
            CallLuaFunction(RemoveEvent);
        }
        else if (GUI.Button(new Rect(10, 610, 120, 40), "event call"))
        {
            listener.OnClickEvent(gameObject);
        }
        else if (GUI.Button(new Rect(200, 10, 120, 40), "+self call"))
        {
            CallLuaFunction(AddSelfClick);
        }
        else if (GUI.Button(new Rect(200, 60, 120, 40), "-self call"))
        {
            CallLuaFunction(RemoveSelfClick);
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
        SafeRelease(ref AddClick1);
        SafeRelease(ref AddClick2);
        SafeRelease(ref RemoveClick1);
        SafeRelease(ref RemoveClick2);
        SafeRelease(ref SetClick1);
        SafeRelease(ref TestOverride);
        state.Dispose();
        state = null;
#if UNITY_5 || UNITY_2017
        Application.logMessageReceived -= ShowTips;
#else
        Application.RegisterLogCallback(null);
#endif    
    }
}

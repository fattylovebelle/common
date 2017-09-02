﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class TestEventListenerTimerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(TestEventListenerTimer), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("SetOnFinished", SetOnFinished);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("onClick", get_onClick, set_onClick);
		L.RegVar("TestFunc", get_TestFunc, set_TestFunc);
		L.RegVar("onClickEvent", get_onClickEvent, set_onClickEvent);
		L.RegFunction("OnClick", TestEventListener_OnClick);
		L.RegFunction("VoidDelegate", TestEventListener_VoidDelegate);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetOnFinished(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<TestEventListenerTimer.VoidDelegate>(L, 2))
			{
				TestEventListenerTimer obj = (TestEventListenerTimer)ToLua.CheckObject(L, 1, typeof(TestEventListenerTimer));
				TestEventListenerTimer.VoidDelegate arg0 = (TestEventListenerTimer.VoidDelegate)ToLua.ToObject(L, 2);
				obj.SetOnFinished(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<TestEventListenerTimer.OnClick>(L, 2))
			{
				TestEventListenerTimer obj = (TestEventListenerTimer)ToLua.CheckObject(L, 1, typeof(TestEventListenerTimer));
				TestEventListenerTimer.OnClick arg0 = (TestEventListenerTimer.OnClick)ToLua.ToObject(L, 2);
				obj.SetOnFinished(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: TestEventListener.SetOnFinished");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onClick(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			TestEventListenerTimer obj = (TestEventListenerTimer)o;
			TestEventListenerTimer.OnClick ret = obj.onClick;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index onClick on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TestFunc(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			TestEventListenerTimer obj = (TestEventListenerTimer)o;
			System.Func<bool> ret = obj.TestFunc;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index TestFunc on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onClickEvent(IntPtr L)
	{
		ToLua.Push(L, new EventObject(typeof(TestEventListenerTimer.OnClick)));
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onClick(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			TestEventListenerTimer obj = (TestEventListenerTimer)o;
			TestEventListenerTimer.OnClick arg0 = (TestEventListenerTimer.OnClick)ToLua.CheckDelegate<TestEventListenerTimer.OnClick>(L, 2);
			obj.onClick = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index onClick on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_TestFunc(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			TestEventListenerTimer obj = (TestEventListenerTimer)o;
			System.Func<bool> arg0 = (System.Func<bool>)ToLua.CheckDelegate<System.Func<bool>>(L, 2);
			obj.TestFunc = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index TestFunc on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onClickEvent(IntPtr L)
	{
		try
		{
			TestEventListenerTimer obj = (TestEventListenerTimer)ToLua.CheckObject(L, 1, typeof(TestEventListenerTimer));
			EventObject arg0 = null;

			if (LuaDLL.lua_isuserdata(L, 2) != 0)
			{
				arg0 = (EventObject)ToLua.ToObject(L, 2);
			}
			else
			{
				return LuaDLL.luaL_throw(L, "The event 'TestEventListenerTimer.onClickEvent' can only appear on the left hand side of += or -= when used outside of the type 'TestEventListener'");
			}

			if (arg0.op == EventOp.Add)
			{
				TestEventListenerTimer.OnClick ev = (TestEventListenerTimer.OnClick)arg0.func;
                obj.onClickEvent += ev;
			}
			else if (arg0.op == EventOp.Sub)
			{
				TestEventListenerTimer.OnClick ev = (TestEventListenerTimer.OnClick)arg0.func;
                obj.onClickEvent -= ev;
            }

			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TestEventListener_OnClick(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			LuaFunction func = ToLua.CheckLuaFunction(L, 1);

			if (count == 1)
			{
				Delegate arg1 = DelegateTraits<TestEventListenerTimer.OnClick>.Create(func);
				ToLua.Push(L, arg1);
			}
			else
			{
				LuaTable self = ToLua.CheckLuaTable(L, 2);
				Delegate arg1 = DelegateTraits<TestEventListenerTimer.OnClick>.Create(func, self);
				ToLua.Push(L, arg1);
			}
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TestEventListener_VoidDelegate(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			LuaFunction func = ToLua.CheckLuaFunction(L, 1);

			if (count == 1)
			{
				Delegate arg1 = DelegateTraits<TestEventListenerTimer.VoidDelegate>.Create(func);
				ToLua.Push(L, arg1);
			}
			else
			{
				LuaTable self = ToLua.CheckLuaTable(L, 2);
				Delegate arg1 = DelegateTraits<TestEventListenerTimer.VoidDelegate>.Create(func, self);
				ToLua.Push(L, arg1);
			}
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

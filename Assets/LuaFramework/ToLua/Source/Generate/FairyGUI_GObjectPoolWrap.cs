﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_GObjectPoolWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.GObjectPool), typeof(System.Object));
		L.RegFunction("Clear", Clear);
		L.RegFunction("GetObject", GetObject);
		L.RegFunction("ReturnObject", ReturnObject);
		L.RegFunction("New", _CreateFairyGUI_GObjectPool);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("initCallback", get_initCallback, set_initCallback);
		L.RegVar("count", get_count, null);
		L.RegFunction("InitCallbackDelegate", FairyGUI_GObjectPool_InitCallbackDelegate);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_GObjectPool(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 1);
				FairyGUI.GObjectPool obj = new FairyGUI.GObjectPool(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.GObjectPool.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GObjectPool obj = (FairyGUI.GObjectPool)ToLua.CheckObject<FairyGUI.GObjectPool>(L, 1);
			obj.Clear();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GObjectPool obj = (FairyGUI.GObjectPool)ToLua.CheckObject<FairyGUI.GObjectPool>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			FairyGUI.GObject o = obj.GetObject(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReturnObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GObjectPool obj = (FairyGUI.GObjectPool)ToLua.CheckObject<FairyGUI.GObjectPool>(L, 1);
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			obj.ReturnObject(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_initCallback(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GObjectPool obj = (FairyGUI.GObjectPool)o;
			FairyGUI.GObjectPool.InitCallbackDelegate ret = obj.initCallback;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index initCallback on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_count(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GObjectPool obj = (FairyGUI.GObjectPool)o;
			int ret = obj.count;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index count on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_initCallback(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GObjectPool obj = (FairyGUI.GObjectPool)o;
			FairyGUI.GObjectPool.InitCallbackDelegate arg0 = (FairyGUI.GObjectPool.InitCallbackDelegate)ToLua.CheckDelegate<FairyGUI.GObjectPool.InitCallbackDelegate>(L, 2);
			obj.initCallback = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index initCallback on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FairyGUI_GObjectPool_InitCallbackDelegate(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			LuaFunction func = ToLua.CheckLuaFunction(L, 1);

			if (count == 1)
			{
				Delegate arg1 = DelegateTraits<FairyGUI.GObjectPool.InitCallbackDelegate>.Create(func);
				ToLua.Push(L, arg1);
			}
			else
			{
				LuaTable self = ToLua.CheckLuaTable(L, 2);
				Delegate arg1 = DelegateTraits<FairyGUI.GObjectPool.InitCallbackDelegate>.Create(func, self);
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


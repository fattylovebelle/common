﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaFramework_ResourceManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaFramework.ResourceManager), typeof(Manager));
		L.RegFunction("Initialize", Initialize);
		L.RegFunction("LoadPrefab", LoadPrefab);
		L.RegFunction("UnloadAssetBundle", UnloadAssetBundle);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Instance", get_Instance, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Initialize(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			LuaFramework.ResourceManager obj = (LuaFramework.ResourceManager)ToLua.CheckObject<LuaFramework.ResourceManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			System.Action arg1 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 3);
			obj.Initialize(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadPrefab(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 4 && TypeChecker.CheckTypes<string[], LuaInterface.LuaFunction>(L, 3))
			{
				LuaFramework.ResourceManager obj = (LuaFramework.ResourceManager)ToLua.CheckObject<LuaFramework.ResourceManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				string[] arg1 = ToLua.ToStringArray(L, 3);
				LuaFunction arg2 = ToLua.ToLuaFunction(L, 4);
				obj.LoadPrefab(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes<string[], System.Action<UnityEngine.Object[]>>(L, 3))
			{
				LuaFramework.ResourceManager obj = (LuaFramework.ResourceManager)ToLua.CheckObject<LuaFramework.ResourceManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				string[] arg1 = ToLua.ToStringArray(L, 3);
				System.Action<UnityEngine.Object[]> arg2 = (System.Action<UnityEngine.Object[]>)ToLua.ToObject(L, 4);
				obj.LoadPrefab(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes<string, System.Action<UnityEngine.Object[]>>(L, 3))
			{
				LuaFramework.ResourceManager obj = (LuaFramework.ResourceManager)ToLua.CheckObject<LuaFramework.ResourceManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				string arg1 = ToLua.ToString(L, 3);
				System.Action<UnityEngine.Object[]> arg2 = (System.Action<UnityEngine.Object[]>)ToLua.ToObject(L, 4);
				obj.LoadPrefab(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFramework.ResourceManager.LoadPrefab");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAssetBundle(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				LuaFramework.ResourceManager obj = (LuaFramework.ResourceManager)ToLua.CheckObject<LuaFramework.ResourceManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				obj.UnloadAssetBundle(arg0);
				return 0;
			}
			else if (count == 3)
			{
				LuaFramework.ResourceManager obj = (LuaFramework.ResourceManager)ToLua.CheckObject<LuaFramework.ResourceManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.UnloadAssetBundle(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFramework.ResourceManager.UnloadAssetBundle");
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
	static int get_Instance(IntPtr L)
	{
		try
		{
			ToLua.Push(L, LuaFramework.ResourceManager.Instance);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}


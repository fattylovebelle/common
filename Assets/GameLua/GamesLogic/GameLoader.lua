require("Engine/Base/BaseClass")
require("Engine/Net/NetManager")


GameLoader = GameLoader or BaseClass()

-- 网络连接
local isInit = false


-- 初始化方法
function GameLoader:__init( ... )
	-- body
	-- 监听网络变化
	GEventDispatcher.Instance:AddEventListner(CommonEvents.CONNECT_SUCCESS, self.ConnectSuccess)
	-- 连接服务器
	NetManager.GetInstance():ConnectToServer()
end


-- 析构化方法
function GameLoader:__delete( ... )
	-- body
	print("GameLoader delete")
	GEventDispatcher.Instance:RemoveEventDispatcher(CommonEvents.CONNECT_SUCCESS)
end


-- 网络链接成功，事件
function GameLoader:ConnectSuccess( ... )
	if self.isInit then 
		return
	end
	self.isInit = true
end


-- 单例
function GameLoader.GetInstance()
	-- body
	if GameLoader.Instance == nil then
		GameLoader.Instance = GameLoader.New()
	end
	return GameLoader.Instance
end
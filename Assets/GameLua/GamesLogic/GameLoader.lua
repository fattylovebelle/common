require("Engine/Base/BaseClass")
require("Engine/Net/NetManager")


GameLoader = GameLoader or BaseClass()

-- 网络连接
local isInit = false


-- 初始化方法
function GameLoader:__init( ... )
	-- body
	GEventDispatcher.Instance:addEventListner(CommonEvents.CONNECT_SUCCESS)
	NetManager.GetInstance():ConnectToServer()
end


-- 析构化方法
function GameLoader:__delete( ... )
	-- body
	print("GameLoader delete")
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
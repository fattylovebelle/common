require("GamesLogic/GameLoader")
--管理器--
GameEntrance = {};

-- 当前游戏对象
local this = GameEntrance

-- 游戏加载类
this.gameLoader = nil

--初始化完成，发送链接服务器信息--
function GameEntrance.Init()
	print("GameEntrence aa")
	this.gameLoader = GameLoader.GetInstance()
end

-- 销毁
function GameEntrance.delete( ... )
	-- body
	this.gameLoader.Destroy()
	this.gameLoader = nil
end
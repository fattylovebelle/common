require("Egine/Base/BaseClass")
require("Egine/Net/NetManager")

GameLoader = GameLoader or BaseClass()


--[[
初始化方法
--]]
function GameLoader:__Init( ... )
	-- body
	print("GameLoader init")
	local netManager1 = NetManager.New()
	local netManager2 = NetManager.New()
end


--[[
析构化方法
--]]
function GameLoader:__Delete( ... )
	-- body
	print("GameLoader delete")
end

function GetInstance( ... )
	-- body
	if GameLoader.Instance == nil then
		GameLoader.Instance = GameLoader.New()
	end
	return GameLoader.Instance
end
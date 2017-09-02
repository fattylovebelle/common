require("Engine/Base/BaseClass")

NetManager = NetManager or BaseClass()

-- 初始化函数
function NetManager:__Init( ... )
	-- body
	print("NetManager init")
	GEventDispatcher.Instance:addEventListner("aaaaaaaaa", )
end


-- 析构函数
function NetManager:__Delete( ... )
	-- body
	print("NetManager delete")
end


-- 网络断开链接
function NetManager:NetClose( ... )
	-- body
end


-- 网络连接成功
function NetManager:NetConnected( ... )
	-- body
end

-- 网络链接失败
function NetManager:NetConnectError( ... )
	-- body
end


-- 开始网络连接
function NetManager:Connect( ... )
	-- body
end
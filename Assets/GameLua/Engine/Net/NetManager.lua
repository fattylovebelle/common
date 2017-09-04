require("Engine/Base/BaseClass")

NetManager = NetManager or BaseClass()

-- 初始化函数
function NetManager:__init( ... )
	-- body
	print("NetManager init")
	GEventDispatcher.Instance:addEventListner(CommonEvents.CONNECT_SUCCESS, self.NetConnected)
	GEventDispatcher.Instance:addEventListner(CommonEvents.CONNECT_FAILT, self.NetConnectFail)
	GEventDispatcher.Instance:addEventListner(CommonEvents.CONNECT_CLOSE, self.NetClose)
	GEventDispatcher.Instance:addEventListner(CommonEvents.NET_EXCEPTION, self.NetError)

end

-- 析构函数
function NetManager:__delete( ... )
	-- body
	print("NetManager delete")
	GEventDispatcher.Instance:removeEventListner(CommonEvents.CONNECT_SUCCESS, self.NetConnected)
	GEventDispatcher.Instance:removeEventListner(CommonEvents.CONNECT_FAILT, self.NetConnectFail)
	GEventDispatcher.Instance:removeEventListner(CommonEvents.CONNECT_CLOSE, self.NetClose)
	GEventDispatcher.Instance:removeEventListner(CommonEvents.NET_EXCEPTION, self.NetError)
end


-- 建立网络连接
function NetManager:Connect2Server( ... )
	-- body
	AppConst.SocketPort = 8080
	AppConst.SocketAddress = "192.168.0.1"
	NetworkManager.Instance:SendConnect()
end


-- 网络断开链接
function NetManager:NetClose( ... )
	-- body
	print("NetClose ...");
end


-- 网络连接成功
function NetManager:NetConnected( ... )
	-- body
	print("NetConnected ...")
end

-- 网络链接失败
function NetManager:NetConnectFail( ... )
	-- body
	print("NetConnectFail ...")
end


-- 网络出现固障
function NetManager:NetError( ... )
	-- body
	print("NetError...")
end


-- 网络连接单例
function NetManager.GetInstance()
	-- body
	if NetManager.Instance == nil then
		NetManager.Instance = NetManager.New()
	end
	return NetManager.Instance
end
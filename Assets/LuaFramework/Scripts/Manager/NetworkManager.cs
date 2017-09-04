using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

namespace LuaFramework {
    public class NetworkManager : Manager {
        private SocketClient socket;
        static readonly object m_lockObject = new object();
        static Queue<KeyValuePair<int, ByteBuffer>> mEvents = new Queue<KeyValuePair<int, ByteBuffer>>();

        SocketClient SocketClient {
            get { 
                if (socket == null)
                    socket = new SocketClient();
                return socket;                    
            }
        }


		/// <summary>
		/// 单例
		/// </summary>
		private static NetworkManager instance;

		/// <summary>
		/// 线程锁
		/// </summary>
		private static readonly object singltonLock = new object();


		/// <summary>
		/// 单例方法
		/// </summary>
		/// <value>The instance.</value>
		public static NetworkManager Instance {
			get { 
				if (instance != null) {
					return instance;
				}
				lock (singltonLock) {
					if (instance != null) {
						return instance;
					}

					instance = FindObjectOfType<NetworkManager> ();
					if (instance != null) {
						return instance;
					}

					GameObject scriptObject = new GameObject ();
					scriptObject.name = typeof(NetworkManager).Name + "_Singleton";
					DontDestroyOnLoad (scriptObject);
					instance = scriptObject.AddComponent<NetworkManager> ();
				}
				return instance;
			}
		}

        void Awake() {
            Init();
        }

        void Init() {
            SocketClient.OnRegister();
        }

        public void OnInit() {
            CallMethod("Start");
        }

        public void Unload() {
            CallMethod("Unload");
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public object[] CallMethod(string func, params object[] args) {
            return Util.CallMethod("Network", func, args);
        }

        ///------------------------------------------------------------------------------------
        public static void AddEvent(int _event, ByteBuffer data) {
			switch (_event) {
			// 连接失败，需要重新发送事件
			case Protocal.ConnectFail:
				GEventDispatcher.Instance.dispatcherEvent (CommonEvents.CONNECT_FAILT, CommonEvents.CONNECT_FAILT);
				break;
				// socket断线
			case Protocal.Disconnect:
				GEventDispatcher.Instance.dispatcherEvent (CommonEvents.CONNECT_CLOSE, CommonEvents.CONNECT_CLOSE);
				break;
				// 出现异常
			case Protocal.Exception:
				GEventDispatcher.Instance.dispatcherEvent (CommonEvents.NET_EXCEPTION, CommonEvents.NET_EXCEPTION);
				break;
				// 连接成功
			case Protocal.Connect:
				GEventDispatcher.Instance.dispatcherEvent (CommonEvents.CONNECT_SUCCESS, CommonEvents.CONNECT_SUCCESS);
				break;
			default:
				// 收到协议消息
				lock (m_lockObject) {
					mEvents.Enqueue(new KeyValuePair<int, ByteBuffer>(_event, data));
				}
				break;
			}
        }

        /// <summary>
        /// 交给Command，这里不想关心发给谁。
        /// </summary>
        void Update() {
            if (mEvents.Count > 0) {
                while (mEvents.Count > 0) {
                    KeyValuePair<int, ByteBuffer> _event = mEvents.Dequeue();
                    facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);
                }
            }
        }

        /// <summary>
        /// 发送链接请求
        /// </summary>
        public void SendConnect() {
            SocketClient.SendConnect();
        }

        /// <summary>
        /// 发送SOCKET消息
        /// </summary>
        public void SendMessage(ByteBuffer buffer) {
            SocketClient.SendMessage(buffer);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        new void OnDestroy() {
            SocketClient.OnRemove();
            Debug.Log("~NetworkManager was destroy");
        }
    }
}
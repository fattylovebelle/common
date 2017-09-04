using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Reflection;
using System.IO;


namespace LuaFramework {
    public class GameManager2 : Manager {

		/// <summary>
		/// 网络资源下载下来的时候，会以\r\n结尾
		/// </summary>
		public const string CHAR_N = "\n";
		public const string CHAR_R = "\r";

        protected static bool initialize = false;


        /// 初始化游戏管理器
        void Awake() {
            Init();
        }

        /// 初始化
        void Init() {
            DontDestroyOnLoad(gameObject);  //防止销毁自己
			gameObject.SetActive(true);

            CheckExtractResource(); //释放资源
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = AppConst.GameFrameRate;
        }
        
        /// 释放资源
        public void CheckExtractResource() {
			StartCoroutine (dowloadFiles ());
        }

		/// <summary>
		/// 下载热更新资源文件名列表
		/// </summary>
		/// <returns>The files.</returns>
		IEnumerator dowloadFiles() {
			if (AppConst.DebugMode) {
				OnResourceInited ();
				yield break;
			}

			// 如果缓存文件里面没有内容，就把app内的文件copy到缓存里面
			string cacheFiles = Util.CacheDataPath + AppConst.File_LIST;
			if (!File.Exists (cacheFiles)) {
				string localFile = Util.AppContentPath () + AppConst.File_LIST;
				if (Application.platform == RuntimePlatform.Android) {
					WWW www11 = new WWW (localFile);
					yield return www11;
					if (www11.isDone) {
						string path = Path.GetDirectoryName (cacheFiles);
						if (!Directory.Exists (path)) {
							DirectoryInfo dire = Directory.CreateDirectory (path);
						}
						File.WriteAllBytes (cacheFiles, www11.bytes);
					}
				} else {
					File.Copy (localFile, cacheFiles);
				}
			}

			// 读取本地缓存file.text文件异常
			string[] cacheContents = File.ReadAllLines (cacheFiles);
			if (cacheContents == null || cacheContents.Length <= 0) {
				//GlobalDispatcher.GetInstance ().DispatchEvent (NotiConst.LOAD_GAME_ERROR, "本地files.txt读取错误");
				yield break;
			}


			string remoteFiles = AppConst.WebUrl + AppConst.File_LIST + "?radom=" + DateTime.Now.ToString("yyyymmddhhmmss");
			WWW www = new WWW (remoteFiles);
			yield return www;

			if (!string.IsNullOrEmpty (www.error)) {
				//GlobalDispatcher.GetInstance ().DispatchEvent (NotiConst.LOAD_GAME_ERROR, www.error);
				yield break;
			}

			// 热更新列表为空，直接走下一步
			if (string.IsNullOrEmpty (www.text)) {
				StartCoroutine (CompareCacheFileFromLocalFile());
				yield break;
			}
			string remoteContentString = www.text.Replace(CHAR_N, "");
			string[] remoteContents = remoteContentString.Split(CHAR_R.ToCharArray());
			if (remoteContents == null || remoteContents.Length <= 0) {
				StartCoroutine (CompareCacheFileFromLocalFile());
				yield break;
			}

			List<string> cacheContentList = new List<string> ();
			cacheContentList.AddRange (cacheContents);

			StartCoroutine(UpdateRemoteFiles(remoteContents, cacheContentList, remoteContentString));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="remoteFiles">无程更新列表</param>
		/// <param name="cacheFiles">本地lua文件列表</param>
		IEnumerator UpdateRemoteFiles(string[] remoteFiles, List<string> cacheFiles, string remoteContentString) {
			yield return new WaitForSeconds (0.01f);

			// 删除列表里的特殊字符
			removeItemSpecailChars (cacheFiles);

			string[] outerFileInfos = null;
			string outerFileInfoString = null;
			string random = DateTime.Now.ToString("yyyymmddhhmmss");

			for (int outerIndex = 0; outerIndex < remoteFiles.Length; outerIndex++) {
				outerFileInfoString = remoteFiles[outerIndex];
				if (string.IsNullOrEmpty (outerFileInfoString)) {
					continue;
				}

				// 删除字符串里的特殊字符
				if (outerFileInfoString.Contains (CHAR_R)) {
					outerFileInfoString = outerFileInfoString.Replace(CHAR_R, "");
				}
				if (outerFileInfoString.Contains (CHAR_N)) {
					outerFileInfoString = outerFileInfoString.Replace(CHAR_N, "");
				}

				outerFileInfos = outerFileInfoString.Split ('|');
				if (outerFileInfos == null || outerFileInfos.Length < 2) {
					continue;
				}

				// 本地文件版本号与远程版本号文件对比，如果不一样就需要更新
				bool needToUpdate = true;
				// 本地缓存数据
				string updateFileName = outerFileInfos [0];

				int index = cacheFiles.IndexOf (outerFileInfoString);
				if (index < 0) {
					needToUpdate = true;
				} else {
					// 缓存里面没有数据
					if (string.IsNullOrEmpty (cacheFiles[index])) {
						needToUpdate = true;
						continue;
					}
					string cacheFile = cacheFiles [index];
					// 更新数据为空
					string[] innerFileInfos = cacheFile.Split ('|');
					if (innerFileInfos == null || innerFileInfos.Length < 2) {
						needToUpdate = true;
						continue;
					}

					// 文件的md5相同，不更新
					//TODO 现在是判断md5是否一样，如果一样就不更新
					//TODO 是否需要进一步判断缓存和安装目录里面到底有没有文件，如果没有就强制更新？？？
					if (outerFileInfos [1].Equals (innerFileInfos [1])) {
						needToUpdate = false;
						// 缓存中己经有文件
						string cacheFilePath = Util.CacheDataPath + updateFileName;
						if (File.Exists (cacheFilePath)) {
							continue;
						}
						string appContentData = Util.AppContentPath () + updateFileName;
						// 因为android资源是apk的形式,所以不能直接用File去操作,
						if (Application.platform == RuntimePlatform.Android) {
							WWW copyWWW = new WWW (appContentData);
							yield return copyWWW;
							if (copyWWW.isDone) {
								string path = Path.GetDirectoryName (cacheFilePath);
								if (!Directory.Exists (path)) {
									Directory.CreateDirectory (path);
								}
								File.WriteAllBytes (cacheFilePath, copyWWW.bytes);
							}
						} else {
							// 如果是iOS,就直接把数据copy到缓存里
							string path = Path.GetDirectoryName (cacheFilePath);
							if (!Directory.Exists (path)) {
								Directory.CreateDirectory (path);
							}
							File.Copy(appContentData, cacheFilePath, true);
						}
					} else {
						needToUpdate = true;
					}
				}

				// 下载更新数据
				if (needToUpdate) {
					WWW www = new WWW (AppConst.WebUrl + updateFileName + "?version=" + random);
					yield return www;
					// 加载成功
					if (string.IsNullOrEmpty (www.error)) {
						string localFile = Util.CacheDataPath + updateFileName;
						string path = Path.GetDirectoryName (localFile);
						if (!Directory.Exists (path)) {
							Directory.CreateDirectory (path);
						}
						File.WriteAllBytes (localFile, www.bytes);
					} else {
						// 加载失败,就要在原来的版本号基础上加个资随机数，这样下次进游戏，就会重新加载当前资源
						remoteContentString = remoteContentString.Replace(outerFileInfoString, outerFileInfoString + UnityEngine.Random.Range(1, 100));
					}
				}
			}

			StartCoroutine (WriteRemoteFilesToLocalCache (remoteContentString));
		}


		/// <summary>
		/// 删除列表字符串的特殊字符
		/// </summary>
		/// <param name="target">Target.</param>
		private void removeItemSpecailChars(List<string> target) {
			if (target == null || target.Count <= 0) {
				return;
			}
			string values = null;
			for (int index = 0; index < target.Count; index++) {
				values = target [index];
				if (string.IsNullOrEmpty (values)) {
					continue;
				}
				if (values.Contains (CHAR_N)) {
					values = values.Replace (CHAR_N, "");
				}
				if (values.Contains (CHAR_R)) {
					values = values.Replace (CHAR_R, "");
				}
				target [index] = values;
			}
		}

		/// <summary>
		/// 远程files.text替换或者写在本地cache目录,然后再与本地app内的files.text进行比较
		/// </summary>
		/// <returns>The remote files to local cache.</returns>
		/// <param name="remoteContentString">Remote content string.</param>
		IEnumerator WriteRemoteFilesToLocalCache(string remoteContentString) {
			yield return new WaitForSeconds (0.01f);
			// 如果缓存文件里面没有内容，就把app内的文件copy到缓存里面
			string cacheFiles = Util.CacheDataPath + AppConst.File_LIST;
			File.WriteAllText (cacheFiles, remoteContentString);

			StartCoroutine (CompareCacheFileFromLocalFile ());
		}

		/// <summary>
		/// 本地缓存files.txt与app内的files.text文件进行对比,如果缓存目录和app内同时存在同一个文件
		/// 就把app内的文件加入到lua的忽略列表里面，即不会使用app内的files.lua文件，而是用热更下来的lua
		/// </summary>
		/// <returns>The cache file from local file.</returns>
		IEnumerator CompareCacheFileFromLocalFile() {
			yield return new WaitForSeconds(0.01f);
			// 如果缓存文件里面没有内容，就把app内的文件copy到缓存里面

			facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, "更新完成!!");
			//GlobalDispatcher.GetInstance().DispatchEvent(NotiConst.LOADER_COMPLETED, "更新完成!!");
			OnResourceInited();
		}



        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited() {
#if ASYNC_MODE
			ResourceManager.Instance.Initialize(AppConst.AssetDir, delegate() {
                Debug.Log("================>All Initialize OK!!!<====================");
                this.OnInitialize();
                Screen.sleepTimeout = 30;//休眠
            });
#else
            ResManager.Initialize();
            this.OnInitialize();
#endif
        }

        void OnInitialize() {
            LuaManager.InitStart();
			LuaManager.DoFile("GameEntrance");         //加载游戏
            //LuaManager.DoFile("Logic/Network");      //加载网络
            //NetManager.OnInit();                     //初始化网络
			Util.CallMethod("GameEntrance", "Init");     //初始化完成

            initialize = true;
/*
            //类对象池测试
            var classObjPool = ObjPoolManager.CreatePool<TestObjectClass>(OnPoolGetElement, OnPoolPushElement);
            //方法1
            //objPool.Release(new TestObjectClass("abcd", 100, 200f));
            //var testObj1 = objPool.Get();

            //方法2
            ObjPoolManager.Release<TestObjectClass>(new TestObjectClass("abcd", 100, 200f));
            var testObj1 = ObjPoolManager.Get<TestObjectClass>();

            Debugger.Log("TestObjectClass--->>>" + testObj1.ToString());

            //游戏对象池测试
            var prefab = Resources.Load("TestGameObjectPrefab", typeof(GameObject)) as GameObject;
            var gameObjPool = ObjPoolManager.CreatePool("TestGameObject", 5, 10, prefab);

            var gameObj = Instantiate(prefab) as GameObject;
            gameObj.name = "TestGameObject_01";
            gameObj.transform.localScale = Vector3.one;
            gameObj.transform.localPosition = Vector3.zero;

            ObjPoolManager.Release("TestGameObject", gameObj);
            var backObj = ObjPoolManager.Get("TestGameObject");
            backObj.transform.SetParent(null);

            Debug.Log("TestGameObject--->>>" + backObj);
*/
        }
/*
        /// <summary>
        /// 当从池子里面获取时
        /// </summary>
        /// <param name="obj"></param>
        void OnPoolGetElement(TestObjectClass obj) {
            Debug.Log("OnPoolGetElement--->>>" + obj);
        }

        /// <summary>
        /// 当放回池子里面时
        /// </summary>
        /// <param name="obj"></param>
        void OnPoolPushElement(TestObjectClass obj) {
            Debug.Log("OnPoolPushElement--->>>" + obj);
        }
*/
        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy() {
            if (NetManager != null) {
                NetManager.Unload();
            }
            if (LuaManager != null) {
                LuaManager.Close();
            }
            Debug.Log("~GameManager was destroyed");
        }
    }
}
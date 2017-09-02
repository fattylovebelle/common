using System;


/// <summary>
/// 时间工具
/// </summary>
public class TimeUtil {

	/// <summary>
	/// 获取当前毫秒数
	/// </summary>
	/// <returns>The millisecon.</returns>
	public static long CurrentMillisecon() {
		return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JPush;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class ROPush {

		public static bool hasInit = false;
		/// <summary>
		/// 初始化 JPush。
		/// </summary>
		/// <param name="gameObject">游戏对象名。</param>
		public static void Init(string gameObject)
		{
			JPushBinding.Init (gameObject);
			hasInit = true;
		}

		/// <summary>
		/// 设置是否开启 Debug 模式。
		/// <para>Debug 模式将会输出更多的日志信息，建议在发布时关闭。</para>
		/// </summary>
		/// <param name="enable">true: 开启；false: 关闭。</param>
		public static void SetDebug(bool enable)
		{
			JPushBinding.SetDebug (enable);
		}

		/// <summary>
		/// 获取当前设备的 Registration Id。
		/// </summary>
		/// <returns>设备的 Registration Id。</returns>
		public static string GetRegistrationId()
		{
			return JPushBinding.GetRegistrationId ();
		}

		/// <summary>
		/// 为设备设置标签（tag）。
		/// <para>注意：这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。</para>
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		/// <param name="tags">
		///     标签列表。
		///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
		///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
		///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
		/// </param>
		public static void SetTags(int sequence, List<string> tags)
		{
			JPushBinding.SetTags (sequence, tags);
		}

		/// <summary>
		/// 为设备新增标签（tag）。
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		/// <param name="tags">
		///     标签列表。
		///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
		///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
		///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
		/// </param>
		public static void AddTags(int sequence, List<string> tags)
		{
			JPushBinding.AddTags (sequence, tags);
		}

		/// <summary>
		/// 删除标签（tag）。
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		/// <param name="tags">
		///     标签列表。
		///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
		///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
		///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
		/// </param>
		public static void DeleteTags(int sequence, List<string> tags)
		{
			JPushBinding.DeleteTags (sequence, tags);
		}

		/// <summary>
		/// 清空当前设备设置的标签（tag）。
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		public static void CleanTags(int sequence)
		{
			JPushBinding.CleanTags (sequence);
		}

		/// <summary>
		/// 获取当前设备设置的所有标签（tag）。
		/// <para>需要实现 OnJPushTagOperateResult 方法获得操作结果。</para>
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		public static void GetAllTags(int sequence)
		{
			JPushBinding.GetAllTags (sequence);
		}

		/// <summary>
		/// 查询指定标签的绑定状态。
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		/// <param name="tag">待查询的标签。</param>
		public static void CheckTagBindState(int sequence, string tag)
		{
			JPushBinding.CheckTagBindState (sequence, tag);
		}

		/// <summary>
		/// 设置别名。
		/// <para>注意：这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。</para>
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		/// <param name="alias">
		///     别名。
		///     <para>有效的别名组成：字母（区分大小写）、数字、下划线、汉字、特殊字符@!#$&*+=.|。</para>
		///     <para>限制：alias 命名长度限制为 40 字节（判断长度需采用 UTF-8 编码）。</para>
		/// </param>
		public static void SetAlias(int sequence, string alias)
		{
			JPushBinding.SetAlias (sequence, alias);
		}

		/// <summary>
		/// 删除别名。
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		public static void DeleteAlias(int sequence)
		{
			JPushBinding.DeleteAlias (sequence);
		}

		/// <summary>
		/// 获取当前设备设置的别名。
		/// </summary>
		/// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
		public static void GetAlias(int sequence)
		{
			JPushBinding.GetAlias (sequence);
		}

		//IOS
		public static void SetBadge(int badge)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.SetBadge (badge);
			#endif		
		}
		
		public static void ResetBadge()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.ResetBadge ();
			#endif	
		}

		public static void SetApplicationIconBadgeNumber(int badge)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.SetApplicationIconBadgeNumber (badge);
			#endif
		}

		public static int GetApplicationIconBadgeNumber()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				return JPushBinding.GetApplicationIconBadgeNumber ();
			#endif

			return 0;
		}

		public static void StartLogPageView(string pageName)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.StartLogPageView (pageName);
			#endif
		}

		public static void StopLogPageView(string pageName)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.StopLogPageView (pageName);
			#endif
		}
			
		public static void BeginLogPageView(string pageName, int duration)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.BeginLogPageView (pageName, duration);
			#endif
		}

		public static void SendLocalNotification(string localParams)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.SendLocalNotification (localParams);
			#endif
		}

		public static void SetLocalNotification(int delay, string content, int badge, string idKey) 
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.SetLocalNotification (delay, content, badge, idKey);
			#endif
		}

		public static void DeleteLocalNotificationWithIdentifierKey(string idKey) 
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.DeleteLocalNotificationWithIdentifierKey (idKey);
			#endif
		}

		public static void ClearAllLocalNotifications() {
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

			#elif UNITY_IOS
				JPushBinding.ClearAllLocalNotifications ();
			#endif
		}


		public static void StopPush() {
			#if UNITY_ANDROID && !UNITY_EDITOR
			JPushBinding.StopPush ();
			#endif
		}

		public static void ResumePush() {
			#if  UNITY_ANDROID && !UNITY_EDITOR
			JPushBinding.ResumePush ();
			#endif
		}
	}
}
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ghost.Utility;
using SLua;

namespace RO
{
	public class ROProtolFactory : LuaObject
	{
		public static int uncompressLimitSize = 1024;
		public static int sendBufferCapacity = 100;
		public static int receiveBufferCapacity = 1024;
		public static ObjectPool<ROProtol_PB> pool_protol_PB = new ObjectPool<ROProtol_PB>();

		#region lua
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static public int get_uncompressLimitSize(IntPtr l) {
			try {
				pushValue(l,true);
				pushValue(l,uncompressLimitSize);
				return 2;
			}
			catch(Exception e) {
				return error(l,e);
			}
		}
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static public int set_uncompressLimitSize(IntPtr l) {
			try {
				Int32 v;
				checkType(l,2,out v);
				uncompressLimitSize=v;
				pushValue(l,true);
				return 1;
			}
			catch(Exception e) {
				return error(l,e);
			}
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static public int get_sendBufferCapacity(IntPtr l) {
			try {
				pushValue(l,true);
				pushValue(l,sendBufferCapacity);
				return 2;
			}
			catch(Exception e) {
				return error(l,e);
			}
		}
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static public int set_sendBufferCapacity(IntPtr l) {
			try {
				Int32 v;
				checkType(l,2,out v);
				sendBufferCapacity=v;
				pushValue(l,true);
				return 1;
			}
			catch(Exception e) {
				return error(l,e);
			}
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static public int get_receiveBufferCapacity(IntPtr l) {
			try {
				pushValue(l,true);
				pushValue(l,receiveBufferCapacity);
				return 2;
			}
			catch(Exception e) {
				return error(l,e);
			}
		}
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static public int set_receiveBufferCapacity(IntPtr l) {
			try {
				Int32 v;
				checkType(l,2,out v);
				receiveBufferCapacity=v;
				pushValue(l,true);
				return 1;
			}
			catch(Exception e) {
				return error(l,e);
			}
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		public static int CreateProtol_PB(IntPtr l)
		{
			try 
			{
				var ret = CreateProtol_PB(sendBufferCapacity, l);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			catch(Exception e) 
			{
				return error(l,e);
			}
		}
		
		public static void reg(IntPtr l)
		{
			getTypeTable(l, "RO.ROProtolFactory");
			addMember(l,"uncompressLimitSize",get_uncompressLimitSize,set_uncompressLimitSize,false);
			addMember(l,"sendBufferCapacity",get_sendBufferCapacity,set_sendBufferCapacity,false);
			addMember(l,"receiveBufferCapacity",get_receiveBufferCapacity,set_receiveBufferCapacity,false);
			addMember(l, CreateProtol_PB, false);
			createTypeMetatable(l, null, typeof(ROProtolFactory));
		}
		#endregion lua

		public static ROProtol_PB CreateProtol_PB(int capacity, int ID)
		{
			var p = pool_protol_PB.SafeCreate();
			p.Construct(capacity, ID);
			return p;
		}

		public static ROProtol_PB CreateProtol_PB(int capacity, IntPtr l)
		{
			var p = pool_protol_PB.SafeCreate();
			p.Construct(capacity, l);
			return p;
		}

		public static void DestroyProtol_PB(ROProtol_PB p)
		{
			pool_protol_PB.SafeDestroy(p);
		}
	}

	public interface IROProtol
	{
		int GetID();
		byte[] GetData();
		int GetDataLength();
		void Compress();
		void Decompress();
	}

	[SLua.CustomLuaClassAttribute]
	public class ROProtol_PB : IROProtol, IReuseableObject, IDisposable
	{
		public static int Flag_None = 0;
		public static int Flag_Compressed = 1;

		public static int HeadLength = 3;
		public static int IDLength = 2;

		#region serialization
		public static int SerializeHead(byte[] buffer, int bufferIndex, int flag, int dataLength)
		{
			buffer[bufferIndex++] = Convert.ToByte(Flag_None);
			buffer[bufferIndex++] = Convert.ToByte((dataLength & 0xff)); 
			buffer[bufferIndex++] = Convert.ToByte((dataLength >> 8) & 0xff);
			return bufferIndex;
		}
		public static int UnserializeHead(byte[] buffer, int bufferIndex, out int flag, out int dataLength)
		{
			flag = Convert.ToInt32(buffer[bufferIndex++]);
			dataLength = Convert.ToInt32(BitConverter.ToUInt16(buffer, bufferIndex));
			bufferIndex += 2;
			return bufferIndex;
		}

		public static int SerializePID(byte[] buffer, int bufferIndex, int pID1, int pID2)
		{
			buffer[bufferIndex++] = Convert.ToByte(pID1);
			buffer[bufferIndex++] = Convert.ToByte(pID2);
			return bufferIndex;
		}
		public static int UnserializePID(byte[] buffer, int bufferIndex, out int pID1, out int pID2)
		{
			pID1 = Convert.ToInt32(buffer[bufferIndex++]);
			pID2 = Convert.ToInt32(buffer[bufferIndex++]);
			return bufferIndex;
		}
		#endregion serialization

		public int ID{get; private set;}
		public int pID1{get; private set;}
		public int pID2{get; private set;}
		public MemoryStream stream{get; private set;}

		public ROProtol_PB()
		{
			stream = new MemoryStream();
		}

		public void SetDataLength(int len)
		{
			stream.SetLength(len);
		}

		public void Unserialize()
		{
			int a1, a2;
			UnserializePID(stream.GetBuffer(), HeadLength, out a1, out a2);
		}

		#region IROProtol
		public int GetID()
		{
			return ID;
		}
		public byte[] GetData()
		{
			return stream.GetBuffer();
		}
		public int GetDataLength()
		{
			return (int)stream.Length;
		}
		public void Compress()
		{
			// TODO
		}
		public void Decompress()
		{
			// TODO
		}
		#endregion IROProtol

		#region lua
		[SLua.DoNotToLuaAttribute]
		public void ParseLua(IntPtr l)
		{
			Int32 a1;
			LuaObject.checkType(l,1,out a1);
			Int32 a2;
			LuaObject.checkType(l,2,out a2);
			Int32 a3;
			LuaObject.checkType(l,3,out a3);

			ID = a1;
			pID1 = a2;
			pID2 = a3;

			if(LuaDLL.lua_isstring(l,4))
			{
				int strlen;
				IntPtr str = LuaDLL.luaS_tolstring32(l, 4, out strlen);
				if (IntPtr.Zero != str)
				{
					stream.SetLength(HeadLength+IDLength+strlen);
					Marshal.Copy(str, stream.GetBuffer(), HeadLength+IDLength, strlen);
					return;
				}
			}
			stream.SetLength(HeadLength+IDLength);
		}
		#endregion lua

		public void Construct(int capacity, int id)
		{
			stream.Capacity = capacity;
			ID = id;
		}

		public void Construct(int capacity, IntPtr l)
		{
			stream.Capacity = capacity;
			ParseLua(l);
			var dataLength = (int)stream.Length - HeadLength;
			var buffer = stream.GetBuffer();
			var bufferIndex = 0;
			bufferIndex = SerializeHead(buffer, bufferIndex, Flag_None, dataLength);
			bufferIndex = SerializePID(buffer, bufferIndex, pID1, pID2);
		}

		#region IReuseableObject
		// constructor 1(for receive): 1, capacity, ID
		// constructor 2(for send): 2, capacity, luaL
		[SLua.DoNotToLuaAttribute]
		public void Construct()
		{
		}
		[SLua.DoNotToLuaAttribute]
		public void Destruct()
		{
			ID = 0;
			pID1 = 0;
			pID2 = 0;
			stream.SetLength(0);
		}
		[SLua.DoNotToLuaAttribute]
		public bool reused{get;set;}
		
		[SLua.DoNotToLuaAttribute]
		public void Destroy()
		{
			Destruct();
		}
		#endregion IReuseableObject
		
		public void Dispose()
		{
			ROProtolFactory.DestroyProtol_PB(this);
		}
	}
} // namespace RO

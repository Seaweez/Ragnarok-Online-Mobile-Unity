using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class MagicArray<T> where T:new()
	{
		private int blockSize_ = 0;
		private int size_ = 0;
		private T[] array_ = null;

		public int Length
		{
			get
			{
				return size_;
			}
		}

		public T this[int index]
		{
			get
			{
				ResizeByIndex(index);
				var e = array_[index];
				if (null == e)
				{
					e = new T();
					array_[index] = e;
				}
				return e;
			}
			set
			{
				ResizeByIndex(index);
				array_[index] = value;
			}
		}

		public MagicArray(int blockSize)
		{
			blockSize_ = blockSize;
			if (0 >= blockSize_)
			{
				blockSize_ = 1;
			}
			Resize();
		}

		private void ResizeByIndex(int index)
		{
			if (index >= Length)
			{
				size_ = index+1;
				Resize();
			}
		}

		private void Resize()
		{
			var newSize = blockSize_ * (size_ / blockSize_ + 1);
			if (null == array_ || newSize > array_.Length)
			{
				var newArray = new T[newSize];
				if (null != array_)
				{
					for (int i = 0; i < array_.Length; ++i)
					{
						newArray[i] = array_[i];
					}
				}
				array_ = newArray;
			}
		}
	
	}
} // namespace RO

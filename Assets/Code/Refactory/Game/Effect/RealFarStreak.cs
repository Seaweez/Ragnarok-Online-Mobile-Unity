using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO
{
	public class RealFarStreak : MonoBehaviour 
	{
		public class Node
		{
			public Node prevNode = null;
			public Node nextNode = null;
			public Vector3 position;

			private float duration = 0f;
			private Vector3 startPosition;
			private float timeEscaped = 0f;

			public void Reset(float d, Vector3 p)
			{
				duration = d;
				startPosition = p;
				position = p;
				timeEscaped = 0f;
			}

			public bool valid
			{
				get
				{
					return timeEscaped < duration;
				}
			}

			public void Update()
			{
				if (null == prevNode || 0 >= duration)
				{
					return;
				}
				timeEscaped += Time.deltaTime;
				var progress = timeEscaped / duration;
				var endPosition = prevNode.position;
				position = Vector3.Lerp(startPosition, endPosition, progress);
			}
		}


		public RealFar realFar;
		public new LineRenderer renderer;

		public float interval = 0.3f;
		public float distance = 0.3f;

		public float duration = 1f;

		private float nextTime = -1f;
		private Vector3 nextPosition;

		private Node headNode = new Node();
		private Node tailNode = null;
		private int nodeCount = 1;

		public bool empty
		{
			get
			{
				return 1 >= nodeCount;
			}
		}

		private void Push(Vector3 position)
		{
			var newNode = ObjectPool.getObject<Node>();
			newNode.nextNode = headNode.nextNode;
			if (null != headNode.nextNode)
			{
				headNode.nextNode.prevNode = newNode;
			}
			newNode.prevNode = headNode;
			headNode.nextNode  = newNode;

			newNode.Reset(duration, position);

			nextTime = Time.time + interval;
			nextPosition = headNode.position;
			++nodeCount;
			if (null == tailNode)
			{
				tailNode = newNode;
			}
		}

		private void Pop()
		{
			var n = tailNode;
			while (null != n)
			{
				var nextN = n.prevNode;
				if (n.valid)
				{
					tailNode = n;
					tailNode.nextNode = null;
					break;
				}
				ObjectPool.addToPool(n);
				--nodeCount;

				if (nextN == headNode)
				{
					headNode.nextNode = null;
					tailNode = null;
					break;
				}
				n = nextN;
			}
		}

		private void Render()
		{
			if (null != renderer)
			{
				if (null != realFar && 1 < nodeCount)
				{
					renderer.positionCount = nodeCount;
					var n = headNode;
					for (int i = 0; i < nodeCount; ++i)
					{
						renderer.SetPosition(i, realFar.TranslatePoint(n.position));
						n = n.nextNode;
					}
				}
				else
				{
					renderer.positionCount = 0 ;
				}
			}
		}

		public void Clear()
		{
			var n = headNode.nextNode;
			while (null != n)
			{
				var nextNode = n.nextNode;
				ObjectPool.addToPool(n);
				n = nextNode;
			}
			headNode.nextNode = null;
			tailNode = null;
			nodeCount = 1;
			nextTime = -1f;
		}

		void OnDisable()
		{
			Clear();
		}
	
		void LateUpdate()
		{
			if (null != realFar)
			{
				if (0 > nextTime)
				{
					// first init
					nextPosition = realFar.farPosition;
					nextTime = 0;
				}
				if (!GeometryUtils.PositionAlmostEqual(headNode.position, realFar.farPosition))
				{
					headNode.position = realFar.farPosition;
					if (Time.time >= nextTime || Vector3.Distance(headNode.position, nextPosition) >= distance)
					{
						Push(nextPosition);
					}
				}
			}
			var n = headNode.nextNode;
			while (null != n)
			{
				n.Update();
				n = n.nextNode;
			}
			Pop();

			Render();
		}
	}
} // namespace RO

#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public static class EDocker
    {

        #region Reflection Types
        private class _EditorWindow
        {
            public EditorWindow instance;
            private Type type;

            public _EditorWindow(EditorWindow instance)
            {
                this.instance = instance;
                type = instance.GetType();
            }

			_DockArea _Parent;
			public _DockArea m_Parent
            {
                get
                {
					if (_Parent == null) 
					{
						var field = type.GetField ("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic);
						var da = field.GetValue (instance);
						_Parent = new _DockArea (da);
					}
					return _Parent;
                }
				set
				{ 
					var field = type.GetField ("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic);
					field.SetValue (instance, value.instance);
				}
            }

			public bool docked
			{
				get 
				{
					var property = type.GetProperty ("docked", BindingFlags.Instance | BindingFlags.NonPublic);
					return (bool)property.GetValue (instance, null);
				}
			}

			public void RemoveFromDockArea()
			{
				var method = type.GetMethod ("RemoveFromDockArea", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke (instance, null);
			}
        }

        private class _DockArea
        {
			public object instance;
			private Type type;

			public _DockArea()
			{
				Assembly asm = Assembly.GetAssembly (typeof(Editor));
				type = asm.GetType ("UnityEditor.DockArea");
				instance = ScriptableObject.CreateInstance(type);
			}

            public _DockArea(object instance)
            {
                this.instance = instance;
                type = instance.GetType();
			}

			public Rect position
			{
				set
				{
					var property = type.GetProperty ("position", BindingFlags.Instance | BindingFlags.Public);
					property.SetValue (instance, value, null);
				}
			}

            public object window
            {
                get
                {
                    var property = type.GetProperty("window", BindingFlags.Instance | BindingFlags.Public);
                    return property.GetValue(instance, null);
                }
            }

			static _DockArea _OriginalDragSource;
			public static _DockArea s_OriginalDragSource
            {
				get
				{ 
					if (_OriginalDragSource == null) 
					{
						Assembly asm = Assembly.GetAssembly (typeof(Editor));
						Type t = asm.GetType ("UnityEditor.DockArea");
						var field = t.GetField ("s_OriginalDragSource", BindingFlags.Static | BindingFlags.NonPublic);
						var da = field.GetValue (null);
						_OriginalDragSource = new _DockArea (da);
					}
					return _OriginalDragSource;
				}
                set
				{
					Assembly asm = Assembly.GetAssembly (typeof(Editor));
					Type t = asm.GetType ("UnityEditor.DockArea");
					var field = t.GetField("s_OriginalDragSource", BindingFlags.Static | BindingFlags.NonPublic);
					field.SetValue(null, value.instance);
                }
			}

			public void RemoveTab(_EditorWindow window)
			{
				var method = type.GetMethod ("RemoveTab", new Type[]{ typeof(EditorWindow) } );
				method.Invoke (instance, new object[]{ window.instance });
			}

			public void AddTab(_EditorWindow window)
			{
				var method = type.GetMethod ("AddTab", new Type[]{ typeof(EditorWindow) } );
				method.Invoke (instance, new object[]{ window.instance });
			}

			public void MakeVistaDWMHappyDance()
			{
				var method = type.GetMethod ("MakeVistaDWMHappyDance", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke (instance, null);
			}
		}

		private class _View
		{
			public object instance;
			private Type type;

			public _View(object instance)
			{
				this.instance = instance;
				type = instance.GetType();
			}

			public object[] children
			{
				get
				{
					var property = type.GetProperty("children", BindingFlags.Instance | BindingFlags.Public);
					return property.GetValue(instance, null) as object[];
				}
			}
		}

        private class _ContainerWindow
        {
            private object instance;
            private Type type;

            public _ContainerWindow(object instance)
            {
                this.instance = instance;
                type = instance.GetType();
            }

            private int showMode
            {
                get
                {
                    var property = type.GetProperty("showMode", BindingFlags.Instance | BindingFlags.NonPublic);
                    return (int)property.GetValue(instance, null);
                }
            }

            private _View _MainView;
            private _View mainView
            {
                get
                {
                    if(_MainView == null)
					{
						var property = type.GetProperty("rootView", BindingFlags.Instance | BindingFlags.Public);
						var mv = property.GetValue(instance, null);
                        _MainView = new _View(mv);
                    }
                    return _MainView;
                }
            }

            public object rootSplitView
            {
                get
				{
					//Type svType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SplitView");
                    object sv;
                    new _View(mainView);
                    if (showMode == 4 && mainView != null && mainView.children.Length == 3)
                        sv = mainView.children[0];
                    else
						sv = mainView.instance;
					//sv = Convert.ChangeType(sv, svType);
                    return sv;
                }
            }
        }

        private class _SplitView
        {
            private object instance;
            private Type type;

			public bool vertical
			{
				set
				{ 
					var field = type.GetField ("vertical", BindingFlags.Instance | BindingFlags.Public);
					field.SetValue (instance, value);
				}
			}

			public object splitState
			{
				set
				{ 
					var field = type.GetField ("splitState", BindingFlags.Instance | BindingFlags.NonPublic);
					field.SetValue (instance, value);
				}
			}

			public Rect screenPosition
			{
				get
				{
					var property = type.GetProperty ("screenPosition", BindingFlags.Instance | BindingFlags.Public);
					Rect r = (Rect)property.GetValue (instance, null);
					return r;
				}
			}
			private ScriptableObject[] _Children = null;
			public ScriptableObject[] children
			{
				get
				{
					if(_Children == null)
					{
						var property = type.GetProperty("children", BindingFlags.Instance | BindingFlags.Public);
						_Children = property.GetValue(instance, null) as ScriptableObject[];
					}
					return _Children;
				}
			}

			public _SplitView(EditorWindow wd)
			{
				var parent = new _EditorWindow(wd);
				var containerWindow = new _ContainerWindow(parent.m_Parent.window);
				this.instance = containerWindow.rootSplitView;
				this.type = containerWindow.rootSplitView.GetType();
			}

            public _SplitView(object instance)
			{
				this.instance = instance;
				type = instance.GetType();
            }

            public object DragOver(EditorWindow child, Vector2 screenPoint)
            {
                var method = type.GetMethod("DragOver", BindingFlags.Instance | BindingFlags.Public);
                return method.Invoke(instance, new object[] { child, screenPoint });
            }

            public void PerformDrop(EditorWindow child, object dropInfo, Vector2 screenPoint)
            {
                var method = type.GetMethod("PerformDrop", BindingFlags.Instance | BindingFlags.Public);
                method.Invoke(instance, new object[] { child, dropInfo, screenPoint });
            }

			public void MakeRoomForRect(Rect r)
			{
				var method = type.GetMethod ("MakeRoomForRect", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke (instance, new object[]{ r });
			}

			public void AddChild(_DockArea da, int idx)
			{
				Assembly asm = Assembly.GetAssembly (typeof(Editor));
				asm.GetType ("UnityEditor.View");
				var method = type.GetMethod ("AddChild", BindingFlags.Public 
					| BindingFlags.Instance 
					| BindingFlags.DeclaredOnly);
				method.Invoke (instance, new object[]{ da.instance, idx });
			}

			public void Reflow()
			{
				var method = type.GetMethod ("Reflow", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke (instance, null);
			}

			public void RecalcMinMaxAndReflowAll()
			{
				var method = type.GetMethod ("RecalcMinMaxAndReflowAll", BindingFlags.Static | BindingFlags.NonPublic);
				method.Invoke (null, new object[]{ instance });
			}
        }
        #endregion

//        public enum DockPosition
//        {
//            Left,
//            Top,
//            Right,
//            Bottom,
//			BtmLeft
//        }

//        /// <summary>
//        /// Docks the second window to the first window at the given position
//        /// </summary>
//        public static void Dock(this EditorWindow wnd, EditorWindow other, DockPosition position)
//        {
//            var mousePosition = GetFakeMousePosition(wnd, position);
//			DragTo(wnd, other, mousePosition);
//        }
//
//        public static void DockTwo(EditorWindow wnd, EditorWindow first, EditorWindow second)
//        {
//			var mousePosition = GetFakeMousePosition(wnd, DockPosition.Right);
//            DragTo(wnd, first, mousePosition);
//            DragTo(wnd, second, mousePosition);
//        }
//
//        static void DragTo(EditorWindow wnd, EditorWindow other, Vector2 mousePosition)
//        {
//            var parent = new _EditorWindow(wnd);
//			var child = new _EditorWindow(other);
//			var containerWindow = new _ContainerWindow(parent.m_Parent.window);
//            var splitView = new _SplitView(containerWindow.rootSplitView);
//            var dropInfo = splitView.DragOver(other, mousePosition);
//			_DockArea.s_OriginalDragSource = child.m_Parent;
//            splitView.PerformDrop(other, dropInfo, mousePosition);
//        }

		public static bool WindowDocked(EditorWindow wd)
		{
			var window = new _EditorWindow (wd);
			return window.docked;
		}

		/// <summary>
		/// 组合两个窗口.
		/// </summary>
		/// <param name="wnd">主窗口</param>
		/// <param name="other">待组合窗口</param>
		/// <param name="idx">为0表示停靠左侧，不为0表示是右侧第idx个窗口</param>
		public static void Dock(EditorWindow wnd, EditorWindow other, Rect rect, int idx = 0)
		{
			var splitView = new _SplitView(wnd);
			if (splitView.children.Length <= idx)
			{
				var child = new _EditorWindow (other);
				_DockArea.s_OriginalDragSource = child.m_Parent;
				_DockArea childDockArea = new _DockArea ();
				splitView.vertical = false;
				splitView.MakeRoomForRect (rect);
				splitView.AddChild (childDockArea, idx);
				childDockArea.position = rect;
				child.RemoveFromDockArea ();
				child.m_Parent = childDockArea;
				childDockArea.AddTab (child);
				splitView.Reflow ();
				splitView.RecalcMinMaxAndReflowAll ();
				childDockArea.MakeVistaDWMHappyDance ();
			}
		}

//        private static Vector2 GetFakeMousePosition(EditorWindow wnd, DockPosition position)
//        {
//            Rect r = wnd.position;
//            Vector2 mousePosition = Vector2.zero;
//
//            // The 20 is required to make the docking work.
//            // Smaller values might not work when faking the mouse position.
//            switch (position)
//            {
//                case DockPosition.Left:
//                    mousePosition = new Vector2(r.x + 20, r.y + r.height / 2);
//                    break;
//                case DockPosition.Top:
//                    mousePosition = new Vector2(r.x + r.width / 2, r.y + 20);
//                    break;
//                case DockPosition.Right:
//                    mousePosition = new Vector2(r.x + r.width - 20, r.y + r.height / 2);
//                    break;
//                case DockPosition.Bottom:
//                    mousePosition = new Vector2(r.x + r.width / 2, r.y + r.height - 20);
//                    break;
//				case DockPosition.BtmLeft:
//					mousePosition = new Vector2(r.x, r.y + r.height - 40);
//					break;
//            }
//
//            return mousePosition;
//        }
    }
}
#endif
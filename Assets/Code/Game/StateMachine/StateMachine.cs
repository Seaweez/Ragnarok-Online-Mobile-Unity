using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public abstract class State
	{
		public bool running{get;private set;}
		public bool exitPending{get;private set;}

		public bool AllowInterruptedBy(State other)
		{
			return DoAllowInterruptedBy(other);
		}
		
		public bool Enter()
		{
			if (running)
			{
				return true;
			}
			running = DoEnter();
			if (running)
			{
				exitPending = false;
			}
			return running;
		}

		public bool Reset()
		{
			if (!running)
			{
				return Enter();
			}
			else
			{
				if (!DoReset())
				{
					Exit();
					return false;
				}
				return true;
			}
		}
		
		public void Exit()
		{
			if (!running)
			{
				return;
			}
			running = false;
			exitPending = false;
			DoExit();
			#region next state
			nextState = null;
			#endregion next state
		}

		public void DelayExit()
		{
			if (!running)
			{
				return;
			}
			exitPending = true;
		}

		public void Update()
		{
			if (!running)
			{
				return;
			}
			if (exitPending)
			{
				Exit();
				return;
			}
			DoUpdate();
		}

		#region preview
		public bool previewing{get; private set;}
		public void BeginPreview()
		{
			if (previewing)
			{
				return;
			}
			previewing = true;
			DoBeginPreview();
		}
		public void RefreshPreview()
		{
			if (!previewing)
			{
				return;
			}
			DoRefreshPreview();
		}
		public void EndPreview()
		{
			if (!previewing)
			{
				return;
			}
			previewing = false;
			DoEndPreview();
		}
		
		protected virtual void DoBeginPreview()
		{
			DoRefreshPreview();
		}
		protected virtual void DoRefreshPreview()
		{
			
		}
		protected virtual void DoEndPreview()
		{
			
		}
		#endregion preview

		#region next state
		private State nextState_ = null;
		public State nextState
		{
			get
			{
				return nextState_;
			}
			set
			{
				if (value == this)
				{
					return;
				}
				if (value == nextState)
				{
					return;
				}
				var oldNextState = nextState;
				if (null != oldNextState)
				{
					oldNextState.EndPreview();
					oldNextState.nextState = null;
				}
				nextState_ = value;
				if (null != nextState)
				{
					nextState.BeginPreview();
				}
				OnNextStateChanged(oldNextState, nextState);
			}
		}
		public void AppendNextState(State ns)
		{
			if (null == nextState)
			{
				nextState = ns;
			}
			else
			{
				nextState.AppendNextState(ns);
			}
		}
		public virtual bool WillSwitchToNext()
		{
			return null != nextState;
		}
		public virtual bool SwitchToNext()
		{
			var ns = nextState;
			Exit();
			if (null != ns)
			{
				return DoSwitchToNext(ns);
			}
			return false;
		}
		protected virtual bool DoSwitchToNext(State nextState)
		{
			return false;
		}
		#endregion next state

		protected virtual bool DoAllowInterruptedBy(State other)
		{
			if (this == other || !running)
			{
				return true;
			}
			return false;
		}

		protected virtual bool DoApply()
		{
			return true;
		}
		
		protected virtual bool DoEnter()
		{
			return DoApply();
		}

		protected virtual bool DoReset()
		{
			return DoApply();
		}
		
		protected virtual void DoExit()
		{
			
		}

		protected virtual void DoUpdate()
		{
		}

		protected virtual void OnNextStateChanged(State oldNextState, State newNextState)
		{
		}
	}

	[SLua.CustomLuaClassAttribute]
	public class StateMachine<T> where T:State 
	{
		private T currentState_ = null;
		public T currentState
		{
			get
			{
				return currentState_;
			}
			private set
			{
				if (value == currentState)
				{
					if (null != currentState)
					{
						currentState.Reset();
					}
				}
				else
				{
					if (null != currentState)
					{
						currentState.Exit();
					}
					currentState_ = value;
					if (null != currentState)
					{
						currentState.Enter();
					}
				}
			}
		}
		private bool updating{get;set;}

		public bool TrySwitch(T other)
		{
			if (null != currentState
			    && currentState.running
			    && !currentState.AllowInterruptedBy(other))
			{
				#region preview
				if (null != other && other.previewing)
				{
					other.RefreshPreview();
				}
				#endregion preview
				return false;
			}

			currentState = other;
			return null == currentState || currentState.running;
		}

		public void ForceSwitch(T other)
		{
			currentState = other;
		}

		public void Update()
		{
			if (null != currentState)
			{
				currentState.Update();
			}
		}
	
	}
} // namespace RO

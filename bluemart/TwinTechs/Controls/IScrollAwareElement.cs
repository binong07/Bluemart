using System;

namespace TwinTechs.Controls
{
	public class ControlScrollEventArgs : EventArgs
	{
		public float Delta { get; set; }

		public float CurrentY { get; set; }

		public int CurrentRow { get; set; }

		public ControlScrollEventArgs (float delta, float currentY, int currentRow)
		{
			this.Delta = delta;
			this.CurrentY = currentY;
			this.CurrentRow = currentRow;
		}

	}

	public interface IScrollAwareElement
	{
		event EventHandler OnStartScroll;
		event EventHandler OnStopScroll;
		event EventHandler<ControlScrollEventArgs> OnScroll;

		void RaiseOnScroll (float delta, float currentY, int currentRow);

		void RaiseOnStartScroll ();

		void RaiseOnStopScroll ();
	}
}


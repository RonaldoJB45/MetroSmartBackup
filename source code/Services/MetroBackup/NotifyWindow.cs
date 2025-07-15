// NotifyWindow.cs
// Copyright © 2004 by Robert Misiak <rmisiak@users.sourceforge.net>
// All Rights Reserved.
//
// Permission is granted to use, modify and distribute this code, as long as credit is given to the original author, and the copyright notice
// is retained.
//
// Based on a similar implementation used in ChronosXP, an open-source project:  http://chronosxp.sourceforge.net

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MetroFramework.Forms;

namespace TestNotifyWindow
{
    /// <summary>
    /// Display An MSN-Messenger-Style NotifyWindow.
    /// </summary>
    public class NotifyWindow : MetroForm
	{
		#region Public Variables
		/// <summary>
		/// Gets or sets the title text to be displayed in the NotifyWindow.
		/// </summary>
		public string Title;
		/// <summary>
		/// Gets or sets a value specifiying whether or not the window should continue to be displayed if the mouse cursor is inside the bounds
		/// of the NotifyWindow.
		/// </summary>
		public bool WaitOnMouseOver;
		/// <summary>
		/// An EventHandler called when the NotifyWindow title text is clicked.
		/// </summary>
		public event System.EventHandler TitleClicked;
		/// <summary>
		/// Gets or sets the full height of the NotifyWindow, used after the opening animation has been completed.
		/// </summary>
		public int ActualHeight;
		/// <summary>
		/// Gets or sets the full width of the NotifyWindow.
		/// </summary>
		public int ActualWidth;

		public enum ClockStates { Opening, Closing, Showing, None };
		public ClockStates ClockState;
		#endregion

		#region Protected Variables
		protected bool closePressed = false, textPressed = false, titlePressed = false, closeHot = false, textHot = false, titleHot = false;
		protected Rectangle rClose, rText, rTitle, rDisplay, rScreen, rGlobClose, rGlobText, rGlobTitle, rGlobDisplay;
		protected System.Windows.Forms.Timer viewClock;
		#endregion

		#region Constructor
		/// <param name="title">Title text displayed in the NotifyWindow</param>
		/// <param name="text">Main text displayedin the NotifyWindow</param>
		public NotifyWindow (string title, string text) : this() { Title = title; Text = text; }
		/// <param name="text">Text displayed in the NotifyWindow</param>
		public NotifyWindow (string text) : this() { Text = text; }
		public NotifyWindow()
		{
			SetStyle (ControlStyles.UserMouse, true);
			SetStyle (ControlStyles.UserPaint, true);
			SetStyle (ControlStyles.AllPaintingInWmPaint, true);		// WmPaint calls OnPaint and OnPaintBackground
			SetStyle (ControlStyles.DoubleBuffer, true);

			ShowInTaskbar = false;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			StartPosition = System.Windows.Forms.FormStartPosition.Manual;

            //Default values
            
            ClockState = ClockStates.None;
            BackColor = Color.SteelBlue;
            WaitOnMouseOver = true;
            ActualWidth = 130;
            ActualHeight = 110;
        }
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets the width and height of the NotifyWindow.
		/// </summary>
		public void SetDimensions (int width, int height)
		{
			ActualWidth = width;
			ActualHeight = height;
		}

		/// <summary>
		/// Displays the NotifyWindow.
		/// </summary>
		public void Notify()
		{
			//if (Text == null || Text.Length < 1)
			//	throw new System.Exception ("You must set NotifyWindow.Text before calling Notify()");

			Width = ActualWidth;
			rScreen = Screen.GetWorkingArea (Screen.PrimaryScreen.Bounds);
			Height = 0;
			Top = rScreen.Bottom;
			Left = rScreen.Width - Width - 11;
            
			rDisplay = new Rectangle (0, 0, Width, ActualHeight);
			rClose = new Rectangle (Width - 21, 10, 13, 13);

			// rGlob* are Rectangle's Offset'ed to their actual position on the screen, for use with Cursor.Position.
			rGlobClose = rClose;
			rGlobClose.Offset (Left, rScreen.Bottom - ActualHeight);
			rGlobText = rText;
			rGlobText.Offset (Left, rScreen.Bottom - ActualHeight);
			rGlobTitle = rTitle;
			if (Title != null)
				rGlobTitle.Offset (Left, rScreen.Bottom - ActualHeight);
			rGlobDisplay = rDisplay;
			rGlobDisplay.Offset (Left, rScreen.Bottom - ActualHeight);
			rGlobClose = rClose;
			rGlobClose.Offset (Left, rScreen.Bottom - ActualHeight);
			rGlobDisplay = rDisplay;
			rGlobDisplay.Offset (Left, rScreen.Bottom - ActualHeight);

			// Use unmanaged ShowWindow() and SetWindowPos() instead of the managed Show() to display the window - this method will display
			// the window TopMost, but without stealing focus (namely the SW_SHOWNOACTIVATE and SWP_NOACTIVATE flags)
			ShowWindow (Handle, SW_SHOWNOACTIVATE);
			SetWindowPos (Handle, HWND_TOPMOST, rScreen.Width - ActualWidth - 11, rScreen.Bottom, ActualWidth, 0, SWP_NOACTIVATE);

			viewClock = new System.Windows.Forms.Timer();
			viewClock.Tick += new System.EventHandler (viewTimer);
			viewClock.Interval = 1;
			viewClock.Start();

			ClockState = ClockStates.Opening;
		}
		#endregion

		#region Drawing
        
		/// <summary>
		/// Draw a Windows 95 style close button.
		/// </summary>
		protected void drawLegacyCloseButton (Graphics fx)
		{
			ButtonState bState;
			if (closePressed)
				bState = ButtonState.Pushed;
			else // the Windows 95 theme doesn't have a "hot" button
				bState = ButtonState.Normal;
			ControlPaint.DrawCaptionButton (fx, rClose, CaptionButton.Close, bState);
		}
        
		#endregion

		#region Timers and EventHandlers
		protected void viewTimer (object sender, System.EventArgs e)
		{
			switch (ClockState)
			{
				case ClockStates.Opening:
					if (Top - 2 <= rScreen.Height - ActualHeight)
					{
						Top = rScreen.Height - ActualHeight;
						Height = ActualHeight;
					}
					else
					{
						Top -= 2;
						Height += 2;
					}
					break;

				case ClockStates.Showing:
					if (!WaitOnMouseOver || !rGlobDisplay.Contains (Cursor.Position))
					{
						viewClock.Interval = 1;
						ClockState = ClockStates.Closing;
					}
					break;

				case ClockStates.Closing:
					Top += 2;
					Height -= 2;
					if (Top >= rScreen.Height)
					{
						ClockState = ClockStates.None;
						viewClock.Stop();
						viewClock.Dispose();
						Close();
					}
					break;
			}
		}

		protected override void OnMouseMove (System.Windows.Forms.MouseEventArgs e)
		{
			if (Title != null && rGlobTitle.Contains (Cursor.Position) && !textPressed && !closePressed)
			{
				Cursor = Cursors.Hand;
				titleHot = true;
				textHot = false;  closeHot = false;
				Invalidate();
			}
			else if (rGlobText.Contains (Cursor.Position) && !titlePressed && !closePressed)
			{
				Cursor = Cursors.Hand;
				textHot = true;
				titleHot = false;  closeHot = false;
				Invalidate();
			}
			else if (rGlobClose.Contains (Cursor.Position) && !titlePressed && !textPressed)
			{
				Cursor = Cursors.Hand;
				closeHot = true;
				titleHot = false;  textHot = false;
				Invalidate();
			}
			else if ((textHot || titleHot || closeHot) && (!titlePressed && !textPressed && !closePressed))
			{
				Cursor = Cursors.Default;
				titleHot = false;  textHot = false;  closeHot = false;
				Invalidate();
			}
			base.OnMouseMove (e);
		}

		protected override void OnMouseDown (System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (rGlobClose.Contains (Cursor.Position))
				{
					closePressed = true;
					closeHot = false;
					Invalidate();
				}
				else if (rGlobText.Contains (Cursor.Position))
				{
					textPressed = true;
					Invalidate();
				}
				else if (Title != null && rGlobTitle.Contains (Cursor.Position))
				{
					titlePressed = true;
					Invalidate();
				}
			}
			base.OnMouseDown (e);
		}

		protected override void OnMouseUp (System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (closePressed)
				{
					Cursor = Cursors.Default;
					closePressed = false;
					closeHot = false;
					Invalidate();
					if (rGlobClose.Contains (Cursor.Position))
						Close();
				}
				else if (textPressed)
				{
					Cursor = Cursors.Default;
					textPressed = false;
					textHot = false;
					Invalidate();
					if (rGlobText.Contains (Cursor.Position))
					{
						Close();
					}
				}
				else if (titlePressed)
				{
					Cursor = Cursors.Default;
					titlePressed = false;
					titleHot = false;
					Invalidate();
					if (rGlobTitle.Contains (Cursor.Position))
					{
						Close();
						if (TitleClicked != null)
							TitleClicked (this, new System.EventArgs());
					}
				}
			}
			base.OnMouseUp (e);
		}
		#endregion

		#region P/Invoke

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NotifyWindow
            // 
            this.ClientSize = new System.Drawing.Size(658, 300);
            this.Name = "NotifyWindow";
            this.ResumeLayout(false);

        }

		// SetWindowPos()
		protected const Int32 HWND_TOPMOST = -1;
		protected const Int32 SWP_NOACTIVATE = 0x0010;

		// ShowWindow()
		protected const Int32 SW_SHOWNOACTIVATE = 4;
        
        //// user32.dll
        [DllImport("user32.dll")]
        protected static extern bool ShowWindow(IntPtr hWnd, Int32 flags);
        [DllImport("user32.dll")]
        protected static extern bool SetWindowPos(IntPtr hWnd, Int32 hWndInsertAfter, Int32 X, Int32 Y, Int32 cx, Int32 cy, uint uFlags);
        #endregion
    }
}

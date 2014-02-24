using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickConfig
{
	public partial class MainForm : Form
	{
		[DllImport("user32.dll", EntryPoint = "FindWindow")]
		private static extern IntPtr FindWindow(string lp1, string lp2);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);





		private System.Windows.Forms.Timer m_Timer = new System.Windows.Forms.Timer();
		private vJoyInterfaceWrap.vJoy m_Interface = null;
		private bool m_ButtonState = false;

		// Device ID can only be in the range 1-16
		private uint m_Id = 1;

		private vJoyInterfaceWrap.vJoy.JoystickState m_State = new vJoyInterfaceWrap.vJoy.JoystickState();

		private uint m_ButtonId = 0;

		public MainForm()
		{
			InitializeComponent();

			base.Disposed += OnDisposed;

			try
			{
				m_Interface = new vJoyInterfaceWrap.vJoy();

				// Get the driver attributes (Vendor ID, Product ID, Version Number)
				if (!m_Interface.vJoyEnabled())
				{
					Console.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
					return;
				}
				else
				{
					Console.WriteLine("v{0}\nVendor: {1}\nProduct :{2}\nVersion Number:{3}\n", m_Interface.GetvJoyVersion(), m_Interface.GetvJoyManufacturerString(), m_Interface.GetvJoyProductString(), m_Interface.GetvJoySerialNumberString());
				}



				// Check which axes are supported
				bool AxisX = m_Interface.GetVJDAxisExist(m_Id, HID_USAGES.HID_USAGE_X);
				bool AxisY = m_Interface.GetVJDAxisExist(m_Id, HID_USAGES.HID_USAGE_Y);
				bool AxisZ = m_Interface.GetVJDAxisExist(m_Id, HID_USAGES.HID_USAGE_Z);
				bool AxisRX = m_Interface.GetVJDAxisExist(m_Id, HID_USAGES.HID_USAGE_RX);
				bool AxisRZ = m_Interface.GetVJDAxisExist(m_Id, HID_USAGES.HID_USAGE_RZ);

				// Get the number of buttons and POV Hat switchessupported by this vJoy device
				int nButtons = m_Interface.GetVJDButtonNumber(m_Id);
				int ContPovNumber = m_Interface.GetVJDContPovNumber(m_Id);
				int DiscPovNumber = m_Interface.GetVJDDiscPovNumber(m_Id);

				// Spit out the details
				Console.WriteLine(string.Format("Device[{0}]: Buttons={1}; DiscPOVs:{2}; ContPOVs:{3}", m_Id, nButtons, ContPovNumber, DiscPovNumber));



				// Write access to vJoy Device - Basic
				VjdStat status = m_Interface.GetVJDStatus(m_Id);

				// Acquire the target
				if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!m_Interface.AcquireVJD(m_Id))))
				{
					Console.WriteLine(string.Format("Failed to acquire vJoy device number {0}.", m_Id));
				}
				else
				{
					Console.WriteLine(string.Format("Acquired: vJoy device number {0}.", m_Id));

					m_Interface.ResetAll();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			m_Timer.Tick += OnTick;
			m_Timer.Interval = 2000;
			m_Timer.Start();
		}

		private void OnDisposed(object sender, EventArgs e)
		{
			try
			{
				m_Interface.RelinquishVJD(m_Id);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void OnTick(object sender, EventArgs e)
		{
			m_Timer.Enabled = false;

			//IntPtr handle = FindWindow(null, "ZSNES");
			IntPtr handle = FindWindow(null, "Snes9x v1.53 for Windows");
			if (!handle.Equals(IntPtr.Zero))
			{
				// activate Notepad window
				if (SetForegroundWindow(handle))
				{
					// send key "Tab"
					//SendKeys.Send("{TAB}");
					// send key "Enter"
					//SendKeys.Send("{ENTER}");

					//System.Windows.Forms.SendKeys.Send("{UP}");
					//System.Windows.Forms.SendKeys.Send("{DOWN}");
					//System.Windows.Forms.SendKeys.Send("{LEFT}");
					//System.Windows.Forms.SendKeys.Send("{RIGHT}");

					//InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
					//InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);

					/*
					InputSimulator.SimulateKeyPress(m_CurrentKey);

					if (m_CurrentKey == VirtualKeyCode.UP)
						m_CurrentKey = VirtualKeyCode.DOWN;
					else
						m_CurrentKey = VirtualKeyCode.UP;
					 */

					m_ButtonId = 1;

					try
					{
						m_ButtonId = uint.Parse(this.activeButtonTextBox.Text);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}

					try
					{
						CheckStatus();

						RefreshInput();

						/*
						var state = new vJoyInterfaceWrap.vJoy.JoystickState();
						m_Interface.UpdateVJD(m_Id, ref state);

						if (m_Interface.SetBtn(m_ButtonState, m_Id, buttonId))
						{
							m_ButtonState = !m_ButtonState;
						}
						*/
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}

			m_Timer.Enabled = true;
		}

		private void CheckStatus()
		{
			// Get the state of the requested device
			VjdStat status = m_Interface.GetVJDStatus(m_Id);

			switch (status)
			{
				case VjdStat.VJD_STAT_OWN:
					Console.WriteLine("vJoy Device {0} is already owned by this feeder\n", m_Id);
					break;
				case VjdStat.VJD_STAT_FREE:
					Console.WriteLine("vJoy Device {0} is free\n", m_Id);
					break;
				case VjdStat.VJD_STAT_BUSY:
					Console.WriteLine(
					"vJoy Device {0} is already owned by another feeder\nCannot continue\n", m_Id);
					return;
				case VjdStat.VJD_STAT_MISS:
					Console.WriteLine(
					"vJoy Device {0} is not installed or disabled\nCannot continue\n", m_Id);
					return;
				default:
					Console.WriteLine("vJoy Device {0} general error\nCannot continue\n", m_Id);
					return;
			};
		}

		private void RefreshInput()
		{
			// Feed the device id
			m_State.bDevice = (byte)m_Id;

			// Feed position data per axis
			m_State.AxisX = 0;
			m_State.AxisY = 0;
			m_State.AxisZ = 0;
			m_State.AxisZRot = 0;
			m_State.AxisXRot = 0;

			// Set buttons one by one
			m_State.Buttons = 0;

			if (m_ButtonState)
				m_State.Buttons = (uint)(0x1 << (int)m_ButtonId);

			// Feed the driver with the position packet
			m_Interface.UpdateVJD(m_Id, ref m_State);

			m_ButtonState = !m_ButtonState;
		}

		private void exitButton_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}

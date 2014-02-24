using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class Joystick
    {
		private vJoyInterfaceWrap.vJoy m_Interface = null;

		public Joystick()
		{
			m_Interface = new vJoyInterfaceWrap.vJoy();

			try
			{
				if (m_Interface.AcquireVJD(1))
				{
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
    }
}

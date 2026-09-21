using System;
using System.Windows.Forms;

namespace Ambertation.Windows.Forms;

/// <summary>
/// A ToolStrip that still delivers the click that activates an inactive window.
/// </summary>
/// <remarks>
/// System.Windows.Forms.ToolStrip.WndProc answers WM_MOUSEACTIVATE for a click on
/// the strip itself with MA_ACTIVATEANDEAT (2) whenever its top-level window is not
/// the active window: the window is activated and the click is swallowed, so the
/// first click on a toolbar button does nothing. That is by design in WinForms, but
/// under Wine the SimPE window is inactive far more often than it appears to be, so
/// it shows up as toolbar buttons that "often need a second click". Turning the
/// answer into MA_ACTIVATE (1) still activates the window but lets the click through.
/// </remarks>
public class ClickThroughToolStrip : ToolStrip
{
	private const int WM_MOUSEACTIVATE = 0x0021;
	private const int MA_ACTIVATE = 1;
	private const int MA_ACTIVATEANDEAT = 2;

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		if (m.Msg == WM_MOUSEACTIVATE && m.Result == (IntPtr)MA_ACTIVATEANDEAT)
		{
			m.Result = (IntPtr)MA_ACTIVATE;
		}
	}
}

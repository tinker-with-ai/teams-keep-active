// Tray app: nudges the mouse 1px and back when you've been idle, so Windows/Teams stay "Active".
// Build: build.bat (uses the C# compiler that ships with Windows).
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

static class KeepAwake
{
    [DllImport("user32.dll")] static extern void mouse_event(uint f, int dx, int dy, uint d, UIntPtr e);
    [DllImport("kernel32.dll")] static extern uint SetThreadExecutionState(uint s);
    [StructLayout(LayoutKind.Sequential)] struct LASTINPUTINFO { public uint cbSize; public uint dwTime; }
    [DllImport("user32.dll")] static extern bool GetLastInputInfo(ref LASTINPUTINFO p);

    static uint IdleMs()
    {
        var i = new LASTINPUTINFO(); i.cbSize = (uint)Marshal.SizeOf(i); GetLastInputInfo(ref i);
        return (uint)Environment.TickCount - i.dwTime;
    }

    static int seconds = 60;
    static bool running = true;
    static NotifyIcon tray;
    static Timer timer;

    [STAThread]
    static void Main()
    {
        bool first;
        using (new System.Threading.Mutex(true, "KeepAwakeTrayApp", out first))
        {
            if (!first) { MessageBox.Show("Keep Awake is already running (see the tray icon)."); return; }
            Application.EnableVisualStyles();

            timer = new Timer();
            timer.Tick += delegate {
                // Only nudge if you haven't touched mouse/keyboard yourself; real input event resets Windows' idle timer.
                if (IdleMs() >= seconds * 1000 - 1000) { mouse_event(1, 1, 0, 0, UIntPtr.Zero); mouse_event(1, -1, 0, 0, UIntPtr.Zero); }
            };

            var menu = new ContextMenuStrip();
            var toggle = new ToolStripMenuItem("Pause");
            toggle.Click += delegate { SetRunning(!running); toggle.Text = running ? "Pause" : "Resume"; };
            var interval = new ToolStripMenuItem("Interval");
            foreach (int s in new[] { 30, 60, 120, 240 })
            {
                int sec = s;
                var item = new ToolStripMenuItem(sec < 60 ? sec + " seconds" : sec / 60 + " minute" + (sec > 60 ? "s" : ""));
                item.Checked = sec == seconds;
                item.Click += delegate {
                    foreach (ToolStripMenuItem m in interval.DropDownItems) m.Checked = false;
                    item.Checked = true; seconds = sec; SetRunning(running);
                };
                interval.DropDownItems.Add(item);
            }
            menu.Items.Add(toggle);
            menu.Items.Add(interval);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Exit", null, delegate { Application.Exit(); });

            tray = new NotifyIcon { Icon = SystemIcons.Shield, ContextMenuStrip = menu, Visible = true };
            tray.DoubleClick += delegate { toggle.PerformClick(); };
            SetRunning(true);
            tray.ShowBalloonTip(3000, "Keep Awake", "Running. Right-click the tray icon to pause or exit.", ToolTipIcon.Info);

            Application.Run();

            tray.Visible = false;
            SetThreadExecutionState(0x80000000); // restore normal sleep
        }
    }

    static void SetRunning(bool on)
    {
        running = on;
        timer.Interval = seconds * 1000;
        timer.Enabled = on;
        // ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED keeps screen/PC awake while active.
        SetThreadExecutionState(on ? 0x80000003u : 0x80000000u);
        tray.Text = on ? "Keep Awake: active (every " + seconds + "s)" : "Keep Awake: paused";
    }
}

# Keep Awake – Stop Microsoft Teams Showing "Away" on Windows

**Keep Awake** is a tiny, free Windows tray app that keeps your **Microsoft Teams status green (Available)** and stops your PC from going idle, locking, or sleeping. It works by nudging the mouse 1 pixel and back when you've been idle — invisible to you, but Windows counts it as real activity.

No install, no admin rights, no internet access, no data collected. One 8 KB `.exe`.

> Built for **Microsoft Teams** (new and classic). It should also help with other apps that mark you "Away" after inactivity, such as Slack or Outlook, since they rely on the same Windows idle timer.

---

## Why use it?

- Teams switches you to **Away / yellow status after ~5 minutes** of no mouse or keyboard input.
- Your **screen turns off or locks** while you're reading, watching a video, or on a long call.
- You need the PC to **stay awake during long downloads, builds, or remote sessions**.

Keep Awake fixes all of these without changing any Windows power or Teams settings.

## Features

- 🟢 Keeps Microsoft Teams status **Available** instead of Away
- 🖱️ **Mouse jiggler**: 1-pixel move-and-back, invisible on screen
- 🧠 **Smart**: only nudges when you're actually idle — never fights your own mouse
- 💤 Prevents **sleep, screen-off and auto-lock** while running
- ⏱️ Choose the interval: 30 seconds, 1, 2 or 4 minutes
- ⏸️ Pause / resume from the system tray (or double-click the icon)
- 🪶 Tiny (8 KB), no installer, no dependencies, no telemetry
- 🔓 Open source: one readable C# file you can build yourself

## Download & run

1. Download **`KeepAwake.exe`** from this repository (click the file → **Download raw file**).
2. Double-click it. A shield icon appears in the system tray near the clock (check the **^** arrow).
3. That's it. Right-click the icon for **Pause / Resume**, **Interval**, or **Exit**.

**Start automatically with Windows:** press `Win + R`, type `shell:startup`, press Enter, and put a shortcut to `KeepAwake.exe` in that folder.

> Windows SmartScreen may warn about an unknown app because the `.exe` isn't code-signed. Choose **More info → Run anyway**, or build it yourself from source (below) if you prefer.

## How it works

Microsoft Teams, Windows lock screen and sleep timers all use the Windows **idle timer** (`GetLastInputInfo`). Keep Awake checks that timer; when you've been idle for the chosen interval, it sends a real mouse input event (1 px right, then 1 px back), which resets the idle timer. It also calls `SetThreadExecutionState` so Windows doesn't sleep or turn the display off while the app is active. Exiting restores normal behaviour.

Tools that only reposition the cursor with `SetCursorPos` **don't** reset the idle timer — that's why Keep Awake uses a genuine input event.

## Build from source

Requires only Windows — it uses the C# compiler that ships with the .NET Framework built into Windows 10/11.

```bat
build.bat
```

This produces `KeepAwake.exe` from `KeepAwake.cs`. Nothing to install.

## FAQ

**How do I keep Microsoft Teams from going Away?**
Run Keep Awake. As long as it's running and not paused, Teams stays Available.

**Does it work with the new Teams (2024+)?**
Yes. It resets the Windows idle timer, which is what new Teams uses for presence.

**Will it move my mouse while I'm working?**
No. It only acts after you've been idle for the full interval, and the move is 1 pixel and back.

**Does it need admin rights or change system settings?**
No. Nothing is installed, and power settings return to normal when you exit.

**Is it safe?**
The full source is one short C# file in this repo. It makes no network connections and writes no files.

## Alternatives

Microsoft PowerToys **Awake** keeps the PC awake but does **not** keep Teams Available. Hardware USB mouse jigglers work but cost money and need a spare port. Keep Awake does both jobs in software for free.

## Disclaimer

Check your employer's IT and acceptable-use policies before using presence or idle tools on a work device. This project is an independent personal experiment and is **not affiliated with or endorsed by Microsoft**. "Microsoft Teams" is a trademark of Microsoft Corporation.

## Keywords

teams keep active, teams always available, teams status away fix, stop teams going away, keep teams green, microsoft teams idle, mouse jiggler windows, mouse mover, prevent screen lock, prevent sleep windows, keep pc awake, caffeine for windows, anti idle, auto mouse mover, tray app

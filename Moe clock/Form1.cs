using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Moe_clock
{
    public partial class Form1 : Form
    {   
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private readonly object _lock = new object();
        private Dictionary<char, Image> _digitCache = new Dictionary<char, Image>();
        private readonly ThemeManager themeManager = new();
        private PictureBox[] digB;
        private Label cL;
        private ContextMenuStrip contextMenuSettings;
        private string CurThemeFolder = "gelbooru";
        private readonly string prPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\"));
        public Form1()
        {   
            InitializeComponent();

            this.Owner = null;
            this.ShowInTaskbar = false;
            themeManager.OnThemeChanged += OnThemeChanged;
            this.FormClosing += (s, e) => 
            {
                foreach (var img in _digitCache.Values) { img.Dispose(); };
                Save();
            };
            this.AllowTransparency = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(255, 1, 2);
            this.TransparencyKey = Color.FromArgb(255, 1, 2);
           
            this.Visible = true;
            
            InitializeDigits();
            CacheDigits();
            Load();
            InitializeContextMenu();

            

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += UpdateTimeDisplay;
            timer.Start();
            UpdateTimeDisplay(null, null);
            
            
            this.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left) DragWindow();
                else if (e.Button == MouseButtons.Right) contextMenuSettings.Show(this, e.Location);
            };
            cL.MouseDown += (s, e) => { 
                if (e.Button == MouseButtons.Left) DragWindow();
                else if (e.Button == MouseButtons.Right) contextMenuSettings.Show(this, e.Location);
            };
        }

        private void InitializeDigits()
        {
            digB = new PictureBox[4];

            int xPos = 10;
            int digW = 68;
            int digH = 150;

            for (int i = 0; i < digB.Length; i++)
            {
                digB[i] = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(digW, digH),
                    Location = new Point(xPos, 10),
                    BackColor = Color.FromArgb(255, 1, 2)
                };
                this.Controls.Add(digB[i]);
                xPos += digW;

                if (i == 1)
                {
                    cL = new Label
                    {
                        Text = ":",
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = Color.FromArgb(255, 1, 2),
                        AutoSize = true,
                        Location = new Point(xPos, 50)
                    };
                    this.Controls.Add(cL);
                    xPos += 20;
                }
            }

            this.ClientSize = new Size(xPos + 10, 165);
        }

        private void CacheDigits()
        {
            lock (_lock)
            {
                if (digB != null)
                {
                    foreach (var pb in digB)
                    {
                        if (pb != null && ImageAnimator.CanAnimate(pb.Image))
                        {
                            ImageAnimator.StopAnimate(pb.Image, OnFrameChanged);
                            pb.Image = null;
                        }
                    }

                    foreach (var q in _digitCache.Values)
                    {
                        if (q != null)
                        {
                            if (ImageAnimator.CanAnimate(q))
                            {
                                ImageAnimator.StopAnimate(q, OnFrameChanged);
                            }
                            q.Dispose();
                        }
                    }
                    _digitCache.Clear();
                    string digits = "0123456789";
                    foreach (char dig in digits)
                    {
                        string digPath = Path.Combine(prPath, "Resources", CurThemeFolder, $"{dig}.{(CurThemeFolder == "gelbooru-h" ? "png" : "gif")}");
                        _digitCache[dig] = Image.FromFile(digPath);
                    }
                }
            }
        }

        private void UpdateTimeDisplay(object sender, EventArgs e)
        {
            lock (_lock)
            {
                string timeStr = DateTime.Now.ToString("HHmm");

                for (int i = 0; i < digB.Length; i++)
                {
                    if (i < timeStr.Length && _digitCache.TryGetValue(timeStr[i], out Image img))
                    {
                        if (digB[i].Image != img)
                        {
                            try
                            {
                                if (digB[i].Image != null && ImageAnimator.CanAnimate(digB[i].Image)) { ImageAnimator.StopAnimate(digB[i].Image, OnFrameChanged); }
                            } catch (Exception ex) { Debug.WriteLine($"Error stop animation: {ex}"); }
                            digB[i].Image = null;
                            digB[i].Image = img;
                        }
                        else { continue; }
                    }
                }
            }
        }
    private void OnFrameChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => OnFrameChanged(sender, e)));
                return;
            }
            lock (_lock)
            {
                foreach (var pb in digB)
                {
                    try
                    {
                        if (pb.Image != null && ImageAnimator.CanAnimate(pb.Image))
                        {
                            ImageAnimator.UpdateFrames(pb.Image);
                            pb.Invalidate();
                        }
                    } catch (Exception ex) { Debug.WriteLine($"Error updating frames: {ex}"); }
                }
            }
        }
        private void DragWindow()
        {
            ReleaseCapture();
            _ = SendMessage(Handle, 0xA1, 0x2, 0);
        }
        private void InitializeContextMenu()
        {
            contextMenuSettings = new ContextMenuStrip();
            var themeItem = new ToolStripMenuItem("Theme");
            
            foreach (var theme in themeManager.GetThemeNames())
            {
                var item = new ToolStripMenuItem(theme.Value)
                {
                    Tag = theme.Key,
                    CheckOnClick = true,
                    Checked = theme.Key == themeManager.CurTheme,
                    BackColor = Color.FromArgb(40, 40, 40),
                    ForeColor = Color.White
                };
                    
                item.Click += (s, e) =>
                {
                    foreach (ToolStripMenuItem otherItem in themeItem.DropDownItems)
                    {
                        otherItem.Checked = false;
                    }

                    item.Checked = true;
                    themeManager.SetTheme((ThemeManager.Theme)item.Tag);

                };
                themeItem.DropDownItems.Add(item);
            }

            contextMenuSettings.Items.Add(themeItem);
            contextMenuSettings.BackColor = Color.FromArgb(40, 40, 40);
            contextMenuSettings.ForeColor = Color.White;

            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) => this.Close();
            contextMenuSettings.Items.Add(exitItem);
        }


        private void OnThemeChanged(ThemeManager.Theme theme)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnThemeChanged(theme)));
                return;
            }
            switch (theme)
            {
                case ThemeManager.Theme.gelbooru:
                    CurThemeFolder = "gelbooru";
                    break;
                case ThemeManager.Theme.gelbooru_h:
                    CurThemeFolder = "gelbooru-h";
                    break;
                case ThemeManager.Theme.asoul:
                    CurThemeFolder = "asoul";
                    break;
                case ThemeManager.Theme.booru_lewd:
                    CurThemeFolder = "booru-lewd";
                    break;
                case ThemeManager.Theme.booru_lisu:
                    CurThemeFolder = "booru-lisu";
                    break;
                case ThemeManager.Theme.booru_qh:
                    CurThemeFolder = "booru-qualityhentais";
                    break;
                case ThemeManager.Theme.original:
                    CurThemeFolder = "original";
                    break;
            }
            CacheDigits();
            UpdateTimeDisplay(null, null);
        }

        private void Save()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MoeClock");
            key.SetValue("WindowLeft", this.Left);
            key.SetValue("WindowTop", this.Top);
            key.SetValue("Theme", (int)themeManager.CurTheme, RegistryValueKind.DWord);
            key.Close();
        }

        private void Load()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MoeClock");
            if (key != null)
            {
                int left = (int)key.GetValue("WindowLeft", this.Left);
                int top = (int)key.GetValue("WindowTop", this.Top);
                this.Location = new Point(left, top);
                object themeValue = key.GetValue("Theme");
                if (themeValue != null)
                {
                    themeManager.SetTheme((ThemeManager.Theme)Convert.ToInt32(themeValue));
                }
                key.Close();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle = 0x80;
                Params.ExStyle |= 0x80000;
                return Params;
            }
        }
    }
}

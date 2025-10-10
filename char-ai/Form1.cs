using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using System.Runtime.InteropServices;

namespace char_ai;

public partial class MainForm : Form
{
    // Maybe I'll ask the people at char ai about the API
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    // I don't even know it even supports light mode
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
    private const int DWMWA_CAPTION_COLOR = 35;
    private const int DWMWA_TEXT_COLOR = 36;
    private WebView2 view;
    private const string cai = "https://character.ai";

    public static void SetDarkTitleBar(Form form)
    {
        var hwnd = form.Handle;
        int useDark = 1;
        DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, sizeof(int));

        int captionColor = unchecked((int)0xFF1E1E1E);
        DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref captionColor, sizeof(int));

        int textColor = unchecked((int)0xFFFFFFFF);
        DwmSetWindowAttribute(hwnd, DWMWA_TEXT_COLOR, ref textColor, sizeof(int));
    }
    public MainForm()
    {
        Load += InitializeMain;
        SetDarkTitleBar(this);
        this.Width = 1280;
        this.Height = 720;
        view = new WebView2{
            Dock = DockStyle.Fill
        };
        Controls.Add(view);

        InitializeComponent();
    }
    private async void InitializeMain(object sender, EventArgs e){
        try{
            await view.EnsureCoreWebView2Async();
            view.CoreWebView2.Navigate(cai);
        }
        catch (Exception ex){
            MessageBox.Show($"Failed to show {cai}. Is it down?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.IO;

namespace Jellyfin.Vue.WinUI
{
    /// <summary>
    /// The main window.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async();
            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "jellyfin.winui.local",
                "Client",
                Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            WebView.Source = new Uri("http://jellyfin.winui.local/index.html");

            WebView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
        }

        private void CoreWebView2_NavigationStarting(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            var extension = System.IO.Path.GetExtension(args.Uri);
            if (string.IsNullOrEmpty(extension))
            {
                sender.Navigate(args.Uri.TrimEnd('/') + "/index.html");
            }
        }
    }
}

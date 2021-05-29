using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using Serilog;
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
            InitializeComponent();
            Title = "Jellyfin Vue";
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                await WebView.EnsureCoreWebView2Async();
                // Content files are in the parent directory.
                WebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "jellyfin.winui.local",
                    "../jellyfin-vue",
                    CoreWebView2HostResourceAccessKind.Allow);
                WebView.Source = new Uri("http://jellyfin.winui.local/index.html");

                WebView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            }
            catch(Exception ex)
            {
                Log.Logger.Error(ex, "Error initializing router");
                throw;
            }
        }

        private void CoreWebView2_NavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs args)
        {
            var extension = Path.GetExtension(args.Uri);
            if (string.IsNullOrEmpty(extension))
            {
                sender.Navigate(args.Uri.TrimEnd('/') + "/index.html");
            }
        }
    }
}

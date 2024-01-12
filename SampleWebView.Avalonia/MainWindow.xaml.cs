using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WebViewControl;

namespace SampleWebView.Avalonia {

    internal class MainWindow : Window {

        public MainWindow() {
            WebView.Settings.OsrEnabled = false;
            WebView.Settings.LogFile = "ceflog.txt";
            var view = new WebView();
            view.Address = "http://www.google.com/";
            DataContext = view;
        }
    }
}
﻿using System;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;

using SlightPenLighter.Hooks;

using Application = System.Windows.Application;

namespace SlightPenLighter.UI {

    public partial class PenHighlighter {

        public MouseTracker MouseTracker {
            get;
            set;
        }

        public static IntPtr WindowPointer {
            get;
            set;
        }

        public PenHighlighter() {

            InitializeComponent();

            CreateIcon();

            Top = 0;
            Left = 0;
        }

        public OptionWindow OptionWindow {
            get;
            set;
        }

        private void MainWindow_OnSourceInitialized(object sender, EventArgs e) {

            Dispatcher.Invoke(new Action(Target), DispatcherPriority.ContextIdle, null);
        }

        private void Target() {

            WindowPointer = new WindowInteropHelper(this).Handle;
            DwmHelper.SetWindowExTransparent(WindowPointer);
            MouseTracker = new MouseTracker(WindowPointer);

            OptionWindow = new OptionWindow(this);
        }

        private void CreateIcon() {

            var menu = new ContextMenu();

            var optionsItem = new MenuItem("Show Options");
            optionsItem.Click += OpenOptions;
            menu.MenuItems.Add(optionsItem);

            var item = new MenuItem("Show/Hide Highlighter");
            item.Click += HideShow;
            menu.MenuItems.Add(item);

            var exitItem = new MenuItem("Exit");
            exitItem.Click += Exit;
            menu.MenuItems.Add(exitItem);

            new NotifyIcon {
                ContextMenu = menu,
                Icon = Properties.Resources.icon,
                Text = @"Pen Highlighter",
                Visible = true
            };
        }

        private void OpenOptions(object sender, EventArgs eventArgs) {

            OptionWindow.Show();
        }

        private static void Exit(object sender, EventArgs eventArgs) {

            Application.Current.Shutdown(0);
        }

        private void HideShow(object sender, EventArgs eventArgs) {

            if(IsVisible)
                Hide();
            else
                Show();
        }

    }

}
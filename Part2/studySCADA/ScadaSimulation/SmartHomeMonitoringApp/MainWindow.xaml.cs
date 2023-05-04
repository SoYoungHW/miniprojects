﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics; // 필요

namespace SmartHomeMonitoringApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.DataBaseControl();
        }

        // 끝내기 버튼 클릭이번트 핸들러
        private void MnuExitProgram_Click(object sender, RoutedEventArgs e)
        {
            // Environment.Exit(0); 조금 느림
            Process.GetCurrentProcess().Kill(); // 작업관리자에서 프로세스 종료!
        }
    }
}
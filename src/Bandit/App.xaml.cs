﻿using Bandit.Models;
using Bandit.Utilities;
using Bandit.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace Bandit
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private bool IsInternetConnected()
        {
            const string NCSI_TEST_URL = "http://www.msftncsi.com/ncsi.txt";
            const string NCSI_TEST_RESULT = "Microsoft NCSI";
            const string NCSI_DNS = "dns.msftncsi.com";
            const string NCSI_DNS_IP_ADDRESS = "131.107.255.255";

            try
            {
                // NCSI 테스트 링크 접속 확인.
                var webClient = new WebClient();
                string result = webClient.DownloadString(NCSI_TEST_URL);
                if (result != NCSI_TEST_RESULT)
                {
                    return false;
                }

                // NCSI DNS 주소 확인.
                var dnsHost = Dns.GetHostEntry(NCSI_DNS);
                if (dnsHost.AddressList.Count() < 0 || dnsHost.AddressList[0].ToString() != NCSI_DNS_IP_ADDRESS)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }

        private string GetWebContents(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            using (StreamReader reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                // 스트림을 읽어와서 문자열로 저장한다.
                string versionString = reader.ReadToEnd();

                // 버전 값을 파싱한 후 반환한다.
                return versionString;
            }
        }

        private bool IsLatestVersion()
        {
            if (!File.Exists(Settings.PATH_VERSION))
            {
                return true;
            }

            string current = FileUtility.ReadTextFile(Settings.PATH_VERSION, Encoding.UTF8);

            string latest = GetWebContents(Settings.URL_BANDIT_LATEST_VERSION);

            return latest.Contains(current);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // 인터넷 사용 가능 여부 확인.
            if (!IsInternetConnected())
            {
                MessageBox.Show("인터넷에 접속할 수 없습니다.", "Bandit", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            // 프로세스 중복 실행 여부 확인.
            Process[] procs = Process.GetProcessesByName("Bandit");
            if (procs.Length > 1)
            {
                MessageBox.Show("프로그램이 이미 실행중입니다.", "Bandit", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            // 업데이트 존재 여부 확인.
            if (!IsLatestVersion())
            {
                UpdateView updateView = new UpdateView();
                updateView.Show();
            }

            base.OnStartup(e);

            Settings.Instance = Settings.Deserialize(Settings.PATH_SETTINGS);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Settings.Instance.Serialize(Settings.PATH_SETTINGS);
        }
    }
}

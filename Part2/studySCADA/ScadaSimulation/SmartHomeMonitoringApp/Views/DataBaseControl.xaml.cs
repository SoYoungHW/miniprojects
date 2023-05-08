using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// DataBaseControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBaseControl : UserControl
    {
        public bool IsConnected { get; set; }    
        public DataBaseControl()
        {
            InitializeComponent();
        }

        // 유저컨트롤 로드이벤트 핸들러
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TxbBrokerUrl.Text = Commons.BROKERHOST;
            TxbMqttTopic.Text = Commons.MQTTTOPIC;
            TxtConnString.Text = Commons.MYSQL_CONNSTRING;

            IsConnected = false; // 아직 접속이 안됨
            BtnConnDb.IsChecked = false;
        }

        // 토글버튼 클릭(1:접속 2:접속끊음) 이벤트 핸들러
        private void BtnConnDb_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected == false)
            {
                // MQTT 브로커에 접속
                Commons.MQTT_CLIENT = new uPLibrary.Networking.M2Mqtt.MqttClient(Commons.BROKERHOST);

                try
                {
                    if (Commons.MQTT_CLIENT.IsConnected == false)
                    {
                        // MQTT 접속
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived; // 메시지를 받는 쪽
                        Commons.MQTT_CLIENT.Connect("MONITOR"); // clientID = 모니터
                        Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.MQTTTOPIC },
                            new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE }); // QOS는 네트워크 통신 옵션
                        UpdateLog(">>> MQTT Broker Connected");

                        BtnConnDb.IsChecked = true;
                        IsConnected = true; // 예외 발생하면 true로 변경할 필요 없음
                        BtnConnDb.Content = "Connect";

                    }
                }
                catch
                {
                    //
                }
            }

            else
            {
                BtnConnDb.IsChecked = false;
                IsConnected = false;
            }       
        }
          
        public void UpdateLog(string msg) 
        {
            // 예외처리 필요
            this.Invoke(new Action(() => {
                TxtLog.Text += $"{msg}\n";
                TxtLog.ScrollToEnd();
            }));
        }

        // Subscribe가 발생할 때 이벤트 핸들러
        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Message);
            UpdateLog(msg);
            SetToDataBase(msg, e.Topic); // 실제 DB에 저장처리
        }

        // DB 저장처리 메서드
        private void SetToDataBase(string msg, string topic)
        {
            var currValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);
            if (currValue != null) 
            {
                //Debug.Writeline(currValue["Home_Id"]);
                //Debug.Writeline(currValue["Room_Name"]);
                //Debug.Writeline(currValue["Sensing_Datatime"]);
                //Debug.Writeline(currValue["Temp"]);
                //Debug.Writeline(currValue["Humid"]);

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Commons.MYSQL_CONNSTRING))
                    {
                        if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }
                        string insQuery = "";

                        MySqlCommand cmd = new MySqlCommand(insQuery, conn);
                        cmd.Parameters.AddWithValue("@Home_Id", currValue["Home_Id"]);

                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            UpdateLog(">>> DB Insert succed");
                        }
                        else
                        {
                            UpdateLog(">>> DB Insert failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateLog($"!! Erorr 발생 : {ex.Message}");
                }
                
            }
        }
    }
}

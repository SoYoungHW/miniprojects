﻿using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// RealTimeControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RealTimeControl : UserControl
    {
        public RealTimeControl()
        {
            InitializeComponent();

            LvcLivingTemp.Value = LvcDiningTemp.Value = LvcBedTemp.Value = LvcBathTemp.Value = 0;
            LvcLivingHumid.Value = LvcDiningHumid.Value = LvcBedHumid.Value = LvcBathHumid.Value = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Commons.MQTT_CLIENT != null && Commons.MQTT_CLIENT.IsConnected)
            {
                Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
            } // DB 모니터링을 실행한 뒤 실시간 모니터링으로 넘어왔다면

            else
            {
                Commons.MQTT_CLIENT = new MqttClient(Commons.BROKERHOST);
                Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
                Commons.MQTT_CLIENT.Connect("MONITOR");
                Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.MQTTTOPIC },
                    new byte[] {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE});

            } // DB 모니터링 실행하지 않고 바로 실시간 모니터링으로 갔다면
        }

        // MQTTClient는 단독스레드 사용, UI스레드에 직접 접근 안됨
        // this.Invoke(); --> UI스레드 안에 있는 리소스 직접 접근가능 // 중요!!
        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Message);
            Debug.WriteLine(msg);
            var currSensor = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);

            if (currSensor["DEV_ID"] == "IOT54") // IOT54로 변경
            {
                this.Invoke(() => 
                {
                    var dfValue = DateTime.Parse(currSensor["CURR_DT"]).ToString("yyyy-MM-dd HH:mm:ss");
                    LblSensingDt.Content = $"Sensing DateTime : {dfValue}";
                });

                switch ("Living".ToUpper()) // currSensor["Room_Name"] 값을 안받기때문에 Living으로 고정
                {
                    case "LIVING":
                        this.Invoke(() => // UI스레드와 충돌안나도록
                        {
							var tmp = currSensor["STAT"].Split('|'); // 29.0 | 45.0 잘라준다음
							var temp = tmp[0].Trim(); // Trim으로 앞뒤공백제거(필수!)
							var humid = tmp[1].Trim();

							LvcLivingTemp.Value = Math.Round(Convert.ToDouble(temp), 1);
                            LvcLivingHumid.Value = Convert.ToDouble(humid);
                        });
                        break;

                    //case "DINING":
                    //    this.Invoke(() =>
                    //    {
                    //        LvcDiningTemp.Value = Math.Round(Convert.ToDouble(currSensor["Temp"]), 1);
                    //        LvcDiningHumid.Value = Convert.ToDouble(currSensor["Humid"]);
                    //    });
                    //    break;

                    //case "BED":
                    //    this.Invoke(() => 
                    //    { 
                    //        LvcBedTemp.Value = Math.Round(Convert.ToDouble(currSensor["Temp"]), 1); 
                    //        LvcBedHumid.Value = Convert.ToDouble(currSensor["Humid"]); 
                    //    });
                    //    break;

                    //case "BATH":
                    //    this.Invoke(() => 
                    //    { 
                    //        LvcBathTemp.Value = Math.Round(Convert.ToDouble(currSensor["Temp"]), 1);
                    //        LvcBathHumid.Value = Convert.ToDouble(currSensor["Humid"]);
                    //    });
                    //    break;

                    default: break;
                }
            }
            
        }

		private void BtnOpen_Click(object sender, RoutedEventArgs e)
		{
            // json으로 서보모터 90도 오픈한다는 데이터 생성
            var topic = "pknu/monitor/control/";
            JObject origin_data = new JObject();
            origin_data.Add("DEV_ID", "MONITOR");
            origin_data.Add("CURR_DT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            origin_data.Add("STAT", "OPEN");
            string pub_data = JsonConvert.SerializeObject(origin_data, Formatting.Indented); // json 줄맞춰주기


            Commons.MQTT_CLIENT.Publish(topic, Encoding.UTF8.GetBytes(pub_data)); // 형변환 Bytes 배열
			LblDoorStat.Content = "OPEN";
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e)
		{
			// json으로 서보모터 90도 오픈한다는 데이터 생성
			var topic = "pknu/monitor/control/";
			JObject origin_data = new JObject();
			origin_data.Add("DEV_ID", "MONITOR");
			origin_data.Add("CURR_DT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			origin_data.Add("STAT", "CLOSE");
			string pub_data = JsonConvert.SerializeObject(origin_data, Formatting.Indented); // json 줄맞춰주기


			Commons.MQTT_CLIENT.Publish(topic, Encoding.UTF8.GetBytes(pub_data)); // 형변환 Bytes 배열
            LblDoorStat.Content = "CLOSE";
		}
	}
}
﻿using Microsoft.Xaml.Behaviors.Media;
using SmartHomeMonitoringApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace SmartHomeMonitoringApp.Logics
{
    public class Commons
    {
        // 화면마다 공유할 MQTT 브로커 IP 변수
        public static string BROKERHOST = "127.0.0.1";
        
        public static string MQTTTOPIC = "SmartHome/IoTData/";

        public static string MYSQL_CONNSTRING = "Server=localhost;" +
                                                "Port=3306;" +
                                                "Database=miniproject;" +
                                                "Uid=root;" +
                                                "Pwd=12345;";

        public static MqttClient MQTT_CLIENT { get; set; }
        
    }
}

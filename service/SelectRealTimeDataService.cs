


using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using AlphaFlash.Select.Dto;
using AlphaFlash.Select.Stomp;
using Extend;

namespace AlphaFlash.Select.Service {

    abstract class SelectRealTimeDataService {

        public Action<List<Observation>> ObservationHandler {get; set;} = o => {};
        public Action<StompMessage> ConnectHandler {get; set;} = o => {};
        public Action<StompMessage> ConnectFailHandler {get; set;} = o => {};
        public Action<Exception> ExceptionHandler {get; set;} = o => {};
        public Action DisconnectHandler {get; set;} = () => {};
        public Action HeartbeatHandler {get; set;} = () => {};
        public bool Running {get;set;} = true;
        public int ReconnectQuietMillis {get;set;} = 5000;
        public string AccessToken {get;set;}
        public abstract void Run();







    }
}
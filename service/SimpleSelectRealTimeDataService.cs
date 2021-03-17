

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using AlphaFlash.Select.Dto;
using AlphaFlash.Select.Stomp;

namespace AlphaFlash.Select.Service {


    class SimpleSelectRealTimeDataService : SelectRealTimeDataService {
        private const string V = "/topic/observations";
        private readonly StompConnection _stompConnection;

        public SimpleSelectRealTimeDataService(string host, int port, bool ssl){
            this._stompConnection = new StompConnection(host,port,ssl);
        }
        public override void Run(){
        
        
            while (Running){

                try{
                    
                    StompMessage connectResult =  this._stompConnection.connect(AccessToken);

                    if (connectResult.MessageType.Equals("CONNECTED")){
                        ConnectHandler.Invoke(connectResult);
                    } else {
                        ConnectFailHandler.Invoke(connectResult);
                        throw new Exception("Connect result: " + connectResult.MessageType);
                    }
                        
                    _stompConnection.Subscribe(V);

                    while (Running)
                    {
                        StompMessage stompMessage = _stompConnection.readMessage();

                        if (V.Equals(stompMessage.GetFirstHeader("destination"))){
                            List<Observation> observatons = JsonSerializer.Deserialize<List<Observation>>(stompMessage.Body);

                            ObservationHandler.Invoke(observatons);
                        }
                    }


                } catch (Exception e){
                    ExceptionHandler.Invoke(e);
                    Thread.Sleep(ReconnectQuietMillis);
                } finally {
                    this.DisconnectHandler.Invoke();
                    this._stompConnection.Close();
                }

            }
        
        
        }
    }
}
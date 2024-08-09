using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace BotrisBattle.NET
{
    public class BotrisWebsocket
    {
        private readonly string _token;
        private string _roomKey;
        private ClientWebSocket _clientWebSocket = new();
        string _wssUrl = "wss://botrisbattle.com/ws?token={0}&roomKey={1}";

        private Channel<Memory<byte>> _channel;

        private Dictionary<string, Delegate> _handlerDict = [];

        public BotrisWebsocket(string token)
        {
            _token = token;
        }
        public void On(string type, Action action)
        {
            if (!_handlerDict.ContainsKey(type))
            {
                _handlerDict[type] = action;
            }
            else
            {
                Console.WriteLine("{0}事件已有处理函数", type);
            }
        }
        public void On<T>(string type, Action<T> action)
        {
            if (!_handlerDict.ContainsKey(type))
            {
                _handlerDict[type] = action;
            }
            else
            {
                Console.WriteLine("{0}事件已有处理函数", type);
            }
        }

        public void Handle(BotrisMessage message)
        {
            //Console.WriteLine("Handle {0}", message.type);

            if (_handlerDict.TryGetValue(message.type, out var action))
            {
                var handlerParamTypes = action.Method.GetParameters();
            //object payload = JsonSerializer.Deserialize(message.payload, handlerParamType, options);
                //    Console.WriteLine("GetParameters");

                ////((Action<dynamic>)action)(payload);
                //Console.WriteLine("Any = {0}", handlerParamTypes.Any());
                //Console.WriteLine("Length = {0}", handlerParamTypes.Length);
                if (handlerParamTypes.Length != 0)
                {
                    //Console.WriteLine("before payload Deserialize");

                    var payload = JsonSerializer.Deserialize(message.payload, handlerParamTypes[0].ParameterType, options);
                    //Console.WriteLine("before DynamicInvoke");
                    //Console.WriteLine(action.GetType());
                    action.Method.Invoke(action.Target, [payload]);
                    //var minfo = action.GetMethodInfo();
                    //minfo.Invoke(action, [payload]);
                    //Console.WriteLine("after DynamicInvoke");


                }
                else
                {
                    //action.DynamicInvoke();
                    //Console.WriteLine("before Invoke");

                    ((Action)action).Invoke();
                    //Console.WriteLine("after Invoke");

                    //action();
                }

            }
            else
            {
                //Console.WriteLine("没有实现这种解析：{0}", message.type);
            }
        }

        //CancellationToken? _ct = null;
        public async Task Connect(string roomKey, CancellationToken token)
        {
            _roomKey = roomKey;
            _channel = Channel.CreateUnbounded<Memory<byte>>();
            var handler = new HttpClientHandler
            {
                UseProxy = true,
                Proxy = WebRequest.DefaultWebProxy
            };
            _clientWebSocket.Options.Proxy = handler.Proxy;
            await _clientWebSocket.ConnectAsync(new Uri(string.Format(_wssUrl, _token, _roomKey)), token);


            _ = DoRecvice(token);
            _ = DoSend(token);
        }

        public async Task DoRecvice(CancellationToken token)
        {
            byte[] data = new byte[1024 * 128];
            try
            {
                while (!token.IsCancellationRequested && _clientWebSocket.State == WebSocketState.Open)
                {

                    var result = await _clientWebSocket.ReceiveAsync(data, token);
                    //Console.WriteLine(result.CloseStatusDescription);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string value = Encoding.UTF8.GetString(data.AsSpan().Slice(0, result.Count));
                        //Console.WriteLine(value);
                        Handle(JsonSerializer.Deserialize<BotrisMessage>(value));
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("连接关闭");
                //throw;  
            }
           

        }
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public void Send(BotrisMessage1 message)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(message, options);
            _channel.Writer.TryWrite(data);
        }

        public async Task DoSend(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    var data = await _channel.Reader.ReadAsync(token);
                    //Console.WriteLine("发出数据：" + Encoding.UTF8.GetString(data.Span));
                    await _clientWebSocket.SendAsync(data, WebSocketMessageType.Text, true, token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }

            }

            
        }
    }
}

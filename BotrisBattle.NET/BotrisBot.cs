using System.Text.Json;

namespace BotrisBattle.NET
{
    public class BotrisBot
    {
        BotrisWebsocket _websocket;

        public event Action GameStart;
        public event Action GameReset;
        public event Action<RequestMovePayload> RequestMove;
        public event Action<UpdateConfigPayload> UpdateConfig;

        public void SendMove(Command[] commands)
        {
            _websocket.Send(new BotrisMessage1
            {
                type = "action",
                payload = new ActionPayload
                {
                    Commands = commands
                }
            });
        }

        public BotrisBot(string token)
        {
            _websocket = new BotrisWebsocket(token);

            _websocket.On<AuthenticatedPayload>("authenticated", (payload) =>
            {
                //Console.WriteLine("认证成功：{0}", payload.SessionId);
            });

            _websocket.On<RoomDataPayload>("room_data", payload =>
            {
                // TODO: Consider network latency
                UpdateConfig?.Invoke(
                    new UpdateConfigPayload
                    {
                        Duration = (int)Math.Floor(1000 / payload.roomData.pps)
                    }
                );
            });

            _websocket.On("game_reset", () => { GameReset?.Invoke(); });

            _websocket.On("game_started", () => { GameStart?.Invoke(); });

            _websocket.On<RequestMovePayload>("request_move", (payload) =>
            {
                //Console.WriteLine("befory event invoke");
                RequestMove?.Invoke(payload);
            });

            _websocket.On<RoomDataPayload>("settings_changed", payload =>
            {
                // TODO: Consider network latency
                UpdateConfig?.Invoke(
                    new UpdateConfigPayload
                    {
                        Duration = (int)Math.Floor(1000 / payload.roomData.pps)
                    }
                );
            });
        }


        public async void Connect(string room, CancellationToken token)
        {
            await _websocket.Connect(room, token);
        }
    }
}
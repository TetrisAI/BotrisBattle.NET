using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotrisBattle.NET
{
    public class RoomDataPayload
    {
        public RoomData roomData { get; set; }
    }

    public class AuthenticatedPayload
    {
        public string SessionId { get; set; }
    }

    public class ErrorPayload
    {
        public string Payload { get; set; }
    }


    public class PlayerJoinedPayload
    {
        public PlayerData PlayerData { get; set; }
    }



    public class PlayerLeftPayload
    {
        public string SessionId { get; set; }
    }



    public class PlayerBannedPayload
    {
        public PlayerInfo PlayerInfo { get; set; }
    }


    public class PlayerUnbannedPayload
    {
        public PlayerInfo PlayerInfo { get; set; }
    }
    public class SettingsChangedPayload
    {
        public RoomData RoomData { get; set; }
    }


    public class HostChangedPayload
    {
        public PlayerInfo HostInfo { get; set; }
    }

 

    public class RoundStartedPayload
    {
        public long StartsAt { get; set; }
        public RoomData RoomData { get; set; }
    }


    public class RequestMovePayload
    {
        public GameState GameState { get; set; }
        public List<PlayerData> Players { get; set; }
    }


    public class ActionPayload
    {
        public Command[] Commands { get; set; }
    }


    public class PlayerActionPayload
    {
        public string SessionId { get; set; }
        public List<Command> Commands { get; set; }
        public GameState GameState { get; set; }
        public List<GameEvent> Events { get; set; }
    }

    public class PlayerDamageReceivedPayload
    {
        public string SessionId { get; set; }
        public int Damage { get; set; }
        public GameState GameState { get; set; }
    }


    public class RoundOverPayload
    {
        public string WinnerId { get; set; }
        public PlayerInfo WinnerInfo { get; set; }
        public RoomData RoomData { get; set; }
    }


    public class GameOverPayload
    {
        public string WinnerId { get; set; }
        public PlayerInfo WinnerInfo { get; set; }
        public RoomData RoomData { get; set; }
    }


    public class GameResetPayload
    {
        public RoomData RoomData { get; set; }
    }
}

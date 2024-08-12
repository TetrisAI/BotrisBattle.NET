using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace BotrisBattle.NET
{

    public class BotrisMessage
    {
        public string type { get; set; }
        public JsonElement payload { get; set; } = new JsonElement();
    }
    public class BotrisMessage1
    {
        public string type { get; set; }
        public object payload { get; set; }
    }
    public class GameEvent: BotrisMessage;

    public class PlayerInfo
    {
        public required string userId { get; set; }
        public required string creator { get; set; }
        public required string bot { get; set; }
    }

    public class PlayerData
    {
        public string sessionId { get; set; }
        public bool playing { get; set; }
        public PlayerInfo info { get; set; }
        public int wins { get; set; }
        public GameState? gameState { get; set; }
    }

    public class PieceData
    {
        public string piece { get; set; } // Assuming Piece is a string type "I", "O", etc.
        public int x { get; set; }
        public int y { get; set; }
        public int rotation { get; set; }
    }

    public class GarbageLine
    {
        public int delay { get; set; }
    }

    public class GameState
    {
        public Block?[][] board { get; set; }
        public string[] queue { get; set; } // Assuming Piece is represented as string
        public GarbageLine[] garbageQueued { get; set; }
        public string held { get; set; } // Assuming Piece is represented as string
        public PieceData current { get; set; }
        public bool canHold { get; set; }
        public int combo { get; set; }
        public bool b2b { get; set; }
        public int score { get; set; }
        public int piecesPlaced { get; set; }
        public bool dead { get; set; }
    }

    public class RoomData
    {
        public required string id { get; set; }
        public required PlayerInfo host { get; set; }
        public bool @private { get; set; }
        public int ft { get; set; }
        public double pps { get; set; }
        public int startMargin { get; set; }
        public int endMargin { get; set; }
        public int maxPlayers { get; set; }
        public bool gameOngoing { get; set; }
        public bool roundOngoing { get; set; }
        public long? startedAt { get; set; }
        public long? endedAt { get; set; }
        public string? lastWinner { get; set; }
        public List<PlayerData> players { get; set; } = [];
        public List<PlayerInfo> banned { get; set; } = [];
    }

    // Enum for Piece
    public enum Piece
    {
        I, O, J, L, S, Z, T
    }
    public enum Block
    {
        I, O, J, L, S, Z, T, G, N
    }
    // Enum for Command
    public enum Command
    {
        hold, move_left, move_right, sonic_left, sonic_right,
        rotate_cw, rotate_ccw, drop, sonic_drop
    }

    // Enum for ClearName
    public enum ClearName
    {
        Single, Triple, Double, Quad, Perfect_Clear, All_Spin_Single,
        All_Spin_Double, All_Spin_Triple
    }
}

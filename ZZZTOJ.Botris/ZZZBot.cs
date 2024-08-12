using BotrisBattle.NET;
using ScixingTetrisCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZZZTOJ.Botris
{
    public class MoveResult
    {
        public bool hold { get; set; }
        public List<Command> moves { get; set; } = [];
        public int[][] expected_cells { get; set; }

    }
    class ZZZBot
    {
        static Queue<MinoType> _nextQueue = new();
        static TetrisGameBoard _IOBoard = new(ShowHeight: 22);
        static int _garbage = 0;
        static object _lockQueue = new();
        static object _lockBoard = new();
        public BotSetting BotSetting = new BotSetting() { BPM = 200, Level = 8, NextCnt = 6 };

        static DateTime _startTime;
        static int _nowIdx = 0;

        public ZZZBot()
        {
                
        }

        public MoveResult GetMove(RequestMovePayload requestMovePayload)
        {
            int[] field1 = new int[40];
            int[] field2 = new int[17];

            //for (int i = 0; i < requestMovePayload.GameState.board.GetLength(0); i++)
            //{
            //    for (int j = 0; j < 10; ++j)
            //    {
            //        if (requestMovePayload.GameState.board[i][9 - j] != null) field1[i] |= (1 << j);
            //    }
            //}

            for (int i = 0; i < 40; i++)
            {
                if (i < requestMovePayload.GameState.board.GetLength(0)) {
                    for (int j = 0; j < 10; ++j)
                    {
                        if (requestMovePayload.GameState.board[i][j] != null) _IOBoard.Field[i][j] = 1;
                        else {
                            _IOBoard.Field[i][j] = 0;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        _IOBoard.Field[i][j] = 0;
                    }
                }
                
            }

            for (int i = 0; i < 40; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (_IOBoard.Field[i][j] == 1) field1[i] |= (1 << j);
                }

            }

            //for (int i = 0; i < 17; ++i)
            //{
            //    for (int j = 0; j < 10; ++j)
            //    {
            //        if (_IOBoard.Field[39 - i][j] == 1) field2[i] |= (1 << j);
            //    }

            //}

            //for (int i = 17; i < 40; ++i)
            //{
            //    if (39 - i < requestMovePayload.GameState.board.GetLength(0))
            //        for (int j = 0; j < 10; ++j)
            //        {
            //            if (requestMovePayload.GameState.board[39 - i][j] != null) field1[i - 17] |= (1 << j);
            //        }

            //}
            //_IOBoard.PrintBoard();
            //_IOBoard.HoldMino = requestMovePayload.GameState.held == null ? null : requestMovePayload.GameState.held switch
            //{
            //    "I" => TetrisMino.I,
            //    "O" => TetrisMino.O,
            //    "S" => TetrisMino.S,
            //    "Z" => TetrisMino.Z,
            //    "T" => TetrisMino.T,
            //    "J" => TetrisMino.J,
            //    "L" => TetrisMino.L,
            //    _ => null,
            //};
            _IOBoard.TetrisMinoStatus =  new TetrisMinoStatus { Position = _IOBoard.DefaultPos, Stage = 0, 
                TetrisMino = requestMovePayload.GameState.current.piece switch
                {
                    "I" => TetrisMino.I,
                    "O" => TetrisMino.O,
                    "S" => TetrisMino.S,
                    "Z" => TetrisMino.Z,
                    "T" => TetrisMino.T,
                    "J" => TetrisMino.J,
                    "L" => TetrisMino.L,
                    _ => null,
                }
            };



          
            //for (int i = 17; i < 40; ++i)
            //{
            //    for (int j = 0; j < 10; ++j)
            //    {
            //        if (_IOBoard.Field[39 - i][j] == 1) field1[i - 17] |= (1 << j);
            //    }

            //}
            int[] comboTable = new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 3, 3, 4,4,4,4, -1 };
            Console.WriteLine("T={0},X={1},Y={2},R={3}",requestMovePayload.GameState.current.piece[0],requestMovePayload.GameState.current.x, requestMovePayload.GameState.current.y, requestMovePayload.GameState.current.rotation);
            var path = ZZZTOJCore.BotrisAI2(field1, 10, 22, requestMovePayload.GameState.b2b ? 1 : 0,
                    requestMovePayload.GameState.combo, 
                    requestMovePayload.GameState.queue.Select(s => s[0]).ToArray(),

                    requestMovePayload.GameState.held == null ? ' ': requestMovePayload.GameState.held[0],
                    requestMovePayload.GameState.canHold, requestMovePayload.GameState.current.piece[0],
                    requestMovePayload.GameState.current.x, 20 - requestMovePayload.GameState.current.y, requestMovePayload.GameState.current.rotation,
                    true, false, requestMovePayload.GameState.garbageQueued.Length, comboTable, BotSetting.NextCnt, BotSetting.Level, 0);
            string resultpath = Marshal.PtrToStringAnsi(path);
            _IOBoard.NextQueue.Enqueue(TetrisMino.Z);
            Console.WriteLine(resultpath.PadRight(50));
            MoveResult moveResult = new MoveResult();
            foreach (char move in resultpath)
            {
                switch (move)
                {
                    case 'z':
                    case 'Z':
                        _IOBoard.LeftRotation();
                        moveResult.moves.Add(Command.rotate_ccw);
                        break;
                    case 'c':
                    case 'C':
                        _IOBoard.RightRotation();
                        moveResult.moves.Add(Command.rotate_cw);
                        break;
                    case 'l':
                        _IOBoard.MoveLeft();
                        moveResult.moves.Add(Command.move_left);
                        break;
                    case 'L':
                        while (_IOBoard.MoveLeft()) ;
                        moveResult.moves.Add(Command.sonic_left);
                        break;
                    case 'r':
                        _IOBoard.MoveRight();
                        moveResult.moves.Add(Command.move_right);

                        break;
                    case 'R':
                        while (_IOBoard.MoveRight()) ;
                        moveResult.moves.Add(Command.sonic_right);
                        break;
                    case 'd':
                        _IOBoard.SoftDrop();
                        moveResult.moves.Add(Command.drop);
                        break;
                    case 'D':
                        _IOBoard.SonicDrop();
                        moveResult.moves.Add(Command.sonic_drop);
                        break;
                    case 'v':
                        //_IOBoard.OnHold();
                        _IOBoard.TetrisMinoStatus.TetrisMino = requestMovePayload.GameState.held == null ?
                            requestMovePayload.GameState.queue[0] switch
                            {
                                "I" => TetrisMino.I,
                                "O" => TetrisMino.O,
                                "S" => TetrisMino.S,
                                "Z" => TetrisMino.Z,
                                "T" => TetrisMino.T,
                                "J" => TetrisMino.J,
                                "L" => TetrisMino.L,
                                _ => null,
                            }
                            : requestMovePayload.GameState.held switch
                        {
                            "I" => TetrisMino.I,
                            "O" => TetrisMino.O,
                            "S" => TetrisMino.S,
                            "Z" => TetrisMino.Z,
                            "T" => TetrisMino.T,
                            "J" => TetrisMino.J,
                            "L" => TetrisMino.L,
                            _ => null,
                        };
                        moveResult.hold = true;
                        moveResult.moves.Add(Command.hold);


                        break;
                    case 'V':
                        _IOBoard.SonicDrop();
                        //moveResult.expected_cells = new int[4][];
                        //var list = _IOBoard.TetrisMinoStatus.GetMinoFieldListInBoard();
                        //for (int i = 0; i < 4; ++i)
                        //{
                        //    moveResult.expected_cells[i] = new int[2];
                        //    moveResult.expected_cells[i][1] = list[i].X;
                        //    moveResult.expected_cells[i][0] = list[i].Y;
                        //}
                        _IOBoard.HardDrop();
                        //moveResult.moves.Add(Command.hard_drop);
                        break;
                    default:
                        break;
                }
                if (move == 'V') break;
            }


            if (!BotSetting.Quiet) {
            _IOBoard.PrintBoard();
                Console.WriteLine($"combo: {requestMovePayload.GameState.combo,-3} b2b: {requestMovePayload.GameState.b2b} garbage: {requestMovePayload.GameState.garbageQueued.Length,-3}");
                Console.WriteLine($"mino: {requestMovePayload.GameState.current.piece} pos: {requestMovePayload.GameState.current.x} {requestMovePayload.GameState.current.y}");
            }

           
            //if (_nowIdx == 0)
            //{
            //    _startTime = DateTime.Now;
            //}
            //_nowIdx++;
            //TimeSpan hopeTime = TimeSpan.FromSeconds(60.0 / _botSetting.BPM * _nowIdx);
            ////Console.WriteLine("延时" + (_startTime + hopeTime - DateTime.Now).TotalMilliseconds + "毫秒");
            //if (DateTime.Now < _startTime + hopeTime)
            //{
            //    Task.Delay(_startTime + hopeTime - DateTime.Now).Wait();
            //}

            //_IOBoard.PrintBoard(WithMino:false);
            return moveResult;
        }
    }
}

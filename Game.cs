using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Game
    {
        static void Main(string[] args)
        {
            int board_mode;
            int ai_mode;
            int ai_search_depth;
            while (true)
            {
                Console.Clear();
                Console.Out.WriteLine("Connect4 - MinMax");
                Console.Out.WriteLine("Chose playing board(heightxwidth):");
                Console.Out.WriteLine("   1. 5x7");
                Console.Out.WriteLine("   2. 6x7 (standard)");
                if (!int.TryParse(Console.In.ReadLine(), out board_mode) || board_mode == 1 || board_mode == 2)
                    break;
            }
            while (true)
            {
                Console.Clear();
                Console.Out.WriteLine("Connect4 - MinMax - board size:" + (board_mode == 1 ? "5x7":"6x7"));
                Console.Out.WriteLine("With or without alpha-beta-pruning:");
                Console.Out.WriteLine("   1. with");
                Console.Out.WriteLine("   2. without (very slow for depth>8)");
                if (!int.TryParse(Console.In.ReadLine(), out ai_mode) || ai_mode == 1 || ai_mode == 2)
                    break;
            }
            while (true)
            {
                Console.Clear();
                Console.Out.WriteLine("Connect4 - MinMax - board size: " + (board_mode == 1 ? "5x7" : "6x7") + " - " + (ai_mode == 1 ? "ABPMinMax":"MinMax"));
                Console.Out.WriteLine("Chose opponent search depth [0-13]:");
                if (!int.TryParse(Console.In.ReadLine(), out ai_search_depth) || ai_search_depth>=0 && ai_search_depth < 14)
                    break;
            }

            
            
            Board board =board_mode==1? new Board(5, 7):new Board(6,7);
            bool game_over = false;
            long time;
            var time_stamp=new List<long>();
            //start game
            while (!game_over)
            {

                //first player
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                AiPlayMove(board, 1, ai_mode, ai_search_depth);
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds() - time;
                time_stamp.Add(time);
                if (CheckGameOver(board))
                    break;


                Console.Clear();
                Console.Out.WriteLine(board);
                Console.Out.WriteLine("Ai think time: "+time+"ms");
                //second player
                HumanPlayMove(board, 2);
                if (CheckGameOver(board))
                    break;
            }
            Console.Out.WriteLine("Time spent thinking for each move:");
            int j = 1;
            foreach (long i in time_stamp)
            {
                Console.Out.WriteLine((j++) + ". " + i + "ms");
            }
            Console.In.ReadLine();

        }


        public static bool CheckGameOver(Board board)
        {
            if (board.Winner == null && board.IsBoardFull)
            {
                Console.Clear();
                Console.Out.WriteLine(board);
                Console.Out.WriteLine("Game Over!");
                Console.Out.WriteLine("TIE");
                return true;
            }
            if (board.Winner != null)
            {
                Console.Clear();
                Console.Out.WriteLine(board);
                Console.Out.WriteLine("Game Over!");

                switch (board.Winner)
                {
                    case 1:
                        Console.Out.WriteLine("Player 1 is the winner.");
                        break;
                    case 2:
                        Console.Out.WriteLine("Player 2 is the winner.");
                        break;
                }
                return true;

            }
            return false;
        }
        public static void AiPlayMove(Board board, int player, int ai_mode, int ai_search_depth)
        {
            var moves = new List<Tuple<int, int>>();
            for (int i = 0; i < board.Width; i++)
            {
                if (!board.PutPiece(i, player))
                    continue;
                if (ai_mode == 1)
                {
                    moves.Add(Tuple.Create(i, SearchAlgorythms.AlphaBetaMinMax(ai_search_depth, board, int.MinValue, int.MaxValue, false)));

                }
                else
                {
                    moves.Add(Tuple.Create(i, SearchAlgorythms.MinMax(ai_search_depth, board, false)));
                }
                board.RemovePiece(i);
            }
            int maxMoveScore = moves.Max(t => t.Item2);
            var bestMoves = moves.Where(t => t.Item2 == maxMoveScore).ToList();
            board.PutPiece(bestMoves[new Random().Next(0, bestMoves.Count)].Item1,player);
        }
        public static void HumanPlayMove(Board board,int player)
        {
            while (true)
            {
                Console.Out.WriteLine("Enter column number [1-" + board.Width + "]:");
                if (!int.TryParse(Console.ReadLine(), out int move) || move < 1 || move > board.Width)
                {
                    Console.WriteLine("Number out of range.");
                    continue;
                }
                if (!board.PutPiece(move - 1, player))
                {
                    Console.WriteLine("That column is full, pick another one");
                    continue;
                }
                break;

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class SearchAlgorythms
    {
        public static int AlphaBetaMinMax(int depth, Board board, int alpha, int beta, bool max_player)
        {
            if (depth <= 0)
                return 0;
            var winner = board.Winner;
            if (winner == 2)
                return -depth;
            if (winner == 1)
                return depth;
            if (board.IsBoardFull)
                return 0;
            {
                int value = max_player ? int.MinValue : int.MaxValue;
                for (int i = 0; i < board.Width; i++)
                {
                    if (!board.PutPiece(i, max_player ? 1 : 2))
                        continue;
                    if (max_player)
                    {
                        value = Math.Max(value, AlphaBetaMinMax(depth - 1, board, alpha, beta, false));
                        alpha = Math.Max(alpha, value);
                        board.RemovePiece(i);
                        if (alpha >= beta)
                            break;
                    }
                    else
                    {
                        value = Math.Min(value, AlphaBetaMinMax(depth - 1, board, alpha, beta, true));
                        beta = Math.Min(beta, value);
                        board.RemovePiece(i);
                        if (alpha >= beta)
                            break;
                    }
                }
                return value;
            }
        }
        
        public static int MinMax(int depth, Board board, bool max_player)
        {
            if (depth <= 0)
                return 0;
            var winner = board.Winner;
            if (winner == 2)
                return -depth;
            if (winner == 1)
                return depth;
            if (board.IsBoardFull)
                return 0;
            int value = max_player ? 1 : -1;
            for (int i = 0; i < board.Width; i++)
            {
                if (!board.PutPiece(i, max_player ? 1 : 2))
                    continue;
                int v = MinMax(depth - 1, board, max_player);
                value = max_player ? Math.Max(value, v) : Math.Min(value, v);
                board.RemovePiece(i);
            }
            return value;
        }
        public static int MinMax(int depth, Board board, int player)
        {

            int opponent = player == 1 ? 2 : 1;
            if (depth <= 0)
                return 0;
            var winner = board.Winner;
            if (winner == player)
                return -depth;
            if (winner == opponent)
                return depth;
            if (board.IsBoardFull)
                return 0;
            int value = player==1 ? -1 : 1;
            for (int i = 0; i < board.Width; i++)
            {
                if (!board.PutPiece(i, opponent))
                    continue;
                int v = MinMax(depth - 1, board, opponent);
                value = player==2 ? Math.Max(value, v) : Math.Min(value, v);
                board.RemovePiece(i);
            }
            return value;
        }
        public static int AlphaBetaMinMax(int depth, Board board, int alpha, int beta, int player)
        {
            int opponent = player == 1 ? 2 : 1;
            if (depth <= 0)
                return 0;
            var winner = board.Winner;
            if (winner == player)
                return -depth;
            if (winner == opponent)
                return depth;
            if (board.IsBoardFull)
                return 0;
            {
                int value = int.MinValue;
                for (int i = 0; i < board.Width; i++)
                {
                    if (!board.PutPiece(i, opponent))
                        continue;
                    if (player==1)
                    {
                        value = Math.Max(value, AlphaBetaMinMax(depth - 1, board, alpha, beta, opponent));
                        alpha = Math.Max(alpha, value);
                        board.RemovePiece(i);
                        if (alpha >= beta)
                            break;
                    }
                    else
                    {
                        value = int.MaxValue;
                        value = Math.Min(value, AlphaBetaMinMax(depth - 1, board, alpha, beta, player));
                        beta = Math.Min(beta, value);
                        board.RemovePiece(i);
                        if (alpha >= beta)
                            break;
                    }
                }
                return value;
            }
        }
    }
}

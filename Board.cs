using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Board
    {
        private readonly int? [,] _board;
        private int? _winner;
        private bool _changed;

        public int Height { get; }
        public int Width { get; }

        public Board(int height, int width)
        {
            this.Height = height;
            this.Width = width;
            this._board = new int?[height, width];
        }

        public bool IsBoardFull
        {
            get
                {
                    for(int i = 0; i < this.Width; i++)
                    {
                        if (!this.ColumnFull(i))
                            return false;
                    }
                    return true;
                }
        }
        public bool ColumnFull(int column)
        {
            return this._board[0, column].HasValue;
        }
        public bool PutPiece(int column, int player)
        {
            for (int row = this.Height - 1; row > -1; row--)
            {
                if (!this._board[row, column].HasValue)
                {
                    this._changed = true;
                    this._board[row, column] = player;
                    return true;
                }
            }
            return false;
        }
        public bool RemovePiece(int column)
        {
            for (int row = 0; row < this.Height; row++)
            {
                if (this._board[row, column].HasValue)
                {
                    this._changed = true;
                    this._board[row, column] = null;
                    return true;
                }
            }
            return false;
        }
        public int? Winner
        {
            get
            {
                if (!this._changed)
                    return _winner;
                this._changed = false;
                for(int i = 0; i < this.Height; i++)
                {
                    for (int j = 0; j < this.Width; j++)
                    {
                        if (!this._board[i, j].HasValue)
                            continue;
                        bool horizontal = i + 3 < this.Height;
                        bool vertical = j + 3 < this.Width;
                        if (!horizontal && !vertical)
                            continue;
                        bool diagonal = vertical && horizontal;
                        bool anti_diagonal = vertical && i - 3 >= 0;

                        for (int k = 1; k < 4; k++)
                        {
                            horizontal = horizontal && this._board[i, j] == this._board[i+k, j];
                            vertical = vertical && this._board[i, j + k] == this._board[i, j];
                            diagonal = diagonal && this._board[i + k, j + k] == this._board[i, j];
                            anti_diagonal = anti_diagonal && this._board[i - k, j + k] == this._board[i, j];
                            if (!horizontal && !vertical && !diagonal && !anti_diagonal)
                                break;
                        }
                        if (horizontal || vertical || diagonal || anti_diagonal)
                        {
                            this._winner = this._board[i, j];
                            return this._winner;
                        }
                    }
                }
                this._winner = null;
                return this._winner;
            }
        }
        public override string ToString()
        {
            var string_builder = new StringBuilder();
            for(int i = 0; i < this.Height; i++)
            {
                string_builder.Append("|");
                for(int j = 0; j < this.Width; j++) {
                    string_builder.Append(this._board[i, j].HasValue ? this._board[i, j].Value.ToString() : " ").Append("|");
                }
                string_builder.AppendLine();
            }
            return string_builder.ToString();
        }
        
    }
}

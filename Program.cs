using System;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Xml;

namespace StoreKeeper
{
    internal class Program
    {
        class Board
        {
            public int[,] board = new int[10, 10];
            public int 
                sx =  0, sy =  0,
                bx =  0, by =  0, 
                cx =  0, cy = 0;

            public Board()
            {
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        board[i, j] = -1;
            }

            public void process_line( int line, string line_content )
            {
                for (int row = 0; row < 10; row++)
                {
                    char sign = line_content[row];
                    switch (sign)
                    {
                        case '.':
                            break;
                        case 'X':
                            board[line, row] = -2;  // -2 means wall
                            break;
                        case 'S':
                            sx = line;
                            sy = row;
                            break;
                        case 'B':
                            bx = line;
                            by = row;
                            break;
                        case 'C':
                            cx = line;
                            cy = row;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        class StoreKeeper
        {
            public int[,] moves = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            public int get_shortest_path_length(
                Board board_class,
                int sx, int sy, 
                int fx, int fy)
            {
                if (sx == fx && sy == fy)
                {
                    return 0;
                }

                int cx = board_class.cx, cy = board_class.cy;
                int[,] b = new int[10, 10];

                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        b[i, j] = board_class.board[i, j];

                b[cx, cy] = -1;

                if (b[sx, sy] == -2 || b[fx, fy] == -2)
                {
                    return -1;
                }

                Queue<int[]> q = new Queue<int[]>();
                q.Enqueue([sx, sy, 0]);

                while (b[fx, fy] == -1 && q.Count > 0)
                {
                    int[] coor_and_step = q.Dequeue();
                    int i1 = coor_and_step[0], 
                        i2 = coor_and_step[1], 
                        step = coor_and_step[2] + 1;
                    
                    for (int i = 0; i < 4; i++)
                    {
                        int j1 = i1 + moves[i, 0];
                        int j2 = i2 + moves[i, 1];

                        if (j1 >= 0 && j1 < 10 && j2 >= 0 && j2 < 10)
                        {
                            if (b[j1, j2] == -1)
                            {
                                b[j1, j2] = step;
                                q.Enqueue([j1, j2, step]);
                            }
                        }
                    }
                }

                return b[fx, fy];
            }
        }
        static void Main(string[] args)
        {
            Board b = new Board();
            StoreKeeper sk = new StoreKeeper();
            int[,] moves = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            int[, ,] board = new int[10, 10, 4];
            int shortest_road = 10000;

            for (int i = 0; i < 10; i++)
            {
                b.process_line(i, Console.ReadLine().Trim());
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        board[i, j, k] = b.board[i, j];
                    }
                }
            }

            int cx = b.cx, cy = b.cy;

            Queue<int[]> q = new Queue<int[]>();
            q.Enqueue([b.bx, b.by, 0, b.sx, b.sy]);
            for (int k = 0; k < 4; k++)
            {
                board[b.bx, b.by, k] = 0;
            }

            while (q.Count > 0)
            {
                int[] coorBox_step_coorSK = q.Dequeue();
                int i1 = coorBox_step_coorSK[0],
                    i2 = coorBox_step_coorSK[1],
                    step = coorBox_step_coorSK[2],
                    si1 = coorBox_step_coorSK[3],
                    si2 = coorBox_step_coorSK[4];

                    for (int i = 0; i < 4; i++)
                    {
                        int new_step = step;
                        int j1 = i1 + moves[i, 0];
                        int j2 = i2 + moves[i, 1];

                        if (j1 >= 0 && j1 < 10 && j2 >= 0 && j2 < 10)
                        {
                            int sj1 = i1 - moves[i, 0];
                            int sj2 = i2 - moves[i, 1];
                            if (sj1 >= 0 && sj1 < 10 && sj2 >= 0 && sj2 < 10)
                            {
                                b.board[i1, i2] = -2;
                                int path_length = sk.get_shortest_path_length(b, si1, si2, sj1, sj2);
                                if (path_length != -1)
                                    {
                                    int k = 0;
                                    if (moves[i, 0] == -1)
                                    {
                                        k = 1;
                                    }
                                    else if (moves[i, 0] == 1)
                                    {
                                        k = 2;
                                    }
                                    else if (moves[i, 1] == -1)
                                    {
                                        k = 3;
                                    }
                                    new_step += path_length + 1;
                                    if (board[j1, j2, k] == -1 || board[j1, j2, k] > new_step)
                                    {
                                        if (j1 == cx && j2 == cy && new_step < shortest_road)
                                        {
                                            shortest_road = new_step;
                                        }
                                        else if (new_step < shortest_road)
                                        {
                                            q.Enqueue([j1, j2, new_step, i1, i2]);
                                        }
                                        board[j1, j2, k] = new_step;
                                    }
                                }
                            }
                        }
                    }
                b.board[i1, i2] = -1;
            }

            if (shortest_road != 10000)
            {
                Console.WriteLine(shortest_road);
            } else
            {
                Console.WriteLine(-1);
            }
        }
    }
}

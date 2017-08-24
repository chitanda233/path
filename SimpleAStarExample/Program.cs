using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleAStarExample
{
    /// <summary>
    /// A simple console routine to show examples of the A* implementation in use
    /// </summary>
    class Program
    {
        private bool[,] map;
        private SearchParameters searchParameters;

        static void Main(string[] args)
        {
            var program = new Program();
//            program.Run();
            int a = 1;
            program.Run(1, 1, ref a);
            Console.ReadKey();
        }

        /// <summary>
        /// Outputs three examples of path finding to the Console.
        /// </summary>
        /// <remarks>The examples have copied from the unit tests!</remarks>
        public void Run()
        {
            // Start with a clear map (don't add any obstacles)
            //InitializeMap();
            InitializeMap(1);
            PathFinder pathFinder = new PathFinder(searchParameters);
            List<Point> path = pathFinder.FindPath();
            ShowRoute("The algorithm should find a direct path without obstacles:", path);
            Console.WriteLine();


            /*
            // Now add an obstacle
            InitializeMap();
            AddWallWithGap();
            pathFinder = new PathFinder(searchParameters);
            path = pathFinder.FindPath();
            ShowRoute("The algorithm should find a route around the obstacle:", path);
            Console.WriteLine();

            // Finally, create a barrier between the start and end points
            InitializeMap();
            AddWallWithoutGap();
            pathFinder = new PathFinder(searchParameters);
            path = pathFinder.FindPath();
            ShowRoute("The algorithm should not be able to find a route around the barrier:", path);
            Console.WriteLine();
            */

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public void Run(int x, int y, ref int a)
        {
            a += 1;
            int[] dis;
            Point p = new Point(x, y);

            var disNum = Getdir(p, out dis);
            Thread.Sleep(1000);
            Random ran = new Random();
            int r = ran.Next(0, disNum - 1);
//            Console.WriteLine(r);
            int key = dis[r];
            Console.WriteLine(key);

            var pnext = GetAdjacentLocationsbyRandom(p, key);

            if (pnext.X > 0 && pnext.X <= 7 && pnext.Y > 0 && pnext.Y <= 7)
            {
                Console.WriteLine(pnext);
                if (a > 10)
                {
                    return;
                }
                Run(pnext.X, pnext.Y, ref a);
            }
        }


        /// <summary>
        /// 移动点到下一个选定的方向
        /// </summary>
        /// <param name="fromLocation">出发点坐标</param>
        /// <param name="i">选择的方向坐标</param>
        /// <returns>返回下一个点的坐标Piont</returns>
        private Point GetAdjacentLocationsbyRandom(Point fromLocation, int i)
        {
            switch (i)
            {
                case 1: //向上
                    return new Point(fromLocation.X, fromLocation.Y + 1);

                case 2: //向下
                    return new Point(fromLocation.X, fromLocation.Y - 1);

                case 3: //向右
                    return new Point(fromLocation.X + 1, fromLocation.Y);
                    ;
                case 4: //向左
                    return new Point(fromLocation.X - 1, fromLocation.Y);
                    ;
                default:
                    return new Point(0, 0);
            }
        }


        /// <summary>
        /// 判断当前点可移动的方向
        /// </summary>
        /// <param name="p">需要判断的点Piont</param>
        /// <param name="dis">out返回的方向数组</param>
        /// <returns>返回可行的方向有几个</returns>
        private int Getdir(Point p, out int[] dis)
        {
            dis = new int[4] {0, 0, 0, 0};
            int i = 0;

            if (p.X > 0 && p.Y > 0)
            {
                if (p.Y + 1 <= 7)
                {
                    //向上
                    dis[i] = 1;
                    i = i + 1;
                }

                if (p.Y - 1 > 0)
                {
                    //向下
                    dis[i] = 2;
                    i = i + 1;
                }

                if (p.X - 1 > 0)
                {
                    //向左
                    dis[i] = 4;
                    i = i + 1;
                }

                if (p.X + 1 <= 7)
                {
                    //向右
                    dis[i] = 3;
                    i=i+1;
                }



                return i;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Displays the map and path as a simple grid to the console
        /// </summary>
        /// <param name="title">A descriptive title</param>
        /// <param name="path">The points that comprise the path</param>
        private void ShowRoute(string title, IEnumerable<Point> path)
        {
            Console.WriteLine("{0}\r\n", title);
            for (int y = this.map.GetLength(1) - 1;
                y >= 0;
                y--) // Invert the Y-axis so that coordinate 0,0 is shown in the bottom-left
            {
                for (int x = 0; x < this.map.GetLength(0); x++)
                {
                    if (this.searchParameters.StartLocation.Equals(new Point(x, y)))
                        // Show the start position
                        Console.Write('S');
                    else if (this.searchParameters.EndLocation.Equals(new Point(x, y)))
                        // Show the end position
                        Console.Write('F');
                    else if (this.map[x, y] == false)
                        // Show any barriers
                        Console.Write('░');
                    else if (path.Where(p => p.X == x && p.Y == y).Any())
                        // Show the path in between
                        Console.Write('*');
                    else
                        // Show nodes that aren't part of the path
                        Console.Write('·');
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Creates a clear map with a start and end point and sets up the search parameters
        /// </summary>
        private void InitializeMap()
        {
            //  □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □
            //  □ S □ □ □ F □
            //  □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □

            this.map = new bool[7, 5];
            for (int y = 0; y < 5; y++)
            for (int x = 0; x < 7; x++)
                map[x, y] = true;

            var startLocation = new Point(1, 2);
            var endLocation = new Point(5, 2);
            this.searchParameters = new SearchParameters(startLocation, endLocation, map);
        }

        private void InitializeMap(int a)
        {
            this.map = new bool[7, 7];
            for (int y = 0; y < 7; y++)
            for (int x = 0; x < 7; x++)
                map[x, y] = true;
            var startLocation = new Point(0, 0);
            var endLocation = new Point(5, 5);
            this.searchParameters = new SearchParameters(startLocation, endLocation, map);
        }

        /// <summary>
        /// Create an L-shaped wall between S and F
        /// </summary>
        private void AddWallWithGap()
        {
            //  □ □ □ ■ □ □ □
            //  □ □ □ ■ □ □ □
            //  □ S □ ■ □ F □
            //  □ □ □ ■ ■ □ □
            //  □ □ □ □ □ □ □

            // Path: 1,2 ; 2,1 ; 3,0 ; 4,0 ; 5,1 ; 5,2

            this.map[3, 4] = false;
            this.map[3, 3] = false;
            this.map[3, 2] = false;
            this.map[3, 1] = false;
            this.map[4, 1] = false;
        }

        /// <summary>
        /// Create a closed barrier between S and F
        /// </summary>
        private void AddWallWithoutGap()
        {
            //  □ □ □ ■ □ □ □
            //  □ □ □ ■ □ □ □
            //  □ S □ ■ □ F □
            //  □ □ □ ■ □ □ □
            //  □ □ □ ■ □ □ □

            // No path

            this.map[3, 4] = false;
            this.map[3, 3] = false;
            this.map[3, 2] = false;
            this.map[3, 1] = false;
            this.map[3, 0] = false;
        }
    }
}
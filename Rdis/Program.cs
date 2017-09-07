using System;
using System.Drawing;
using System.Threading;
using Rdis.Properties;

namespace Rdis
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length!=3)
            {
                Console.WriteLine(Resources.help);
                Console.ReadKey();
                return;
            }
            var program = new Program();
            var a = 1;
            var x = int.Parse(args[0]);
            var y = int.Parse(args[1]);
            var max = int.Parse(args[2]);
            var srcDir = 0;
            program.Run(x, y, ref a, ref srcDir,max);
            Console.ReadKey();
        }


        //循环写法，resharper自动生成
        private void Run(int x, int y, ref int a, ref int srcDir, int maxNum)
        {
            while (true)
            {
                a += 1;
                var p = new Point(x, y);
                var disNum = GetAbleDir(p, out int[] dis);

                //获取上次路径中选取的方向的反方向
                var target = Array.IndexOf(dis, GetReDir(srcDir));

                //把来源路径屏蔽
                if (target != -1)
                    dis[target] = 0;

                //排序和反排序，用来将数组中不为0的数放到前面位置来
                Array.Sort(dis);
                Array.Reverse(dis);

                //确定随机的范围，因为随机是从1-N，数组下标是0-N-1
                if (srcDir != 0)
                    disNum -= 2;
                else
                    disNum -= 1;

                //等灯
                Thread.Sleep(20);
                //随机一个方向
                var ran = new Random();
                var r = ran.Next(0, disNum);
                var key = dis[r];

                //记录本次移动的方向
                srcDir = key;
//                Console.WriteLine(srcDir);
                //移动到下一个点
                var pnext = GetNextPointbyDis(p, key);

                if (pnext.X > 0 && pnext.X <= 6 && pnext.Y > 0 && pnext.Y <= 7)
                {
                    Console.WriteLine(pnext.Y.ToString()+pnext.X.ToString());
                    if (a > maxNum)
                        return;
                    x = pnext.X;
                    y = pnext.Y;
                    continue;
                }
                break;
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void RunbyRecursie(int x, int y, ref int a, ref int srcDir,int maxNum)
        {
            a += 1;
            var p = new Point(x, y);
            var disNum = GetAbleDir(p, out int[] dis);

            //获取上次路径中选取的方向的反方向
            var target = Array.IndexOf(dis, GetReDir(srcDir));
            //找到坐标之后替换
            if (target != -1)
                dis[target] = 0;

            //排序和反排序，把0放在后面
            Array.Sort(dis);
            Array.Reverse(dis);

            //确定随机的范围，因为随机是从1-N，数组下标是0-N-1
            if (srcDir != 0)
                disNum -= 2;
            else
                disNum -= 1;

            //等100ms，要不然随机种子一样(17ms)
            Thread.Sleep(100);
            var ran = new Random();
            var r = ran.Next(0, disNum);
            var key = dis[r];

            //记录方向
            srcDir = key;
            var pnext = GetNextPointbyDis(p, key);

            if (pnext.X > 0 && pnext.X <= 6 && pnext.Y > 0 && pnext.Y <= 7)
            {
                Console.WriteLine(pnext);
                if (a > maxNum)
                    return;
                RunbyRecursie(pnext.X, pnext.Y, ref a, ref srcDir,maxNum);
            }
        }


        /// <summary>
        ///     移动点到下一个选定的方向
        /// </summary>
        /// <param name="fromLocation">出发点坐标</param>
        /// <param name="i">选择的方向坐标</param>
        /// <returns>返回下一个点的坐标Piont</returns>
        private Point GetNextPointbyDis(Point fromLocation, int i)
        {
            switch (i)
            {
                case 1: //向上
                    return new Point(fromLocation.X, fromLocation.Y + 1);
                case 2: //向下
                    return new Point(fromLocation.X, fromLocation.Y - 1);
                case 3: //向右
                    return new Point(fromLocation.X + 1, fromLocation.Y);
                case 4: //向左
                    return new Point(fromLocation.X - 1, fromLocation.Y);
                case 5: //向左下
                    return new Point(fromLocation.X - 1, fromLocation.Y - 1);
                case 6: //向左上
                    return new Point(fromLocation.X - 1, fromLocation.Y + 1);
                case 7: //向右下
                    return new Point(fromLocation.X + 1, fromLocation.Y - 1);
                case 8: //向右上
                    return new Point(fromLocation.X + 1, fromLocation.Y + 1);

                default:
                    return new Point(0, 0);
            }
        }

        /// <summary>
        ///     通过来源方向获取反方向
        /// </summary>
        /// <param name="dis">来源的方向</param>
        /// <returns>对应的反方向</returns>
        private int GetReDir(int dis)
        {
            switch (dis)
            {
                case 1: //向上
                    return 2;
                case 2: //向下
                    return 1;
                case 3: //向右
                    return 4;
                case 4: //向左
                    return 3;
                case 5: //向左下
                    return 8;
                case 6: //向左上
                    return 7;
                case 7: //向右下
                    return 6;
                case 8: //向右上
                    return 5;
            }
            return 0;
        }


        /// <summary>
        ///     判断当前点可移动的方向
        /// </summary>
        /// <param name="p">需要判断的点Piont</param>
        /// <param name="dis">out返回的方向数组</param>
        /// <returns>返回可行的方向有几个</returns>
        private int GetAbleDir(Point p, out int[] dis)
        {
            dis = new[] {0, 0, 0, 0, 0, 0, 0, 0};
            var i = 0;

            //现在是按照8行7列算的，Y轴最大到8，X轴最大到7
            if (p.X > 0 && p.Y > 0)
            {
                if (p.X + 1 <= 6 && p.Y + 1 <= 7)
                {
                    dis[i] = 8;
                    i++;
                }
                if (p.X + 1 <= 6 && p.Y - 1 > 0)
                {
                    dis[i] = 7;
                    i++;
                }
                if (p.X - 1 > 0 && p.Y + 1 <= 7)
                {
                    dis[i] = 6;
                    i++;
                }
                if (p.X - 1 > 0 && p.Y - 1 > 0)
                {
                    dis[i] = 5;
                    i++;
                }
                if (p.X - 1 > 0)
                {
                    dis[i] = 4;
                    i++;
                }
                if (p.X + 1 <= 6)
                {
                    dis[i] = 3;
                    i++;
                }
                if (p.Y - 1 > 0)
                {
                    dis[i] = 2;
                    i++;
                }
                if (p.Y + 1 <= 7)
                {
                    dis[i] = 1;
                    i++;
                }
                return i;
            }
            return 0;
        }
    }
}
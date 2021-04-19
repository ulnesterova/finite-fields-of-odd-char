using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;

namespace Конечное_поле_нечетной_характеристики
{
    class Solution
    {
        private readonly BigInteger root1, root2;
        private readonly bool exists;

        public Solution(BigInteger root1, BigInteger root2, bool exists)
        {
            this.root1 = root1;
            this.root2 = root2;
            this.exists = exists;
        }

        public BigInteger Root1()
        {
            return root1;
        }

        public BigInteger Root2()
        {
            return root2;
        }

        public bool Exists()
        {
            return exists;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BigInteger q = 1000000009;
            BigInteger a = 665820697;
            //Метод Тонелли-Шенкса
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            Solution TS = Tonelli_Shanks(q, a);
            if (TS.Exists())
            {
                Console.WriteLine("Tonelli_Shanks root1 = {0}", TS.Root1());
                Console.WriteLine("Tonelli_Shanks root2 = {0}", TS.Root2());
            }
            else
            {
                Console.WriteLine("No solution exists");
            }
            watch1.Stop();
            Console.WriteLine(
            "Время выполнения программы в миллисекундах : " + watch1.ElapsedMilliseconds + "мс.\r\n"
            );
            //Метод Чиполлы
            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            Solution Cip = Cipolla(q, a);
            if (Cip.Exists())
            {
                Console.WriteLine("\nCipollo root1 = {0}", Cip.Root1());
                Console.WriteLine("Cipollo root2 = {0}", Cip.Root2());
            }
            else
            {
                Console.WriteLine("No solution exists");
            }
            watch2.Stop();
            Console.WriteLine(
            "Время выполнения программы в миллисекундах : " + watch2.ElapsedMilliseconds + "мс.\r\n"
            );
            //Метод Чиполлы-Леймера
            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            Solution Cip_Leh = Cipolla_Lehmer(q, a);
            if (Cip_Leh.Exists())
            {
                Console.WriteLine("\nCipollo_Lehmer root1 = {0}", Cip_Leh.Root1());
                Console.WriteLine("Cipollo_Lehmer root2 = {0}", Cip_Leh.Root2());
            }
            else
            {
                Console.WriteLine("No solution exists");
            }
            watch3.Stop();
            Console.WriteLine(
            "Время выполнения программы в миллисекундах : " + watch3.ElapsedMilliseconds + "мс.\r\n"
            );
            //Метод Поклингтона
            Stopwatch watch4 = new Stopwatch();
            watch4.Start();
            Solution Pockl = Pocklington(q, a);
            if (Pockl.Exists())
            {
                Console.WriteLine("\nPocklington root1 = {0}", Pockl.Root1());
                Console.WriteLine("Pocklington root2 = {0}", Pockl.Root2());
            }
            else
            {
                Console.WriteLine("No solution exists");
            }
            watch4.Stop();
            Console.WriteLine(
            "Время выполнения программы в миллисекундах : " + watch4.ElapsedMilliseconds + "мс.\r\n"
            );
            //Метод, восходящий к Лежандру
            Stopwatch watch5 = new Stopwatch();
            watch5.Start();
            Solution Leg = Legendre(q, a);
            if (Leg.Exists())
            {
                Console.WriteLine("\nLegendre root1 = {0}", Leg.Root1());
                Console.WriteLine("Legendre root2 = {0}", Leg.Root2());
            }
            else
            {
                Console.WriteLine("No solution exists");
            }
            watch5.Stop();
            Console.WriteLine(
            "Время выполнения программы в миллисекундах : " + watch5.ElapsedMilliseconds + "мс.\r\n"
            );
        }

        static Solution Tonelli_Shanks(BigInteger q, BigInteger a)
        {
            if (BigInteger.ModPow(a, (q - 1) / 2, q) != 1)
            {
                return new Solution(0, 0, false);
            }
            //Вычисление s,t из N таких, что q-1=(2^s)t, t нечетно
            int s = 0;
            BigInteger t = q - 1;
            while (t % 2 == 0)
            {
                s = s + 1;
                t = t / 2;
            }

            if (s == 1)
            {
                BigInteger r1 = BigInteger.ModPow(a, (q + 1) / 4, q);
                return new Solution(r1, q - r1, true);
            }

            //Вычисляем (путем перебора) квадратичный невычет b поля F_q
            BigInteger b = 2;
            while (BigInteger.ModPow(b, (q - 1) / 2, q) != q - 1)
            {
                b = b + 1;
            }

            BigInteger g = BigInteger.ModPow(b, t, q);
            BigInteger r = BigInteger.ModPow(a, (t + 1) / 2, q);
            BigInteger c = BigInteger.ModPow(a, t, q);
            BigInteger m = s;

            while (true)
            {
                if (c == 1)
                {
                    return new Solution(r, q - r, true);
                }
                BigInteger i = 0;
                BigInteger zz = c;
                while (zz != 1 && i < (m - 1))
                {
                    zz = zz * zz % q;
                    i = i + 1;
                }
                BigInteger h = g;
                BigInteger e = m - i - 1;
                while (e > 0)
                {
                    h = h * h % q;
                    e = e - 1;
                }
                r = r * h % q;
                g = h * h % q;
                c = c * g % q;
                m = i;
            }
        }

        static Solution Pocklington(BigInteger q, BigInteger a)
        {
            if (BigInteger.ModPow(a, (q - 1) / 2, q) != 1)
            {
                return new Solution(0, 0, false);
            }



            a = q - a;

            BigInteger Pr(BigInteger r)
            {
                if (r >= 0)
                    return r % q;
                else
                    return (q - (-r) % q) % q;
            }
            BigInteger c = 1;
            BigInteger Not_Square;
            BigInteger u = Pr(q - 1);
            while (true)
            {
                Not_Square = (c * c + u * a) % q;
                if (BigInteger.ModPow(Not_Square, (q - 1) / 2, q) == q - 1)
                {
                    break;
                }
                ++c;
            }

            BigInteger pow = (q - 1) / 4;
            Tuple<BigInteger, BigInteger> z = new Tuple<BigInteger, BigInteger>(c, 1);
            Tuple<BigInteger, BigInteger> y = new Tuple<BigInteger, BigInteger>(1, 0);

            Tuple<BigInteger, BigInteger> mult(Tuple<BigInteger, BigInteger> x, Tuple<BigInteger, BigInteger> y)
            {
                return new Tuple<BigInteger, BigInteger>(
                    Pr(x.Item1 * y.Item1 + x.Item2 * y.Item2 * a) % q,
                    Pr(x.Item1 * y.Item2 + y.Item1 * x.Item2) % q
                );
            }

            while (pow > 0)
            {
                if (pow % 2 != 0)
                {
                    y = mult(y, z);
                }
                pow = pow / 2;
                z = mult(z, z);
            }
            BigInteger temp_g = BigInteger.ModPow(y.Item2, q - 2, q);
            BigInteger result = y.Item1 * temp_g % q;

            return new Solution(result, q - result, true);
        }

        static Solution Cipolla(BigInteger q, BigInteger a)
        {
            if (BigInteger.ModPow(a, (q - 1) / 2, q) != 1)
            {
                return new Solution(0, 0, false);
            }

            //Находим b и неквадрат b^2-a
            BigInteger b = 0;
            BigInteger Not_Square;
            while (true)
            {
                Not_Square = (b * b + q - a) % q;
                if (BigInteger.ModPow(Not_Square, (q - 1) / 2, q) == q - 1)
                {
                    break;
                }
                ++b;
            }

            BigInteger pow = (q + 1) / 2;

            Tuple<BigInteger, BigInteger> mult(Tuple<BigInteger, BigInteger> x, Tuple<BigInteger, BigInteger> y)
            {
                return new Tuple<BigInteger, BigInteger>(
                    (x.Item1 * y.Item1 + x.Item2 * y.Item2 * Not_Square) % q,
                    (x.Item1 * y.Item2 + y.Item1 * x.Item2) % q
                );
            }

            Tuple<BigInteger, BigInteger> r = new Tuple<BigInteger, BigInteger>(1, 0);
            Tuple<BigInteger, BigInteger> s = new Tuple<BigInteger, BigInteger>(b, 1);
            while (pow > 0)
            {
                if ((pow % 2) != 0)
                {
                    r = mult(r, s);
                }
                s = mult(s, s);
                pow = pow / 2;
            }

            return new Solution(r.Item1, q - r.Item1, true);
        }

        static Solution Cipolla_Lehmer(BigInteger q, BigInteger a)
        {
            if (BigInteger.ModPow(a, (q - 1) / 2, q) != 1)
            {
                return new Solution(0, 0, false);
            }


            BigInteger Pr(BigInteger r)
            {
                if (r >= 0)
                    return r % q;
                else
                    return (q - (-r) % q) % q;
            }
            BigInteger b = 1;
            BigInteger Not_Square;
            BigInteger u = Pr(q - 4);
            while (true)
            {
                Not_Square = (b * b + u * a) % q;
                if (BigInteger.ModPow(Not_Square, (q - 1) / 2, q) == q - 1)
                {
                    break;
                }
                ++b;
            }

            BigInteger pow = (q + 1) / 2;


            Tuple<BigInteger, BigInteger> s = new Tuple<BigInteger, BigInteger>(1, 0);
            Tuple<BigInteger, BigInteger> t = new Tuple<BigInteger, BigInteger>(0, 1);

            Tuple<BigInteger, BigInteger> mul(Tuple<BigInteger, BigInteger> x, Tuple<BigInteger, BigInteger> y)
            {
                return new Tuple<BigInteger, BigInteger>(
                    (x.Item1 * y.Item1 - x.Item2 * y.Item2 * a) % q,
                    (x.Item1 * y.Item2 + y.Item1 * x.Item2 + x.Item2 * y.Item2 * b) % q
                );
            }

            while (pow > 0)
            {
                if (pow % 2 != 0)
                {
                    s = mul(s, t);
                }
                pow = pow / 2;
                t = mul(t, t);
            }

            return new Solution(Pr(s.Item1), Pr(q - s.Item1), true);
        }

        static Solution Legendre(BigInteger q, BigInteger a)
        {
            if (BigInteger.ModPow(a, (q - 1) / 2, q) != 1)
            {
                return new Solution(0, 0, false);
            }


            BigInteger Pr(BigInteger r)
            {
                if (r >= 0)
                    return r % q;
                else
                    return (q - (-r) % q) % q;
            }
            BigInteger c = 1;
            BigInteger Not_Square;
            BigInteger u = Pr(q - 1);
            while (true)
            {
                Not_Square = (c * c + u * a) % q;
                if (BigInteger.ModPow(Not_Square, (q - 1) / 2, q) == q - 1)
                {
                    break;
                }
                ++c;
            }

            BigInteger pow = (q - 1) / 2;
            Tuple<BigInteger, BigInteger> z = new Tuple<BigInteger, BigInteger>(c, 1);
            Tuple<BigInteger, BigInteger> y = new Tuple<BigInteger, BigInteger>(1, 0);

            Tuple<BigInteger, BigInteger> mult(Tuple<BigInteger, BigInteger> x, Tuple<BigInteger, BigInteger> y)
            {
                return new Tuple<BigInteger, BigInteger>(
                    (x.Item1 * y.Item1 + x.Item2 * y.Item2 * a) % q,
                    (x.Item1 * y.Item2 + y.Item1 * x.Item2) % q
                );
            }

            while (pow > 0)
            {
                if (pow % 2 != 0)
                {
                    y = mult(y, z);
                }
                pow = pow / 2;
                z = mult(z, z);
            }

            BigInteger temp = BigInteger.ModPow(y.Item2, q - 2, q);
            return new Solution(temp, q - temp, true);
        }
    }
}

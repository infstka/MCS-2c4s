using System;
using static ConsoleApp1.BS;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // ---------- Получаем необходимые данные и заполняем ими объекты БС
            Console.Write("Введите количество БС : ");
            int amountOfBS = int.Parse(Console.ReadLine());
            Console.Write("Введите координационное расстояние : ");
            int radius = int.Parse(Console.ReadLine());

            // массив для хранения объектов станций
            BS[] bs = new BS[amountOfBS];

            //for (int i = 0; i < amountOfBS; i++)
            //{
            //    bs[i] = new BS();

            //    // номер БС
            //    Console.Write("\nВведите номер БС : ");
            //    bs[i].Number = int.Parse(Console.ReadLine());

            //    // координаты БС


            //    Console.Write("Введите координаты x, y (Пример \"5 1\", где 5 по x, 1 по y) : ");
            //    string coordsArray = Console.ReadLine();
            //    string[] coords = coordsArray.Split(' '); // массив координат (x, y)
            //    bs[i].CoordinatesXY = new BS.Coordinate(int.Parse(coords[0]), int.Parse(coords[1]));

            //    // БС, работающие на той же частоте
            //    Console.Write("Введите номера БС, которые работают на той же частоте, что и данная (Пример: 1 4 5) : ");
            //    string sameFrequencyBSArray = Console.ReadLine();
            //    string[] sameFrequencyBS = sameFrequencyBSArray.Split(' ');

            //    // так как в классе мы храним массив БС на той же частоте, то определяем новый массив для заполнения
            //    // номерами базовых станций той же частоты
            //    bs[i].ArrayOfFrequencySameBS = new int[sameFrequencyBS.Length];
            //    // заполняем массив номерами БС
            //    for (int a = 0; a < sameFrequencyBS.Length; a++)
            //    {
            //        bs[i].ArrayOfFrequencySameBS[a] = int.Parse(sameFrequencyBS[a]);
            //    }
            //}

            bs[0] = new BS(1, new Coordinate(0, 0), new int[] { 4 });
            bs[1] = new BS(2, new Coordinate(2, 9), new int[] { 3, 5 });
            bs[2] = new BS(3, new Coordinate(3, 3), new int[] { 2, 5 });
            bs[3] = new BS(4, new Coordinate(-3, 5), new int[] { 1 });
            bs[4] = new BS(5, new Coordinate(-5, 3), new int[] { 2, 3 });
            bs[5] = new BS(6, new Coordinate(8, 9), new int[] { 7, 8 });
            bs[6] = new BS(7, new Coordinate(-4, 9), new int[] { 6, 8 });
            bs[7] = new BS(8, new Coordinate(2, 6), new int[] { 6, 7 });

            Console.WriteLine("\nВывод введенных данных :");
            for (int i = 0; i < amountOfBS; i++)
                Console.WriteLine(bs[i].ToString());

            // ---------- Построение графа связи с помощью координационых колец
            // создаем матрицу смежности, по умолчанию она заполнится нулями
            int[,] matrix = new int[amountOfBS, amountOfBS];

            // i - индекс БС в массиве bs объектов базовых станций
            for (int i = 0; i < amountOfBS; i++)
            {
                // a = индекс номера БС, которая записана в массив базовых станций той же частоты, что рассматриваемая
                for (int a = 0; a < bs[i].ArrayOfFrequencySameBS.Length; a++)
                {
                    // расчет дистанции между рассматриваемой БС (bs[i]) и БС той же частоты (bs[bs[i].ArrayOfFrequencySameBS[a] - 1])
                    // bs[i].ArrayOfFrequencySameBS[a] - 1 => 
                    // bs[i] - берем рассматриваемую БС, 
                    // ArrayOfFrequencySameBS[a] - 1 - берем номер БС из массива базовых станций той же частоты и отнимаем 1 
                    // для получения индекса данной БС в массиве объектов базовых станций (bs)
                    double dist = BS.Distance(bs[i].CoordinatesXY, bs[bs[i].ArrayOfFrequencySameBS[a] - 1].CoordinatesXY);

                    if (dist < radius) // если Координационное расстояние  больше расстояние то есть пересечение и в матрицу записывается 1
                        matrix[i, bs[bs[i].ArrayOfFrequencySameBS[a] - 1].Number - 1] = 1;
                }
            }

            // ---------- Выводим матрицу смежности полученного графа
            Console.Write("\n\nГраф:\n\n\t  ");
            for (int i = 1; i <= amountOfBS; i++)
                Console.Write(i + " ");
            Console.WriteLine();

            for (int row = 0; row < amountOfBS; row++)
            {
                Console.Write("\t" + (row + 1).ToString() + " "); // номер строки матрицы

                for (int column = 0; column < amountOfBS; column++)
                {
                    Console.Write(matrix[row, column] + " ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }

    internal class BS
    {
        public int Number { get; set; }
        public Coordinate CoordinatesXY { get; set; }
        public int[] ArrayOfFrequencySameBS;

        public BS(int number, Coordinate cord, int[] neib)
        {
            Number = number;
            CoordinatesXY = cord;
            ArrayOfFrequencySameBS = neib;
        }

        public BS()
        {
        }

        public override string ToString()
        {
            string coordsArray = "";
            for (int i = 0; i < this.ArrayOfFrequencySameBS.Length; i++)
            {
                coordsArray += ArrayOfFrequencySameBS[i] + " ";
            }
            return
                "Номер: " + this.Number +
                ", Координаты: (X:" + this.CoordinatesXY.X + ",Y:" + this.CoordinatesXY.Y +
                "), на одной частоте с " + coordsArray;
        }

        public static double Distance(Coordinate A, Coordinate B)
        {
            // высчитываем расстояние между станциями как гипотенузу по теореме Пифагора
            return Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
        }

        internal class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Coordinate(int _x, int _y)
            {
                X = _x;
                Y = _y;
            }
        }
    }
}

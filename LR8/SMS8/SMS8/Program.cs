using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SMS8
{
    internal class Program
    { 
        static class Task1 {
        public static void task1(string input)
        {
            string mask = RandMask("01234567");
            Console.WriteLine($"Маска - {mask}");

            string afterInput = InterLeaving(input, mask);
            Console.WriteLine($"Строка после перемежения - {afterInput}");
            string afterErrors = ErrorsGenerator(afterInput, 3, 9);
            Console.WriteLine($"Строка после воздействия ошибок - {afterErrors}");
            string afterAntiInput = AntiInterLeaving(afterErrors, mask);
            Console.WriteLine($"Строка после восстановления - {afterAntiInput}");
            LessErorrs(afterInput, mask, 3);
        }

        static string RandMask(string str)
        {
            List<char> randArr = new List<char>();
            Random random = new Random();
            while (randArr.Count != str.Length)
            {
                char rand = random.Next(0, str.Length).ToString()[0];
                if (!randArr.Contains(rand))
                    randArr.Add(rand);
            }
            return new string(randArr.ToArray());
        }

        static string InterLeaving(string str, string mask) // Перемежение
        {
            string output = "";
            for (int i = 0; i < 4; i++)
            {
                foreach (char m in mask)
                {
                    output += str.Substring(i * 8, 8)[(int)Char.GetNumericValue(m)];
                }
            }
            return output;
        }

        static string AntiInterLeaving(string str, string mask)
        {
            char[] antiArr = new char[32];
            for (int i = 0; i < 4; i++)
            {
                foreach (char m in mask)
                {
                    antiArr[(int)Char.GetNumericValue(m) + 8 * i] = str.Substring(i * 8, 8)[mask.IndexOf(m)];
                }
            }
            return new string(antiArr);
        }

        static string ErrorsGenerator(string str, int variant, int length)
        {
            char[] errors = str.ToCharArray();
            string final = "";

            for (int i = variant * 2; i < variant * 2 + length; i++)
            {
                errors[i] = ' ';
            }

            for (int i = 0; i < errors.Length; i++)
                final += errors[i];

            return final;
        }

        static string LessErorrs(string before, string mask, int variant)
        {
            Regex reg = new Regex(@"[а-яА-Я]\s\s[а-яА-Я|_*|]");
            int losing_bits = 8;
            string after_losing = before;
            for (int i = 8; i != 0; i--)
            {
                after_losing = AntiInterLeaving(ErrorsGenerator(before, variant, i), mask);
                Console.WriteLine(after_losing);
                losing_bits = i;
                if (reg.Match(after_losing).Success)
                {
                    break;
                }
            }
            Console.WriteLine("\nВыходная строка: " + after_losing);
            Console.WriteLine("Число потерянных бит: " + losing_bits);
            return after_losing;
        }
    }

        static class Task2 {
        public static void task2(string str)
        {
            string[] firstlvl = FirstLevel(str);
            Console.WriteLine("Первый уровень перемежения:");
            foreach (string el in firstlvl)
            {
                Console.WriteLine(el);
            }
            Console.WriteLine("Второй уровень перемежения:");
            string[] secondlvl = SecondLevel(firstlvl);
            foreach (string el in secondlvl)
            {
                Console.WriteLine(el);
            }
            Console.WriteLine("После потери 11 бит:");
            string[] afterErrors = Errors(secondlvl, 4);
            foreach (string el in afterErrors)
            {
                Console.WriteLine(el);
            }
            Console.WriteLine("Первоначальная строка");
            Console.WriteLine(InitialString(afterErrors));
        }

        static string[] FirstLevel(string str)
        {
            string[] sublvl = new string[8];
            for(int i = 0; i < 8; i++)
            {
                sublvl[i] = str.Substring(i * 4, 4);
            }
            string[] output = new string[4];
            for (int i = 0; i < 4; i++)
            {
                output[i] = $"{i + 1}{i + 1}{i + 1}{sublvl[i*2]}--{sublvl[i*2+1]}{i + 1}{i + 1}{i + 1}";
            }
            return output;
        }
        static string[] SecondLevel(string[] str)
        {
            string[] output = new string[4];
            output[0] = $"{str[0]}--{str[3]}";
            for (int i = 0; i < str[0].Length; i++)
                output[0] += ' ';
            for (int i = 1; i < 4; i++)
            {
                output[i] = $"{str[i]}--{str[i-1]}";
            }
            return output;
        }

        static string[] Errors(string[] str, int variant)
        {
            string[] output = new string[4];
            string temp = "";
            foreach (string i in str)
            {
                temp += i;
            }
            temp = temp.Substring(0, temp.Length / 4 - 4) + temp.Substring(temp.Length / 4 + 12);
            char[] errors = temp.ToCharArray();
            for (int i = variant * 2; i < variant * 2 + 11; i++)
            {
                errors[i] = ' ';
            }
            for (int i = 0; i < 4; i++)
            {
                output[i] = new string(errors).Substring(errors.Length/4 * i, errors.Length / 4);
            }
            return output;
        }

        static string InitialString(string[] str)
        { 
            string[] temp = {str[1], str[2], str[3] };
            string tempStr = "";
            string output = "";
            foreach (string i in temp)
            {
                tempStr += i;
            }
            tempStr = tempStr.Substring(16);
            tempStr = tempStr.Substring(0, tempStr.Length - 16);
            temp = tempStr.Split('-');
            tempStr = "";
            foreach (string i in temp)
            {
                tempStr += i;
            }
            return tempStr.Substring(3, 8) + tempStr.Substring(31, 8) + tempStr.Substring(17, 8) + tempStr.Substring(45, 8);
        }
    }
        
        public static void Main(string[] args)
        {
            string input = "БобровичГлебСергеевич_группа7___";


            Console.WriteLine($"<- Задание 1 ->\n" +
                              $"Исходная строка - {input}");
            Task1.task1(input);
            Console.ReadKey();
            Console.Clear();
            
            Console.WriteLine($"<- Задание 2 ->" +
                              $"\nИсходная строка - {input}");
            Task2.task2(input);
        }
    }
}
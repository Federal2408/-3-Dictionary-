using System;
using System.Collections.Generic;

namespace HashTable
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dct = new CustomDictionary<string, int>
            {
                {"Санкт-Петербург", 1800}, {"Саратов", 0}, {"Таганрог", 1800}
            };
            Print(dct);
            Console.WriteLine();
            Console.WriteLine("После изменений:" + '\n');
            dct.Remove("Кастрома");
            dct.Remove("Саратов");
            Print(dct);
            dct.Clear();
            Console.WriteLine("------------");
            Print(dct);
            dct.Add("Санкт-Петербург", 900);
            dct.Add("Саратов", 0);
            dct.Add("Таганрог", 300);
            Console.WriteLine(dct["Саратов"]);
            Console.WriteLine(dct.ContainsKey("Санкт-Петербург"));

        }

        public static void Print(CustomDictionary<string, int> dct)
        {
            if (dct.Count == 0)
            {
                Console.WriteLine("Словарь пуст!");
            }

            foreach (var item in dct)
            {
                Console.WriteLine($"Расстояние: {item.Key} - Саратов = {item.Value} км.");
            }
        }
    }
}
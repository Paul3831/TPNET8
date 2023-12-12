using System.Collections.Generic;
using System;
using System.Linq;

namespace consoleProject;

class Program
{
    static void Main(string[] args)
    {
        // Console.WriteLine("Hello, World!");

        // Console.WriteLine("Press any key to exit.");

        var maListe = new List<object> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        var maListe2 = new List<object> { 1, 2, 3, 4, 5, 6, 7, 8, 9, maListe };


        var somme = Sum(maListe2);

        Console.WriteLine($"Somme = {somme}");

        String? test = null;

        Console.WriteLine($"test = {test}");

    }


    public static int Sum(IEnumerable<object> values)
    {
        var sum = 0;
        foreach (var item in values)
        {
            switch (item)
            {
                case 0:
                    break;
                case int val:
                    sum += val;
                    break;
                case IEnumerable<object> subList when subList.Any():
                    sum += Sum(subList);
                    break;
                case IEnumerable<object> subList:
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentException("Invalid item type");
            }
        }
        return sum;
    }
}

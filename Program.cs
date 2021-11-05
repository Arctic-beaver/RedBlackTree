using System;

namespace RedBlackTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            tree.SeeCool();

            Console.WriteLine();
            Console.WriteLine("Отсоритрованные элементы: ");
            tree.SortOutput();

            Console.WriteLine();
            Console.WriteLine($"Min: {tree.Min().Key}");
            Console.WriteLine($"Max: {tree.Max().Key}");
            tree.BFS();
            tree.CLR();
            tree.LCR();
            tree.LRC();
            tree.Delete(10);
            Console.WriteLine("Deleted 10: ");
            tree.SeeCool();
            tree.Delete(0);
            Console.WriteLine("Deleted 0: ");
            tree.SeeCool();
            
        }
    }
}

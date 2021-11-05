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
            Console.WriteLine($"Min: {tree.Min().Key}");
            Console.WriteLine($"Max: {tree.Max().Key}");
            tree.BFS();

        }
    }
}

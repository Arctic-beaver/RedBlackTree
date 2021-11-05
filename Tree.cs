using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public class Tree
    {
        private TreeNode _root;
        public int AmountOfElements { get; private set; }
        public int TreeHeight { get; private set; }
        public int BlackTreeHeight { get; private set; }

        public Tree()
        {
            Insert(4);
            SeeCool();
            Insert(45);
            SeeCool();
            Insert(34);
            SeeCool();
            Insert(23);
            SeeCool();
            Insert(1234);
            SeeCool();
            Insert(9);
            SeeCool();
            Insert(10);
            SeeCool();
            //Insert(345);
            //SeeCool();
            //Insert(0);
            //SeeCool();
            //Insert(5);
            //SeeCool();
        }

        public void SeeCool()
        {
            _root.Print(textFormat: "(0)", spacing: 2);
        }

        public TreeNode Search(int key)
        {
            TreeNode shovel = _root;
            while (shovel != null && shovel.Key != key)
            {
                if (shovel.Key > key)
                {
                    shovel = shovel.Left;
                }
                else
                {
                    shovel = shovel.Right;
                }
            }
            return shovel;
        }

        public TreeNode Min()
        {
            //ищем самый левый узел в дереве
            TreeNode shovel = _root;
            while (shovel.Left != null)
            {
                shovel = shovel.Left;
            }
            return shovel;
        }

        public TreeNode Max()
        {
            //ищем самый правый узел в дереве
            TreeNode shovel = _root;
            while (shovel.Right != null)
            {
                shovel = shovel.Right;
            }
            return shovel;
        }

        public TreeNode SearchSuccessor(TreeNode node)
        {
            if (node.Right != null) return Min();
            TreeNode parent = node.Parent;
            while (parent != null && node == parent.Right)
            {
                node = parent;
                parent = node.Parent;
            }
            return parent;
        }

        private void TreeRotateR(TreeNode child, TreeNode parent)
        {
            parent.Left = child.Right;
            child.Right = parent;
            child.Color = parent.Color;
            parent.Color = "Red";
            if (_root == parent) _root = child;

            ChangeParent(child, parent);
        }


        private void TreeRotateL(TreeNode child, TreeNode parent)
        {
            //левосторонний поворот просходит только тогда, когда цвет childNode - красный
            parent.Right = child.Left;
            child.Left = parent;
            child.Color = parent.Color;
            parent.Color = "Red";
            if (_root == parent) _root = child;

            ChangeParent(child, parent);
        }



        private void ChangeParent(TreeNode child, TreeNode parent)
        {
            if (parent.Parent != null)
            {
                TreeNode grandParent = parent.Parent;
                if (grandParent.Left == parent) grandParent.Left = child;
                else grandParent.Right = child;

                child.Parent = grandParent;
                parent.Parent = child;
            }
            else
            {
                child.Parent = null;
                parent.Parent = child;
            }
        }

        public void Insert(int key)
        {
            if (_root == null)
            {
                _root = new TreeNode(key);
                _root.Color = "Black";
                return;
            }

            TreeNode newNode = FindPlaceAndInsert(_root, key);
            if (newNode.Key == -1) throw new InvalidOperationException("-1!!");
            SeeCool();
            newNode.Color = "Red";
            TreeNode y;
            while (newNode != _root && newNode.Parent.Color == "Red")
            {
                if (newNode.Parent == newNode.Parent.Parent.Left)
                {
                    y = newNode.Parent.Parent.Right;
                    if (y != null)
                    {
                        if (y.Color == "Red")
                        {
                            newNode.Parent.Color = "Black";
                            y.Color = "Black";
                            newNode.Parent.Parent.Color = "Red";
                            newNode = newNode.Parent.Parent;
                        }
                        else
                        {
                            if (newNode == newNode.Parent.Right)
                            {
                                newNode = newNode.Parent;
                                TreeRotateL(newNode.Left, newNode);
                            }

                            newNode.Parent.Color = "Black";
                            newNode.Parent.Parent.Color = "Red";
                            TreeRotateR(newNode.Parent, newNode.Parent.Parent);
                        }
                    }
                    
                } else
                {
                    
                    y = newNode.Parent.Parent.Left;
                    if (y != null)
                    {
                        if (y.Color == "Red")
                        {
                            newNode.Parent.Color = "Black";
                            y.Color = "Black";
                            newNode.Parent.Parent.Color = "Red";
                            newNode = newNode.Parent.Parent;
                        }
                        else
                        {
                            if (newNode == newNode.Parent.Left)
                            {
                                newNode = newNode.Parent;
                                TreeRotateL(newNode.Right, newNode);
                            }

                            newNode.Parent.Color = "Black";
                            newNode.Parent.Parent.Color = "Red";
                            TreeRotateR(newNode.Parent, newNode.Parent.Parent);
                        }
                    }
                    
                }
                _root.Color = "Black";
            }


           // BalanseAndRepainting(newNode);

            AmountOfElements++;
        }

        //Рекурсивная ставка.
        //возвращает вставленный узел
        private TreeNode FindPlaceAndInsert(TreeNode node, int value)
        {
            //Случай 1: Вставляемое значение меньше значения узла
            if (value < node.Key)
            {
                if (node.Left == null)
                {
                    node.Left = new TreeNode(value);
                    node.Left.Parent = node;
                    return node.Left;
                }
                return FindPlaceAndInsert(node.Left, value);
            }
            //Случай 2: Вставляемое значение больше или равно значению узла.
            else
            {
                if (node.Right == null)
                {
                    node.Right = new TreeNode(value);
                    node.Right.Parent = node;
                    return node.Right;
                }
                return FindPlaceAndInsert(node.Right, value);
            }
        }

        private void SwapFamilyColor(TreeNode node)
        {
            //Свап цвета применяется тогда, когда у parentNode два красных потомка.
            //Потомки становятся черными, а parentNode - красным.

            node.Color = "Red";
            node.Left.Color = "Black";
            node.Right.Color = "Black";

            if (node == _root) node.Color = "Black";
        }

        private void SwapColour(TreeNode node)
        {
            if (node.Color == "Red") node.Color = "Black";
            else node.Color = "Red";
        }

        

        private void BalanseAndRepainting(TreeNode insertedNode)
        {
            // 1. Дядя добавляемого узла красный


        }

        public void BFS()
        {
            Console.WriteLine("Breadth first (BFS): ");
            Queue<TreeNode> visited = BreadthFirstSearch();
            while (visited.Any())
            {
                Console.Write($"{visited.Dequeue().Key} ");
            }
        }

        //Dequeue: извлекает и возвращает первый элемент очереди

        //Enqueue: добавляет элемент в конец очереди

        //Peek: просто возвращает первый элемент из начала очереди без его удаления

        private Queue<TreeNode> BreadthFirstSearch()
        {
            Queue<TreeNode> notVisited = new Queue<TreeNode>();
            notVisited.Enqueue(_root);
            Queue<TreeNode> visited = new Queue<TreeNode>();

            TreeNode shovel;
            while (notVisited.Any())
            {
                shovel = notVisited.Dequeue();
                Console.WriteLine(" " + shovel.Key);
                visited.Enqueue(shovel);
                if (shovel.Left != null)
                    notVisited.Enqueue(shovel.Left);
                if (shovel.Right != null)
                    notVisited.Enqueue(shovel.Right);
            }
            return visited;
        }



        //        • КЛП — «корень - левый - правый» (обход в прямом порядке):

        //посетить корень
        //обойти левое поддерево
        //обойти правое поддерево
        //• ЛКП — «левый - корень - правый» (симметричный обход):

        //обойти левое поддерево
        //посетить корень
        //обойти правое поддерево
        //• ЛПК — «левый - правый - корень» (обход в обратном порядке):

        //обойти левое поддерево
        //обойти правое поддерево
        //посетить корень

    }
}

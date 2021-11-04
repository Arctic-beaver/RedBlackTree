using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    class Tree
    {
        private TreeNode _root;
        public int AmountOfElements { get; private set; }
        public int TreeHeight { get; private set; }
        public int BlackTreeHeight { get; private set; }

        public Tree()
        {
            AmountOfElements = 5;
        }

        public void PrintTree()
        {

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

        public void Insert(int key)
        {
            if (_root == null)
            {
                _root = new TreeNode(key);
                _root.Color = "Black";
                return;
            }

            TreeNode newNode = FindPlaceAndInsert(_root, key);
            newNode.Color = "Red";
            if (newNode )

            AmountOfElements++;
        }

        //Рекурсивная ставка.
        //возвращает вставленный узел
        private TreeNode FindPlaceAndInsert(TreeNode node, int value)
        {
            //Случай 1: Вставляемое значение меньше значения узла
            if (value.CompareTo(node.Key) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new TreeNode(value);
                    return node.Left;
                }
                else
                {
                    FindPlaceAndInsert(node.Left, value);
                }
            }
            //Случай 2: Вставляемое значение больше или равно значению узла.
            else
            {
                if (node.Right == null)
                {
                    node.Right = new TreeNode(value);
                    return node.Right;
                }
                else
                {
                    FindPlaceAndInsert(node.Right, value);
                }
            }
            return null;
        }

        public bool Delete(int key)
        {
            TreeNode current, parent;

            // Находим удаляемый узел.
            current = Search(key);
            parent = current.Parent;

            if (current == null)
            {
                //нет узла с таким значением
                return false;
            }

            //Случай 1: Если нет детей справа,
            //левый ребенок встает на место удаляемого.
            if (current.Right == null)
            {
                DeleteIfNoRightChildren(parent, current);
            }

            //Случай 2: Если у правого ребенка нет детей слева
            //то он занимает место удаляемого узла.
            else if (current.Right.Left == null)
            {
                DeleteIfRightChildHaveNoLeftChildren(parent, current);
            }

            //Случай 3: Если у правого ребенка есть дети слева,
            //крайний левый ребенок 
            //из правого поддерева заменяет удаляемый узел.
            else
            {
                DeleteIfFullFamily(parent, current);
            }
            AmountOfElements--;
            return true;
        }

        private void DeleteIfNoRightChildren(TreeNode parent, TreeNode current)
        {
            if (parent == null)
            {
                _root = current.Left;
            }
            else
            {
                if (parent.Key > current.Key)
                {
                    //левый ребенок текущего узла становится левым ребенком родителя.
                    parent.Left = current.Left;
                }
                else if (parent.Key < current.Key)
                {
                    //левый ребенок текущего узла становится правым ребенком родителя.
                    parent.Right = current.Left;
                }
            }
        }

        private void DeleteIfRightChildHaveNoLeftChildren(TreeNode parent, TreeNode current)
        {
            current.Right.Left = current.Left;
            if (parent == null)
            {
                _root = current.Right;
            }
            else
            {
                if (parent.Key > current.Key)
                {
                    //правый ребенок текущего узла становится левым ребенком родителя.
                    parent.Left = current.Right;
                }
                else if (parent.Key < current.Key)
                {
                    //правый ребенок текущего узла становится правым ребенком родителя.
                    parent.Right = current.Right;
                }
            }
        }

        private void DeleteIfFullFamily(TreeNode parent, TreeNode current)
        {
            //Найдем крайний левый узел.
            TreeNode leftmost = current.Right.Left;
            TreeNode leftmostParent = current.Right;
            while (leftmost.Left != null)
            {
                leftmostParent = leftmost; leftmost = leftmost.Left;
            }
            //Левое поддерево родителя становится правым поддеревом
            //крайнего левого узла.
            leftmostParent.Left = leftmost.Right;
            //Левый и правый ребенок текущего узла становится левым
            //и правым ребенком крайнего левого.
            leftmost.Left = current.Left;
            leftmost.Right = current.Right;

            if (parent == null)
            {
                _root = leftmost;
            }
            else
            {
                if (parent.Key > current.Key)
                {
                    //крайний левый узел становится левым ребенком родителя.
                    parent.Left = leftmost;
                }
                else if (parent.Key < current.Key)
                {
                    //крайний левый узел становится правым ребенком родителя.
                    parent.Right = leftmost;
                }
            }
        }



    }
}

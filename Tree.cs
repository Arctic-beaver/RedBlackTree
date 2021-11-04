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
            BalanseAndRepainting(newNode);

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

        private void LeftHandTurn(TreeNode child, TreeNode parent)
        {
            //левосторонний поворот просходит только тогда, когда цвет childNode - красный
            parent.Right = child.Left;
            child.Left = parent;
            child.Color = parent.Color;
            parent.Color = "Red";
            if (_root == parent) _root = child;

            ChangeParent(child, parent);
        }

        private void RightHandTurn(TreeNode child, TreeNode parent)
        {
            parent.Left = child.Right;
            child.Right = parent;
            child.Color = parent.Color;
            parent.Color = "Red";
            if (_root == parent) _root = child;

            ChangeParent(child, parent);
        }

        private void ChangeParent(TreeNode child, TreeNode parent)
        {
            if (parent.Parent.Exists)
            {
                TreeNode grandParent = parent.Parent;
                if (grandParent.Left == parent) grandParent.Left = child;
                else grandParent.Right = child;

                child.Parent = grandParent;
                parent.Parent = child;
            }
        }

        private void BalanseAndRepainting(TreeNode insertedNode)
        {
            //если левая нода красная и левая нода левой ноды красная - правосторонний поворот
            //если правая нода красная и правая нода правой ноды красная - левосторонний поворот
            //если левая нода красная и правосторонняя нода красная -делаем свап цвета.
            if (insertedNode == _root) return;

            TreeNode parent = insertedNode.Parent;
            if (parent.Color == "Black" && 
                (parent.Left.Exists && parent.Left.Color == "Red") && 
                (parent.Right.Exists && parent.Right.Color == "Red") &&
                (!(parent.Parent.Exists && parent.Parent.Color == "Red")))
            {
                SwapFamilyColor(parent);
                return;
            }

            if (insertedNode.Color == "Red")
            {
                if (parent.Color == "Red")
                {
                    if (parent.Left == insertedNode)
                    {
                        RightHandTurn(parent, parent.Parent);
                    }
                    else
                    {
                        LeftHandTurn(parent, parent.Parent);
                    }
                    return;
                }

                if (insertedNode.Left.Exists && insertedNode.Left.Color == "Red")
                {
                    RightHandTurn(insertedNode, parent);
                    return;
                }

                if (insertedNode.Right.Exists && insertedNode.Right.Color == "Red")
                {
                    LeftHandTurn(insertedNode, parent);
                    return;
                }
            }
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

        private void DeleteRed(TreeNode current, TreeNode parent)
        {
            // Удаление красной вершины с 0 детьми
            if (current.Left == null && current.Right == null)
            {
                DeleteRedNoChildren(current, parent);
            }


            // Удаление красной вершины с 1 ребенком
            else if (current.Left != null ^ current.Right != null)
            {
                DeleteRedOneChild(current, parent);
            }

            // Удаление красной с двумя детьми

        }


        private void DeleteRedNoChildren(TreeNode current, TreeNode parent)
        {
            if (parent.Left == current) parent.Left = null;
            else parent.Right = null;
            AmountOfElements--;
        }

        private void DeleteRedOneChild(TreeNode current, TreeNode parent)
        {
            //никаких проблем не возникнет, тк и ребенок и родитель точно чёрные
            if (current.Left != null)
            {
                if (parent.Left == current) parent.Left = current.Left;
                else parent.Right = current.Left;
            }
            if (current.Right != null)
            {
                if (parent.Left == current) parent.Left = current.Right;
                else parent.Right = current.Right;
            }
        }

        private void DeleteBlack(TreeNode current, TreeNode parent)
        {
            if (current.Color == "Black" && (current.Left != null ^ current.Right != null))
            {
                DeleteBlackOneChild(current, parent);
            }

        }

        private void DeleteBlackOneChild(TreeNode current, TreeNode parent)
        {
            if (current.Left != null && current.Left.Color == "Red")
            {
                if (parent.Left == current) parent.Left = current.Left;
                else parent.Right = current.Left;

                current.Left.Color = "Black";
            }

            else if (current.Right != null && current.Right.Color == "Red")
            {
                if (parent.Left == current) parent.Left = current.Right;
                else parent.Right = current.Right;

                current.Right.Color = "Black";
            }
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

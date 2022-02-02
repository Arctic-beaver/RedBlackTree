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

        private bool _noBalance;
        public Tree()
        {
            Insert(4);
            Insert(45);
            Insert(34);
            Insert(23);
            Insert(1234);
            Insert(9);
            Insert(10);
            Insert(345);
            Insert(0);
            Insert(5);
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

        public void SortOutput()
        {
            SortoutputRec(_root);
        }

        public void SortoutputRec(TreeNode shovel)
        {
            if (shovel != null)
            {
                SortoutputRec(shovel.Left);
                Console.Write($"{shovel.Key} ");
                SortoutputRec(shovel.Right);
            }
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

        public TreeNode Min(TreeNode shovel)
        {
            //ищем самый левый узел в дереве
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

        public TreeNode Max(TreeNode shovel)
        {
            //ищем самый правый узел в дереве
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
            if (newNode.Key == -1) throw new InvalidOperationException("-1!!");

            newNode.Color = "Red";

            if (_noBalance)
            {
                newNode.Right.Color = "Black";
                newNode.Left.Color = "Black";
                _noBalance = false;
            }
            BalanseAndRepainting(newNode);

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
                if (node.Left.Key < value)
                {
                    TreeNode tmp = node.Left;
                    node.Left = new TreeNode(value);
                    node.Left.Parent = node;
                    node.Left.Left = tmp;
                    tmp.Parent = node.Left;

                    if (tmp.Right != null && tmp.Right.Key >= node.Left.Key)
                    {
                        node.Left.Right = tmp.Right;
                        tmp.Right.Parent = node.Left;
                        tmp.Right = null;
                        _noBalance = true;

                    }

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
                if (node.Right.Key > value)
                {
                    TreeNode tmp = node.Right;
                    node.Right = new TreeNode(value);
                    node.Right.Parent = node;
                    node.Right.Right = tmp;
                    tmp.Parent = node.Right;

                    if (tmp.Left != null && tmp.Left.Key <= node.Right.Key)
                    {
                        node.Right.Left = tmp.Left;
                        tmp.Left.Parent = node.Right;
                        tmp.Left = null;
                        _noBalance = true;
                    }

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

        private void BalanseAndRepainting(TreeNode insertedNode)
        {
            //если левая нода красная и левая нода левой ноды красная - правосторонний поворот
            //если правая нода красная и правая нода правой ноды красная - левосторонний поворот
            //если левая нода красная и правосторонняя нода красная -делаем свап цвета.
            if (insertedNode == _root) return;

            TreeNode parent = insertedNode.Parent;
            if (parent.Color == "Black" &&
                (parent.Left != null && parent.Left.Color == "Red") &&
                (parent.Right != null && parent.Right.Color == "Red") &&
                (!(parent.Parent != null && parent.Parent.Color == "Red")))
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

                if (insertedNode.Left != null && insertedNode.Left.Color == "Red")
                {
                    RightHandTurn(insertedNode, parent);
                    return;
                }

                if (insertedNode.Right != null && insertedNode.Right.Color == "Red")
                {
                    LeftHandTurn(insertedNode, parent);
                    return;
                }
            }
        }


        private TreeNode GetBrother(TreeNode node)
        {
            if (node.Parent.Left == node) return node.Parent.Right;
            else return node.Parent.Left;
        }

        private TreeNode GetUncle(TreeNode node)
        {
            if (node.Parent.Parent == null) return null;


            if (node.Parent.Parent.Left == node.Parent)
            {
                return node.Parent.Parent.Right;
            }
            else
            {
                return node.Parent.Parent.Left;
            }
        }

        public bool Delete(int value)
        {
            TreeNode iMustBeDead = Search(value);

            if (iMustBeDead == null) return false;

            if (iMustBeDead.Left == null && iMustBeDead.Right == null)
            {
                if (iMustBeDead.Color == "Black")
                {
                    DeleteBlackNodeNoChildren(iMustBeDead);
                }
                else DeleteChildlessRedNode(iMustBeDead);
                return true;
            }
            if (iMustBeDead.Left != null ^ iMustBeDead.Right != null)
            {
                if (iMustBeDead.Color == "Black")
                {
                    DeleteBlackNodeOneChild(iMustBeDead);
                }
                else DeleteRedNodeWithOneChild(iMustBeDead);
                return true;
            }
            if (iMustBeDead.Left != null && iMustBeDead.Right != null)
            {
                if (iMustBeDead.Color == "Black")
                {
                    DeleteProlificBlackNode(iMustBeDead);
                }
                else DeleteProlificRedNode(iMustBeDead);
            }
            return false;
        }

        private void DeleteBlackNodeNoChildren(TreeNode deleting)
        {
            if (deleting.Parent.Left != null ^ deleting.Parent.Right != null)
            {
                //один ребёнок в семье
                if (deleting.Parent.Color == "Red")
                {
                    deleting.Parent.Color = "Black";
                }
                else 
                {
                    if (deleting.Parent.Parent.Left == deleting.Parent)
                    {
                        LeftHandTurn(deleting.Parent.Parent.Right, deleting.Parent.Parent);
                    }
                    else RightHandTurn(deleting.Parent.Parent.Left, deleting.Parent.Parent);
                }
            }

            //если их два и у удаляемого нет детей, ничто не поменяется
            if (deleting.Parent.Left == deleting) deleting.Parent.Left = null;
            else deleting.Parent.Right = null;
        }

        private void DeleteBlackNodeOneChild(TreeNode deleting)
        {
            TreeNode child = deleting.Right ?? deleting.Left;
            TreeNode parent = deleting.Parent;

            DeleteRedNodeWithOneChild(deleting);
            //Это мы убрали сам узел. теперь восстановим цвета.

            TreeNode brother = GetBrother(child);
            
            if (brother.Color == "Black")
            {
                DeleteBlackNodeOneChildBrotherIsBlack(child, brother, parent);
                return;
            }

            //Случай 5: Брат узла красный
            if (brother.Color == "Red")
            {
                if (parent.Right == brother)
                {
                    LeftHandTurn(brother, parent);
                }
                else
                {
                    RightHandTurn(brother, parent);
                }
                brother.Color = "Black";
                parent.Color = "Red";

                DeleteBlackNodeOneChildBrotherIsBlack(child, brother, parent);
                return;
            }
        }

        private void DeleteBlackNodeOneChildBrotherIsBlack(TreeNode child, TreeNode brother, TreeNode parent)
        {
            //Случай 1: отец красный, брат черный, дети брата,  если таковые есть, чёрные

            if (parent.Color == "Red" && (brother.Left == null || brother.Left.Color == "Black")
                && (brother.Right == null || brother.Right.Color == "Black"))
            {
                DeleteBlackNodeOneChildSit1(child, brother, parent);
                return;
            }

            //Случай 2: брат черный, его правый сын красный, цвет отца не важен
            if (brother.Right != null && brother.Right.Color == "Red")
            {
                DeleteBlackNodeOneChildSit2(child, brother, parent);
                return;
            }

            //Случай 3: брат черный, его правый сын чёрный, левый сын красный
            if ((brother.Left != null && brother.Left.Color == "Red")
               && (brother.Right != null && brother.Right.Color == "Black"))
            {

                RightHandTurn(brother.Left, brother);
                brother.Left.Color = "Black";
                brother.Color = "Red";

                DeleteBlackNodeOneChildSit2(child, brother, parent);
                return;
            }
            //Случай 4: Вся семья чёрная
            if (parent.Color == "Black" && (brother.Left == null || brother.Left.Color == "Black")
               && (brother.Right == null || brother.Right.Color == "Black"))
            {
                brother.Color = "Red";
            }
        }

        private void DeleteBlackNodeOneChildSit1(TreeNode child, TreeNode brother, TreeNode parent)
        {
            parent.Color = "Black";
            brother.Color = "Red";
        }

        private void DeleteBlackNodeOneChildSit2(TreeNode child, TreeNode brother, TreeNode parent)
        {
            string tmp = parent.Color;
            parent.Color = brother.Color;
            brother.Color = tmp;

            if (parent.Left == child)
            {
                LeftHandTurn(brother, parent);
            }
            else
            {
                RightHandTurn(brother, parent);
            }

            brother.Right.Color = "Black";
        }

        private void DeleteProlificBlackNode(TreeNode deleting)
        {
            TreeNode maxOfLeftSubtree = Max(deleting.Left);

            deleting.Key = maxOfLeftSubtree.Key;
            
            if (maxOfLeftSubtree.Color == "Black")
            {
                DeleteBlackNodeNoChildren(maxOfLeftSubtree);
            }
            else
            {
                DeleteChildlessRedNode(maxOfLeftSubtree);
            }
        }

        private void DeleteChildlessRedNode(TreeNode deleting)
        {
            if (deleting.Parent.Left == deleting) deleting.Parent.Left = null;
            else deleting.Parent.Right = null;
        }

        private void DeleteRedNodeWithOneChild(TreeNode deleting)
        {
            if (deleting.Parent.Left == deleting)
            {
                if (deleting.Left != null)
                {
                    deleting.Parent.Left = deleting.Left;
                }
                else
                {
                    deleting.Parent.Left = deleting.Right;
                }
            }
            else
            {
                if (deleting.Left != null)
                {
                    deleting.Parent.Right = deleting.Left;
                }
                else
                {
                    deleting.Parent.Right = deleting.Right;
                }
            }
        }

        private void DeleteProlificRedNode(TreeNode deleting)
        {
            TreeNode maxOfLeftSubtree = Max(deleting.Left);

            deleting.Key = maxOfLeftSubtree.Key;
            DeleteChildlessRedNode(maxOfLeftSubtree);
        }


        public void BFS()
        {
            Console.WriteLine("Breadth first (BFS): ");
            Queue<TreeNode> visited = BreadthFirstSearch();
            while (visited.Any())
            {
                Console.Write($"{visited.Dequeue().Key} ");
            }
            Console.WriteLine();
        }


        private Queue<TreeNode> BreadthFirstSearch()
        {
            Queue<TreeNode> notVisited = new Queue<TreeNode>();
            notVisited.Enqueue(_root);
            Queue<TreeNode> visited = new Queue<TreeNode>();

            TreeNode shovel;
            while (notVisited.Any())
            {
                shovel = notVisited.Dequeue();
                visited.Enqueue(shovel);
                if (shovel.Left != null)
                    notVisited.Enqueue(shovel.Left);
                if (shovel.Right != null)
                    notVisited.Enqueue(shovel.Right);
            }
            return visited;
        }

        public void CLR()
        {
            Console.WriteLine("CLR: ");
            Queue<TreeNode> visited = CenterLeftRight();
            while (visited.Any())
            {
                Console.Write($"{visited.Dequeue().Key} ");
            }
            Console.WriteLine();
        }

        public void LCR()
        {
            Console.WriteLine("LCR: ");
            Queue<TreeNode> visited = LeftCenterRight();
            while (visited.Any())
            {
                Console.Write($"{visited.Dequeue().Key} ");
            }
            Console.WriteLine();
        }

        public void LRC()
        {
            Console.WriteLine("LRC: ");
            Queue<TreeNode> visited = LeftRightCenter();
            while (visited.Any())
            {
                Console.Write($"{visited.Dequeue().Key} ");
            }
            Console.WriteLine();
        }

        private Queue<TreeNode> CenterLeftRight()
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(_root);

            Queue<TreeNode> visited = new Queue<TreeNode>();

            while (stack.Any())
            {
                TreeNode node = stack.Pop();
                visited.Enqueue(node);

                if (node.Right != null)
                {
                    stack.Push(node.Right);
                }

                if (node.Left != null)
                {
                    stack.Push(node.Left);
                }
            }

            return visited;
        }

        private Queue<TreeNode> LeftCenterRight()
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(_root);

            Queue<TreeNode> visited = new Queue<TreeNode>();

            while (stack.Any())
            {
                TreeNode node = stack.Pop();
                if (node == _root.Right) visited.Enqueue(_root);
                if (node != _root) visited.Enqueue(node);

                if (node.Right != null)
                {
                    stack.Push(node.Right);
                }

                if (node.Left != null)
                {
                    stack.Push(node.Left);
                }
            }

            return visited;
        }

        private Queue<TreeNode> LeftRightCenter()
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(_root);

            Queue<TreeNode> visited = new Queue<TreeNode>();

            while (stack.Any())
            {
                TreeNode node = stack.Pop();
                if (node != _root) visited.Enqueue(node);

                if (node.Right != null)
                {
                    stack.Push(node.Right);
                }

                if (node.Left != null)
                {
                    stack.Push(node.Left);
                }
            }

            visited.Enqueue(_root);

            return visited;
        }
    }
}
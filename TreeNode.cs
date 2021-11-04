using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    class TreeNode
    {
        public int Key { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode Parent { get; set; }
        public string Color { get; set; }

        public TreeNode(int key)
        {
            Key = key;
        }

        public int CompareTo(TreeNode other)
        {
            if (Key == other.Key) return 0;
            if (Key < other.Key) return -1;
            else return 1;
        }
    }
}

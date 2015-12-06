using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdBilancer
{
    /// <summary>
    /// Simple class representing of TreeStructure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class TreeNode<T>
    {
        public List<TreeNode<T>> Children { get; private set; }
        public T Item { get; set; }

        public TreeNode (T item)
        {
            Item = item;
        }
        public TreeNode<T> AddChild(T item)
        {
            TreeNode<T> nodeItem = new TreeNode<T>(item);
            Children.Add(nodeItem);
            return nodeItem;
        }
    }
}

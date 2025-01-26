using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    [Serializable]
    public class Tree<T>
    {
        public TreeNode<T> Root { get; set; }

        public void RestoreParents()
        {
            RestoreParents(this.Root);
        }

        public void RestoreParents(TreeNode<T> node)
        {
            if (node != null)
            {
                if (node.Children != null)
                {
                    foreach (TreeNode<T> childNode in node.Children)
                    {
                        childNode.Parent = node;
                        RestoreParents(childNode);
                    }
                }
            }
        }

        public bool Has(T data)
        {
            TreeNode<T> nodeFound = null;
            Find(Root, data, ref nodeFound);
            return nodeFound != null;
        }

        public TreeNode<T> Find(T data)
        {
            TreeNode<T> nodeFound = null;
            Find(Root, data, ref nodeFound);
            return nodeFound;
        }

        public void Find(TreeNode<T> nodeCurrent, T data, ref TreeNode<T> nodeFound)
        {
            if (nodeCurrent != null)
            {
                if (nodeCurrent.Data.Equals(data))
                {
                    nodeFound = nodeCurrent;
                }
                else
                {
                    if (nodeCurrent.Children != null)
                    {
                        foreach (TreeNode<T> aNode in nodeCurrent.Children)
                        {
                            Find(aNode, data, ref nodeFound);

                            if (nodeFound != null) //Already found? Leave early
                                return;
                        }
                    }
                }
            }
        }

        public void AddChild(TreeNode<T> nodeCurrent, T data)
        {
            if (nodeCurrent != null)
            {
                if (nodeCurrent.Children == null)
                    nodeCurrent.Children = new List<TreeNode<T>>();

                nodeCurrent.Children.Add(new TreeNode<T>() { Data = data, Parent = nodeCurrent });
            }
        }

        public void RemoveChild(TreeNode<T> nodeCurrent)
        {
            if (nodeCurrent != null)
            {
                if (nodeCurrent.Children != null)
                {
                    foreach (TreeNode<T> treeNode in nodeCurrent.Children)
                    {
                        RemoveChild(treeNode);
                    }
                }

                nodeCurrent.Children?.Clear();
                
                if (nodeCurrent.Parent != null)
                    nodeCurrent.Parent.Children.Remove(nodeCurrent);
                else //Must be the root
                    this.Root = null;
            }
        }
    }
}

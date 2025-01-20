using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    internal class Tree<T>
    {
        public TreeNode<T> Root { get; set; }

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
    }
}

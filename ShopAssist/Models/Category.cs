using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    [Serializable]
    public class Category
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is Category)
                return this.Name == (obj as Category).Name;
            return false;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}

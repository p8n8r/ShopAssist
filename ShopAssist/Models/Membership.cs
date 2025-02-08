using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    public enum MembershipLevel { High, Medium, Low, No }

    [Serializable]
    public class Membership
    {
        public int Id { get; set; }
        public MembershipLevel MembershipLevel { get; set; } = MembershipLevel.No;
    }
}

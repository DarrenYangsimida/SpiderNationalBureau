using System;
using System.Collections.Generic;

namespace SpiderNationalBureau.DataModel_MySql
{
    public partial class Community
    {
        public long RunId { get; set; }
        public long CommunityCode { get; set; }
        public string CommunityName { get; set; }
        public long StreetCode { get; set; }
        public bool DeletedFlag { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}

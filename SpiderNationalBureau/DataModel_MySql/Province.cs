using System;
using System.Collections.Generic;

namespace SpiderNationalBureau.DataModel_MySql
{
    public partial class Province
    {
        public long RunId { get; set; }
        public long ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public bool DeletedFlag { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080031.DomainModels
{
    /// <summary>
    /// 
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 
        /// </summary>
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public string Description { get; set; } = "";
    }
}

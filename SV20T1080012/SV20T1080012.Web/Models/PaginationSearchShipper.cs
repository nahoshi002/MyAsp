﻿using SV20T1080012.DomainModels;

namespace SV20T1080012.Web.Models
{
    public class PaginationSearchShipper : PaginationSearchBaseResult
    {
        public IList<Shipper> Data { get; set; }
    }
}

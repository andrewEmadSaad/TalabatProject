﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 10;
        private const int DefualtPageIndex = 1;

        private string search;

        public string Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }



        public string Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        private int pageIndex =1;

        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value ; }
        }


        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

    }
}

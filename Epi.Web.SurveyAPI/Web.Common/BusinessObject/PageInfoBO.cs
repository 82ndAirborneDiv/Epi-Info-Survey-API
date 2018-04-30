﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.SurveyAPI.Web.Common.BusinessObject
{
   public class PageInfoBO
    {
        private int _numberOfPages;
        private int _pageSize;

        public int NumberOfPages
        {
            get { return _numberOfPages; }
            set { _numberOfPages = value; }
        }
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace BlackLegionBot.TwitchApi.Models
{
    public class ListResultWithPaginationWithTotal<T> : ListResultWithPagination<T>
    {
        public int Total { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BlackLegionBot.TwitchApi.Models
{
    public class ListResultPure<T>
    {
        public IEnumerable<T> Data { get; set; }
    }
}

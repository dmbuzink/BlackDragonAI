using System;
using System.Collections.Generic;
using System.Text;

namespace BlackLegionBot.CommandStorage
{
    public class Existence
    {
        public Existence(bool exists)
        {
            this.Exists = exists;
        }

        public bool Exists { get; set; }
    }
}

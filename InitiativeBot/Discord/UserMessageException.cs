using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    internal class UserMessageException : Exception
    {
        public UserMessageException(string message) : base(message)
        {
        }
    }
}

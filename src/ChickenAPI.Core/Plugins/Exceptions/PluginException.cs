using System;
using System.Collections.Generic;
using System.Text;

namespace ChickenAPI.Core.Plugins.Exceptions
{
    public class PluginException : Exception
    {
        public PluginException(string message) : base(message)
        {
        }
    }
}
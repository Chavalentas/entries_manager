﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Frontend.Exceptions
{
    public class UnsuccessfulHttpOperationException : Exception
    {
        public UnsuccessfulHttpOperationException(string message) : base(message)
        {
        }
    }
}

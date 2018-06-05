
using System;
using System.Collections;
using System.Collections.Generic;

namespace RawQueryApp
{
    public class TodoItem
    {
        public Guid Id{ get; set; }
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
         
    }
}

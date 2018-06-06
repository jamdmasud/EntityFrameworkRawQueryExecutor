using System;
using System.Collections.Generic;

namespace RawQueryApp
{
    public class TodoDetail
    {

        public Guid Id { get; set; }
        public Guid TodoId { get; set; }
        public byte[] Description { get; set; }
        
    }
}

using System;
using UnityEngine;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class BookData : LocalData<BookData>
    {
        public Dictionary<int, int> virusCount = new Dictionary<int, int>();
    }
}

using System;
using UnityEngine;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class BookData : LocalData<BookData>
    {
        [SerializeField]
        private int[] virusCount = new int[100];

        public BookData()
        {
            for (int i = 0; i < virusCount.Length; i++)
            {
                virusCount[i] = -1;
            }
        }

        public void Add(int id, int count)
        {
            virusCount[id] += count;
        }

        public void Set(int id, int count)
        {
            virusCount[id] = count;
        }

        public int Get(int id)
        {
            return virusCount[id];
        }

        public bool Exist(int id)
        {
            return virusCount[id] >= 0;
        }
    }
}

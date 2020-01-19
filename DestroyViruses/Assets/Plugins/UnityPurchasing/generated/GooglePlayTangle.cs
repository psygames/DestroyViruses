#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("ebaI1/plVFIZDfneGwqB8M65omr1bTilNyPy2zQ/uc5xajTsrRKqSHZdpin3zeE16d/zNSloV9Ijcn+6jX9EmK3jsKJ0+YylHEFQHtGrPE3nDSvgjL9p8mi2sxlrDZ5vXF4KnCET8KZb7i5lgYk5IafQX/1wLCAexVuF4AahFILFfHvswY5Iw3qGbDh7IRj5uAo/dlituuv3IyTqJDJTpEHbgsYDjyCqLiwoAgBl3S+syl41DFVS5XbWbrfNxLbJ4YGt+Ph53mEsHA/0hQN8Tps1bQhkyzU0icSo1wg75Imv/c1oUnHaPdsWlwPP6dMksQOAo7GMh4irB8kHdoyAgICEgYIDgI6BsQOAi4MDgICBNtjundI0P9QEE+dIxzDfJIOCgIGA");
        private static int[] order = new int[] { 8,4,3,11,12,6,10,13,10,9,11,13,13,13,14 };
        private static int key = 129;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

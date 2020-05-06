#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("JXMdryw8Ky54MA0prywlHa8sKR1BSA1kQ04DHAsdCSsueCkmPjBsXSgtLq8sIi0drywnL68sLC3JvIQkuLNXIYlqpnb5Oxoe5ukiYOM5RPxES0ROTFlEQkMNbFhZRUJfRFlUHB2vKZYdry6OjS4vLC8vLC8dICsk9BtS7Kp49Iq0lB9v1vX4XLNTjH9fTE5ZRE5IDV5ZTFlIQEhDWV4DHQerZavaICwsKCgtHU8cJh0kKy547U4eWtoXKgF7xvciDCP3l140YpgNTENJDU5IX1lES0ROTFlEQkMNXeQ0X9hwI/hScrbfCC6XeKJgcCDcUmyFtdT850uxCUY8/Y6WyTYH7jKvLC0rJAerZavaTkkoLB2s3x0HK11BSA1/QkJZDW5sHTM6IB0bHRkfCc/G/Jpd8iJozArn3EBVwMqYOjoYHxwZHR4bdzogHhgdHx0UHxwZHX9IQURMQ05IDUJDDVlFRF4NTkhfT0FIDV5ZTENJTF9JDVlIX0BeDUwCHazuKyUGKywoKCovLx2smzesngNti9pqYFIlcx0yKy54MA4pNR07DUJLDVlFSA1ZRUhDDUxdXUFETkwyvPYzan3GKMBzVKkAxhuPemF4wVcdryxbHSMrLngwIiws0ikpLi8sDW5sHa8sDx0gKyQHq2Wr2iAsLCxDSQ1OQkNJRFlEQkNeDUJLDVheSGT1W7IeOUiMWrnkAC8uLC0sjq8sHTwrLngpJz4nbF1dQUgNZENOAxyaNpC+bwk/B+oiMJtgsXNO5WatOqJerE3rNnYkAr+f1Wll3U0VszjYWloDTF1dQUgDTkJAAkxdXUFITkwrHSIrLngwPiws0ikoHS4sLNIdMCkrPi94fhw+HTwrLngpJz4nbF1dAQ1OSF9ZREtETkxZSA1dQkFETlQeG3cdTxwmHSQrLngpKz4veH4cPlQNTF5eWEBIXg1MTk5IXVlMQ05IdIooJFE6bXs8M1n+mqYOFmqO+EKGjly/an547IICbJ7V1s5d4MuOYQsdCSsueCkmPjBsXV1BSA1uSF9ZG7RhAFWawKG28d5att9b/1odYuyYF4DZIiMtvyacDDsDWfgRIPZPOzsdOSsueCkuPiBsXV1BSA1/QkJZpjSk89RmQdgqhg8dL8U1E9V9JP4isBDeBmQFN+XT45iUI/RzMfvmECArJAerZavaICwsKCgtLq8sLC1xnB11wXcpH6FFnqIw80he0kpzSJErLngwIyk7KTkG/URquVsk09lGoCUGKywoKCovLDszRVlZXV4XAgJaWURLRE5MWUgNT1QNTENUDV1MX1mtOQb9RGq5WyTT2UagA22L2mpgUhALSg2nHkfaIK/i88aOAtR+R3ZJk9letsP/SSLmVGIZ9Y8T1FXSRuVdQUgNbkhfWURLRE5MWURCQw1sWCrBUBSupn4N/hXpnJK3YidG0gbRWUVCX0RZVBw7HTkrLngpLj4gbF1JGA44Zjh0MJ652tuxs+J9l+x1fWhTMmFGfbtspOlZTyY9rmyqHqesSqIlmQ3a5oEBDUJdmxIsHaGabuKF8VMPGOcI+PQi+0b5jwkOPNqMgTKorqg2tBBqGt+Etm2jAfmcvT/1fYen+PfJ0f0kKhqdWFgM");
        private static int[] order = new int[] { 33,3,14,18,11,33,58,46,18,57,45,19,17,34,58,53,46,38,42,37,31,41,53,28,34,43,27,57,59,55,36,55,58,44,42,40,52,42,45,48,59,45,46,53,47,57,52,59,48,58,56,56,52,56,59,55,56,57,59,59,60 };
        private static int key = 45;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

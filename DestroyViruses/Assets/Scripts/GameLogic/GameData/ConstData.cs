using System;
namespace DestroyViruses
{
    public class CT
    {
        public static TableConst table
        {
            get
            {
                return TableConst.Get(GameLocalData.Instance.lastConstGroup);
            }
        }
    }
}
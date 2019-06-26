//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Task
{
    /// <summary>
    /// 任务状态。
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 空闲。
        /// </summary>
        Free,

        /// <summary>
        /// 等待中。
        /// </summary>
        Waiting,

        /// <summary>
        /// 运行中。
        /// </summary>
        Running,

        /// <summary>
        /// 已完成。
        /// </summary>
        Completed,

        /// <summary>
        /// 已失败。
        /// </summary>
        Failed,

        /// <summary>
        /// 已取消。
        /// </summary>
        Canceled,
    }
}

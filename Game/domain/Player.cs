using System.Collections.Generic;

namespace Game.domain
{
    /// <summary>
    /// 选手
    /// </summary>
    public class Player
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int No;

        /// <summary>
        /// 自身得分。
        /// </summary>
        public int Score;

        /// <summary>
        /// 选手类型。
        /// </summary>
        private PlayerType _type;

        /// <summary>
        /// 合作列表。key为对方id，value为上一次合作结果。
        /// </summary>
        private Dictionary<int, bool> Cooperationers;

        /// <summary>
        /// 根据对方选手的id，决定是否合作。
        /// </summary>
        /// <param name="no">对方选手Id</param>
        /// <returns></returns>
        public bool Cooperation(int no)
        {
            return true;
        }
    }

    /// <summary>
    /// 选手类型。
    /// </summary>
    public enum PlayerType
    {
        /// <summary>
        /// 老实人，永不背板
        /// </summary>
        Honest,

        /// <summary>
        /// 偶尔背叛 可以自行设置一个比例。
        /// </summary>
        OccasionallyBetray,

        /// <summary>
        /// 一直背叛
        /// </summary>
        Betray,

        /// <summary>
        /// 以眼还眼。上一次合作，这次合作，上一次背板，这次也背叛。
        /// </summary>
        Mirror,

        /// <summary>
        /// 记仇型，被背叛一次，不再合作。
        /// </summary>
        Grudges,

        /// <summary>
        /// 晚节不保。一开始全合作，一定比例后全背叛。
        /// </summary>
        Faltered
    }
}

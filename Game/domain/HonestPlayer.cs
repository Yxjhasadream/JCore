using System;
using Game.domain;

namespace Game
{
    public class HonestPlayer :IPlayer
    {
        /// <inheritdoc />
        public int No { get; set; }

        /// <inheritdoc />
        public int Score { get; set; }

        /// <inheritdoc />
        public PlayerType Type => PlayerType.Honest;

        /// <inheritdoc />
        public bool Cooperation(int no)
        {
            throw new NotImplementedException();
        }
    }
}

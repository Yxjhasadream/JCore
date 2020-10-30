using System;
using System.Collections.Generic;
using System.Text;

namespace Game.domain
{
    class GrudgesPlayer:IPlayer
    {
        /// <inheritdoc />
        public int No { get; set; }

        /// <inheritdoc />
        public int Score { get; set; }

        /// <inheritdoc />
        public PlayerType Type => PlayerType.Grudges;

        /// <inheritdoc />
        public bool Cooperation(int no)
        {
            throw new NotImplementedException();
        }
    }
}

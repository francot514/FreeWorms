using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace FreeWorms
{

    public class PlayerIndexEventArgs : EventArgs
    {

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.m_playerIndex = playerIndex;
        }



        public PlayerIndex PlayerIndex
        {
            get { return m_playerIndex; }
        }

        private PlayerIndex m_playerIndex;
    }
}
using System;

namespace _Game.Scripts.Systems.Player
{
    public class InteractionEventArgs : EventArgs
    {
        public readonly bool State;
        public InteractionEventArgs(bool state)
        {
            State = state;
        }
    }
}
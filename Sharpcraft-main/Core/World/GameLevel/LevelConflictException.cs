using System;

namespace SharpCraft.Core.World.GameLevel
{
    public class LevelConflictException : Exception
    {
        public LevelConflictException(string string1) : base(string1)
        {
        }
    }
}
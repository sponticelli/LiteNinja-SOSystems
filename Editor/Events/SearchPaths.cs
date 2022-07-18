using System;
using System.Collections.Generic;

namespace LiteNinja.SOSystems.Editor
{
    [Serializable]
    public struct SearchPaths
    {
        public int arraySize;
        public List<string> pathsToSearch;
    }
}
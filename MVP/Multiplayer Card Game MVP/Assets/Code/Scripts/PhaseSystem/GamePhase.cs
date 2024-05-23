using System.Collections;
using UnityEngine;

namespace PhaseSystem
{
    public abstract class GamePhase : ScriptableObject
    {
        public abstract string Name { get; }

        public abstract IEnumerator Execute();
    }
}
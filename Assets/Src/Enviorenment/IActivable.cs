using UnityEngine;

namespace Environment
{
    public interface IActivable
    {
        public void Activate();
        public void DeActivate();
        public void SwapState();
    }
}


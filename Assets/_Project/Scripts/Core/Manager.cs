using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [DisallowMultipleComponent]
    public abstract class Manager 
    {
      
            public virtual void Setup()
            { }

            public virtual void Dispose()
            { }

            public virtual void Tick(float deltaTime)
            { }
    }
}

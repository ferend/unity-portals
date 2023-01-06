using UnityEngine;

namespace _Project.Scripts.Behaviours
{
    public interface ITeleport
    { 
        public void DuringTeleport(GameObject go);
        public void OnTeleportEnd(GameObject go);
        
    }
}

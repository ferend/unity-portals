using UnityEngine;

namespace _Project.Scripts.Models
{
    public class Level : MonoBehaviour
    {
        [field : SerializeField] public Mechanic[] LevelState { get; private set; }
        [field : SerializeField] public Collider LevelEndTrigger { get; private set; }
        [field : SerializeField] public GameObject[] LevelObjects { get; private set; }

    }

}

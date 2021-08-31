using UnityEngine;

namespace ShinyBoxInteractive
{
    [CreateAssetMenu(fileName = "CollectableDatabase", menuName = "ScriptableObjects/CollectableDatabase")]
    public class CollectableDatabase : ScriptableObject
    {
        private static CollectableDatabase instance;
        public static CollectableDatabase Instance
        {
            get
            {
                if(ReferenceEquals(instance, null))
                {
                    instance = Resources.Load<CollectableDatabase>("CollectableDatabase");
                }
                return instance;
            }
        }
        public GameObject[] Items;
        public SerializableDictionary<Color32, Item> Modifiers = new SerializableDictionary<Color32, Item>();
    }
}

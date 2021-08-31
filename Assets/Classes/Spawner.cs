using UnityEngine;

namespace ShinyBoxInteractive
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner instance;
        public static Spawner Instance
        {
            get
            {
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType<Spawner>();
                }
                return instance;
            }
        }

        public Transform RandomGoal;
        public int MaxScreenObjects;
        public float MinX, MaxX, MaxY, StaticZ;
        private Pool pool;
        private Vector3 randomPosition;
        public int Count;
        private bool gameOver;
        public bool IsGameOver
        {
            get
            {
                return gameOver;
            }
        }
        public void Start()
        {
            gameOver = false;
            Count = 0;
            pool = new Pool(200);
            Create();
        }
        public void Create()
        {
            if (gameOver) return;
            if (Count > MaxScreenObjects) return;
            Count += 1;
            var collectable = pool.GetItem();
            collectable.transform.position = GetSpawnPosition();
        }
        public void Recycle(GameObject collectable)
        {
            Count -= 1;
            pool.RecycleItem(collectable);
            Create();
        }
        public void GameOver()
        {
            gameOver = true;
        }
        public void OnTriggerEnter(Collider other)
        {            
            Recycle(other.gameObject);
        }
        private Vector3 GetSpawnPosition()
        {
            randomPosition.x = Random.Range(MinX, MaxX);
            randomPosition.y = MaxY;
            randomPosition.z = StaticZ;
            return randomPosition;
        }
    }
}

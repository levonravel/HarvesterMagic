using UnityEngine;

namespace ShinyBoxInteractive
{
    public class Controller : MonoBehaviour
    {
        public float Speed;
        public Vector3 direction;
        private Material mat;
        private int priorValue;

        public void Start()
        {
            mat = GetComponent<MeshRenderer>().material;
        }
        void Update()
        {
            direction.x = Input.GetAxis("Horizontal");
            transform.position += direction * Time.deltaTime * Speed;
        }
        public void OnTriggerEnter(Collider other)
        {
            var objectColor = other.GetComponent<MeshRenderer>().material.color;

            if (CollectableDatabase.Instance.Modifiers[objectColor] != null)
            {
                if (CollectableDatabase.Instance.Modifiers[objectColor].ShouldMultiply)
                {
                    ScoreKeeper.Instance.AdjustScore = priorValue * CollectableDatabase.Instance.Modifiers[objectColor].Points;
                }
                ScoreKeeper.Instance.AdjustScore = CollectableDatabase.Instance.Modifiers[objectColor].Points;
            }
            Spawner.Instance.Recycle(other.gameObject);
        }
    }
}

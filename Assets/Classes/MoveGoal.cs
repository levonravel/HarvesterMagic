using UnityEngine;

namespace ShinyBoxInteractive
{
    public class MoveGoal : MonoBehaviour
    {
        public float MinY, MaxY;
        private Vector3 wantedPosition;

        public void Start()
        {
            wantedPosition = transform.position;
        }
        public void OnTriggerEnter(Collider other)
        {
            Spawner.Instance.Create();
            wantedPosition.y = Random.Range(MinY, MaxY);
            transform.position = wantedPosition;
        }
    }
}

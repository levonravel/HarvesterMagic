using UnityEngine;

namespace ShinyBoxInteractive
{
    public class Controller : MonoBehaviour
    {
        public float Speed;
        public Vector3 Direction;
        private int priorValue;
        private Vector3 priorPosition;

        void LateUpdate()
        {
            Direction.x = Input.GetAxis("Horizontal");
            priorPosition = transform.position;
            transform.position += Direction * Time.deltaTime * Speed;
            if(transform.position.x > Spawner.Instance.MaxX)
            {
                transform.position = priorPosition;
            }
            if(transform.position.x < Spawner.instance.MinX)
            {
                transform.position = priorPosition;
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            var objectColor = other.GetComponent<MeshRenderer>().material.color;
            
            if (CollectableDatabase.Instance.Modifiers[objectColor].ShouldMultiply)
            {
                if (priorValue == 2)
                {
                    priorValue = 0;
                }
                ScoreKeeper.Instance.AdjustScore = priorValue * CollectableDatabase.Instance.Modifiers[objectColor].Points;
                priorValue = 0;
            }
            else
            {
                priorValue = CollectableDatabase.Instance.Modifiers[objectColor].Points;
                ScoreKeeper.Instance.AdjustScore = priorValue;
            }
            Spawner.Instance.Recycle(other.gameObject);
        }
    }
}

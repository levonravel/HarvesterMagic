using UnityEngine;

namespace ShinyBoxInteractive
{
    public class Controller : MonoBehaviour
    {
        public Color32 CurrentColor;
        public float Speed;
        public Vector3 direction;
        private Color priorColor;
        private Material mat;

        public void Start()
        {
            mat = GetComponent<MeshRenderer>().material;
            priorColor = new Color32(0, 0, 0, 255);
            mat.color = priorColor;
        }
        void Update()
        {
            direction.x = Input.GetAxis("Horizontal");
            transform.position += direction * Time.deltaTime * Speed;
        }
        public void OnTriggerEnter(Collider other)
        {
            priorColor = CurrentColor;
            CurrentColor = other.GetComponent<MeshRenderer>().material.color + CurrentColor;
            //ugly serizable doesnt have a Try method.. Implement one day?
            if(CurrentColor == priorColor)
            {
                CurrentColor = new Color32(0, 0, 0, 255);
            }
            try
            {
                ScoreKeeper.Instance.AdjustScore = CollectableDatabase.Instance.Modifiers[CurrentColor].Points;                
            }
            catch
            {
                CurrentColor = new Color32(0, 0, 0, 255);
            }
            mat.color = CurrentColor;
            Spawner.Instance.Recycle(other.gameObject);
        }
    }
}

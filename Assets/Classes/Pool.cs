using System.Collections.Generic;
using UnityEngine;

namespace ShinyBoxInteractive
{
    public class Pool
    {
        private Queue<GameObject> collectables = new Queue<GameObject>();
        public Pool(int count)
        {
            for(int i = 0; i < count; i++)
            {
                var collectable = CreateRandomCollectable();
                collectable.SetActive(false);
                collectables.Enqueue(collectable);
            }
        }
        public GameObject GetItem()
        {
            GameObject collectable;

            if (collectables.Count > 0)
            {
                collectable = collectables.Dequeue();
                collectable.SetActive(true);
            }
            else
            {
                collectable = CreateRandomCollectable();
            }

            return collectable;
        }
        public void RecycleItem(GameObject collectable)
        {
            collectable.SetActive(false);
            collectable.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collectables.Enqueue(collectable);
        }
        private GameObject CreateRandomCollectable()
        {
            var randomIndex = Random.Range(0, CollectableDatabase.Instance.Items.Length);
            var collectable = GameObject.Instantiate(CollectableDatabase.Instance.Items[randomIndex]);
            randomIndex = Random.Range(0, CollectableDatabase.Instance.Modifiers.Count);
            collectable.GetComponent<MeshRenderer>().material.color = CollectableDatabase.Instance.Modifiers.GetKeyAtIndex(randomIndex);
            return collectable;
        }
    }
}

using System;
using UnityEngine;

namespace Features.Character
{
    public class CharacterItemManager : MonoBehaviour
    {
        private Transform Root;

        private GameObject RootGameObject;
        
        private void Awake()
        {
            Root = transform.root;

            RootGameObject = Root.gameObject;
        }
    }
}
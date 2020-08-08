using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class ObjectPoolManager : Singelton<ObjectPoolManager>
    {
        #region VARIABLES

        private Dictionary<Type, GameModel> prefabs = new Dictionary<Type, GameModel>();
        private Dictionary<Type, Stack<GameModel>> pooledObjects = new Dictionary<Type, Stack<GameModel>>();

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            InitializeGameModelPrefabs();
        }

        private void InitializeGameModelPrefabs()
        {
            var gameModelPrefabs = Resources.LoadAll<GameModel>("Prefabs/");

            foreach(var gameModel in gameModelPrefabs)
            {
                prefabs.Add(gameModel.GetType(), gameModel);
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private GameModel GetPrefab<TYPE>() where TYPE : GameModel
        {
            if(prefabs.ContainsKey(typeof(TYPE)) == false)
            {
                return null;
            }

            return prefabs[typeof(TYPE)];
        }

        public TYPE Spawn<TYPE>(Vector2 position, Quaternion rotation, Transform parent) where TYPE : GameModel
        {
            var type = typeof(TYPE);

            if(pooledObjects.ContainsKey(type) == false || pooledObjects[type].Count <= 0)
            {
                return Instantiate(GetPrefab<TYPE>(), position, rotation, parent) as TYPE;
            }

            var prefabInstnace = pooledObjects[type].Pop();
            prefabInstnace.transform.SetPositionAndRotation(position, rotation);
            prefabInstnace.transform.SetParent(parent);
            prefabInstnace.gameObject.SetActive(true);
            return prefabInstnace as TYPE;
        }

        public void Despawn(GameModel gameModel)
        {
            gameModel.gameObject.SetActive(false);

            var type = gameModel.GetType();

            if(pooledObjects.ContainsKey(type) == false)
            {
                pooledObjects.Add(type, new Stack<GameModel>());
            }

            pooledObjects[type].Push(gameModel);          
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

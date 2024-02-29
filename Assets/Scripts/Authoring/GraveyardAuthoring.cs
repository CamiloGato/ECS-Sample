using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Authoring
{
    public class GraveyardAuthoring : MonoBehaviour
    {
        public float2 fieldDimensions;
        public int numberTombstonesToSpawn;
        public uint randomSeed;
        public GameObject tombstonePrefab;
        
        private class GraveyardAuthoringBaker : Baker<GraveyardAuthoring>
        {
            public override void Bake(GraveyardAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new GraveyardProperties()
                {
                    FieldDimensions = authoring.fieldDimensions,
                    NumberTombstonesToSpawn = authoring.numberTombstonesToSpawn,
                    TombstonePrefab = GetEntity(authoring.tombstonePrefab, TransformUsageFlags.Dynamic)
                });
                AddComponent(entity, new GraveyardRandom()
                {
                    Value = Random.CreateFromIndex(authoring.randomSeed)
                });
            }
        }
    }
}
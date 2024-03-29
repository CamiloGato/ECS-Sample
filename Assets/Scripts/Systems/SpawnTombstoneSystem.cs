﻿using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardProperties>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
            var graveyard = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var spawnPoints = new NativeList<float3>(Allocator.Temp);
            var tombstoneOffset = new float3(0f, -2f, 1f);
            
            for (int i = 0; i < graveyard.NumberTombstonesToSpawn; i++)
            {
                var newTombstone = ecb.Instantiate(graveyard.TombstonePrefab);
                var newTombstoneTransform = graveyard.GetRandomTombstonePosition;
                ecb.SetComponent(newTombstone, new LocalTransform()
                {
                    Position = newTombstoneTransform.Position,
                    Rotation = newTombstoneTransform.Rotation,
                    Scale = newTombstoneTransform.Scale
                });
                var newZombieSpawnPoint = newTombstoneTransform.Position + tombstoneOffset;
                spawnPoints.Add(newZombieSpawnPoint);
            }

            // graveyard.ZombieSpawnPoints = spawnPoints.ToArray(Allocator.Persistent);
            ecb.Playback(state.EntityManager);
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}
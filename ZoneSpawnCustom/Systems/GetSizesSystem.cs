using Game;
using Game.Prefabs;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;
using ZoneSpawnCustom.Classes;

namespace ZoneSpawnCustom.Systems
{
    public class GetSizesSystem : GameSystemBase
    {
        private struct TypeHandle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void __AssignHandles(ref SystemState state)
            {
                __Game_Prefabs_BuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingData>(true);
                __Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<BuildingSpawnGroupData>();
            }

            [ReadOnly]
            public ComponentTypeHandle<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentTypeHandle;

            public SharedComponentTypeHandle<BuildingSpawnGroupData> __Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle;
        }
        private EntityQuery m_BuildingQuery;
        private TypeHandle __TypeHandle;

        [Preserve]
        protected override void OnCreate()
        {
            base.OnCreate();

            m_BuildingQuery = GetEntityQuery(new ComponentType[]
            {
                ComponentType.ReadOnly<BuildingData>(),
                ComponentType.ReadOnly<SpawnableBuildingData>(),
                ComponentType.ReadOnly<BuildingSpawnGroupData>(),
                ComponentType.ReadOnly<PrefabData>()
            });
            RequireForUpdate(m_BuildingQuery);
        }

        [Preserve]
        protected override void OnUpdate()
        {
            __TypeHandle.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle.Update(ref CheckedStateRef);

            if (!Plugin.initialized)
            {
                NativeArray<ArchetypeChunk> buildingChunks = m_BuildingQuery.ToArchetypeChunkArray(World.UpdateAllocator.ToAllocator);

                List<BuildingInfo> buildingInfos = new List<BuildingInfo>();
                foreach (var buildingChunk in buildingChunks)
                {
                    NativeArray<BuildingData> buildingData = buildingChunk.GetNativeArray(ref __TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle);
                    var zoneType = buildingChunk.GetSharedComponent(__TypeHandle.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle).m_ZoneType;
                    foreach (var building in buildingData)
                    {
                        if (buildingInfos.Count(s => s.Equals(zoneType.m_Index, building.m_LotSize)) == 0)
                        {
                            buildingInfos.Add(new BuildingInfo(zoneType.m_Index, new int2(building.m_LotSize)));
                        }
                    }

                }
                Plugin.Instance.InitializeSizes(buildingInfos);
            }
            var updateSystem = World.GetExistingSystemManaged<UpdateSystem>();
            this.Enabled = false;
        }

        protected override void OnCreateForCompiler()
        {
            base.OnCreateForCompiler();
            __TypeHandle.__AssignHandles(ref CheckedStateRef);
        }
    }
}

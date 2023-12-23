using Game;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

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

                Dictionary<string, int2> rSizes = new Dictionary<string, int2>();
                Dictionary<string, int2> cSizes = new Dictionary<string, int2>();
                Dictionary<string, int2> iSizes = new Dictionary<string, int2>();
                Dictionary<string, int2> oSizes = new Dictionary<string, int2>();
                foreach (var buildingChunk in buildingChunks)
                {
                    NativeArray<BuildingData> buildingData = buildingChunk.GetNativeArray(ref __TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle);
                    var zoneType = buildingChunk.GetSharedComponent(__TypeHandle.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle).m_ZoneType;
                    foreach (var building in buildingData)
                    {
                        int2 size = building.m_LotSize;
                        string key = $"{size.x}*{size.y}";
                        if (Plugin.ResidentialIndexes.Contains(zoneType.m_Index))
                        {
                            if (!rSizes.ContainsKey(key))
                            {
                                rSizes.Add(key, size);
                            }
                        }
                        if (Plugin.CommercialIndexes.Contains(zoneType.m_Index))
                        {
                            if (!cSizes.ContainsKey(key))
                            {
                                cSizes.Add(key, size);
                            }
                        }
                        if (Plugin.IndustrialIndexes.Contains(zoneType.m_Index))
                        {
                            if (!iSizes.ContainsKey(key))
                            {
                                iSizes.Add(key, size);
                            }
                        }
                        if (Plugin.OfficeIndexes.Contains(zoneType.m_Index))
                        {
                            if (!oSizes.ContainsKey(key))
                            {
                                oSizes.Add(key, size);
                            }
                        }
                    }

                }
                Plugin.InitializeSizes(
                    new Dictionary<string, int2>(rSizes.OrderBy(s => s.Key)),
                    new Dictionary<string, int2>(cSizes.OrderBy(s => s.Key)),
                    new Dictionary<string, int2>(iSizes.OrderBy(s => s.Key)),
                    new Dictionary<string, int2>(oSizes.OrderBy(s => s.Key))
                );
            }
        }

        protected override void OnCreateForCompiler()
        {
            base.OnCreateForCompiler();
            __TypeHandle.__AssignHandles(ref CheckedStateRef);
        }
    }
}

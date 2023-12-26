using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Scripting;

namespace ZoneSpawnCustom.Systems
{
    [CompilerGenerated]
    public class MyZoneSpawnSystem : GameSystemBase
    {
        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 16;
        }

        public bool debugFastSpawn { get; set; }

        [Preserve]
        protected override void OnCreate()
        {
            base.OnCreate();
            m_ZoneSystem = World.GetOrCreateSystemManaged<ZoneSystem>();
            m_ResidentialDemandSystem = World.GetOrCreateSystemManaged<ResidentialDemandSystem>();
            m_CommercialDemandSystem = World.GetOrCreateSystemManaged<CommercialDemandSystem>();
            m_IndustrialDemandSystem = World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
            m_PollutionSystem = World.GetOrCreateSystemManaged<GroundPollutionSystem>();
            m_TerrainSystem = World.GetOrCreateSystemManaged<TerrainSystem>();
            m_SearchSystem = World.GetOrCreateSystemManaged<Game.Zones.SearchSystem>();
            m_ResourceSystem = World.GetOrCreateSystemManaged<ResourceSystem>();
            m_CityConfigurationSystem = World.GetOrCreateSystemManaged<CityConfigurationSystem>();
            m_EndFrameBarrier = World.GetOrCreateSystemManaged<EndFrameBarrier>();
            m_LotQuery = GetEntityQuery(new EntityQueryDesc[]
            {
                new EntityQueryDesc
                {
                    All = new ComponentType[]
                    {
                        ComponentType.ReadOnly<Block>(),
                        ComponentType.ReadOnly<Owner>(),
                        ComponentType.ReadOnly<CurvePosition>(),
                        ComponentType.ReadOnly<VacantLot>()
                    },
                    Any = new ComponentType[0],
                    None = new ComponentType[]
                    {
                        ComponentType.ReadWrite<Temp>(),
                        ComponentType.ReadWrite<Deleted>()
                    }
                }
            });
            m_BuildingQuery = GetEntityQuery(new ComponentType[]
            {
                ComponentType.ReadOnly<BuildingData>(),
                ComponentType.ReadOnly<SpawnableBuildingData>(),
                ComponentType.ReadOnly<BuildingSpawnGroupData>(),
                ComponentType.ReadOnly<PrefabData>()
            });
            m_DefinitionArchetype = EntityManager.CreateArchetype(new ComponentType[]
            {
                ComponentType.ReadWrite<CreationDefinition>(),
                ComponentType.ReadWrite<ObjectDefinition>(),
                ComponentType.ReadWrite<Updated>(),
                ComponentType.ReadWrite<Deleted>()
            });
            m_ProcessQuery = GetEntityQuery(new ComponentType[]
            {
                ComponentType.ReadOnly<IndustrialProcessData>()
            });
            m_BuildingConfigurationQuery = GetEntityQuery(new ComponentType[]
            {
                ComponentType.ReadOnly<BuildingConfigurationData>()
            });
            RequireForUpdate(m_LotQuery);
            RequireForUpdate(m_BuildingQuery);
        }

        private bool CheckDemand(ref Unity.Mathematics.Random random, int demand)
        {
            return random.NextInt(5000) < demand * demand;
        }

        private bool CheckStorageDemand(ref Unity.Mathematics.Random random, int demand)
        {
            return demand > 0;
        }

        [Preserve]
        protected override void OnUpdate()
        {
            Unity.Mathematics.Random random = RandomSeed.Next().GetRandom(0);
            bool flag = debugFastSpawn || CheckDemand(ref random, m_ResidentialDemandSystem.buildingDemand.x + m_ResidentialDemandSystem.buildingDemand.y + m_ResidentialDemandSystem.buildingDemand.z);
            bool flag2 = debugFastSpawn || CheckDemand(ref random, m_CommercialDemandSystem.buildingDemand);
            bool flag3 = debugFastSpawn || CheckDemand(ref random, m_IndustrialDemandSystem.industrialBuildingDemand + m_IndustrialDemandSystem.officeBuildingDemand);
            bool flag4 = debugFastSpawn || CheckStorageDemand(ref random, m_IndustrialDemandSystem.storageBuildingDemand);
            NativeQueue<SpawnLocation> residential = new NativeQueue<SpawnLocation>(Allocator.TempJob);
            NativeQueue<SpawnLocation> commercial = new NativeQueue<SpawnLocation>(Allocator.TempJob);
            NativeQueue<SpawnLocation> industrial = new NativeQueue<SpawnLocation>(Allocator.TempJob);
            __TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_ProcessEstimate_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_WarehouseData_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_VacantLot_RO_BufferTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_CurvePosition_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle.Update(ref CheckedStateRef);
            __TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref CheckedStateRef);

            EvaluateSpawnAreas evaluateSpawnAreas = default;
            JobHandle job;
            evaluateSpawnAreas.m_BuildingChunks = m_BuildingQuery.ToArchetypeChunkListAsync(World.UpdateAllocator.ToAllocator, out job);
            evaluateSpawnAreas.m_ZonePrefabs = m_ZoneSystem.GetPrefabs();
            evaluateSpawnAreas.m_Preferences = __query_1944910156_0.GetSingleton<ZonePreferenceData>();
            evaluateSpawnAreas.m_SpawnResidential = flag ? 1 : 0;
            evaluateSpawnAreas.m_SpawnCommercial = flag2 ? 1 : 0;
            evaluateSpawnAreas.m_SpawnIndustrial = flag3 ? 1 : 0;
            evaluateSpawnAreas.m_SpawnStorage = flag4 ? 1 : 0;
            evaluateSpawnAreas.m_MinDemand = debugFastSpawn ? 0 : 1;
            evaluateSpawnAreas.m_ResidentialDemands = m_ResidentialDemandSystem.buildingDemand;
            JobHandle job2;
            evaluateSpawnAreas.m_CommercialDemands = m_CommercialDemandSystem.GetBuildingDemands(out job2);
            JobHandle job3;
            evaluateSpawnAreas.m_IndustrialDemands = m_IndustrialDemandSystem.GetBuildingDemands(out job3);
            JobHandle job4;
            evaluateSpawnAreas.m_StorageDemands = m_IndustrialDemandSystem.GetStorageBuildingDemands(out job4);
            evaluateSpawnAreas.m_RandomSeed = RandomSeed.Next();
            evaluateSpawnAreas.m_EntityType = __TypeHandle.__Unity_Entities_Entity_TypeHandle;
            evaluateSpawnAreas.m_BlockType = __TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_OwnerType = __TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_CurvePositionType = __TypeHandle.__Game_Zones_CurvePosition_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_VacantLotType = __TypeHandle.__Game_Zones_VacantLot_RO_BufferTypeHandle;
            evaluateSpawnAreas.m_BuildingDataType = __TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_SpawnableBuildingType = __TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_BuildingPropertyType = __TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_ObjectGeometryType = __TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_BuildingSpawnGroupType = __TypeHandle.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle;
            evaluateSpawnAreas.m_WarehouseType = __TypeHandle.__Game_Prefabs_WarehouseData_RO_ComponentTypeHandle;
            evaluateSpawnAreas.m_ZoneData = __TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup;
            evaluateSpawnAreas.m_Availabilities = __TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup;
            evaluateSpawnAreas.m_LandValues = __TypeHandle.__Game_Net_LandValue_RO_ComponentLookup;
            evaluateSpawnAreas.m_BlockData = __TypeHandle.__Game_Zones_Block_RO_ComponentLookup;
            JobHandle job5;
            evaluateSpawnAreas.m_Processes = m_ProcessQuery.ToComponentDataListAsync<IndustrialProcessData>(World.UpdateAllocator.ToAllocator, out job5);
            evaluateSpawnAreas.m_ProcessEstimates = __TypeHandle.__Game_Zones_ProcessEstimate_RO_BufferLookup;
            evaluateSpawnAreas.m_ResourceDatas = __TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
            evaluateSpawnAreas.m_ResourcePrefabs = m_ResourceSystem.GetPrefabs();
            JobHandle job6;
            evaluateSpawnAreas.m_PollutionMap = m_PollutionSystem.GetMap(true, out job6);
            evaluateSpawnAreas.m_Residential = residential.AsParallelWriter();
            evaluateSpawnAreas.m_Commercial = commercial.AsParallelWriter();
            evaluateSpawnAreas.m_Industrial = industrial.AsParallelWriter();
            EvaluateSpawnAreas jobData = evaluateSpawnAreas;
            __TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup.Update(ref CheckedStateRef);
            __TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref CheckedStateRef);
            SpawnBuildingJob spawnBuildingJob = default;
            spawnBuildingJob.m_BlockData = __TypeHandle.__Game_Zones_Block_RO_ComponentLookup;
            spawnBuildingJob.m_ValidAreaData = __TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup;
            spawnBuildingJob.m_TransformData = __TypeHandle.__Game_Objects_Transform_RO_ComponentLookup;
            spawnBuildingJob.m_PrefabRefData = __TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
            spawnBuildingJob.m_PrefabBuildingData = __TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup;
            spawnBuildingJob.m_PrefabPlaceableObjectData = __TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
            spawnBuildingJob.m_PrefabSpawnableObjectData = __TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
            spawnBuildingJob.m_PrefabObjectGeometryData = __TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
            spawnBuildingJob.m_PrefabAreaGeometryData = __TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
            spawnBuildingJob.m_PrefabNetGeometryData = __TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup;
            spawnBuildingJob.m_Cells = __TypeHandle.__Game_Zones_Cell_RO_BufferLookup;
            spawnBuildingJob.m_PrefabSubAreas = __TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup;
            spawnBuildingJob.m_PrefabSubAreaNodes = __TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup;
            spawnBuildingJob.m_PrefabSubNets = __TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup;
            spawnBuildingJob.m_PrefabPlaceholderElements = __TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
            spawnBuildingJob.m_DefinitionArchetype = m_DefinitionArchetype;
            spawnBuildingJob.m_RandomSeed = RandomSeed.Next();
            spawnBuildingJob.m_LefthandTraffic = m_CityConfigurationSystem.leftHandTraffic;
            spawnBuildingJob.m_TerrainHeightData = m_TerrainSystem.GetHeightData(false);
            JobHandle job7;
            spawnBuildingJob.m_ZoneSearchTree = m_SearchSystem.GetSearchTree(true, out job7);
            spawnBuildingJob.m_BuildingConfigurationData = m_BuildingConfigurationQuery.GetSingleton<BuildingConfigurationData>();
            spawnBuildingJob.m_Residential = residential;
            spawnBuildingJob.m_Commercial = commercial;
            spawnBuildingJob.m_Industrial = industrial;
            spawnBuildingJob.m_CommandBuffer = m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter();
            SpawnBuildingJob jobData2 = spawnBuildingJob;
            JobHandle jobHandle = jobData.ScheduleParallel(m_LotQuery, JobUtils.CombineDependencies(job, job2, job3, job4, job6, Dependency, job5));
            JobHandle jobHandle2 = jobData2.Schedule(3, 1, JobHandle.CombineDependencies(jobHandle, job7));
            m_ResourceSystem.AddPrefabsReader(jobHandle);
            m_PollutionSystem.AddReader(jobHandle);
            m_CommercialDemandSystem.AddReader(jobHandle);
            m_IndustrialDemandSystem.AddReader(jobHandle);
            residential.Dispose(jobHandle2);
            commercial.Dispose(jobHandle2);
            industrial.Dispose(jobHandle2);
            m_ZoneSystem.AddPrefabsReader(jobHandle);
            m_TerrainSystem.AddCPUHeightReader(jobHandle2);
            m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
            m_SearchSystem.AddSearchTreeReader(jobHandle2);
            Dependency = jobHandle2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void __AssignQueries(ref SystemState state)
        {
            __query_1944910156_0 = state.GetEntityQuery(new EntityQueryDesc[]
            {
                new EntityQueryDesc
                {
                    All = new ComponentType[]
                    {
                        ComponentType.ReadOnly<ZonePreferenceData>()
                    },
                    Any = new ComponentType[0],
                    None = new ComponentType[0],
                    Disabled = new ComponentType[0],
                    Absent = new ComponentType[0],
                    Options = EntityQueryOptions.IncludeSystems
                }
            });
        }

        protected override void OnCreateForCompiler()
        {
            base.OnCreateForCompiler();
            this.__AssignQueries(ref CheckedStateRef);
            __TypeHandle.__AssignHandles(ref CheckedStateRef);
        }

        [Preserve]
        public MyZoneSpawnSystem()
        {
        }

        private ZoneSystem m_ZoneSystem;

        private ResidentialDemandSystem m_ResidentialDemandSystem;

        private CommercialDemandSystem m_CommercialDemandSystem;

        private IndustrialDemandSystem m_IndustrialDemandSystem;

        private GroundPollutionSystem m_PollutionSystem;

        private TerrainSystem m_TerrainSystem;

        private Game.Zones.SearchSystem m_SearchSystem;

        private ResourceSystem m_ResourceSystem;

        private CityConfigurationSystem m_CityConfigurationSystem;

        private EndFrameBarrier m_EndFrameBarrier;

        private EntityQuery m_LotQuery;

        private EntityQuery m_BuildingQuery;

        private EntityQuery m_ProcessQuery;

        private EntityQuery m_BuildingConfigurationQuery;

        private EntityArchetype m_DefinitionArchetype;

        private TypeHandle __TypeHandle;

        private EntityQuery __query_1944910156_0;

        public struct SpawnLocation
        {
            public Entity m_Entity;

            public Entity m_Building;

            public int4 m_LotArea;

            public float m_Priority;

            public ZoneType m_ZoneType;

            public Game.Zones.AreaType m_AreaType;

            public LotFlags m_LotFlags;
        }

        [BurstCompile]
        public struct EvaluateSpawnAreas : IJobChunk
        {
            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                Unity.Mathematics.Random random = m_RandomSeed.GetRandom(unfilteredChunkIndex);
                SpawnLocation spawnLocation = default;
                SpawnLocation spawnLocation2 = default;
                SpawnLocation spawnLocation3 = default;
                NativeArray<Entity> nativeArray = chunk.GetNativeArray(m_EntityType);
                BufferAccessor<VacantLot> bufferAccessor = chunk.GetBufferAccessor(ref m_VacantLotType);
                if (bufferAccessor.Length != 0)
                {
                    NativeArray<Owner> nativeArray2 = chunk.GetNativeArray(ref m_OwnerType);
                    NativeArray<CurvePosition> nativeArray3 = chunk.GetNativeArray(ref m_CurvePositionType);
                    NativeArray<Block> nativeArray4 = chunk.GetNativeArray(ref m_BlockType);
                    for (int i = 0; i < nativeArray.Length; i++)
                    {
                        Entity entity = nativeArray[i];
                        DynamicBuffer<VacantLot> dynamicBuffer = bufferAccessor[i];
                        Owner owner = nativeArray2[i];
                        CurvePosition curvePosition = nativeArray3[i];
                        Block block = nativeArray4[i];
                        for (int j = 0; j < dynamicBuffer.Length; j++)
                        {
                            VacantLot vacantLot = dynamicBuffer[j];
                            ZoneData zoneData = m_ZoneData[m_ZonePrefabs[vacantLot.m_Type]];
                            DynamicBuffer<ProcessEstimate> estimates = m_ProcessEstimates[m_ZonePrefabs[vacantLot.m_Type]];
                            switch (zoneData.m_AreaType)
                            {
                                case Game.Zones.AreaType.Residential:
                                    if (m_SpawnResidential != 0)
                                    {
                                        float curvePos = CalculateCurvePos(curvePosition, vacantLot, block);
                                        TryAddLot(ref spawnLocation, ref random, owner.m_Owner, curvePos, entity, vacantLot.m_Area, vacantLot.m_Flags, vacantLot.m_Height, zoneData, estimates, m_Processes, true, false);
                                    }
                                    break;
                                case Game.Zones.AreaType.Commercial:
                                    if (m_SpawnCommercial != 0)
                                    {
                                        float curvePos2 = CalculateCurvePos(curvePosition, vacantLot, block);
                                        TryAddLot(ref spawnLocation2, ref random, owner.m_Owner, curvePos2, entity, vacantLot.m_Area, vacantLot.m_Flags, vacantLot.m_Height, zoneData, estimates, m_Processes, true, false);
                                    }
                                    break;
                                case Game.Zones.AreaType.Industrial:
                                    if (m_SpawnIndustrial != 0 || m_SpawnStorage != 0)
                                    {
                                        float curvePos3 = CalculateCurvePos(curvePosition, vacantLot, block);
                                        TryAddLot(ref spawnLocation3, ref random, owner.m_Owner, curvePos3, entity, vacantLot.m_Area, vacantLot.m_Flags, vacantLot.m_Height, zoneData, estimates, m_Processes, m_SpawnIndustrial != 0, m_SpawnStorage != 0);
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (spawnLocation.m_Priority != 0f)
                {
                    m_Residential.Enqueue(spawnLocation);
                }
                if (spawnLocation2.m_Priority != 0f)
                {
                    m_Commercial.Enqueue(spawnLocation2);
                }
                if (spawnLocation3.m_Priority != 0f)
                {
                    m_Industrial.Enqueue(spawnLocation3);
                }
            }

            private float CalculateCurvePos(CurvePosition curvePosition, VacantLot lot, Block block)
            {
                float s = math.saturate((lot.m_Area.x + lot.m_Area.y) * 0.5f / block.m_Size.x);
                return math.lerp(curvePosition.m_CurvePosition.x, curvePosition.m_CurvePosition.y, s);
            }

            private void TryAddLot(ref SpawnLocation bestLocation, ref Unity.Mathematics.Random random, Entity road, float curvePos, Entity entity, int4 area, LotFlags flags, int height, ZoneData zoneData, DynamicBuffer<ProcessEstimate> estimates, NativeList<IndustrialProcessData> processes, bool normal = true, bool storage = false)
            {
                if (m_Availabilities.HasBuffer(road))
                {
                    if ((zoneData.m_ZoneFlags & ZoneFlags.SupportLeftCorner) == 0)
                    {
                        flags &= ~LotFlags.CornerLeft;
                    }
                    if ((zoneData.m_ZoneFlags & ZoneFlags.SupportRightCorner) == 0)
                    {
                        flags &= ~LotFlags.CornerRight;
                    }
                    SpawnLocation spawnLocation = default;
                    spawnLocation.m_Entity = entity;
                    spawnLocation.m_LotArea = area;
                    spawnLocation.m_ZoneType = zoneData.m_ZoneType;
                    spawnLocation.m_AreaType = zoneData.m_AreaType;
                    spawnLocation.m_LotFlags = flags;
                    bool office = zoneData.m_AreaType == Game.Zones.AreaType.Industrial && estimates.Length == 0;
                    DynamicBuffer<ResourceAvailability> availabilities = m_Availabilities[road];
                    if (!m_BlockData.HasComponent(spawnLocation.m_Entity))
                    {
                        return;
                    }
                    float3 position = ZoneUtils.GetPosition(m_BlockData[spawnLocation.m_Entity], spawnLocation.m_LotArea.xz, spawnLocation.m_LotArea.yw);
                    bool extractor = false;
                    GroundPollution pollution = GroundPollutionSystem.GetPollution(position, m_PollutionMap);
                    float2 pollution2 = new float2(pollution.m_Pollution, pollution.m_Pollution - pollution.m_Previous);
                    float landValue = m_LandValues[road].m_LandValue;
                    float maxHeight = height - position.y;
                    if (SelectBuilding(ref spawnLocation, ref random, availabilities, zoneData, curvePos, pollution2, landValue, maxHeight, estimates, processes, normal, storage, extractor, office) && spawnLocation.m_Priority > bestLocation.m_Priority)
                    {
                        bestLocation = spawnLocation;
                    }
                }
            }

            private bool SelectBuilding(ref SpawnLocation location, ref Unity.Mathematics.Random random, DynamicBuffer<ResourceAvailability> availabilities, ZoneData zoneData, float curvePos, float2 pollution, float landValue, float maxHeight, DynamicBuffer<ProcessEstimate> estimates, NativeList<IndustrialProcessData> processes, bool normal = true, bool storage = false, bool extractor = false, bool office = false)
            {
                int2 @int = location.m_LotArea.yw - location.m_LotArea.xz;
                BuildingData buildingData = default;
                bool2 lhs = new bool2((location.m_LotFlags & LotFlags.CornerLeft) > 0, (location.m_LotFlags & LotFlags.CornerRight) > 0);
                bool flag = (zoneData.m_ZoneFlags & ZoneFlags.SupportNarrow) == 0;
                for (int i = 0; i < m_BuildingChunks.Length; i++)
                {
                    ArchetypeChunk archetypeChunk = m_BuildingChunks[i];
                    ZoneType zoneType = archetypeChunk.GetSharedComponent(m_BuildingSpawnGroupType).m_ZoneType;
                    if (zoneType.Equals(location.m_ZoneType))
                    {
                        bool flag2 = archetypeChunk.Has(ref m_WarehouseType);
                        if (!(flag2 & !storage | !flag2 & !normal))
                        {
                            NativeArray<Entity> nativeArray = archetypeChunk.GetNativeArray(m_EntityType);
                            NativeArray<BuildingData> nativeArray2 = archetypeChunk.GetNativeArray(ref m_BuildingDataType);
                            NativeArray<SpawnableBuildingData> nativeArray3 = archetypeChunk.GetNativeArray(ref m_SpawnableBuildingType);
                            NativeArray<BuildingPropertyData> nativeArray4 = archetypeChunk.GetNativeArray(ref m_BuildingPropertyType);
                            NativeArray<ObjectGeometryData> nativeArray5 = archetypeChunk.GetNativeArray(ref m_ObjectGeometryType);
                            for (int j = 0; j < nativeArray3.Length; j++)
                            {
                                if (nativeArray3[j].m_Level == 1)
                                {
                                    BuildingData buildingData2 = nativeArray2[j];
                                    int2 lotSize = buildingData2.m_LotSize;
                                    bool2 rhs = new bool2((buildingData2.m_Flags & Game.Prefabs.BuildingFlags.LeftAccess) > 0U, (buildingData2.m_Flags & Game.Prefabs.BuildingFlags.RightAccess) > 0U);
                                    float y = nativeArray5[j].m_Size.y;

                                    string r_min = Plugin.m_rMinSize.Value;
                                    string r_max = Plugin.m_rMaxSize.Value;
                                    string c_min = Plugin.m_cMinSize.Value;
                                    string c_max = Plugin.m_cMaxSize.Value;
                                    string i_min = Plugin.m_iMinSize.Value;
                                    string i_max = Plugin.m_iMaxSize.Value;
                                    string o_min = Plugin.m_oMinSize.Value;
                                    string o_max = Plugin.m_oMaxSize.Value;

                                    bool flag3 = Plugin.JudgeBuilding(zoneType.m_Index, lotSize);

                                    if (math.all(lotSize <= @int) && y <= maxHeight && flag3)
                                    {
                                        BuildingPropertyData buildingPropertyData = nativeArray4[j];
                                        int num = EvaluateDemandAndAvailability(location.m_AreaType, buildingPropertyData, lotSize.x * lotSize.y, flag2);
                                        if (num >= m_MinDemand || extractor)
                                        {
                                            int2 int2 = math.select(@int - lotSize, 0, lotSize == @int - 1);
                                            float num2 = lotSize.x * lotSize.y * random.NextFloat(1f, 1.05f);
                                            num2 += int2.x * lotSize.y * random.NextFloat(0.95f, 1f);
                                            num2 += @int.x * int2.y * random.NextFloat(0.55f, 0.6f);
                                            num2 /= @int.x * @int.y;
                                            num2 *= num + 1;
                                            num2 *= math.csum(math.select(0.01f, 0.5f, lhs == rhs));
                                            if (!extractor)
                                            {
                                                float num3 = landValue;
                                                float num4;
                                                if (location.m_AreaType == Game.Zones.AreaType.Residential)
                                                {
                                                    num4 = buildingPropertyData.m_ResidentialProperties == 1 ? 2f : buildingPropertyData.CountProperties();
                                                    lotSize.x = math.select(lotSize.x, @int.x, lotSize.x == @int.x - 1 && flag);
                                                    num3 *= lotSize.x * @int.y;
                                                }
                                                else
                                                {
                                                    num4 = buildingPropertyData.m_SpaceMultiplier;
                                                }
                                                float num5 = ZoneEvaluationUtils.GetScore(location.m_AreaType, office, availabilities, curvePos, ref m_Preferences, flag2, flag2 ? m_StorageDemands : m_IndustrialDemands, buildingPropertyData, pollution, num3 / num4, estimates, processes, m_ResourcePrefabs, ref m_ResourceDatas);
                                                num5 = math.select(num5, math.max(0f, num5) + 1f, m_MinDemand == 0);
                                                num2 *= num5;
                                            }
                                            if (num2 > location.m_Priority)
                                            {
                                                location.m_Building = nativeArray[j];
                                                buildingData = buildingData2;
                                                location.m_Priority = num2;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (location.m_Building != Entity.Null)
                {
                    bool flag3 = (buildingData.m_Flags & Game.Prefabs.BuildingFlags.LeftAccess) == 0U && ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.RightAccess) != 0U || random.NextBool());
                    if (flag3)
                    {
                        location.m_LotArea.x = location.m_LotArea.y - buildingData.m_LotSize.x;
                        location.m_LotArea.w = location.m_LotArea.z + buildingData.m_LotSize.y;
                    }
                    else
                    {
                        location.m_LotArea.yw = location.m_LotArea.xz + buildingData.m_LotSize;
                    }
                    return true;
                }
                return false;
            }

            private int EvaluateDemandAndAvailability(Game.Zones.AreaType areaType, BuildingPropertyData propertyData, int size, bool storage = false)
            {
                switch (areaType)
                {
                    case Game.Zones.AreaType.Residential:
                        if (propertyData.m_ResidentialProperties == 1)
                        {
                            return m_ResidentialDemands.z;
                        }
                        if (propertyData.m_ResidentialProperties / (propertyData.m_SpaceMultiplier * size) < 1f)
                        {
                            return m_ResidentialDemands.y;
                        }
                        return m_ResidentialDemands.x;
                    case Game.Zones.AreaType.Commercial:
                        {
                            int num = 0;
                            ResourceIterator iterator = ResourceIterator.GetIterator();
                            while (iterator.Next())
                            {
                                if ((propertyData.m_AllowedSold & iterator.resource) != Resource.NoResource)
                                {
                                    num += m_CommercialDemands[EconomyUtils.GetResourceIndex(iterator.resource)];
                                }
                            }
                            return num;
                        }
                    case Game.Zones.AreaType.Industrial:
                        {
                            int num2 = 0;
                            ResourceIterator iterator2 = ResourceIterator.GetIterator();
                            while (iterator2.Next())
                            {
                                if (storage)
                                {
                                    if ((propertyData.m_AllowedStored & iterator2.resource) != Resource.NoResource)
                                    {
                                        num2 += m_StorageDemands[EconomyUtils.GetResourceIndex(iterator2.resource)];
                                    }
                                }
                                else if ((propertyData.m_AllowedManufactured & iterator2.resource) != Resource.NoResource)
                                {
                                    num2 += m_IndustrialDemands[EconomyUtils.GetResourceIndex(iterator2.resource)];
                                }
                            }
                            return num2;
                        }
                    default:
                        return 0;
                }
            }

            void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                Execute(chunk, unfilteredChunkIndex, useEnabledMask, chunkEnabledMask);
            }

            [ReadOnly]
            public NativeList<ArchetypeChunk> m_BuildingChunks;

            [ReadOnly]
            public ZonePrefabs m_ZonePrefabs;

            [ReadOnly]
            public ZonePreferenceData m_Preferences;

            [ReadOnly]
            public int m_SpawnResidential;

            [ReadOnly]
            public int m_SpawnCommercial;

            [ReadOnly]
            public int m_SpawnIndustrial;

            [ReadOnly]
            public int m_SpawnStorage;

            [ReadOnly]
            public int m_MinDemand;

            public int3 m_ResidentialDemands;

            [ReadOnly]
            public NativeArray<int> m_CommercialDemands;

            [ReadOnly]
            public NativeArray<int> m_IndustrialDemands;

            [ReadOnly]
            public NativeArray<int> m_StorageDemands;

            [ReadOnly]
            public RandomSeed m_RandomSeed;

            [ReadOnly]
            public EntityTypeHandle m_EntityType;

            [ReadOnly]
            public ComponentTypeHandle<Block> m_BlockType;

            [ReadOnly]
            public ComponentTypeHandle<Owner> m_OwnerType;

            [ReadOnly]
            public ComponentTypeHandle<CurvePosition> m_CurvePositionType;

            [ReadOnly]
            public BufferTypeHandle<VacantLot> m_VacantLotType;

            [ReadOnly]
            public ComponentTypeHandle<BuildingData> m_BuildingDataType;

            [ReadOnly]
            public ComponentTypeHandle<SpawnableBuildingData> m_SpawnableBuildingType;

            [ReadOnly]
            public ComponentTypeHandle<BuildingPropertyData> m_BuildingPropertyType;

            [ReadOnly]
            public ComponentTypeHandle<ObjectGeometryData> m_ObjectGeometryType;

            [ReadOnly]
            public SharedComponentTypeHandle<BuildingSpawnGroupData> m_BuildingSpawnGroupType;

            [ReadOnly]
            public ComponentTypeHandle<WarehouseData> m_WarehouseType;

            [ReadOnly]
            public ComponentLookup<ZoneData> m_ZoneData;

            [ReadOnly]
            public BufferLookup<ResourceAvailability> m_Availabilities;

            [ReadOnly]
            public NativeList<IndustrialProcessData> m_Processes;

            [ReadOnly]
            public BufferLookup<ProcessEstimate> m_ProcessEstimates;

            [ReadOnly]
            public ComponentLookup<LandValue> m_LandValues;

            [ReadOnly]
            public ComponentLookup<Block> m_BlockData;

            [ReadOnly]
            public ComponentLookup<ResourceData> m_ResourceDatas;

            [ReadOnly]
            public ResourcePrefabs m_ResourcePrefabs;

            [ReadOnly]
            public NativeArray<GroundPollution> m_PollutionMap;

            public NativeQueue<SpawnLocation>.ParallelWriter m_Residential;

            public NativeQueue<SpawnLocation>.ParallelWriter m_Commercial;

            public NativeQueue<SpawnLocation>.ParallelWriter m_Industrial;
        }

        [BurstCompile]
        public struct SpawnBuildingJob : IJobParallelFor
        {
            public void Execute(int index)
            {
                SpawnLocation location;
                switch (index)
                {
                    case 0:
                        if (!SelectLocation(m_Residential, out location))
                        {
                            return;
                        }
                        break;
                    case 1:
                        if (!SelectLocation(m_Commercial, out location))
                        {
                            return;
                        }
                        break;
                    case 2:
                        if (!SelectLocation(m_Industrial, out location))
                        {
                            return;
                        }
                        break;
                    default:
                        return;
                }
                Unity.Mathematics.Random random = m_RandomSeed.GetRandom(index);
                Spawn(index, location, ref random);
            }

            private bool SelectLocation(NativeQueue<SpawnLocation> queue, out SpawnLocation location)
            {
                location = default;
                SpawnLocation spawnLocation;
                while (queue.TryDequeue(out spawnLocation))
                {
                    if (spawnLocation.m_Priority > location.m_Priority)
                    {
                        location = spawnLocation;
                    }
                }
                return location.m_Priority != 0f;
            }

            private void Spawn(int jobIndex, SpawnLocation location, ref Unity.Mathematics.Random random)
            {
                BuildingData buildingData = m_PrefabBuildingData[location.m_Building];
                ObjectGeometryData objectGeometryData = m_PrefabObjectGeometryData[location.m_Building];
                PlaceableObjectData placeableObjectData = default;
                if (m_PrefabPlaceableObjectData.HasComponent(location.m_Building))
                {
                    placeableObjectData = m_PrefabPlaceableObjectData[location.m_Building];
                }
                CreationDefinition component = default;
                component.m_Prefab = location.m_Building;
                component.m_Flags |= CreationFlags.Permanent | CreationFlags.Construction;
                component.m_RandomSeed = random.NextInt();
                Transform transform = default;
                if (m_BlockData.HasComponent(location.m_Entity))
                {
                    Block block = m_BlockData[location.m_Entity];
                    transform.m_Position = ZoneUtils.GetPosition(block, location.m_LotArea.xz, location.m_LotArea.yw);
                    transform.m_Rotation = ZoneUtils.GetRotation(block);
                }
                else if (m_TransformData.HasComponent(location.m_Entity))
                {
                    component.m_Attached = location.m_Entity;
                    component.m_Flags |= CreationFlags.Attach;
                    Transform transform2 = m_TransformData[location.m_Entity];
                    PrefabRef prefabRef = m_PrefabRefData[location.m_Entity];
                    BuildingData ptr = m_PrefabBuildingData[prefabRef.m_Prefab];
                    transform.m_Position = transform2.m_Position;
                    transform.m_Rotation = transform2.m_Rotation;
                    float z = (ptr.m_LotSize.y - buildingData.m_LotSize.y) * 4f;
                    transform.m_Position += math.rotate(transform.m_Rotation, new float3(0f, 0f, z));
                }
                float3 worldPosition = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
                transform.m_Position.y = TerrainUtils.SampleHeight(ref m_TerrainHeightData, worldPosition);
                if ((placeableObjectData.m_Flags & (Game.Objects.PlacementFlags.Shoreline | Game.Objects.PlacementFlags.Floating)) == Game.Objects.PlacementFlags.None)
                {
                    transform.m_Position.y = transform.m_Position.y + placeableObjectData.m_PlacementOffset.y;
                }
                float maxHeight = GetMaxHeight(transform, buildingData);
                transform.m_Position.y = math.min(transform.m_Position.y, maxHeight - objectGeometryData.m_Size.y - 0.1f);
                ObjectDefinition objectDefinition = default;
                objectDefinition.m_ParentMesh = -1;
                objectDefinition.m_Position = transform.m_Position;
                objectDefinition.m_Rotation = transform.m_Rotation;
                objectDefinition.m_LocalPosition = transform.m_Position;
                objectDefinition.m_LocalRotation = transform.m_Rotation;
                Entity e = m_CommandBuffer.CreateEntity(jobIndex, m_DefinitionArchetype);
                m_CommandBuffer.SetComponent(jobIndex, e, component);
                m_CommandBuffer.SetComponent(jobIndex, e, objectDefinition);
                OwnerDefinition ownerDefinition = default;
                ownerDefinition.m_Prefab = location.m_Building;
                ownerDefinition.m_Position = objectDefinition.m_Position;
                ownerDefinition.m_Rotation = objectDefinition.m_Rotation;
                if (m_PrefabSubAreas.HasBuffer(location.m_Building))
                {
                    Spawn(jobIndex, ownerDefinition, m_PrefabSubAreas[location.m_Building], m_PrefabSubAreaNodes[location.m_Building], buildingData, ref random);
                }
                if (m_PrefabSubNets.HasBuffer(location.m_Building))
                {
                    Spawn(jobIndex, ownerDefinition, m_PrefabSubNets[location.m_Building], ref random);
                }
            }

            private float GetMaxHeight(Transform transform, BuildingData prefabBuildingData)
            {
                float2 xz = math.rotate(transform.m_Rotation, new float3(8f, 0f, 0f)).xz;
                float2 xz2 = math.rotate(transform.m_Rotation, new float3(0f, 0f, 8f)).xz;
                float2 @float = xz * (prefabBuildingData.m_LotSize.x * 0.5f - 0.5f);
                float2 float2 = xz2 * (prefabBuildingData.m_LotSize.y * 0.5f - 0.5f);
                float2 rhs = math.abs(float2) + math.abs(@float);
                Iterator iterator = default;
                iterator.m_Bounds = new Bounds2(transform.m_Position.xz - rhs, transform.m_Position.xz + rhs);
                iterator.m_LotSize = prefabBuildingData.m_LotSize;
                iterator.m_StartPosition = transform.m_Position.xz + float2 + @float;
                iterator.m_Right = xz;
                iterator.m_Forward = xz2;
                iterator.m_MaxHeight = int.MaxValue;
                iterator.m_BlockData = m_BlockData;
                iterator.m_ValidAreaData = m_ValidAreaData;
                iterator.m_Cells = m_Cells;
                Iterator iterator2 = iterator;
                m_ZoneSearchTree.Iterate(ref iterator2, 0);
                return iterator2.m_MaxHeight;
            }

            private void Spawn(int jobIndex, OwnerDefinition ownerDefinition, DynamicBuffer<Game.Prefabs.SubArea> subAreas, DynamicBuffer<SubAreaNode> subAreaNodes, BuildingData prefabBuildingData, ref Unity.Mathematics.Random random)
            {
                NativeParallelHashMap<Entity, int> selectedSpawnables = default;
                bool flag = false;
                int i = 0;
                while (i < subAreas.Length)
                {
                    Game.Prefabs.SubArea subArea = subAreas[i];
                    AreaGeometryData areaGeometryData = m_PrefabAreaGeometryData[subArea.m_Prefab];
                    if (areaGeometryData.m_Type != Game.Areas.AreaType.Surface)
                    {
                        goto IL_51;
                    }
                    if (!flag)
                    {
                        subArea.m_Prefab = m_BuildingConfigurationData.m_ConstructionSurface;
                        flag = true;
                        goto IL_51;
                    }
                IL_2C1:
                    i++;
                    continue;
                IL_51:
                    DynamicBuffer<PlaceholderObjectElement> placeholderElements;
                    int randomSeed;
                    if (m_PrefabPlaceholderElements.TryGetBuffer(subArea.m_Prefab, out placeholderElements))
                    {
                        if (!selectedSpawnables.IsCreated)
                        {
                            selectedSpawnables = new NativeParallelHashMap<Entity, int>(10, Allocator.Temp);
                        }
                        if (!AreaUtils.SelectAreaPrefab(placeholderElements, m_PrefabSpawnableObjectData, selectedSpawnables, ref random, out subArea.m_Prefab, out randomSeed))
                        {
                            goto IL_2C1;
                        }
                    }
                    else
                    {
                        randomSeed = random.NextInt();
                    }
                    Entity e = m_CommandBuffer.CreateEntity(jobIndex);
                    CreationDefinition component = default;
                    component.m_Prefab = subArea.m_Prefab;
                    component.m_RandomSeed = randomSeed;
                    component.m_Flags |= CreationFlags.Permanent;
                    m_CommandBuffer.AddComponent(jobIndex, e, component);
                    m_CommandBuffer.AddComponent(jobIndex, e, default(Updated));
                    m_CommandBuffer.AddComponent(jobIndex, e, ownerDefinition);
                    DynamicBuffer<Game.Areas.Node> dynamicBuffer = m_CommandBuffer.AddBuffer<Game.Areas.Node>(jobIndex, e);
                    if (areaGeometryData.m_Type == Game.Areas.AreaType.Surface)
                    {
                        Quad3 quad = BuildingUtils.CalculateCorners(new Transform(ownerDefinition.m_Position, ownerDefinition.m_Rotation), prefabBuildingData.m_LotSize);
                        dynamicBuffer.ResizeUninitialized(5);
                        dynamicBuffer[0] = new Game.Areas.Node(quad.a, float.MinValue);
                        dynamicBuffer[1] = new Game.Areas.Node(quad.b, float.MinValue);
                        dynamicBuffer[2] = new Game.Areas.Node(quad.c, float.MinValue);
                        dynamicBuffer[3] = new Game.Areas.Node(quad.d, float.MinValue);
                        dynamicBuffer[4] = new Game.Areas.Node(quad.a, float.MinValue);
                        goto IL_2C1;
                    }
                    dynamicBuffer.ResizeUninitialized(subArea.m_NodeRange.y - subArea.m_NodeRange.x + 1);
                    int num = ObjectToolBaseSystem.GetFirstNodeIndex(subAreaNodes, subArea.m_NodeRange);
                    int num2 = 0;
                    for (int j = subArea.m_NodeRange.x; j <= subArea.m_NodeRange.y; j++)
                    {
                        float3 position = subAreaNodes[num].m_Position;
                        float3 position2 = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, position);
                        int parentMesh = subAreaNodes[num].m_ParentMesh;
                        float elevation = math.select(float.MinValue, position.y, parentMesh >= 0);
                        dynamicBuffer[num2] = new Game.Areas.Node(position2, elevation);
                        num2++;
                        if (++num == subArea.m_NodeRange.y)
                        {
                            num = subArea.m_NodeRange.x;
                        }
                    }
                    goto IL_2C1;
                }
                if (selectedSpawnables.IsCreated)
                {
                    selectedSpawnables.Dispose();
                }
            }

            private void Spawn(int jobIndex, OwnerDefinition ownerDefinition, DynamicBuffer<Game.Prefabs.SubNet> subNets, ref Unity.Mathematics.Random random)
            {
                NativeList<float4> nodePositions = new NativeList<float4>(subNets.Length * 2, Allocator.Temp);
                for (int i = 0; i < subNets.Length; i++)
                {
                    Game.Prefabs.SubNet subNet = subNets[i];
                    if (subNet.m_NodeIndex.x >= 0)
                    {
                        while (nodePositions.Length <= subNet.m_NodeIndex.x)
                        {
                            float4 @float = default;
                            nodePositions.Add(@float);
                        }
                        ref NativeList<float4> ptr = ref nodePositions;
                        int index = subNet.m_NodeIndex.x;
                        ptr[index] += new float4(subNet.m_Curve.a, 1f);
                    }
                    if (subNet.m_NodeIndex.y >= 0)
                    {
                        while (nodePositions.Length <= subNet.m_NodeIndex.y)
                        {
                            float4 @float = default;
                            nodePositions.Add(@float);
                        }
                        ref NativeList<float4> ptr = ref nodePositions;
                        int index = subNet.m_NodeIndex.y;
                        ptr[index] += new float4(subNet.m_Curve.d, 1f);
                    }
                }
                for (int j = 0; j < nodePositions.Length; j++)
                {
                    ref NativeList<float4> ptr = ref nodePositions;
                    int index = j;
                    ptr[index] /= math.max(1f, nodePositions[j].w);
                }
                for (int k = 0; k < subNets.Length; k++)
                {
                    Game.Prefabs.SubNet subNet2 = NetUtils.GetSubNet(subNets, k, m_LefthandTraffic, ref m_PrefabNetGeometryData);
                    CreateSubNet(jobIndex, subNet2.m_Prefab, subNet2.m_Curve, subNet2.m_NodeIndex, subNet2.m_ParentMesh, subNet2.m_Upgrades, nodePositions, ownerDefinition, ref random);
                }
                nodePositions.Dispose();
            }

            private void CreateSubNet(int jobIndex, Entity netPrefab, Bezier4x3 curve, int2 nodeIndex, int2 parentMesh, CompositionFlags upgrades, NativeList<float4> nodePositions, OwnerDefinition ownerDefinition, ref Unity.Mathematics.Random random)
            {
                Entity e = m_CommandBuffer.CreateEntity(jobIndex);
                CreationDefinition component = default;
                component.m_Prefab = netPrefab;
                component.m_RandomSeed = random.NextInt();
                component.m_Flags |= CreationFlags.Permanent;
                m_CommandBuffer.AddComponent(jobIndex, e, component);
                m_CommandBuffer.AddComponent(jobIndex, e, default(Updated));
                m_CommandBuffer.AddComponent(jobIndex, e, ownerDefinition);
                NetCourse netCourse = default;
                netCourse.m_Curve = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, curve);
                netCourse.m_StartPosition.m_Position = netCourse.m_Curve.a;
                netCourse.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(netCourse.m_Curve), ownerDefinition.m_Rotation);
                netCourse.m_StartPosition.m_CourseDelta = 0f;
                netCourse.m_StartPosition.m_Elevation = curve.a.y;
                netCourse.m_StartPosition.m_ParentMesh = parentMesh.x;
                if (nodeIndex.x >= 0)
                {
                    netCourse.m_StartPosition.m_Position = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, nodePositions[nodeIndex.x].xyz);
                }
                netCourse.m_EndPosition.m_Position = netCourse.m_Curve.d;
                netCourse.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(netCourse.m_Curve), ownerDefinition.m_Rotation);
                netCourse.m_EndPosition.m_CourseDelta = 1f;
                netCourse.m_EndPosition.m_Elevation = curve.d.y;
                netCourse.m_EndPosition.m_ParentMesh = parentMesh.y;
                if (nodeIndex.y >= 0)
                {
                    netCourse.m_EndPosition.m_Position = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, nodePositions[nodeIndex.y].xyz);
                }
                netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
                netCourse.m_FixedIndex = -1;
                netCourse.m_StartPosition.m_Flags = netCourse.m_StartPosition.m_Flags | CoursePosFlags.IsFirst;
                netCourse.m_EndPosition.m_Flags = netCourse.m_EndPosition.m_Flags | CoursePosFlags.IsLast;
                if (netCourse.m_StartPosition.m_Position.Equals(netCourse.m_EndPosition.m_Position))
                {
                    netCourse.m_StartPosition.m_Flags = netCourse.m_StartPosition.m_Flags | CoursePosFlags.IsLast;
                    netCourse.m_EndPosition.m_Flags = netCourse.m_EndPosition.m_Flags | CoursePosFlags.IsFirst;
                }
                m_CommandBuffer.AddComponent(jobIndex, e, netCourse);
                if (upgrades != default)
                {
                    Upgraded component2 = new Upgraded
                    {
                        m_Flags = upgrades
                    };
                    m_CommandBuffer.AddComponent(jobIndex, e, component2);
                }
            }

            [ReadOnly]
            public ComponentLookup<Block> m_BlockData;

            [ReadOnly]
            public ComponentLookup<ValidArea> m_ValidAreaData;

            [ReadOnly]
            public ComponentLookup<Transform> m_TransformData;

            [ReadOnly]
            public ComponentLookup<PrefabRef> m_PrefabRefData;

            [ReadOnly]
            public ComponentLookup<BuildingData> m_PrefabBuildingData;

            [ReadOnly]
            public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;

            [ReadOnly]
            public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;

            [ReadOnly]
            public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;

            [ReadOnly]
            public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;

            [ReadOnly]
            public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;

            [ReadOnly]
            public BufferLookup<Cell> m_Cells;

            [ReadOnly]
            public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;

            [ReadOnly]
            public BufferLookup<SubAreaNode> m_PrefabSubAreaNodes;

            [ReadOnly]
            public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;

            [ReadOnly]
            public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderElements;

            [ReadOnly]
            public EntityArchetype m_DefinitionArchetype;

            [ReadOnly]
            public RandomSeed m_RandomSeed;

            [ReadOnly]
            public bool m_LefthandTraffic;

            [ReadOnly]
            public TerrainHeightData m_TerrainHeightData;

            [ReadOnly]
            public NativeQuadTree<Entity, Bounds2> m_ZoneSearchTree;

            [ReadOnly]
            public BuildingConfigurationData m_BuildingConfigurationData;

            [NativeDisableParallelForRestriction]
            public NativeQueue<SpawnLocation> m_Residential;

            [NativeDisableParallelForRestriction]
            public NativeQueue<SpawnLocation> m_Commercial;

            [NativeDisableParallelForRestriction]
            public NativeQueue<SpawnLocation> m_Industrial;

            public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

            private struct Iterator : INativeQuadTreeIterator<Entity, Bounds2>, IUnsafeQuadTreeIterator<Entity, Bounds2>
            {
                public bool Intersect(Bounds2 bounds)
                {
                    return MathUtils.Intersect(bounds, m_Bounds);
                }

                public void Iterate(Bounds2 bounds, Entity blockEntity)
                {
                    if (!MathUtils.Intersect(bounds, m_Bounds))
                    {
                        return;
                    }
                    ValidArea validArea = m_ValidAreaData[blockEntity];
                    if (validArea.m_Area.y <= validArea.m_Area.x)
                    {
                        return;
                    }
                    Block block = m_BlockData[blockEntity];
                    DynamicBuffer<Cell> dynamicBuffer = m_Cells[blockEntity];
                    float2 @float = m_StartPosition;
                    int2 @int;
                    @int.y = 0;
                    while (@int.y < m_LotSize.y)
                    {
                        float2 float2 = @float;
                        @int.x = 0;
                        while (@int.x < m_LotSize.x)
                        {
                            int2 cellIndex = ZoneUtils.GetCellIndex(block, float2);
                            if (math.all(cellIndex >= validArea.m_Area.xz & cellIndex < validArea.m_Area.yw))
                            {
                                int index = cellIndex.y * block.m_Size.x + cellIndex.x;
                                Cell cell = dynamicBuffer[index];
                                if ((cell.m_State & CellFlags.Visible) != CellFlags.None)
                                {
                                    m_MaxHeight = math.min(m_MaxHeight, cell.m_Height);
                                }
                            }
                            float2 -= m_Right;
                            @int.x++;
                        }
                        @float -= m_Forward;
                        @int.y++;
                    }
                }

                public Bounds2 m_Bounds;

                public int2 m_LotSize;

                public float2 m_StartPosition;

                public float2 m_Right;

                public float2 m_Forward;

                public int m_MaxHeight;

                public ComponentLookup<Block> m_BlockData;

                public ComponentLookup<ValidArea> m_ValidAreaData;

                public BufferLookup<Cell> m_Cells;
            }
        }

        private struct TypeHandle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void __AssignHandles(ref SystemState state)
            {
                __Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
                __Game_Zones_Block_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Block>(true);
                __Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
                __Game_Zones_CurvePosition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurvePosition>(true);
                __Game_Zones_VacantLot_RO_BufferTypeHandle = state.GetBufferTypeHandle<VacantLot>(true);
                __Game_Prefabs_BuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingData>(true);
                __Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableBuildingData>(true);
                __Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingPropertyData>(true);
                __Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectGeometryData>(true);
                __Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<BuildingSpawnGroupData>();
                __Game_Prefabs_WarehouseData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WarehouseData>(true);
                __Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
                __Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
                __Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
                __Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Block>(true);
                __Game_Zones_ProcessEstimate_RO_BufferLookup = state.GetBufferLookup<ProcessEstimate>(true);
                __Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
                __Game_Zones_ValidArea_RO_ComponentLookup = state.GetComponentLookup<ValidArea>(true);
                __Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
                __Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
                __Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
                __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
                __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
                __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
                __Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
                __Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
                __Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
                __Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
                __Game_Prefabs_SubAreaNode_RO_BufferLookup = state.GetBufferLookup<SubAreaNode>(true);
                __Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
                __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
            }

            [ReadOnly]
            public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<Block> __Game_Zones_Block_RO_ComponentTypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<CurvePosition> __Game_Zones_CurvePosition_RO_ComponentTypeHandle;

            [ReadOnly]
            public BufferTypeHandle<VacantLot> __Game_Zones_VacantLot_RO_BufferTypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentTypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle;

            public SharedComponentTypeHandle<BuildingSpawnGroupData> __Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle;

            [ReadOnly]
            public ComponentTypeHandle<WarehouseData> __Game_Prefabs_WarehouseData_RO_ComponentTypeHandle;

            [ReadOnly]
            public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;

            [ReadOnly]
            public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;

            [ReadOnly]
            public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<Block> __Game_Zones_Block_RO_ComponentLookup;

            [ReadOnly]
            public BufferLookup<ProcessEstimate> __Game_Zones_ProcessEstimate_RO_BufferLookup;

            [ReadOnly]
            public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<ValidArea> __Game_Zones_ValidArea_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;

            [ReadOnly]
            public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;

            [ReadOnly]
            public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;

            [ReadOnly]
            public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;

            [ReadOnly]
            public BufferLookup<SubAreaNode> __Game_Prefabs_SubAreaNode_RO_BufferLookup;

            [ReadOnly]
            public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;

            [ReadOnly]
            public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
        }
    }
}

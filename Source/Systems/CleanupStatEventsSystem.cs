using Unity.Burst.Intrinsics;
using Unity.Entities;

namespace Nanory.Unity.Entities.Stats
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial class CleanupStatEventsSystem : SystemBase
    {
        private EntityQuery _changedStats;
        private EntityQuery _removedStats;

        protected override void OnCreate()
        {
            _changedStats = GetEntityQuery(ComponentType.ReadOnly<StatsChangedEvent>());
            _removedStats = GetEntityQuery(ComponentType.ReadOnly<StatsRemovedEvent>());
        }
        
        protected override void OnUpdate()
        {
            EntityManager.RemoveComponent<StatsChangedEvent>(_changedStats);
            EntityManager.RemoveComponent<StatsRemovedEvent>(_removedStats);

            var bufferTypeHandle = GetBufferTypeHandle<StatReceivedElementEvent>(false);

            var clearBufferJob = new ClearBufferJob {
                BufferHandle = bufferTypeHandle
            };

            // FIX use universal query?
            Dependency = clearBufferJob.ScheduleParallel(EntityManager.UniversalQuery,Dependency);
        }
    }

    struct ClearBufferJob : IJobChunk {
        
        public BufferTypeHandle<StatReceivedElementEvent> BufferHandle;

        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask) {
            
            var buffers = chunk.GetBufferAccessor(ref BufferHandle);
            
            for (var i = 0; i < chunk.Count; i++)
            {
                var buffer = buffers[i];

                buffer.Clear();
            }
        }
    }
}


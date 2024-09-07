using Unity.Collections;
using Unity.Entities;

namespace Nanory.Unity.Entities.Stats
{
    public partial class CalculateStatSystem<TStatComponent> : SystemBase where TStatComponent : unmanaged, IStatComponent
    {
        private EntityQuery _changedStats;
        private EntityQuery _removedStats;
        private EntityQuery _additiveStats;
        private EntityQuery _multiplyStats;

        protected override void OnCreate()
        {
            _changedStats = GetEntityQuery(
                ComponentType.ReadWrite<TStatComponent>(),
                ComponentType.ReadOnly<StatsChangedEvent>(),
                ComponentType.ReadOnly<StatReceiverLink>());

            _removedStats = GetEntityQuery(
                ComponentType.ReadWrite<TStatComponent>(),
                ComponentType.ReadOnly<StatsRemovedEvent>(),
                ComponentType.ReadOnly<StatReceiverLink>());

            _additiveStats = GetEntityQuery(
                ComponentType.ReadOnly<TStatComponent>(),
                ComponentType.ReadOnly<StatReceiverLink>(),
                ComponentType.ReadOnly<AdditiveStatTag>());

            _multiplyStats = GetEntityQuery(
                ComponentType.ReadOnly<TStatComponent>(),
                ComponentType.ReadOnly<StatReceiverLink>(),
                ComponentType.ReadOnly<MultiplyStatTag>());
        }

        protected override void OnUpdate()
        {
            var changedStatsEntities = _changedStats.ToEntityArray(Allocator.TempJob);

            for (var idx = 0; idx < changedStatsEntities.Length; idx++)
            {
                var statReceiver = EntityManager.GetSharedComponentManaged<StatReceiverLink>(changedStatsEntities[idx]);
                Calculate(statReceiver);
            }

            changedStatsEntities.Dispose();

            var removedStatsEntities = _removedStats.ToEntityArray(Allocator.TempJob);

            for (var idx = 0; idx < removedStatsEntities.Length; idx++)
            {
                var statEntity = removedStatsEntities[idx];
                var statReceiver = EntityManager.GetSharedComponentManaged<StatReceiverLink>(statEntity);
                EntityManager.SetSharedComponentManaged(statEntity, new StatReceiverLink() { Value = Entity.Null });
                Calculate(statReceiver);
            }

            removedStatsEntities.Dispose();
        }

        private void Calculate(StatReceiverLink statReceiver)
        {
            var buffer = new EntityCommandBuffer(Allocator.Temp);

            var receiverStat = EntityManager.GetComponentData<TStatComponent>(statReceiver.Value);

            _additiveStats.SetSharedComponentFilter(statReceiver);
            _multiplyStats.SetSharedComponentFilter(statReceiver);

            var totalStatValue = 0f;

            var childrenStatEntites = _additiveStats.ToEntityArray(Allocator.TempJob);

            for (var statIdx = 0; statIdx < childrenStatEntites.Length; statIdx++)
            {
                var childStatEntity = childrenStatEntites[statIdx];
                var stat = SystemAPI.GetComponent<TStatComponent>(childStatEntity);
                totalStatValue += stat.Value;
            }
            childrenStatEntites.Dispose();

            var multiplyChildrenStatEntities = _multiplyStats.ToEntityArray(Allocator.TempJob);

            for (var statIdx = 0; statIdx < multiplyChildrenStatEntities.Length; statIdx++)
            {
                var childStatEntity = multiplyChildrenStatEntities[statIdx];
                var stat = SystemAPI.GetComponent<TStatComponent>(childStatEntity);
                totalStatValue *= stat.Value;
            }
            multiplyChildrenStatEntities.Dispose();

            receiverStat.Value = totalStatValue;

            buffer.AppendToBuffer(statReceiver.Value, new StatReceivedElementEvent()
            {
                StatType = ComponentType.ReadOnly<TStatComponent>() 
            });

            SystemAPI.SetComponent(statReceiver.Value, receiverStat);

            buffer.Playback(EntityManager);
        }
    }
}

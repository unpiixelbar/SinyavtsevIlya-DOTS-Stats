using Unity.Entities;

namespace Nanory.Unity.Entities.Stats
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial class StatSystemGroup : ComponentSystemGroup
    {
    }
}

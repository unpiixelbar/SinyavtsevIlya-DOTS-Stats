using Unity.Entities;

namespace Nanory.Unity.Entities.Stats
{
    public interface IStatComponent : IComponentData {
        
        float Value {
            get;
            set;
        }
    }
}
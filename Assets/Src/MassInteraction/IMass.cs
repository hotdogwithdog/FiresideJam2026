
namespace MassInteraction
{
    public interface IMass
    {
        public float GetMass();
        
        public void AbsorbMass(IMass other);

        public void BeAbsorbed();

        public void ReduceMass(float amount);
        
        public UnityEngine.GameObject GetGameObject();
    }
}

namespace MassInteraction
{
    public interface IMass
    {
        public float GetMass();
        
        public void AbsorbMass(IMass other);

        public void BeAbsorbed();
        
        public UnityEngine.GameObject GetGameObject();
    }
}
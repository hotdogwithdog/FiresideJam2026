using UnityEngine;

namespace MassInteraction
{
    // This class will receive the collision between Mass Objects (IMass) and manager his calls to do that one of them absorbs the other
    public static class MassAbsortionManager
    {
        public static void OnCollisionOfMass(IMass massA, IMass massB)
        {
            if (massA == null || massB == null)
            {
                Debug.LogError($"MassAbsortionManager::OnCollisionOfMass: One of the tow objects do not has IMass Components. The objects are: {massA.GetGameObject().name}, {massB.GetGameObject().name}");
                return;
            }

            if (massA == massB)
            {
                Debug.Log("The objects are the same.");
                return;
            }

            if (massA.GetGameObject().tag == "Player")
            {
                massA.AbsorbMass(massB);
                massB.BeAbsorbed();
                return;
            }
            else if (massB.GetGameObject().tag == "Player")
            {
                massB.AbsorbMass(massA);
                massA.BeAbsorbed();
                return;
            }
            
            if (massA.IsBeingAbsorbed() || massB.IsBeingAbsorbed()) return; // Go out if one of them or both are being absorbing (means one previous call to this method)
            
            bool bAAbsorbsB = massA.GetMass() > massB.GetMass(); // The mass with more mass absorbs except one of the mass are the player that has priority
            if (bAAbsorbsB)
            {
                massA.AbsorbMass(massB);
                massB.BeAbsorbed();
            }
            else
            {
                massB.AbsorbMass(massA);
                massA.BeAbsorbed();
            }
        }
    }
}
using UnityEngine;

namespace TheFlux.Core.Scripts.Mvc.InputSystem
{
    public static class InputData
    {
        public static Vector2 Direction = Vector2.zero;
        public static bool IsFiring = false;
        public static Vector2 Look = Vector2.zero;
        
        public static Vector2 PointerScreen = Vector2.zero;
        public static Vector2 PointerWorld = Vector2.zero;
        
        public static Vector3 HandPosition = Vector3.zero;
        public static Quaternion HandRotation = new Quaternion(0f, 0f, 0f, 0f);
    }
}
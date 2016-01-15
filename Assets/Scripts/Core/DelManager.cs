using UnityEngine;
using System.Collections;


namespace GameCore
{
    public class DelManager
    {
        public delegate void VoidDelegate();

        public delegate void BoolDelegate(bool state);

        public delegate void FloatDelegate(float delta);

        public delegate void VectorDelegate(Vector2 delta);

        public delegate void ObjectDelegate(GameObject obj);

        public delegate void KeyCodeDelegate(KeyCode key);

    }
}
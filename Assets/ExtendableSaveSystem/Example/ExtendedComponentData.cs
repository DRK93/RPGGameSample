using System;
using UnityEngine;

namespace NGS.ExtendableSaveSystem
{
    [Serializable]
    public class ExtendedComponentData : ComponentData
    {
        public virtual void SetVector3(string uniqueName, Vector3 value)
        {
            SetFloat(uniqueName + ".x", value.x);
            Debug.Log("Saved X: " + uniqueName + " x " + " " + value.x);
            SetFloat(uniqueName + ".y", value.y);
            SetFloat(uniqueName + ".z", value.z);
        }

        public virtual void SetTransform(string uniqueName, Transform transform)
        {
            SetVector3(uniqueName + ".position", transform.position);
            SetVector3(uniqueName + ".rotation", transform.eulerAngles);
            SetVector3(uniqueName + ".scale", transform.localScale);
        }


        public Vector3 GetVector3(string uniqueName)
        {
            return new Vector3(
                GetFloat(uniqueName + ".x"),
                GetFloat(uniqueName + ".y"),
                GetFloat(uniqueName + ".z"));
        }

        public void GetTransform(string uniqueName, Transform transform)
        {
            transform.position = GetVector3(uniqueName + ".position");
            Debug.Log("Loaded X: " + uniqueName + " x " + " " + transform.position.x);
            transform.eulerAngles = GetVector3(uniqueName + ".rotation");
            transform.localScale = GetVector3(uniqueName + ".scale");
        }
    }
}

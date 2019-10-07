using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nothing {

    [ExecuteInEditMode]
    public class BendShaderParameters : MonoBehaviour {

        public float maxChange = 30;
        public float maxDist = 100;
        public Vector2 bendOrigin = Vector2.zero;
        public Vector2 bendAmount = new Vector2(0, 1);

        private void Start() {
            if (Application.isPlaying)
                Destroy(gameObject);
        }

        private void Update() {
            Shader.SetGlobalFloat("MaxChange", maxChange);
            Shader.SetGlobalFloat("MaxDist", maxDist);
            Shader.SetGlobalFloat("MaxChange", maxChange);
            Shader.SetGlobalVector("BendOrigin", new Vector4(bendOrigin.x, 0, bendOrigin.y, 0));
            Shader.SetGlobalVector("BendAmount", new Vector4(bendAmount.x, 0, bendAmount.y, 0));
        }
    }
}

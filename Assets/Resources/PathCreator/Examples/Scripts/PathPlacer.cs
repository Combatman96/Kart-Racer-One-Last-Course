﻿using PathCreation;
using UnityEngine;

namespace PathCreation.Examples {

    [ExecuteInEditMode]
    public class PathPlacer : PathSceneTool {

        public GameObject prefab;
        public GameObject holder;
        public float spacing = 3;
        public float roadWidth = 0f;

        const float minSpacing = .1f;

        void Generate () {
            if (pathCreator != null && prefab != null && holder != null) {
                DestroyObjects ();

                VertexPath path = pathCreator.path;

                spacing = Mathf.Max(minSpacing, spacing);
                float dst = 0;
                
                while (dst < path.length) {
                    Vector3 point = path.GetPointAtDistance (dst);
                    Vector3 normal = path.GetNormalAtDistance(dst);
                    Quaternion rot = path.GetRotationAtDistance (dst);
                    point += normal * roadWidth;
                    Instantiate (prefab, point, rot, holder.transform);
                    dst += spacing;
                }
            }
        }

        void DestroyObjects () {
            int numChildren = holder.transform.childCount;
            for (int i = numChildren - 1; i >= 0; i--) {
                DestroyImmediate (holder.transform.GetChild (i).gameObject, false);
            }
        }

        protected override void PathUpdated () {
            if (pathCreator != null) {
                Generate ();
            }
        }
    }
}
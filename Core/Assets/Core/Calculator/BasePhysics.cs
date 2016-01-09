using System;
using UnityEngine;

namespace GameCore.Core
{
    public class BasePhysics
    {
        public static void CalculateMove(Vector3 velocity, Vector3 acceleration, float deltaTime, out Vector3 moveOffset, out Vector3 velocityOffset)
        {
            velocityOffset = acceleration * deltaTime;
            moveOffset = (velocityOffset + velocity * 2f) * deltaTime * 0.5f;
        }

        public static Vector3 CalculateParabolaSpeed(float speedHor, Vector3 startPos, Vector3 endPos, float gravity)
        {
            Vector3 vector = endPos - startPos;
            float y = vector.y;
            vector.y = 0f;
            float time = vector.magnitude / speedHor;
            vector.Normalize();
            return new Vector3(vector.x * speedHor, y / time + 0.5f * gravity * time, vector.z * speedHor);
        }
    }
}

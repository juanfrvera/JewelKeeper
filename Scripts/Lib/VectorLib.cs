using UnityEngine;
using System.Collections;

public static class VectorLib
{
   #region Vector
   public static Vector3 Vec3ToZero(Vector3 vec) => new Vector3(vec.x, vec.y, 0);
   public static Vector2 Vec3ToVec2(Vector3 vec3) => new Vector2(vec3.x, vec3.y);
   public static Vector3 Vec2ToVec3(Vector2 vec2, float z) => new Vector3(vec2.x, vec2.y, z);
   public static Vector3 Vec3ChangeZ(Vector3 vec3, float z) => new Vector3(vec3.x, vec3.y, z);
   public static Vector3 ChangeX(this Vector3 vec3, float x) => new Vector3(x, vec3.y, vec3.z);
   public static Vector3 ChangeY(this Vector3 vec3, float y) => new Vector3(vec3.x, y, vec3.z);
   public static Vector2 ChangeX(this Vector2 vec2, float x) => new Vector2(x, vec2.y);
   public static Vector2 ChangeY(this Vector2 vec2, float y) => new Vector2(vec2.x, y);
   public static Vector3 ChangeZ(this Vector3 vec3, float z) => new Vector3(vec3.x, vec3.y, z);

   /// <summary>
   /// Change X and Z but mantain Y
   /// </summary>
   /// <param name="vec3"></param>
   /// <param name="x"></param>
   /// <param name="z"></param>
   /// <returns></returns>
   public static Vector3 ChangeXZ(this Vector3 vec3, float x, float z) => new Vector3(x, vec3.y, z);

   public static Vector3 AddX(this Vector3 vec3, float delta) => vec3.ChangeX(vec3.x + delta);
   public static Vector3 AddY(this Vector3 vec3, float delta) => vec3.ChangeY(vec3.y + delta);
   public static Vector3 AddZ(this Vector3 vec3, float delta) => vec3.ChangeZ(vec3.z + delta);

   public static Vector2 MultiplyX(this Vector2 vec2, float x) => new Vector2(vec2.x * x, vec2.y);
   public static Vector3 MultiplyX(this Vector3 vec3, float x) => new Vector3(vec3.x * x, vec3.y, vec3.z);
   public static Vector3 MultiplyY(this Vector3 vec3, float y) => new Vector3(vec3.x, vec3.y * y, vec3.z);
   public static Vector3 MultiplyZ(this Vector3 vec3, float z) => new Vector3(vec3.x, vec3.y, vec3.z * z);
   public static Vector3 MultiplyBy(this Vector3 vec3, Vector3 other) => new Vector3(vec3.x * other.x, vec3.y * other.y, vec3.z * other.z);
   public static Vector3 ToVec3(this Vector2 vec2, float z = 0) => new Vector3(vec2.x, vec2.y, z);
   public static Vector3 ToVec3(this Vector4 vec4) => new Vector3(vec4.x, vec4.y, vec4.z);
   public static Vector2 ToVec2(this Vector3 vec3) => new Vector2(vec3.x, vec3.y);

   /// <summary>
   /// Converts the Vector3 to a Vector2 mantaining the X and transforming the Z into Y
   /// </summary>
   /// <param name="vec3"></param>
   /// <returns></returns>
   public static Vector2 ToVec2XZ(this Vector3 vec3) => new Vector2(vec3.x, vec3.z);
   public static Vector2 AddX(this Vector2 vec2, float delta) => vec2.ChangeX(vec2.x + delta);
   public static Vector3 ToVec3FromXZ(this Vector2 vec2XZ, float y = 0) => new Vector3(vec2XZ.x, y, vec2XZ.y);
   public static Vector4 ToVec4(this Vector3 vec3, float w) => new Vector4(vec3.x, vec3.y, vec3.z, w);
   #endregion
   public static int AngleDir(Vector3 vec1, Vector3 vec2, Vector3 up)
   {
      Vector3 perp = Vector3.Cross(vec1, vec2);
      float dir = Vector3.Dot(perp, up);
      if (dir > 0f) return 1;
      if (dir < 0f) return -1;
      return 0;
   }

   public static Vector3 MousePosPerspective()
   {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
      float distance;
      xy.Raycast(ray, out distance);
      return ray.GetPoint(distance);
   }

   public static float VectorBasedRotation(Vector2 vec)
   {
      if (vec == Vector2.right) return 90;
      else
      {
         if (vec == Vector2.left) return 270;
         else
         {
            if (vec == Vector2.up) return 180;
            else return 0;
         }
      }
   }
   /// <summary>
   /// Generates a random direction  considering only left, right, down and up
   /// </summary>
   /// <returns></returns>
   public static Vector2 SquareRandomDirection()
   {
      var val = Random.Range(0, 4);
      switch (val)
      {
         case 0: return Vector2.left;
         case 1: return Vector2.right;
         case 2: return Vector2.down;
         default: return Vector2.up;
      }
   }

   public static Vector3 SwapYZ(this Vector3 vec) => new Vector3(vec.x, vec.z, vec.y);

   public static Vector2 Product(Vector2 vec1, Vector2 vec2) => new Vector2(vec1.x * vec2.x, vec1.y * vec2.y);
   public static Vector3 SphericalToCartesian(float radius, float polar, float elevation)
   {
      elevation *= Mathf.Deg2Rad;
      polar *= Mathf.Deg2Rad;

      float a = radius * Mathf.Cos(elevation);

      return new Vector3(a * Mathf.Cos(polar), radius * Mathf.Sin(elevation), a * Mathf.Sin(polar));
   }
   public static void CartesianToSpherical(Vector3 cartCoords, out float outRadius, out float outPolar, out float outElevation)
   {
      if (cartCoords.x == 0)
      {
         cartCoords.x = Mathf.Epsilon;
      }

      outRadius = Mathf.Sqrt((cartCoords.x * cartCoords.x)
                                                            + (cartCoords.y * cartCoords.y)
                                                            + (cartCoords.z * cartCoords.z));
      outPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
      
      if (cartCoords.x < 0)
      {
         outPolar += Mathf.PI;
      }

      outElevation = Mathf.Asin(cartCoords.y / outRadius);
   }

   public static bool GreaterOrEqual(this Vector2 pos1, Vector2 pos2) => pos1.x >= pos2.x && pos1.y >= pos2.y;
   public static bool LowerOrEqual(this Vector2 pos1, Vector2 pos2) => pos1.x <= pos2.x && pos1.y <= pos2.y;
}
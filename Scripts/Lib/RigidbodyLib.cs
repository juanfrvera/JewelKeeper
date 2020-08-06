using UnityEngine;
public static class RigidbodyLib
{
		public static void SleepForReal(this Rigidbody2D rig)
		{
				rig.Sleep();
				rig.velocity = Vector2.zero;
				rig.bodyType = RigidbodyType2D.Kinematic;
		}
		public static void SleepForReal(this Rigidbody rig)
		{
				rig.velocity = Vector3.zero;
				rig.Sleep();
				rig.isKinematic = true;
		}
		public static void WakeUpForReal(this Rigidbody2D rig)
		{
				rig.WakeUp();
				rig.bodyType = RigidbodyType2D.Dynamic;
		}
		public static void WakeUpForReal(this Rigidbody rig)
		{
				rig.WakeUp();
				rig.isKinematic = false;
		}
}

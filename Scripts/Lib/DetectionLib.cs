using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectionLib
{
		public static IEnumerable<T> OverlapSphereInInterface<T>(Vector3 position, float maxDistance)
		{
				var seen = Physics.OverlapSphere(position, maxDistance);

				return seen.Select(s => s.GetComponent<T>());
		}

		public static T NearestInSphereInterface<T>(Vector3 position, float maxDistance)
		{
				var all = Physics.OverlapSphere(position, maxDistance);

				float minDist = float.MaxValue;
				T nearest = default(T);

				foreach (var obj in all)
				{
						var dist = Vector3.Distance(obj.transform.position, position);

						if (dist < minDist)
						{
								nearest = obj.GetComponent<T>();
								minDist = dist;
						}
				}

				return nearest;
		}
}
using UnityEngine;

public static class PhysicsLib
{
		public static Vector3 ParabolaPositionInSeconds(float t, Vector3 startPos, Vector3 startVel)
		{
				if(t < 0)
				{
						Debug.LogError("Se ingresó un tiempo negativo");
						return startPos;
				}
				return startPos + startVel * t + 0.5f * Physics.gravity * (t * t);
		}

		/// <summary>
		/// Devueve el tiempo que tardará en llegar
		/// </summary>
		/// <param name="velY"></param>
		/// <param name="startY"></param>
		/// <param name="endY"></param>
		/// <returns></returns>
		public static float? ParabolaTime(float velY, float startY = 0, float endY = 0)
		{
				float g = Physics.gravity.y;

				float dif = endY - startY;//Si es positivo es porque hay que subir desde donde estamos, si es negativo es porque hay que bajar

				//Si la velocidad ya es negativa y hay que subir es imposible llegar
				if (velY < 0 && dif > 0)
						return null;

				float extraT = MinBaskara(0.5f * g, -velY, startY - endY);
				return (-2 * velY / g) + extraT;
		}

		/// <summary>
		/// Devuelvo los dos tiempos relativos en que la parábola toca la posición deseada.
		/// Si los tiempos de llegada son negativos, devuelvo null
		/// </summary>
		/// <param name="startSpeed"></param>
		/// <param name="startPos"></param>
		/// <param name="endPos"></param>
		/// <returns></returns>
		public static (float?, float?) ParabolaTimes(float startSpeed, float startPos, float endPos)
		{
				(float?, float?) bask = Baskara(0.5f * Physics.gravity.y, startSpeed, startPos - endPos);
				if (bask.Item1 != null && bask.Item1 < 0) bask.Item1 = null;
				if (bask.Item2 != null && bask.Item2 < 0) bask.Item2 = null;
				return bask;
		}
		public static Vector3 ParabolaVelocity(Vector3 startVel, float startY = 0, float endY = 0)
		{
				float g = Physics.gravity.y;
				float? extraTime = ParabolaTime(-startVel.y, startY, endY);//Obtener el tiempo que falta para tocar el suelo
				if (extraTime != null)
						return new Vector3(startVel.x, -startVel.y + g * extraTime.Value, startVel.z);
				else
						return startVel.MultiplyY(-1);
		}
		public static Vector3 BounceVelocity(Vector3 vel, float bounce) => new Vector3(vel.x * bounce, -vel.y * bounce, vel.z * bounce);

		private static (float?, float?) Baskara(float a, float b, float c)
		{
				float insideRoot = b * b - 4 * a * c;
				if (insideRoot == 0)
						return (-b / (2 * a), null);
				else
				{
						if (insideRoot > 0)
						{
								float root = Mathf.Sqrt(insideRoot);
								return ((-b + root) / (2 * a), (-b - root) / (2 * a));
						}
						else
								return (null, null);
				}
		}
		private static float MinBaskara(float a, float b, float c)
		{
				float insideRoot = b * b - 4 * a * c;
				if (insideRoot == 0)
						return -b / (2 * a);
				else
				{
						if (insideRoot > 0)
						{
								float root = Mathf.Sqrt(insideRoot);
								float sol1 = (-b + root) / (2 * a);
								float sol2 = (-b - root) / (2 * a);

								return Mathf.Max(sol1, sol2);
						}
						else
								return 0;
				}
		}
		#region Parabolic Shots
		/// <summary>
		/// Calcula la velocidad inicial de tiro oblicuo con solo el tiempo como parametro
		/// </summary>
		/// <returns></returns>
		public static Vector2 TimeParabola(Vector2 pos1, Vector2 pos2, float time)
		{
				float gravity = Physics.gravity.y;
				Vector2 dist = pos2 - pos1;

				return new Vector2(dist.x / time, dist.y / time - 0.5f * gravity * time);
		}
		public static Vector3 TimeParabolaXZ(Vector3 pos1, Vector3 pos2, float time)
		{
				float gravity = Physics.gravity.y;
				Vector3 dif = pos2 - pos1;

				return new Vector3(dif.x / time, dif.y / time - 0.5f * gravity * time, dif.z / time);
		}
		/// <summary>
		/// Te devuelvo la velocidad inicial de tiro oblicuo con solo la magnitud de velocidad que querés tirar
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <param name="speed"></param>
		/// <returns></returns>
		public static Vector2 VelocityParabolaXY(Vector2 pos1, Vector2 pos2, float speed) => TimeParabola(pos1, pos2, Vector2.Distance(pos1, pos2) / speed);
		public static Vector3 VelocityParabolaXZ(Vector3 pos1, Vector3 pos2, float speed) => TimeParabolaXZ(pos1, pos2, Vector3.Distance(pos1, pos2) / speed);

		/// <summary>
		/// Calculo y te devuelvo la velocidad de tiro oblicuo, primero voy a tratar de encontrar un ángulo que sea válido para la velocidad y el tiempo
		/// que me pasaste, si no encuentro uno válido dejo el tiempo variable y calculo el tiro oblicuo con solo la velocidad
		/// </summary>
		/// <param name="pos1">Posición inicial desde donde se calculará el tiro</param>
		/// <param name="pos2">Posición que se quiere alcanzar</param>
		/// <param name="vel">Magnitud de velocidad utilizada para calcular</param>
		/// <param name="time">Tiempo de recorrido total de la flecha</param>
		/// <returns></returns>
		public static Vector2 TimeAndVelocityParabola(Vector2 pos1, Vector2 pos2, float vel, float time)
		{
				var dif = pos2 - pos1;
				float gravity = Mathf.Abs(Physics.gravity.y);

				var preAngle = dif.y / (time * vel) + (0.5f * gravity * time) / vel;
				if (Mathf.Abs(preAngle) <= 1)
				{
						var angle = Mathf.Asin(preAngle);
						return new Vector2(dif.x / time, vel * Mathf.Sin(angle));
				}
				else return VelocityParabolaXY(pos1, pos2, vel);
		}

		private static Vector3 OverYVelocityParabolaXZDeterministic(Vector3 pos1, Vector3 pos2, float speed, Vector3 noTouch)
		{
				float gravity = Physics.gravity.y;
				Vector3 dif = pos2 - pos1;

				//En MRU z = pos1.z + vel.z * time;
				float noTouchTime = Mathf.Abs((noTouch.z - pos1.z) / speed);//Lo hacemos con Z por la disposición de la cancha, en un futuro para que sea genérico habría que considerar X también

				//y está en MRUA y = pos1.y + vel.y * time + 0.5f*gravity*time^2
				//Y queremos que se cumpla que pos1.y + vel.y * noTouchTime + 0.5*gravity*noTouchTime^2 > noTouch.y

				float vy = (noTouch.y - pos1.y - 0.5f * gravity * noTouchTime * noTouchTime) / noTouchTime;

				float totalTime = ParabolaTime(vy, pos1.y, pos2.y).Value;

				//Las velocidades en X y Z siempre serán constantes, independientemente de la velocidad en Y
				//La única que tenemos que varía es la velocidad en Y, pero no hay nada que frene a las otras velocidades, por lo que hay que variarlas según la velocidad en Y
				return new Vector3(dif.x / totalTime, vy, dif.z / totalTime);
		}

		private static Vector3 AngleParabola(Vector3 pos1, Vector3 pos2, float angle, float time)
		{
				Vector3 dif = pos2 - pos1;
				float g = Mathf.Abs(Physics.gravity.y);
				float radAngle = angle * Mathf.Deg2Rad;
				//float vy = Mathf.Sqrt(2 * g * (dif.z * Mathf.Tan(radAngle) - dif.y));
				//h = h0 + v*sin(angle)*t - 0.5*g*t^2, where t is time
				//h - h0 = dy = t * (v*sin(angle) - 0.5*g*t)
				//Si v*sin(angle)
				//t = d / (v*cos(angle)) = {v*sin(angle) + sqrt((v*sin(angle))^2 + 2*g*y0)} / g							Siendo y0 la altura inicial
				float v = Mathf.Sqrt((g * Mathf.Pow(dif.z, 2)) / ((dif.z * Mathf.Tan(radAngle) - dif.y) * 2 * Mathf.Pow(Mathf.Cos(radAngle), 2)));
				float vz = v * Mathf.Cos(radAngle);
				float vy = v * Mathf.Sin(radAngle);
				float totalTime = vy / g;

				return new Vector3(dif.x / (totalTime), vy, vz);
		}

		/// <summary>
		/// Devuelvo la velocidad inicial de la parábola que pasa por encima de cierta Y en cierto punto
		/// noTouch indica el punto que no hay que tocar
		/// </summary>
		/// <returns></returns>
		public static Vector3 OverYVelocityParabolaXZ(Vector3 pos1, Vector3 pos2, float speed, Vector3 noTouch, float time)
		{
				return OverYVelocityParabolaXZDeterministic(pos1, pos2, speed, noTouch);//AngleParabola(pos1, pos2, 30f, speed);
		}
		#endregion
		/// <summary>
		/// Tiempo relativo que se tarda en llegar desde startPos a endPos, teniendo en cuenta rapidez inicial, rapidez máxima y aceleración constante
		/// </summary>
		/// <param name="startPos"></param>
		/// <param name="endPos"></param>
		/// <param name="startSpeed"></param>
		/// <param name="maxSpeed"></param>
		/// <param name="acel"></param>
		/// <returns></returns>
		public static float ReachTime(Vector2 startPos, Vector2 endPos, float startSpeed, float maxSpeed, float acel)
		{
				float dist = Vector2.Distance(startPos, endPos);
				float dMax = (maxSpeed * maxSpeed - startSpeed * startSpeed) / (2 * acel);

				if (dist > dMax)
				{
						float midVel = (maxSpeed + startSpeed) / 2;
						float linearTime = (dist - dMax) / maxSpeed;
						return (dMax / midVel) + linearTime;
				}
				else
				{
						float acelTime = (Mathf.Sqrt(2 * dist * acel + (startSpeed * startSpeed)) - startSpeed) / acel;
						return acelTime;
				}
		}
}
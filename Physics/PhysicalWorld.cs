using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace Physics
{
	public class PhysicalWorld
	{
		public Point acceleration;
		public List<PhysicalBody> bodies;

		public PhysicalWorld()
		{
			acceleration = new Point(0, 9.8);
			bodies = new List<PhysicalBody>();
		}

		public void AddBody(PhysicalBody body)
		{
			body.Acceleration = acceleration;
			bodies.Add(body);
		}

		public virtual bool IsBodiesCollided(PhysicalBody a, PhysicalBody b)
		{
			return a.Shape.IntersectWithPolygon(b.Shape).Count > 2;
		}

		public bool CanMove(PhysicalBody a, Point direction)
		{
			if (direction.Length.IsEqual(0))
				return true;
			direction *= 1.1;
			var shape = a.Shape.Move(direction);
			foreach (var body in bodies)
			{
				if (body == a) continue;
				double currentIntersection = shape.IntersectWithPolygon(body.Shape).GetArea();
				double oldIntersection = a.Shape.IntersectWithPolygon(body.Shape).GetArea();
				if (currentIntersection.IsGreater(1) && (oldIntersection / currentIntersection).IsLess(1))
				{
					if (body.IsStatic || !CanMove(body, direction))
						return false;
				}
			}
			return true;
		}

		public Point GetVectorForResolveCollision(PhysicalBody a, PhysicalBody b)
		{
			var intersection = a.Shape.IntersectWithPolygon(b.Shape);
			if (intersection.Count < 3)
				return new Point(0, 0);
			var A = intersection[0];
			var B = intersection[1];
			var C = intersection[intersection.Count - 1];
			if (!b.Shape.IsPointOnBorder(A))
				return A.DistanceToPoint(B) < A.DistanceToPoint(C) ? B - A : C - A;
			return A.DistanceToPoint(B) < A.DistanceToPoint(C) ? A - B : A - C;
		}

		public virtual void ResolveCollision(PhysicalBody a, PhysicalBody b, double dt)
		{
			Point delta = GetVectorForResolveCollision(a, b);
			if (Equals(delta, new Point(0, 0)))
				return;
			if (!a.IsStatic && CanMove(a, delta))
				a.Shape = a.Shape.Move(delta);
			else if (!b.IsStatic && CanMove(b, -delta))
				b.Shape = b.Shape.Move(-delta);

			var e1 = delta.SetLength(1);
			var e2 = e1.RotateAroundOrigin(Math.PI / 2);

			var centerOfMassVelocity = (a.Velocity * a.Mass + b.Velocity * b.Mass) / (a.Mass + b.Mass);

			a.Velocity = centerOfMassVelocity.DotProductWith(e1) * e1 + a.Velocity.DotProductWith(e2) * e2 / (a.FrictionCoefficient * b.FrictionCoefficient * dt + 1);
			b.Velocity = centerOfMassVelocity.DotProductWith(e1) * e1 + b.Velocity.DotProductWith(e2) * e2 / (a.FrictionCoefficient * b.FrictionCoefficient * dt + 1);
		}

		public void OnTick(double dt)
		{
			foreach (var body in bodies)
				body.OnTick(dt);
			for (int i = 0; i < bodies.Count; i++)
				for (int s = i + 1; s < bodies.Count; s++)
				{
					if (IsBodiesCollided(bodies[i], bodies[s]))
						ResolveCollision(bodies[i], bodies[s], dt);
				}
		}
	}
}

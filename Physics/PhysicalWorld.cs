using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
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

		public void RemoveBody(PhysicalBody body)
		{
			bodies.Remove(body);
		}

		public virtual bool IsBodiesCollided(PhysicalBody a, PhysicalBody b)
		{
			return a.Shape.IntersectWithPolygon(b.Shape).Count > 2;
		}

		public Point GetVectorForResolveCollision(PhysicalBody a, PhysicalBody b)
		{
			var intersection = a.Shape.IntersectWithPolygon(b.Shape);
			if (intersection.Count < 3)
				return new Point(0, 0);
			var A = intersection[0];
			var B = intersection[1];
			var C = intersection[intersection.Count - 1];
			var bestDirection = (A.DistanceToPoint(B) < A.DistanceToPoint(C) ? B : C) - A;
			if (a.Shape.GetBoundingBox()[0].DotProductWith(bestDirection).IsGreater(
				b.Shape.GetBoundingBox()[0].DotProductWith(bestDirection)))
				return bestDirection;
			return -bestDirection;
		}

		public virtual void CorrectPositions(PhysicalBody a, PhysicalBody b, Point direction)
		{
			const double percentage = 1;
			const double slop = 0.1;
			double penetration = direction.Length;
			Point correction = Math.Max(penetration - slop, 0.0) * percentage / 
				(1 / a.Mass + 1 / b.Mass) * direction.SetLength(1);
			a.Shape = a.Shape.Move(correction * (1 / a.Mass));
			b.Shape = b.Shape.Move(-correction * (1 / b.Mass));
		}

		public virtual void ResolveCollision(PhysicalBody a, PhysicalBody b, double dt)
		{
			var delta = GetVectorForResolveCollision(a, b);
			var relativeVelocity = a.Velocity - b.Velocity;
			CorrectPositions(a, b, delta);
			if (delta.Length.IsEqual(0) || relativeVelocity.DotProductWith(delta).IsGreater(0))
				return;
			var normal = delta.SetLength(1);

			double e = Math.Min(a.Material.elasticity, b.Material.elasticity);
			double impulse = relativeVelocity.DotProductWith(normal) * (1 + e) / (1 / a.Mass + 1 / b.Mass);
			a.Velocity -= impulse * normal / a.Mass;
			b.Velocity += impulse * normal / b.Mass;

			ApplyFrictionForces(a, b, impulse * normal);
		}

		public bool IsBodyOnGround(PhysicalBody a, Point groundDirection)
		{
			groundDirection = groundDirection.SetLength(1);
			var shape = a.Shape.Move(groundDirection);
			foreach (var body in bodies)
			{
				if (body == a || !IsBodiesCollided(a, body)) continue;
				var delta = GetVectorForResolveCollision(a, body);
				if (delta.DotProductWith(groundDirection).IsLess(0))
					return true;
			}
			return false;
		}

		public virtual void ApplyFrictionForces(PhysicalBody a, PhysicalBody b, Point normalImpulse)
		{
			var relativeVelocity = a.Velocity - b.Velocity;
			var ort = normalImpulse.RotateAroundOrigin(Math.PI / 2);
			var tanget = (relativeVelocity.DotProductWith(ort) * ort);
			if (tanget.Length.IsEqual(0))
				return;
			tanget = tanget.SetLength(1);
			double impulse = -relativeVelocity.DotProductWith(tanget) / (1 / a.Mass + 1 / b.Mass);
			double mu = Math.Sqrt(a.Material.frictionCoefficient.Sqr() + b.Material.frictionCoefficient.Sqr());
			Point frictionImpulse;
			if (Math.Abs(impulse).IsLessOrEqual(normalImpulse.Length * mu))
				frictionImpulse = impulse * tanget;
			else
				frictionImpulse = -(normalImpulse.Length * mu) * tanget;
			a.Velocity += frictionImpulse / a.Mass;
			b.Velocity -= frictionImpulse / b.Mass;
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

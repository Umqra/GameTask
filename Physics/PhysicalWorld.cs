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
			acceleration = new Point(0, -9.8);
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

		public static Point GetContactPoint(PhysicalBody a, PhysicalBody b)
		{
			Point deepPointA = null;
			double distanceA = 0;
			foreach (var point in a.Shape)
			{
				double curDistance = b.Shape.DistanceToPoint(point);
				if (b.Shape.ContainsPoint(point) && curDistance > distanceA)
				{
					deepPointA = point;
					distanceA = curDistance;
				}
			}
			Point deepPointB = null;
			double distanceB = 0;
			foreach (var point in b.Shape)
			{
				double curDistance = a.Shape.DistanceToPoint(point);
				if (a.Shape.ContainsPoint(point) && curDistance > distanceB)
				{
					deepPointB = point;
					distanceB = curDistance;
				}
			}
			return distanceA > distanceB ? deepPointA : deepPointB;
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
				if (shape.IntersectWithPolygon(body.Shape).GetArea().IsGreater(1))
				{
					if (body.IsStatic || !CanMove(body, direction))
						return false;
				}
			}
			return true;
		}

		public virtual void ResolveCollision(PhysicalBody a, PhysicalBody b, double dt)
		{
			var contact = GetContactPoint(a, b);
			var nearEdgeA = a.Shape.NeareseEdgeFromPoint(contact);
			var nearEdgeB = b.Shape.NeareseEdgeFromPoint(contact);

			Point normal = null;
			if (nearEdgeA.ContainsPoint(contact))
			{
				normal = contact.ProjectToLine(nearEdgeB.BaseLine);
				if (!a.IsStatic && CanMove(a, normal - contact))
					a.Shape = a.Shape.Move(normal - contact);
				else
					b.Shape = b.Shape.Move(contact - normal);
			}
			else
			{
				normal = contact.ProjectToLine(nearEdgeA.BaseLine);
				if (!b.IsStatic && CanMove(b, normal - contact))
					b.Shape = b.Shape.Move(normal - contact);
				else
					a.Shape = a.Shape.Move(contact - normal);
			}
			if (Equals(normal, contact))
				return;
			Point e1 = (normal - contact).SetLength(1);
			Point e2 = e1.RotateAroundOrigin(Math.PI / 2);

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

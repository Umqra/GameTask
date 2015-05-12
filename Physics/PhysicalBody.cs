using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace Physics
{
    public class PhysicalBody
    {
	    public double Mass { get; set; }
	    public Point Acceleration { get; set; }
		public Point Velocity { get; set; }
		public double FrictionCoefficient { get; set; }
		public bool IsStatic { get; set; }

		public ConvexPolygon Shape { get; set; }
		public Point CenterOfMass { get; set; }
	    
	    protected PhysicalBody(double mass, Point velocity, double friction, bool isStatic,
			ConvexPolygon shape, Point centerOfMass)
	    {
		    this.Mass = mass;
		    this.Acceleration = new Point(0, 0);
		    this.Velocity = velocity;
		    this.FrictionCoefficient = friction;
		    this.IsStatic = isStatic;
		    this.Shape = shape;
		    this.CenterOfMass = centerOfMass;
	    }
		
		protected PhysicalBody(double mass, Point velocity, double friction, bool isStatic,
			ConvexPolygon shape) : this(mass, velocity, friction, isStatic, shape, shape.GetCenterOfMass())
		{
		}

	    protected PhysicalBody()
	    {
		    
	    }

	    public double KinetickEnergy
	    {
		    get { return Mass * Velocity.Length * Velocity.Length / 2; }
	    }

	    public virtual void ApplyForce(Point point, Point force, double dt)
	    {
		    Velocity += force * dt;
	    }

	    public virtual void OnTick(double dt)
	    {
		    if (IsStatic) return;
		    Velocity += Acceleration * dt;
		    CenterOfMass += Velocity * dt;
		    Shape = Shape.Move(Velocity * dt);
	    }
    }
}

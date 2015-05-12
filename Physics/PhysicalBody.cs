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
	    public virtual Point Acceleration { get; set; }

	    private Point velocity;
	    public Point Velocity
	    {
		    get { return velocity; }
		    set { if (!IsStatic) velocity = value; }
	    }

	    public double FrictionCoefficient { get; set; }
		public bool IsStatic { get; set; }

		public ConvexPolygon Shape { get; set; }

	    public Point CenterOfMass
	    {
		    get { return Shape.GetCenterOfMass(); }
	    }
	    
	    protected PhysicalBody(double mass, Point velocity, double friction, bool isStatic,
			ConvexPolygon shape)
	    {
		    this.Mass = mass;
		    this.Acceleration = new Point(0, 0);
		    this.Velocity = velocity;
		    this.FrictionCoefficient = friction;
		    this.IsStatic = isStatic;
		    this.Shape = shape;
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
		    Shape = Shape.Move(Velocity * dt);
	    }
    }
}

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

	    public void RecalcMass()
	    {
		    if (IsStatic)
			    Mass = double.PositiveInfinity;
		    else if (shape == null || material == null)
			    Mass = 0;
			else
				Mass = Shape.GetArea() * Material.density;
	    }

	    public virtual Point Acceleration { get; set; }

	    private Point velocity;
	    public Point Velocity
	    {
		    get { return velocity; }
		    set { if (!IsStatic) velocity = value; }
	    }

	    public double FrictionCoefficient { get; set; }
		public bool IsStatic { get; set; }

	    private ConvexPolygon shape;
	    public ConvexPolygon Shape
	    {
		    get { return shape; }
		    set
		    {
			    shape = value;
			    RecalcMass();
		    }
	    }

	    private Material material;

	    public Material Material
	    {
		    get { return material; }
		    set
		    {
			    material = value;
			    RecalcMass();
		    }
	    }

	    public Point CenterOfMass
	    {
		    get { return Shape.GetCenterOfMass(); }
	    }
	    
	    protected PhysicalBody(Material material, Point velocity, double friction, bool isStatic,
			ConvexPolygon shape)
	    {
		    if (shape == null)
			    Mass = 0;
			else if (isStatic)
				Mass = double.PositiveInfinity;
			else
				Mass = shape.GetArea() * material.density;

		    Material = material;
			Acceleration = new Point(0, 0);
		    Velocity = velocity;
		    FrictionCoefficient = friction;
		    IsStatic = isStatic;
		    Shape = shape;
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

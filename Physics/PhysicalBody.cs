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
	    internal double mass;
	    internal Point centerOfMass;
	    internal Point acceleration;
	    internal Point velocity;
	    internal bool isStatic;

	    public ConvexPolygon shape;

	    public PhysicalBody(ConvexPolygon shape, bool isStatic, double mass)
	    {
		    this.shape = shape;
		    this.isStatic = isStatic;
		    this.mass = mass;
		    this.centerOfMass = shape.GetCenterOfMass();
		    this.velocity = new Point(10, 0);
		    this.acceleration = new Point(0, 9.8);
	    }

	    public void Move(double deltaTime)
	    {
		    if (isStatic) return;
		    velocity += acceleration * deltaTime;
		    centerOfMass += velocity * deltaTime;
		    shape = shape.Move(velocity * deltaTime);
	    }

	    
    }
}

using Godot;

namespace Legion.addons.MathSpring;

public static class MathSpring
{
    /*
  x     - value             (input/output)
  v     - velocity          (input/output)
  xt    - target value      (input)
  zeta  - damping ratio     (input)
  omega - angular frequency (input)
  h     - time step         (input)
*/
    public static void Spring(ref float value,ref float velocity, float targetValue, float dampingRatio, float angularFrequency, float timeStep)
    {
        float f = 1.0f + 2.0f * timeStep * dampingRatio * angularFrequency;
        float oo = angularFrequency     * angularFrequency;
        float hoo = timeStep        * oo;
        float hhoo = timeStep       * hoo;
        float detInv = 1.0f  / (f + hhoo);
        float detX = f * value + timeStep   * velocity + hhoo * targetValue;
        float detV = velocity     + hoo * (targetValue - value);
        value = detX * detInv;
        velocity = detV * detInv;
    }
    
    public static void Spring(ref Vector2 value,ref Vector2 velocity, Vector2 targetValue, float dampingRatio, float angularFrequency, float timeStep)
    {
	    Spring(ref value.X, ref velocity.X, targetValue.X, dampingRatio, angularFrequency, timeStep);
	    Spring(ref value.Y, ref velocity.Y, targetValue.Y, dampingRatio, angularFrequency, timeStep);
    }
}

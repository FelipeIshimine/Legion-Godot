namespace Legion.addons.MathSpring;

public class MathSpring
{
    /*
  x     - value             (input/output)
  v     - velocity          (input/output)
  xt    - target value      (input)
  zeta  - damping ratio     (input)
  omega - angular frequency (input)
  h     - time step         (input)
*/
    void Spring
    (
        float x, float v, float xt, 
    float zeta, float omega, float h
    )
    {
        float f = 1.0f + 2.0f * h * zeta * omega;
        float oo = omega     * omega;
        float hoo = h        * oo;
        float hhoo = h       * hoo;
        float detInv = 1.0f  / (f + hhoo);
        float detX = f * x + h   * v + hhoo * xt;
        float detV = v     + hoo * (xt - x);
        x = detX * detInv;
        v = detV * detInv;
    }
}

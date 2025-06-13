namespace UnityEngine;

public struct Vector3(float x, float y, float z)
{
	public float x = x;
	public float y = y;
	public float z = z;

	public override string ToString()
	{
		return $"({x}, {y}, {z})";
	}
}

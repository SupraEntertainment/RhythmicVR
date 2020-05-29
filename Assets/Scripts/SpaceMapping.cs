using UnityEngine;

[System.Serializable]
public class SpaceMapping {

	public Vector3 position;
	public Vector3 scale;
	[Header("[Hover over toggle for tooltip] Uses this transforms position and scale for the note space. This allows for notes to be e.g. only on the floor despite beatmap")]
	[Tooltip("Uses this transforms position and scale for the note space. This allows for notes to be e.g. only on the floor despite beatmap")]
	public bool useTransform;
	public Transform origin;
	public Coordinate x = Coordinate.X;
	public Coordinate y = Coordinate.Y;
	public Coordinate z = Coordinate.Z;

	private Vector3 ApplyScale(Vector3 input) {
		if (useTransform) {
			var scale1 = origin.localScale;
			input.x *= scale1.x;
			input.y *= scale1.y;
			input.z *= scale1.z;
			
		} else {
			input.x *= scale.x;
			input.y *= scale.y;
			input.z *= scale.z;
			
		}
		
		return input;
	}
	private Vector3 ApplyPosition(Vector3 input) {
		if (useTransform) {
			var position1 = origin.position;
			input.x += position1.x;
			input.y += position1.y;
			input.z += position1.z;
			
		} else {
			input.x += position.x;
			input.y += position.y;
			input.z += position.z;
			
		}
		
		return input;
	}

	public Vector3 MapCoords(Vector3 input) {
		Vector3 output = new Vector3();

		input = ApplyScale(input);
		
		if (x == Coordinate.X) {
			output.x = input.x;
		} else if (x == Coordinate.Y) {
			output.x = input.y;
		} else {
			output.x = input.z;
		}
		
		if (y == Coordinate.X) {
			output.y = input.x;
		} else if (y == Coordinate.Y) {
			output.y = input.y;
		} else {
			output.y = input.z;
		}
		
		if (z == Coordinate.X) {
			output.z = input.x;
		} else if (z == Coordinate.Y) {
			output.z = input.y;
		} else {
			output.z = input.z;
		}

		output = ApplyPosition(output);

		return output;
	}
}
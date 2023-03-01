/** TeleportCommand.cs
*
*	Created by LIAM WOFFORD of CUBEROOT SOFTWARE, LLC.
*
*	Free to use or modify, with or without creditation,
*	under the Creative Commons 0 License.
*/

#region Includes

using UnityEngine;

#endregion

namespace Cuberoot.Dinocular
{
	/// <summary>
	/// __TODO_ANNOTATE__
	///</summary>

	public sealed class TransformCommands : MonoBehaviour
	{
		#region Methods

		public void SetPosition(string[] args)
		{
			var v = args.ToVector3();

			var rigidbody = GetComponent<Rigidbody>();
			if (rigidbody)
				rigidbody.MovePosition(v);
			else
				transform.position = v;
		}

		public void SetRotationEuler(string[] args)
		{
			var rotation = Quaternion.Euler(args.ToVector3());

			var rigidbody = GetComponent<Rigidbody>();
			if (rigidbody)
				rigidbody.MoveRotation(rotation);
			else
				transform.rotation = rotation;
		}

		public void SetVelocity(string[] args)
		{
			var v = args.ToVector3();

			var rigidbody = GetComponent<Rigidbody>();

			rigidbody.velocity = v;
		}

		#endregion
	}
}

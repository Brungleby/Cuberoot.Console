/** TeleportCommand.cs
*
*	Created by LIAM WOFFORD, USA-TX, for the Public Domain.
*
*	Repo: https://github.com/Brungleby/Cuberoot
*	Kofi: https://ko-fi.com/brungleby
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

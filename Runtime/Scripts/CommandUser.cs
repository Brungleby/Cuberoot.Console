
/** CommandUser.cs
*
*	Created by LIAM WOFFORD of CUBEROOT SOFTWARE, LLC.
*
*	Free to use or modify, with or without creditation,
*	under the Creative Commons 0 License.
*/

#region Includes

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#endregion

namespace Cuberoot.Dinocular
{
	/// <summary>
	/// __TODO_ANNOTATE__
	///</summary>

	public abstract class CommandUser : MonoBehaviour
	{
		#region Methods

		public abstract void Execute(string[] args);

		#endregion
	}
}

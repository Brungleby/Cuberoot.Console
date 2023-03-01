
/** CommandRegistry.cs
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

	[CreateAssetMenu(fileName = "New Command Registry", menuName = "Cuberoot/Dinocular/Command Registry")]

	public sealed class CommandRegistry : ScriptableObject
	{
		#region Construction



		#endregion
		#region Fields

		[SerializeField]
		private CommandRegistry[] _Inherited;

		[SerializeField]
		private Command[] _Commands;

		#endregion
		#region Members

		private DinocularCommandLine _Context;
		public DinocularCommandLine Context => _Context;

		private Command[] _AllCommands;
		public Command[] Commands => _AllCommands;

		#endregion
		#region Properties



		#endregion
		#region Methods

		void OnValidate()
		{
			var result = new List<Command>();
			result.AddAll(_Commands);

			foreach (var i in _Inherited)
				result.AddAll(_Commands);

			_AllCommands = result.ToArray();
		}

		#endregion
	}
}

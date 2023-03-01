
/** DinocularCommand.cs
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
using UnityEngine.Events;

#endregion

namespace Cuberoot.Dinocular
{
	/// <summary>
	/// __TODO_ANNOTATE__
	///</summary>

	[System.Serializable]

	public sealed class Command
	{
		#region (inner) Parameter

		[System.Serializable]

		public sealed class Parameter
		{
			#region Exceptions

			[System.Serializable]

			public class Exception : System.Exception
			{
				public Exception() { }
				public Exception(string message) : base(message) { }
				public Exception(string message, System.Exception inner) : base(message, inner) { }
				protected Exception(
					System.Runtime.Serialization.SerializationInfo info,
					System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
			}

			[System.Serializable]

			public class MultiException : Exception
			{
				public MultiException(Exception[] _exceptions)
				{
					Exceptions = _exceptions;
				}

				public Exception[] Exceptions;
			}

			#endregion
			#region ParamType

			public enum ParamType
			{
				Bool,
				Integer,
				Float,
				String
			}

			#endregion
			#region Fields

			public string Name;
			public ParamType Type;

			#endregion
			#region Methods
			#region ToString

			public override string ToString()
			{
				return Name;
			}

			#endregion
			#region ValidateInput

			public void ValidateInput(string arg)
			{
				switch (Type)
				{
					case ParamType.Bool:
						if (arg != "true" && arg != "false")
							throw new Exception($"{this} expects a boolean value, {arg} must be either \'true\' or \'false\'.");
						break;
					case ParamType.Integer:
						try
						{
							int.Parse(arg);
						}
						catch
						{
							throw new Exception($"{this} expects an integer value, {arg} is not an integer.");
						}
						break;
					case ParamType.Float:
						try
						{
							float.Parse(arg);
						}
						catch
						{
							throw new Exception($"{this} expects a float value, {arg} is not a number.");
						}

						break;
					case ParamType.String:
						// if (EnumNames.Length > 0)
						// {
						// 	// check if the arg matches any enums. If not, throw exception.
						// }
						break;
				}
			}
			#endregion
			#endregion
		}

		#endregion
		#region (inner) Exceptions

		[System.Serializable]
		public class Exception : System.Exception
		{
			public Exception() { }
			public Exception(string message) : base(message) { }
			public Exception(string message, System.Exception inner) : base(message, inner) { }
			protected Exception(
				System.Runtime.Serialization.SerializationInfo info,
				System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}

		#endregion

		#region Fields

		[SerializeField]
		private string _Keyword;
		public string Keyword => _Keyword;

		[SerializeField]
		[TextArea(1, 3)]
		private string _HelpInfo;
		public string HelpInfo => _HelpInfo;

		[SerializeField]
		[TextArea(1, 3)]
		private string _ResponseInfo;
		public string ResponseInfo => _ResponseInfo;

		[SerializeField]
		private Parameter[] _ExpectedParameters;
		public Parameter[] ExpectedParameters => _ExpectedParameters;

		public UnityEvent<string[]> Event;

		#endregion
		#region Members

		private string _UsageInfo = string.Empty;
		public string UsageInfo
		{
			get
			{
				if (_UsageInfo == string.Empty)
				{
					_UsageInfo = $"{Keyword} ";

					foreach (var i in ExpectedParameters)
					{
						_UsageInfo += $"{i.ToString()} ";
					}

					_UsageInfo = _UsageInfo.Remove(_UsageInfo.Length - 1);
				}

				return _UsageInfo;
			}
		}

		#endregion
		#region Properties



		#endregion
		#region Methods

		public string Execute(string[] args)
		{
			ValidateArguments(args); // throws Exceptions

			Event.Invoke(args);

			return CreateResponse(args);
		}

		private void ValidateArguments(string[] args)
		{
			if (args.Length < ExpectedParameters.Length)
				throw new Exception("Not enough arguments.");
			if (args.Length > ExpectedParameters.Length)
				throw new Exception("Too many arguments.");

			var exceptions = new List<Parameter.Exception>();
			for (var i = 0; i < ExpectedParameters.Length; i++)
			{
				try
				{
					ExpectedParameters[i].ValidateInput(args[i]);
				}
				catch (Parameter.Exception e)
				{
					exceptions.Add(e);
				}
			}

			if (exceptions.Count > 0)
				throw new Parameter.MultiException(exceptions.ToArray());
		}

		public string CreateResponse(string[] args)
		{
			string result = string.Empty;
			var chars = ResponseInfo.ToCharArray();

			for (var i = 0; i < ResponseInfo.Length; i++)
			{
				if (i < ResponseInfo.Length - 3 && chars[i] == '$' && chars[i + 1] == '{')
				{
					string argString = string.Empty;
					for (var j = i + 2; chars[j] != '}'; j++)
					{
						if (!char.IsNumber(chars[j]))
							break;

						argString += chars[j];
					}

					int arg;
					try
					{
						arg = int.Parse(argString);
					}
					catch { continue; }

					result += args[arg];
					i += argString.Length + 2;
				}
				else
				{
					result += chars[i];
				}
			}

			return result;
		}

		#endregion
	}

	public static class CommandExtensions
	{
		public static Vector2 ToVector2(this string[] args, int i = 0)
		{
			return new Vector2
				(
					float.Parse(args[i + 0]),
					float.Parse(args[i + 1])
				);
		}
		public static Vector3 ToVector3(this string[] args, int i = 0)
		{
			return new Vector3
				(
					float.Parse(args[i + 0]),
					float.Parse(args[i + 1]),
					float.Parse(args[i + 2])
				);
		}
	}
}

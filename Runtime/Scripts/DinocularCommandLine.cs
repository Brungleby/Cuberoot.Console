
/** DinocularCommandLine.cs
*
*	Created by LIAM WOFFORD, USA-TX, for the Public Domain.
*
*	Repo: https://github.com/Brungleby/Cuberoot
*	Kofi: https://ko-fi.com/brungleby
*/

#region Includes

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using TMPro;

#endregion

namespace Cuberoot.Dinocular
{
	/// <summary>
	/// __TODO_ANNOTATE__
	///</summary>

	public sealed class DinocularCommandLine : MonoBehaviour
	{
		#region Fields

		public DinocularMessageLog MessageLog;
		public TMP_InputField PromptField;
		public TMP_Text Prediction;

		public Command[] Registry;
		public UnityEvent<string[]> OnExecute;

		#endregion
		#region Properties

		public string[] CommandKeys
		{
			get
			{
				var results = new List<string>();
				foreach (var i in Registry)
					results.Add(i.Keyword);

				return results.ToArray();
			}
		}

		#endregion
		#region Members

		private DinocularPanel _Panel;

		private List<string> _History = new List<string>();

		private int _historyIndex = 0;
		public int historyIndex
		{
			get => _historyIndex;
			set
			{
				if (_historyIndex == 0)
					_History[0] = PromptField.text;

				_historyIndex = System.Math.Clamp(value, 0, _History.Count - 1);


				if (value >= 0 && value < _History.Count)
					PromptField.text = _History[value];
			}
		}

		#endregion
		#region Methods

		#region OnValidate

		private void OnValidate()
		{
			_Panel = GetComponentInParent<DinocularPanel>();

			_History = new List<string>();
			_History.Add(string.Empty);

			try
			{
				_Panel.OnOpen.AddListener(() =>
				{
					PromptField.ActivateInputField();
				});
			}
			catch { }

			PromptField.onSubmit.AddListener(OnSubmit);
			PromptField.onValueChanged.AddListener(OnValueChanged);
		}

		#endregion



		#region OnSubmit

		private void OnSubmit(string input)
		{
			_History.Insert(1, input);
			_History[0] = string.Empty;
			_historyIndex = 0;

			PromptField.text = string.Empty;
			PromptField.ActivateInputField();

			var tokens = input.ParseTokens();

			Command command;
			try
			{
				command = FindCommand(tokens[0]);
			}
			catch (Command.Exception e)
			{
				MessageLog.Print(e.Message, MessageType.Error);
				return;
			}

			var args = tokens.Skip(1).ToArray();

			try
			{
				string response = command.Execute(args);
				MessageLog.Print(response, MessageType.Response);
			}
			catch (Command.Exception e)
			{
				MessageLog.Print(e.Message, MessageType.Error);
			}
			catch (Command.Parameter.MultiException e)
			{
				foreach (var i in e.Exceptions)
					MessageLog.Print(i.Message, MessageType.Error);
			}
		}
		#endregion
		#region OnValueChanged

		private void OnValueChanged(string input) =>
			Prediction.text = GetPredictionMessage(input);

		private string GetPredictionMessage(string input)
		{
			var tokens = input.ParseTokens();

			if (tokens.Length == 0)
				return string.Empty;

			else if (tokens.Length == 1)
			{
				try
				{
					return StringExtensions.CreateWhitespace(input.Length - tokens[0].Length) + PredictCommand(tokens[0]).SkipWhitespace(tokens[0].Length);
				}
				catch (Command.Exception e)
				{
					return StringExtensions.CreateWhitespace(input.Length) + " * " + e.Message;
				}
			}
			else
			{
				Command command;
				try
				{
					command = FindCommand(tokens[0]);
				}
				catch (Command.Exception e)
				{
					return StringExtensions.CreateWhitespace(input.Length) + " * " + e.Message;
				}

				var argIndex = tokens.Length - 1;
				if (argIndex <= command.ExpectedParameters.Length)
				{
					var param = command.ExpectedParameters[argIndex - 1];
					string paramResult = $" : ({param.Type}){param.Name}";
					try
					{
						param.ValidateInput(tokens[argIndex]);
					}
					catch (Command.Parameter.Exception e)
					{
						paramResult += $" * {e.Message}";
					}

					return StringExtensions.CreateWhitespace(input.Length) + paramResult;
				}
			}

			return string.Empty;
		}

		#endregion
		#region FindCommand

		private Command FindCommand(string token)
		{
			foreach (var i in Registry)
				if (token == i.Keyword)
					return i;

			throw new Command.Exception($"Command \"{token}\" not found.");
		}

		#endregion
		#region PredictCommand

		private string PredictCommand(string query)
		{
			var queryChars = query.ToCharArray();

			var whitelist = new List<string>();
			whitelist.AddAll(CommandKeys);

			for (var i = 0; i < query.Length; i++)
			{
				var blacklist = new HashSet<string>();
				foreach (var j in whitelist)
				{
					var jChars = j.ToCharArray();
					if (queryChars[i] != jChars[i])
						blacklist.Add(j);
				}
				whitelist.RemoveAll(blacklist);

				if (whitelist.Count == 0)
					throw new Command.Exception("No predictions found.");
			}

			return whitelist[0];
		}

		#endregion

		private void SelectHistoryUp() =>
			historyIndex++;

		private void SelectHistoryDown() =>
			historyIndex--;

		private void Autocomplete()
		{
			PromptField.text += string.Concat(Prediction.text.Skip(PromptField.text.Length));

			PromptField.MoveToEndOfLine(false, false);
		}

		#region OnInputUp

		public void OnInputUp(InputContext context)
		{
			if (context.started)
				SelectHistoryUp();
		}

		#endregion
		#region OnInputDown

		public void OnInputDown(InputContext context)
		{
			if (context.started)
				SelectHistoryDown();
		}

		#endregion
		#region OnInputAutocomplete

		public void OnInputAutocomplete(InputContext context)
		{
			if (context.started)
				Autocomplete();
		}

		#endregion

		#endregion
	}

	public static class StringExtensions
	{
		public static string[] ParseTokens(this string s)
		{
			var result = new List<string>();

			string[] quoteBlocks = s.Split('\"');
			for (var i = 0; i < quoteBlocks.Length; i++)
			{
				if (i % 2 == 0) // if this is not a quote block
				{
					string[] words = quoteBlocks[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
					foreach (var j in words)
						result.Add(j);
				}
				else
					result.Add(quoteBlocks[i]);
			}

			return result.ToArray();
		}

		public static string SkipWhitespace(this string s, int index)
		{
			var chars = s.ToCharArray();

			for (var i = 0; i < chars.Length && i < index; i++)
				chars[i] = ' ';

			return CreateWhitespace((index - chars.Length).Max()) + string.Concat(chars);
		}

		public static string CreateWhitespace(int index)
		{
			var whitespace = string.Empty;
			for (var i = 0; i < index; i++)
				whitespace += ' ';

			return whitespace;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public partial class RpcCoreExtensions {

	extension (String value) {

		#region ReplaceVariables
		//--------------------------------------------------------------------------------------------------------------
		// ReplaceVariables.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the <see cref="System.String" /> for "$MY-VARIABLE$" variables, and replaces the variable with the
		/// associated value.
		/// The variables are matched against this regular expression: [a-zA-Z_][a-zA-Z0-9_\-\d]*[a-zA-Z0-9_]
		/// The variable values are provided in a dictionary.
		/// Variables without a matching value, or a matching empty value, are not replaced.
		/// </summary>
		/// <param name="variableValues">The variable names (keys) and variable values.</param>
		/// <returns>The modified value.</returns>
		public String ReplaceVariables(Dictionary<String, String> variableValues) {
			return RpcCoreExtensions.ReplaceVariables(value, false, variableValues);
		} // ReplaceVariables

		/// <summary>
		/// Searches the <see cref="System.String" /> for "$MY-VARIABLE$" variables, and replaces the variable with the
		/// associated value.
		/// The variables are matched against this regular expression: [a-zA-Z_][a-zA-Z0-9_\-\d]*[a-zA-Z0-9_]
		/// The variable values are provided in a dictionary.
		/// </summary>
		/// <param name="allowEmptyValues">Whether variables without a matching value, or a matching empty value, should be replaced with am empty value.</param>
		/// <param name="variableValues">The variable names (keys) and variable values.</param>
		/// <returns>The modified value.</returns>
		public String ReplaceVariables(Boolean allowEmptyValues, Dictionary<String, String> variableValues) {
			return RpcCoreExtensions.ReplaceVariables(
				value,
				allowEmptyValues,
				(varName) => {
					if (variableValues.TryGetValue(varName, out String varValue) == true) {
						return varValue;
					} else {
						return null;
					}
				}
			);
		} // ReplaceVariables

		/// <summary>
		/// Searches the <see cref="System.String" /> for "$MY-VARIABLE$" variables, and replaces the variable with the
		/// associated value.
		/// The variables are matched against this regular expression: [a-zA-Z_][a-zA-Z0-9_\-\d]*[a-zA-Z0-9_]
		/// The variable values are provided in a <see cref="RpcScandinavia.Core.Shared.IVariableResolver"/> variable resolver.
		/// Variables without a matching value, or a matching empty value, are not replaced.
		/// </summary>
		/// <param name="variableResolver">The variable resolver.</param>
		/// <returns>The modified value.</returns>
		public String ReplaceVariables(IVariableResolver variableResolver) {
			return RpcCoreExtensions.ReplaceVariables(
				value,
				false,
				(varName) => variableResolver.GetVariableValue(varName));
		} // ReplaceVariables

		/// <summary>
		/// Searches the <see cref="System.String" /> for "$MY-VARIABLE$" variables, and replaces the variable with the
		/// associated value.
		/// The variables are matched against this regular expression: [a-zA-Z_][a-zA-Z0-9_\-\d]*[a-zA-Z0-9_]
		/// The variable values are provided in a <see cref="RpcScandinavia.Core.Shared.IVariableResolver"/> variable resolver.
		/// </summary>
		/// <param name="allowEmptyValues">Whether variables without a matching value, or a matching empty value, should be replaced with am empty value.</param>
		/// <param name="variableResolver">The variable resolver.</param>
		/// <returns>The modified value.</returns>
		public String ReplaceVariables(Boolean allowEmptyValues, IVariableResolver variableResolver) {
			return RpcCoreExtensions.ReplaceVariables(
				value,
				allowEmptyValues,
				(varName) => variableResolver.GetVariableValue(varName));
		} // ReplaceVariables

		/// <summary>
		/// Searches the <see cref="System.String" /> for "$MY-VARIABLE$" variables, and replaces the variable with the associated value.
		/// The variable values are retrieved using a function.
		/// The variables are matched against this regular expression: [a-zA-Z_][a-zA-Z0-9_\-\d]*[a-zA-Z0-9_]
		/// </summary>
		/// <param name="allowEmptyValues">Whether variables without a matching value, or a matching empty value, should be replaced with am empty value.</param>
		/// <param name="getVariableValue">Function that returns the variable value associated with the specified variable name, or null.</param>
		/// <returns>The modified value.</returns>
		public String ReplaceVariables(Boolean allowEmptyValues, Func<String, String> getVariableValue) {
			// Handle null and empty values.
			if (value.IsNullOrWhiteSpace() == true) {
				return value;
			}

			// Always try the search and replace one more time, when something is changed.
			// This allows a replacement to contain another variable, which is handled in the next loop.
			String namedRegEx = String.Format(@"(?<={0})([a-zA-Z_][a-zA-Z0-9_\-\d]*[a-zA-Z0-9_])({0})", Regex.Escape("$"));
			List<String> allMatches = new List<String>();
			Boolean matchAgain = true;
			Int32 matchMaxLoop = 50;
			while ((matchAgain == true) && (matchMaxLoop > 0)) {
				// Default is not to search and replace again.
				matchAgain = false;
				matchMaxLoop--;

				foreach (Match regExMatch in Regex.Matches(value, namedRegEx)) {
					// Get the name.
					String varName = regExMatch.Groups[1].Value;

					// Get the value.
					String varValue = getVariableValue(varName);

					// Replace the variable.
					if ((allowEmptyValues == true) || (varValue.IsNullOrEmpty() == false)) {
						value = Regex.Replace(value, Regex.Escape("$" + regExMatch.Value), varValue.NotNull(), RegexOptions.Multiline | RegexOptions.IgnoreCase);
					}

					// If this was a new match, then search and replace again.
					if (allMatches.Contains("$" + regExMatch.Value) == false) {
						matchAgain = true;
					}

					// Register this match.
					if (allMatches.Contains("$" + regExMatch.Value) == false) {
						allMatches.Add("$" + regExMatch.Value);
					}
				}
			}

			// Return the modified value.
			return value;
		} // ReplaceVariables
		#endregion

	}

} // RpcCoreExtensions
#endregion

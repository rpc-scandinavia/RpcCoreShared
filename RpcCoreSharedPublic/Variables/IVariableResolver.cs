using System;
using System.Collections.Generic;
namespace RpcScandinavia.Core.Shared;

#region IVariableResolver
//----------------------------------------------------------------------------------------------------------------------
// IVariableResolver.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Provides variable names and associated variable values.
/// </summary>
public interface IVariableResolver {

	/// <summary>
	/// Gets the variable names.
	/// </summary>
	public IEnumerable<String> GetVariableNames();

	/// <summary>
	/// Gets the variable value associated with the specified variable name.
	/// </summary>
	/// <param name="variableName">The variable name without prefix or suffix.</param>
	/// <param name="defaultValue">The default value returned, when no value is associated with the variable name.</param>
	/// <returns>The variable value or the default value.</returns>
	public String GetVariableValue(String variableName, String defaultValue = null);

} // IVariableResolver
#endregion

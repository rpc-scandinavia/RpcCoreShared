using System;
namespace RpcScandinavia.Core.Shared;

#region RpcDisplayException
//----------------------------------------------------------------------------------------------------------------------
// RpcDisplayException.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This exception adds a integer number and a display message to the base exception.
/// </summary>
public class RpcDisplayException : Exception {
	private readonly Int32 id;
	private readonly String displayMessage;
	private readonly Object[] formatArgs;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Instantiates a new display exception.
	/// </summary>
	/// <param name="id">The exception identifier.</param>
	/// <param name="message">The display message and the exception message.</param>
	public RpcDisplayException(Int32 id, String message) : base(message) {
		this.id = id;
		this.displayMessage = message.NotNull();
		this.formatArgs = new Object[0];
	} // RpcDisplayException

	/// <summary>
	/// Instantiates a new display exception.
	/// </summary>
	/// <param name="id">The exception identifier.</param>
	/// <param name="displayMessage">The display message.</param>
	/// <param name="exceptionMessage">The exception message.</param>
	public RpcDisplayException(Int32 id, String displayMessage, String exceptionMessage, params Object[] formatArgs) : base(exceptionMessage) {
		this.id = id;
		this.displayMessage = displayMessage.NotNull();
		this.formatArgs = formatArgs;
	} // RpcDisplayException

	/// <summary>
	/// Instantiates a new display exception with the specified id and message.
	/// </summary>
	/// <param name="id">The exception identifier.</param>
	/// <param name="exception">The exception with the exception message.</param>
	public RpcDisplayException(Int32 id, Exception exception) : base(exception.Message) {
		this.id = id;
		this.displayMessage = String.Empty;
		this.formatArgs = new Object[0];
	} // RpcDisplayException
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	// Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the exception identifier.
	/// </summary>
	public Int32 Id {
		get {
			return this.id;
		}
	} // Id

	/// <summary>
	/// Gets the formatted display message.
	/// </summary>
	public String DisplayMessage {
		get {
			if ((this.formatArgs != null) && (this.formatArgs.Length > 0)) {
				return String.Format(this.displayMessage, this.formatArgs);
			} else {
				return this.displayMessage;
			}
		}
	} // DisplayMessage

	/// <summary>
	/// Gets the unformatted display message.
	/// </summary>
	public String DisplayMessageUnformatted {
		get {
			return this.displayMessage;
		}
	} // DisplayMessageUnformatted

	/// <summary>
	/// Gets the display message format arguments.
	/// </summary>
	public Object[] DisplayMessageFormatArguments {
		get {
			return this.formatArgs;
		}
	} // DisplayMessageFormatArguments
	#endregion

	#region Methods
	//------------------------------------------------------------------------------------------------------------------
	// Methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the exception as a string.
	/// This includes the formatted display message followed by a newline followed by the ToString from the base exception.
	/// </summary>
	public override String ToString() {
		return $"{this.id}: {this.displayMessage}{Environment.NewLine}{base.ToString()}";
	} // ToString
	#endregion

} // RpcDisplayException
#endregion

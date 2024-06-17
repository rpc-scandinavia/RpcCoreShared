using System;
using System.Collections.Generic;
namespace RpcScandinavia.Core.Shared;

#region RpcGridList event handlers
//----------------------------------------------------------------------------------------------------------------------
//	RpcGridList event handlers.
//----------------------------------------------------------------------------------------------------------------------
public delegate void RpcGridListEventHandler<T>(RpcGridList<T> sender);
public delegate void RpcGridListIndexEventHandler<T>(RpcGridList<T> sender, Int32 index);
public delegate void RpcGridListValueEventHandler<T>(RpcGridList<T> sender, T oldValue, T newValue, Int32 rowIndex, Int32 columnIndex);
#endregion

#region RpcGridList
//----------------------------------------------------------------------------------------------------------------------
//	RpcGridList.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Defines a two dimensional generic list.
/// </summary>
/// <typeparam name="T">The type of the list.</typeparam>
public class RpcGridList<T> {
	// The items are stored in a one dimensional list, and the index of the row/column item is calculated.
	// The items are stored LEFT to RIGHT, like you read this text.
	// Example (first the list, then the list visualized as the grid):
	//
	//		R0C0	R0C1	R0C2	R0C3	...	R0Cn	R1C0	R1C1	R1C2	R1C3	...	R1Cn	...	...	...	...	...	...	RnC0	RnC1	RnC2	RnC3	...	RnCn
	//
	//
	//		R0C0	R0C1	R0C2	R0C3	...	R0Cn
	//		R1C0	R1C1	R1C2	R1C3	...	R1Cn
	//		...	...	...	...	...	...
	//		RnC0	RnC1	RnC2	RnC3	...	RnCn
	//
	private event RpcGridListIndexEventHandler<T> eventRowAdded = null;
	private event RpcGridListIndexEventHandler<T> eventColumnAdded = null;
	private event RpcGridListIndexEventHandler<T> eventRowRemoved = null;
	private event RpcGridListIndexEventHandler<T> eventColumnRemoved = null;
	private event RpcGridListValueEventHandler<T> eventValueValidate = null;
	private event RpcGridListValueEventHandler<T> eventValueChanged = null;
	private event RpcGridListEventHandler<T> eventValuesReset = null;

	private List<T> elements;
	private Int32 columns;
	private Boolean distinct;

	#region Constructors and destructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors and destructors.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Initializes a new instance of the RpcGridList class with one cell (one row and one column).
	/// </summary>
	public RpcGridList() {
		this.elements = new List<T>();
		this.elements.Add(default(T));
		this.columns = 1;
		this.distinct = false;
	} // RpcGridList

	/// <summary>
	/// Initializes a new instance of the RpcGridList class with the specified number of rows and columns.
	/// </summary>
	/// <param name="rowCount">The number of rows in the grid list.</param>
	/// <param name="columnCount">The number of columns in the grid list.</param>
	public RpcGridList(Int32 rowCount, Int32 columnCount) {
		// Validate.
		if (rowCount < 0) {
			throw new ArgumentOutOfRangeException($"The rowCount argument {rowCount} is less then zero.");
		}
		if (columnCount < 0) {
			throw new ArgumentOutOfRangeException($"The columnCount argument {columnCount} is less then zero.");
		}

		this.elements = new List<T>();
		for (Int32 index = 0; index < rowCount * columnCount; index++) {
			this.elements.Add(default(T));
		}
		this.columns = columnCount;
		this.distinct = false;
	} // RpcGridList

	/// <summary>
	/// Initializes a new instance of the RpcGridList class which is a copy of the argumented grid list.
	/// </summary>
	/// <param name="gridList">The source grid list.</param>
	public RpcGridList(RpcGridList<T> gridList) {
		this.elements = new List<T>(gridList.elements);
		this.columns = gridList.columns;
		this.distinct = gridList.distinct;
	} // RpcGridList
	#endregion

	#region Events
	//------------------------------------------------------------------------------------------------------------------
	//	Events.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Occurs after a row is inserted or added.
	/// </summary>
	public event RpcGridListIndexEventHandler<T> OnRowAdded {
		add {
			this.eventRowAdded += value;
		}
		remove {
			this.eventRowAdded -= value;
		}
	} // OnRowAdded

	/// <summary>
	/// Occurs after a column is inserted or added.
	/// </summary>
	public event RpcGridListIndexEventHandler<T> OnColumnAdded {
		add {
			this.eventColumnAdded += value;
		}
		remove {
			this.eventColumnAdded -= value;
		}
	} // OnColumnAdded

	/// <summary>
	/// Occurs before a row is removed.
	/// </summary>
	public event RpcGridListIndexEventHandler<T> OnRowRemoved {
		add {
			this.eventRowRemoved += value;
		}
		remove {
			this.eventRowRemoved -= value;
		}
	} // OnRowRemoved

	/// <summary>
	/// Occurs before a column is removed.
	/// </summary>
	public event RpcGridListIndexEventHandler<T> OnColumnRemoved {
		add {
			this.eventColumnRemoved += value;
		}
		remove {
			this.eventColumnRemoved -= value;
		}
	} // OnColumnRemoved

	/// <summary>
	/// Occurs before a value is changed.
	/// </summary>
	public event RpcGridListValueEventHandler<T> OnValueValidate {
		add {
			this.eventValueValidate += value;
		}
		remove {
			this.eventValueValidate -= value;
		}
	} // OnValueValidate

	/// <summary>
	/// Occurs after a value is changed.
	/// </summary>
	public event RpcGridListValueEventHandler<T> OnValueChanged {
		add {
			this.eventValueChanged += value;
		}
		remove {
			this.eventValueChanged -= value;
		}
	} // OnValueChanged

	/// <summary>
	/// Occurs after all the values are changed.
	/// </summary>
	public event RpcGridListEventHandler<T> OnValuesReset {
		add {
			this.eventValuesReset += value;
		}
		remove {
			this.eventValuesReset -= value;
		}
	} // OnValuesReset

	/// <summary>
	/// Raise the row added event.
	/// </summary>
	private void DoRowAdded(Int32 rowIndex) {
		if (this.eventRowAdded != null) {
			Delegate[] subscribers = this.eventRowAdded.GetInvocationList();
			foreach (Delegate subscriber in subscribers) {
				try {
					// Invoke the event delegate.
					((RpcGridListIndexEventHandler<T>)subscriber)(this, rowIndex);
				} catch {}
			}
		}
	} // DoRowAdded

	/// <summary>
	/// Raise the column added event.
	/// </summary>
	private void DoColumnAdded(Int32 columnIndex) {
		if (this.eventColumnAdded != null) {
			Delegate[] subscribers = this.eventColumnAdded.GetInvocationList();
			foreach (Delegate subscriber in subscribers) {
				try {
					// Invoke the event delegate.
					((RpcGridListIndexEventHandler<T>)subscriber)(this, columnIndex);
				} catch {}
			}
		}
	} // DoColumnAdded

	/// <summary>
	/// Raise the row removed event.
	/// </summary>
	private void DoRowRemoved(Int32 rowIndex) {
		if (this.eventRowRemoved != null) {
			Delegate[] subscribers = this.eventRowRemoved.GetInvocationList();
			foreach (Delegate subscriber in subscribers) {
				try {
					// Invoke the event delegate.
					((RpcGridListIndexEventHandler<T>)subscriber)(this, rowIndex);
				} catch {}
			}
		}
	} // DoRowRemoved

	/// <summary>
	/// Raise the column removed event.
	/// </summary>
	private void DoColumnRemoved(Int32 columnIndex) {
		if (this.eventColumnRemoved != null) {
			Delegate[] subscribers = this.eventColumnRemoved.GetInvocationList();
			foreach (Delegate subscriber in subscribers) {
				try {
					// Invoke the event delegate.
					((RpcGridListIndexEventHandler<T>)subscriber)(this, columnIndex);
				} catch {}
			}
		}
	} // DoColumnRemoved

	/// <summary>
	/// Raise the value validate event.
	/// </summary>
	private void DoValueValidate(T oldValue, T newValue, Int32 rowIndex, Int32 columnIndex) {
		if (this.eventValueValidate != null) {
			Delegate[] subscribers = this.eventValueValidate.GetInvocationList();
			foreach (Delegate subscriber in subscribers) {
				try {
					// Invoke the event delegate.
					((RpcGridListValueEventHandler<T>)subscriber)(this, oldValue, newValue, rowIndex, columnIndex);
				} catch {}
			}
		}
	} // DoValueValidate

	/// <summary>
	/// Raise the value changed event.
	/// </summary>
	private void DoValueChanged(T oldValue, T newValue, Int32 rowIndex, Int32 columnIndex) {
		if (this.eventValueChanged != null) {
			Delegate[] subscribers = this.eventValueChanged.GetInvocationList();
			foreach (Delegate subscriber in subscribers) {
				try {
					// Invoke the event delegate.
					((RpcGridListValueEventHandler<T>)subscriber)(this, oldValue, newValue, rowIndex, columnIndex);
				} catch {}
			}
		}
	} // DoValueChanged

	/// <summary>
	/// Raise the values reset event.
	/// </summary>
	private void DoValuesReset() {
		if (this.eventValuesReset != null) {
			Delegate[] subscribers = this.eventValuesReset.GetInvocationList();
			foreach (Delegate subscriber in subscribers) {
				try {
					// Invoke the event delegate.
					((RpcGridListEventHandler<T>)subscriber)(this);
				} catch {}
			}
		}
	} // DoValuesReset
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	//	Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets or sets a value indicating whether the items in the RpcGridList are distinct.
	/// When this is set to true, the existing items are not validated to see if the same item exist more then one time in the grid.
	/// </summary>
	public Boolean Distinct {
		get {
			return this.distinct;
		}
		set {
			this.distinct = value;
		}
	} // Distinct

	/// <summary>
	/// Gets the number of rows in the grid.
	/// </summary>
	public Int32 RowCount {
		get {
			if (this.elements.Count > 0) {
				return this.elements.Count / this.columns;
			} else {
				return 0;
			}
		}
	} // RowCount

	/// <summary>
	/// Gets the number of columns in the grid.
	/// </summary>
	public Int32 ColumnCount {
		get {
			return this.columns;
		}
	} // ColumnCount

	/// <summary>
	/// Gets all the items in the grid.
	/// </summary>
	public T[] AllItems {
		get {
			return this.elements.ToArray();
		}
	} // AllItems

	/// <summary>
	/// Gets or sets the item at the specified position in the grid.
	/// </summary>
	/// <param name="rowIndex">The row index of the item.</param>
	/// <param name="columnIndex">The column index of the item.</param>
	public T this[Int32 rowIndex, Int32 columnIndex] {
		get {
			// Validate.
			if (rowIndex < 0) {
				throw new IndexOutOfRangeException($"The row index {rowIndex} is less then zero.");
			}
			if (rowIndex >= this.RowCount) {
				throw new IndexOutOfRangeException($"The row index {rowIndex} is greater then or equal to the row count {this.RowCount}.");
			}
			if (columnIndex < 0) {
				throw new IndexOutOfRangeException($"The column index {columnIndex} is less then zero.");
			}
			if (columnIndex >= this.columns) {
				throw new IndexOutOfRangeException($"The column index {columnIndex} is greater then or equal to the column count {this.columns}.");
			}

			// Return the item at the correct position.
			return this.elements[rowIndex * this.columns + columnIndex];
		}
		set {
			// Validate.
			if (rowIndex < 0) {
				throw new IndexOutOfRangeException($"The row index {rowIndex} is less then zero.");
			}
			if (rowIndex >= this.RowCount) {
				throw new IndexOutOfRangeException($"The row index {rowIndex} is greater then or equal to the row count {this.RowCount}.");
			}
			if (columnIndex < 0) {
				throw new IndexOutOfRangeException($"The column index {columnIndex} is less then zero.");
			}
			if (columnIndex >= this.columns) {
				throw new IndexOutOfRangeException($"The column index {columnIndex} is greater then or equal to the column count {this.columns}.");
			}
			if ((this.distinct == true) &&
				((this.elements.IndexOf(value) != (rowIndex * this.columns) + columnIndex) || (this.elements.LastIndexOf(value) != (rowIndex * this.columns) + columnIndex))) {
				throw new ArgumentException("The grid already contain the item, and it is not allowed to add it more then one time (distinct is true).");
			}

			// Get the current (old) item at the correct position.
			T oldValue = this.elements[(rowIndex * this.columns) + columnIndex];

			// Raise the event.
			// Any event handler can throw an exception, to abort the change.
			this.DoValueValidate(oldValue, value, rowIndex, columnIndex);

			// Set the item at the correct position.
			this.elements[(rowIndex * this.columns) + columnIndex] = value;

			// Raise the event.
			this.DoValueChanged(oldValue, value, rowIndex, columnIndex);
		}
	} // this
	#endregion

	#region Methods
	//------------------------------------------------------------------------------------------------------------------
	//	Methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Removes all rows and columns in the grid list.
	/// </summary>
	public void Clear() {
		this.elements.Clear();
		this.columns = 0;

		// Raise the event.
		this.DoValuesReset();
	} // Clear

	/// <summary>
	/// Reset all items in the grid list to default(T).
	/// </summary>
	public void ClearItems() {
		for (Int32 index = 0; index < this.elements.Count; index++) {
			this.elements[index] = default(T);
		}

		// Raise the event.
		this.DoValuesReset();
	} // ClearItems

	/// <summary>
	/// Reset all items in the grid to the item.
	/// This ignores the Distinct property, and allow the same object more then one time.
	/// </summary>
	/// <param name="value">The value of the items.</param>
	public void ClearItems(T value) {
		for (Int32 index = 0; index < this.elements.Count; index++) {
			this.elements[index] = value;
		}

		// Raise the event.
		this.DoValuesReset();
	} // ClearItems

	/// <summary>
	/// Appends one row to the bottom of the grid.
	/// Each item is initialized with default(T).
	/// </summary>
	public void AppendRow() {
		this.InsertRow(this.RowCount);
	} // AppendRow

	/// <summary>
	/// Appends one column to the right of the list.
	/// Each item is initialized with default(T).
	/// </summary>
	public void AppendColumn() {
		this.InsertColumn(this.columns);
	} // AppendColumn

	/// <summary>
	/// Inserts one row at the specified position in the grid.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="rowIndex">The index of the inserted row.</param>
	public void InsertRow(Int32 rowIndex) {
		// Validate.
		if (rowIndex < 0) {
			throw new IndexOutOfRangeException($"The row index {rowIndex} is less then zero.");
		}
		if (rowIndex > this.RowCount) {
			throw new IndexOutOfRangeException($"The row index {rowIndex} is greater then the row count {this.RowCount}.");
		}
		if (this.columns == 0) {
			throw new IndexOutOfRangeException(String.Format("Unable to insert rows, when there is zero columns.", rowIndex));
		}

		// Insert the row items at the correct position.
		for (Int32 index = 0; index < this.columns; index++) {
			this.elements.Insert(rowIndex * this.columns + index, default(T));
		}

		// Raise the event.
		this.DoRowAdded(rowIndex);
	} // InsertRow

	/// <summary>
	/// Inserts one column at the specified position in the grid.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="columnIndex">The index of the inserted column.</param>
	public void InsertColumn(Int32 columnIndex) {
		// Validate.
		if (columnIndex < 0) {
			throw new IndexOutOfRangeException($"The column index {columnIndex} is less then zero.");
		}
		if (columnIndex > this.columns) {
			throw new IndexOutOfRangeException($"The column index {columnIndex} is greater then the column count {this.columns}.");
		}

		// Insert the column items at the correct position.
		if (this.columns == 0) {
			this.elements.Insert(0, default(T));
		} else {
			for (Int32 index = columnIndex; index < this.elements.Count + 1; index = index + this.columns + 1) {
				this.elements.Insert(index, default(T));
			}
		}

		// Increase the column count.
		this.columns++;

		// Raise the event.
		this.DoColumnAdded(columnIndex);
	} // InsertColumn

	/// <summary>
	/// Removes one row at the specified position in the grid.
	/// </summary>
	/// <param name="rowIndex">The index of the row that is removed.</param>
	public void RemoveRow(Int32 rowIndex) {
		// Validate.
		if (rowIndex < 0) {
			throw new IndexOutOfRangeException($"The row index {rowIndex} is less then zero.");
		}
		if (rowIndex >= this.RowCount) {
			throw new IndexOutOfRangeException($"The row index {rowIndex} is greater then or equal to the row count {this.RowCount}.");
		}

		// Raise the event.
		this.DoRowRemoved(rowIndex);

		// Remove the row items at the correct position.
		this.elements.RemoveRange(rowIndex * this.columns, this.columns);
	} // RemoveRow

	/// <summary>
	/// Removes one column at the specified position in the grid.
	/// </summary>
	/// <param name="columnIndex">The index of the column that is removed.</param>
	public void RemoveColumn(Int32 columnIndex) {
		// Validate.
		if (columnIndex < 0) {
			throw new IndexOutOfRangeException($"The column index {columnIndex} is less then zero.");
		}
		if (columnIndex >= this.columns) {
			throw new IndexOutOfRangeException($"The column index {columnIndex} is greater then or equal to the column count {this.columns}.");
		}

		// Remove the column items at the correct position.
		for (Int32 index = columnIndex; index < this.elements.Count; index = index + this.columns - 1) {
			this.elements.RemoveAt(index);
		}

		// Decrease the column count.
		this.columns--;

		// Raise the event.
		this.DoColumnRemoved(columnIndex);
	} // RemoveColumn

	/// <summary>
	/// Gets the item at the specified position in the grid.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="rowIndex">The row index of the item to get.</param>
	/// <param name="columnIndex">The column index of the item to get.</param>
	public T GetItem(Int32 rowIndex, Int32 columnIndex) {
		// Return the item.
		return this[rowIndex, columnIndex];
	} // GetItem

	/// <summary>
	/// Sets the item at the specified position in the grid.
	/// </summary>
	/// <param name="rowIndex">The row index of the item to set.</param>
	/// <param name="columnIndex">The column index of the item to set.</param>
	/// <param name="value">The value of the item.</param>
	public void SetItem(Int32 rowIndex, Int32 columnIndex, T value) {
		// Set the item.
		this[rowIndex, columnIndex] = value;
	} // SetItem

	/// <summary>
	/// Gets all items at the specified row in the grid.
	/// </summary>
	/// <returns>The items.</returns>
	/// <param name="rowIndex">The row index of the items to get.</param>
	public T[] GetRowItems(Int32 rowIndex) {
		// Return the items.
		T[] result = new T[this.ColumnCount];
		for (Int32 columnIndex = 0; columnIndex < this.ColumnCount; columnIndex++) {
			result[columnIndex] = this[rowIndex, columnIndex];
		}
		return result;
	} // GetRowItems

	/// <summary>
	/// Gets all items at the specified column in the grid.
	/// </summary>
	/// <returns>The items.</returns>
	/// <param name="columnIndex">The column index of the items to get.</param>
	public T[] GetColumnItems(Int32 columnIndex) {
		// Return the items.
		T[] result = new T[this.RowCount];
		for (Int32 rowIndex = 0; rowIndex < this.RowCount; rowIndex++) {
			result[rowIndex] = this[rowIndex, columnIndex];
		}
		return result;
	} // GetColumnItems

	/// <summary>
	/// Sets all items at the specified row in the grid.
	/// </summary>
	/// <param name="rowIndex">The row index of the items to set.</param>
	/// <param name="values">The value of the items.</param>
	public void SetRowItems(Int32 rowIndex, T[] values) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException("The values array is null");
		}
		if (values.Length != this.ColumnCount) {
			throw new ArgumentException($"The values array contains {values.Length} items, but must contain {this.ColumnCount} items.");
		}

		// Set the items.
		for (Int32 columnIndex = 0; columnIndex < this.ColumnCount; columnIndex++) {
			this[rowIndex, columnIndex] = values[columnIndex];
		}
	} // SetRowItems

	/// <summary>
	/// Sets all items at the specified column in the grid.
	/// </summary>
	/// <param name="columnIndex">The column index of the items to set.</param>
	/// <param name="values">The value of the items.</param>
	public void SetColumnItems(Int32 columnIndex, T[] values) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException("The values array is null");
		}
		if (values.Length != this.RowCount) {
			throw new ArgumentException($"The values array contains {values.Length} items, but must contain {this.RowCount} items.");
		}

		// Set the items.
		for (Int32 rowIndex = 0; rowIndex < this.RowCount; rowIndex++) {
			this[rowIndex, columnIndex] = values[rowIndex];
		}
	} // SetColumnItems

	/// <summary>
	/// Gets all items in the grid.
	/// The items are returned one row after another in one large array.
	/// </summary>
	/// <returns>All items in the grid.</returns>
	public T[] GetAllItems() {
		return this.AllItems;
	} // GetAllItems

	/// <summary>
	/// Set all items in the grid to the items in the array.
	/// </summary>
	/// <param name="values">The values of the items.</param>
	public void SetAllItems(T[] values) {
		// Validate.
		if (values.Length != this.RowCount * this.ColumnCount) {
			throw new IndexOutOfRangeException($"The number of items in the source is {values.Length}, but it should be {this.elements.Count}.");
		}

		for (Int32 rowIndex = 0; rowIndex < this.RowCount; rowIndex++) {
			for (Int32 columnIndex = 0; columnIndex < this.ColumnCount; columnIndex++) {
				this[rowIndex, columnIndex] = values[(rowIndex * this.ColumnCount) + columnIndex];
			}
		}
	} // SetAllItems

	/// <summary>
	/// Appends any number of rows to the bottom of the grid.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="rowCount">The number of rows to append.</param>
	public void AppendRows(Int32 rowCount) {
		// Validate.
		if (rowCount <= 0) {
			throw new ArgumentOutOfRangeException($"The rowCount argument {rowCount} is less then one.");
		}

		while (rowCount > 0) {
			this.InsertRow(this.RowCount);
			rowCount--;
		}
	} // AppendRows

	/// <summary>
	/// Appends any number of columns to the right of the list.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="columnCount">The number of columns to append.</param>
	public void AppendColumns(Int32 columnCount) {
		// Validate.
		if (columnCount <= 0) {
			throw new ArgumentOutOfRangeException($"The columnCount argument {columnCount} is less then one.");
		}

		while (columnCount > 0) {
			this.InsertColumn(this.columns);
			columnCount--;
		}
	} // AppendColumns

	/// <summary>
	/// Inserts one row at the specified position in the grid.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="rowIndex">The index of the inserted row.</param>
	/// <param name="rowCount">The number of rows to insert.</param>
	public void InsertRow(Int32 rowIndex, Int32 rowCount) {
		// Validate.
		if (rowCount <= 0) {
			throw new ArgumentOutOfRangeException($"The rowCount argument {rowCount} is less then one.");
		}

		while (rowCount > 0) {
			this.InsertRow(rowIndex);
			rowCount--;
		}
	} // InsertRow

	/// <summary>
	/// Inserts one column at the specified position in the grid.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="columnIndex">The index of the inserted column.</param>
	/// <param name="columnCount">The number of columns to insert.</param>
	public void InsertColumn(Int32 columnIndex, Int32 columnCount) {
		// Validate.
		if (columnCount <= 0) {
			throw new ArgumentOutOfRangeException($"The columnCount argument {columnCount} is less then one.");
		}

		while (columnCount > 0) {
			this.InsertColumn(columnIndex);
			columnCount--;
		}
	} // InsertColumn

	/// <summary>
	/// Appends one row to the bottom of the grid.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="values">The value of the items.</param>
	public void AppendRow(T[] values) {
		this.InsertRow(this.RowCount);
		this.SetRowItems(this.RowCount - 1, values);
	} // AppendRow

	/// <summary>
	/// Appends one column to the right of the list.
	/// Each item is initialized with default(T).
	/// </summary>
	/// <param name="values">The value of the items.</param>
	public void AppendColumn(T[] values) {
		this.InsertColumn(this.columns);
		this.SetColumnItems(this.ColumnCount - 1, values);
	} // AppendColumn

	/// <summary>
	/// Inserts one row at the specified position in the grid.
	/// </summary>
	/// <param name="rowIndex">The index of the inserted row.</param>
	/// <param name="values">The value of the items.</param>
	public void InsertRow(Int32 rowIndex, T[] values) {
		this.InsertRow(rowIndex);
		this.SetRowItems(rowIndex, values);
	} // InsertRow

	/// <summary>
	/// Inserts one column at the specified position in the grid.
	/// </summary>
	/// <param name="columnIndex">The index of the inserted column.</param>
	/// <param name="values">The value of the items.</param>
	public void InsertColumn(Int32 columnIndex, T[] values) {
		this.InsertColumn(columnIndex);
		this.SetColumnItems(columnIndex, values);
	} // InsertColumn
	#endregion

	#region Sort methods
	//------------------------------------------------------------------------------------------------------------------
	//	Sort methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Sorts the rows in the grid list, ordered by the column indexes given.
	/// </summary>
	/// <param name="comparer">The comparer.</param>
	/// <param name="sortAscending">The sorting order.</param>
	/// <param name="columns">The column indexes.</param>
	public void SortRows(IComparer<T> comparer, Boolean sortAscending, params Int32[] columns) {
		this.SortRows(comparer.Compare, sortAscending, columns);
	} // SortRows

	/// <summary>
	/// Sorts the rows in the grid list, ordered by the column indexes given.
	/// </summary>
	/// <param name="compare">The compare func.</param>
	/// <param name="sortAscending">The sorting order.</param>
	/// <param name="columns">The column indexes.</param>
	public void SortRows(Func<T, T, Int32> compare, Boolean sortAscending, params Int32[] columns) {
		// Make sure that all columns exist.
		List<Int32> columns1 = new List<Int32>();
		foreach (Int32 column in columns) {
			if ((column >= 0) && (column < this.ColumnCount) && (columns1.Contains(column) == false)) {
				columns1.Add(column);
			}
		}

		// Perform a bubble sort.
		Int32 length = this.RowCount;
		for (Int32 i = 0; i < length; i++) {
			for (Int32 j = i + 1; j < length; j++) {
				Int32 compareColumnIndex = 0;
				Int32 compareResult = 0;
				while ((compareResult == 0) && (compareColumnIndex < columns1.Count)) {
					if (sortAscending == true) {
						compareResult = compare(this[i, columns1[compareColumnIndex]], this[j, columns1[compareColumnIndex]]);
					} else {
						compareResult = compare(this[j, columns1[compareColumnIndex]], this[i, columns1[compareColumnIndex]]);
					}
					compareColumnIndex++;
				}

				if (compareResult > 0) {
					T[] temp = this.GetRowItems(i);
					this.SetRowItems(i, this.GetRowItems(j));
					this.SetRowItems(j, temp);
				}
			}
		}
	} // SortRows

	/// <summary>
	/// Sorts the columns in the grid list, ordered by the row indexes given.
	/// </summary>
	/// <param name="comparer">The comparer.</param>
	/// <param name="sortAscending">The sorting order.</param>
	/// <param name="rows">The row indexes.</param>
	public void SortColumns(IComparer<T> comparer, Boolean sortAscending, params Int32[] rows) {
		this.SortColumns(comparer.Compare, sortAscending, rows);
	} // SortColumns

	/// <summary>
	/// Sorts the columns in the grid list, ordered by the row indexes given.
	/// </summary>
	/// <param name="compare">The compare func.</param>
	/// <param name="sortAscending">The sorting order.</param>
	/// <param name="rows">The row indexes.</param>
	public void SortColumns(Func<T, T, Int32> compare, Boolean sortAscending, params Int32[] rows) {
		// Make sure that all rows exist.
		List<Int32> rows1 = new List<Int32>();
		foreach (Int32 row in rows) {
			if ((row >= 0) && (row < this.RowCount) && (rows1.Contains(row) == false)) {
				rows1.Add(row);
			}
		}

		// Perform a bubble sort.
		Int32 length = this.ColumnCount;

		for (Int32 i = 0; i < length; i++) {
			for (Int32 j = i + 1; j < length; j++) {
				Int32 compareRowIndex = 0;
				Int32 compareResult = 0;
				while ((compareResult == 0) && (compareRowIndex < rows1.Count)) {
					if (sortAscending == true) {
						compareResult = compare(this[rows1[compareRowIndex], i], this[rows1[compareRowIndex], j]);
					} else {
						compareResult = compare(this[rows1[compareRowIndex], j], this[rows1[compareRowIndex], i]);
					}
					compareRowIndex++;
				}

				if (compareResult > 0) {
					T[] temp = this.GetColumnItems(i);
					this.SetColumnItems(i, this.GetColumnItems(j));
					this.SetColumnItems(j, temp);
				}
			}
		}
	} // SortColumns
	#endregion

} // RpcGridList
#endregion

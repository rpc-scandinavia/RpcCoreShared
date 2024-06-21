using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcGridListTests {

	[TestMethod]
	public void Test() {
		// Prepare.
		Int32[] arrayEmpty = [];
		Int32[] arrayZero1 = [ 0 ];
		Int32[] arrayZero5 = [ 0, 0, 0, 0, 0 ];

		Int32[] arrayRow0 = [ 0, 1, 2, 3, 4];
		Int32[] arrayRow1 = [ 5, 6, 7, 8, 9];
		Int32[] arrayRow2 = [ 10, 11, 12, 13, 14];
		Int32[] arrayRow3 = [ 15, 16, 17, 18, 19];
		Int32[] arrayRow4 = [ 20, 21, 22, 23, 24];

		Int32[] arrayColumn0 = [ 0, 5, 10, 15, 20 ];
		Int32[] arrayColumn1 = [ 1, 6, 11, 16, 21 ];
		Int32[] arrayColumn2 = [ 2, 7, 12, 17, 22 ];
		Int32[] arrayColumn3 = [ 3, 8, 13, 18, 23 ];
		Int32[] arrayColumn4 = [ 4, 9, 14, 19, 24 ];

		Int32[] arrrayFull = [ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24];
		RpcGridList<Int32> grid = new RpcGridList<Int32>();

		// Assert that the grid has one cell (one row and one column).
		CollectionAssert.AreEqual(grid.GetAllItems(), arrayZero1);
		Assert.AreEqual(grid.RowCount, 1);
		Assert.AreEqual(grid.ColumnCount, 1);

		// Assert that the grid has 25 cells (5 rows and 5 columns).
		grid = new RpcGridList<Int32>(5, 5);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Assert that the grid has 25 cells (5 rows and 5 columns).
		grid = new RpcGridList<Int32>();
		grid.AppendRows(4);
		grid.AppendColumns(4);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 5);



		// Assert append and insert rows with data.
		grid = new RpcGridList<Int32>(0, 5);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrayEmpty);
		Assert.AreEqual(grid.RowCount, 0);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Append row data 1.
		grid.AppendRow(arrayRow1);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrayRow1);
		Assert.AreEqual(grid.RowCount, 1);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Insert and append the four other rows.
		grid.AppendRow(arrayRow2);
		grid.InsertRow(0, arrayRow0);
		grid.AppendRow(arrayRow4);
		grid.InsertRow(3, arrayRow3);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrrayFull);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Remove middle row.
		grid.RemoveRow(2);
		CollectionAssert.AreEqual(grid.GetRowItems(2), arrayRow3);
		Assert.AreEqual(grid.RowCount, 4);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Remove first row.
		grid.RemoveRow(0);
		CollectionAssert.AreEqual(grid.GetRowItems(0), arrayRow1);
		Assert.AreEqual(grid.RowCount, 3);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Remove last row.
		grid.RemoveRow(2);
		CollectionAssert.AreEqual(grid.GetRowItems(1), arrayRow3);
		Assert.AreEqual(grid.RowCount, 2);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Remove all remaining rows.
		grid.RemoveRow(0);
		grid.RemoveRow(0);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrayEmpty);
		Assert.AreEqual(grid.RowCount, 0);
		Assert.AreEqual(grid.ColumnCount, 5);



		// Assert append and insert columns with data.
		grid = new RpcGridList<Int32>(5, 0);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrayEmpty);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 0);

		// Append column data 1.
		grid.AppendColumn(arrayColumn1);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrayColumn1);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 1);

		// Insert and append the four other columns.
		grid.AppendColumn(arrayColumn2);
		grid.InsertColumn(0, arrayColumn0);
		grid.AppendColumn(arrayColumn4);
		grid.InsertColumn(3, arrayColumn3);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrrayFull);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 5);

		// Remove middle column.
		grid.RemoveColumn(2);
		CollectionAssert.AreEqual(grid.GetColumnItems(2), arrayColumn3);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 4);

		// Remove first column.
		grid.RemoveColumn(0);
		CollectionAssert.AreEqual(grid.GetColumnItems(0), arrayColumn1);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 3);

		// Remove last column.
		grid.RemoveColumn(2);
		CollectionAssert.AreEqual(grid.GetColumnItems(1), arrayColumn3);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 2);

		// Remove all remaining columns.
		grid.RemoveColumn(0);
		grid.RemoveColumn(0);
		CollectionAssert.AreEqual(grid.GetAllItems(), arrayEmpty);
		Assert.AreEqual(grid.RowCount, 5);
		Assert.AreEqual(grid.ColumnCount, 0);


	} // Test

} // RpcGridListTests

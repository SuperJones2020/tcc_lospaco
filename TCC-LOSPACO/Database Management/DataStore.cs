using System;
using System.Collections.Generic;
using System.Linq;

public class DataStore {
    public object[][] Rows { get; set; }
    public object[] Columns { get; set; }

    public DataStore(object[][] rows, object[] columnsArray) {
        Rows = rows;
        Columns = columnsArray;
    }

    public void LoopColumns(Action<object> function) {
        Columns.ToList().ForEach(col => function(col));
    }

    public void LoopRows(Action<object[]> function) {
        Rows.ToList().ForEach(row => function(row));
    }
}
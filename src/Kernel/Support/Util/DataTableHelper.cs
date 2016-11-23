namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    #endregion

    /// <summary>数据表辅助类</summary>
    public static class DataTableHelper
    {
        /// <summary>获取数据表的指定行的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="length">记录行数</param>
        /// <returns></returns>
        public static DataTable Limit(DataTable table, int length)
        {
            return Limit(table, 0, length);
        }

        /// <summary>获取数据表的指定行的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="length">记录行数</param>
        /// <returns></returns>
        public static DataTable Limit(DataTable table, int offset, int length)
        {
            // 复制表结构
            DataTable newTable = table.Clone();

            int maxIndex = offset + length;

            // 偏移位置超过记录数 返回空表数据 
            if (offset > table.Rows.Count) return newTable;

            // 最大索引位置超过记录数 设置索引记录为表的记录数
            if (maxIndex > table.Rows.Count) maxIndex = table.Rows.Count;

            for (int i = offset; i < maxIndex; i++)
            {
                newTable.ImportRow((DataRow)table.Rows[i]);
            }

            return newTable;
        }

        /// <summary>获取数据表的指定行的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="length">记录行数</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        public static DataTable Limit(DataTable table, int offset, int length, string sort)
        {
            DataView view = table.DefaultView;

            view.Sort = sort;

            return Limit(view.ToTable(), offset, length);
        }

        /// <summary>合并数据表的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="childTables">子表</param>
        /// <returns></returns>
        public static DataTable Union(DataTable table, params DataTable[] childTables)
        {
            foreach (DataTable childTable in childTables)
            {
                foreach (DataColumn column in childTable.Columns)
                {
                    if (!table.Columns.Contains(column.ColumnName))
                    {
                        table.Columns.Add(column.ColumnName, column.DataType);
                    }
                }

                foreach (DataRow item in childTable.Rows)
                {
                    DataRow row = table.NewRow();

                    foreach (DataColumn column in childTable.Columns)
                    {
                        row[column.ColumnName] = item[column.ColumnName];
                    }

                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }

    // -------------------------------------------------------
    // 扩展方法
    // -------------------------------------------------------

    /// <summary>数据表扩展方法类</summary>
    public static class DataTableExtensions
    {
        /// <summary>获取数据表的指定行的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="length">记录行数</param>
        /// <returns></returns>
        public static DataTable Limit(this DataTable table, int length)
        {
            return DataTableHelper.Limit(table, length);
        }

        /// <summary>获取数据表的指定行的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="length">记录行数</param>
        /// <returns></returns>
        public static DataTable Limit(this DataTable table, int offset, int length)
        {
            return DataTableHelper.Limit(table, offset, length);
        }

        /// <summary>获取数据表的指定行的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="length">记录行数</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        public static DataTable Limit(this DataTable table, int offset, int length, string sort)
        {
            return DataTableHelper.Limit(table, offset, length, sort);
        }

        /// <summary>合并数据表的数据</summary>
        /// <param name="table">数据表</param>
        /// <param name="childTables">子表</param>
        /// <returns></returns>
        public static DataTable Union(this DataTable table, params DataTable[] childTables)
        {
            return DataTableHelper.Union(table, childTables);
        }
    }
}

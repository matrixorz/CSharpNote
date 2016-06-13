using System.Drawing;
using System.Threading;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;
 

复制代码
protected void btnExport_Click(object sender, EventArgs e)
    {
        this.labPercent.Text = "";
        IList<ViewUserInfo> userList = viewUserInfoService.GetUserInfoListAll();
        DataTable dt = IListOut(userList);
        WriteExcel(dt, "d:\\a.xls");
    }
    #region 导出Excel
    public void WriteExcel(DataTable ds, string path)
    {
        long totalCount = ds.Rows.Count;
        Thread.Sleep(1000);
        long rowRead = 0;
        float percent = 0;

        StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
        StringBuilder sb = new StringBuilder();
        for (int k = 0; k < ds.Columns.Count; k++)
        {
            sb.Append(ds.Columns[k].ColumnName.ToString() + "\t");
        }
        sb.Append(Environment.NewLine);
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            //rowRead++;
            //percent = ((float)(100 * rowRead)) / totalCount;
            this.labPercent.Text ="<a href='UserInfo.xls' target='_blank'>此处下载</a>";
            for (int j = 0; j < ds.Columns.Count; j++)
            {
                sb.Append(ds.Rows[i][j].ToString() + "\t");
            }
            sb.Append(Environment.NewLine);
        }
        sw.Write(sb.ToString());
        sw.Flush();
        sw.Close();
    }
    public DataTable IListOut(IList<ViewUserInfo> ResList)
    {
        DataTable TempDT = new DataTable();

        //此处遍历IList的结构并建立同样的DataTable
        System.Reflection.PropertyInfo[] p = ResList[0].GetType().GetProperties();
        foreach (System.Reflection.PropertyInfo pi in p)
        {
            TempDT.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
        }

        for (int i = 0; i < ResList.Count; i++)
        {
            ArrayList TempList = new ArrayList();
            //将IList中的一条记录写入ArrayList
            foreach (System.Reflection.PropertyInfo pi in p)
            {
                object oo = pi.GetValue(ResList[i], null);
                TempList.Add(oo);
            }

            object[] itm = new object[p.Length];
            //遍历ArrayList向object[]里放数据
            for (int j = 0; j < TempList.Count; j++)
            {
                itm.SetValue(TempList[j], j);
            }
            //将object[]的内容放入DataTable
            TempDT.LoadDataRow(itm, true);
        }
        //返回DataTable
        return TempDT;
    }
    

    #endregion

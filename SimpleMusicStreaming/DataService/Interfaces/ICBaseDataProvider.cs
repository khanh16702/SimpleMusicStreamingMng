using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Interfaces
{
    public interface ICBaseDataProvider
    {
        string m_ConnectionString { get; set; }
        string ConnectionString { get; set; }
        SqlParameterCollection Parameters { get; }
        SqlCommand Command { get; set; }
        SqlConnection Connection { get; set; }
        SqlDataReader DataReader { get; set; }
        int ExecDuration { get; }
        ILogger Logger { get; }

        string GetParamInfo(DynamicParameters DP);

        string GetParamInfo(IDbDataParameter[] DP);

        bool _OpenConnection(string _m_ConnectionString);
        void _CloseConnection();

        DataSet GetDatasetFromSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString);

        void WriteToFile(Exception ex);
    }
}

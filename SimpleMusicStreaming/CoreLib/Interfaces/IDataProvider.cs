using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IDataProvider
    {
        bool _OpenConnection(string _m_ConnectionString);
        void _CloseConnection();

        DataSet GetDatasetFromSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString);
        SqlDataReader GetDataReaderFromSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString);
        bool ExecuteSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString);

        CResponseMessage GetResponseFromExecutedSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString);
    }
}

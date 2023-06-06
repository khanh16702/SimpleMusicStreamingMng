using CommonLib.Implementations;
using CommonLib.Interfaces;
using CoreLib.Entities;
using Dapper;
using DataServiceLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataServiceLib.Implementations
{
    public class CBaseDataProvider : CErrorHandler, ICBaseDataProvider
    {
        // const
        public const int DEFAULT_PARAM_OUTPUT_CODE = -1;
        public const string DEFAULT_PARAM_OUTPUT_MSG = "out";

        public string m_ConnectionString { get; set; }
        private SqlCommand m_Command;
        private SqlConnection m_Connection;
        private SqlDataAdapter m_DataAdapter;
        private SqlDataReader m_DataReader;
        private double m_dblDuration;


        public string ConnectionString { get; set; }

        public CBaseDataProvider(ISerilogProvider serilogProvider)
            : base(serilogProvider)
        {
            m_Command = new SqlCommand();
        }

        public SqlParameterCollection Parameters
        {
            get
            {
                m_Command.Parameters.Clear();
                return m_Command.Parameters;
            }
        }

        public SqlCommand Command {
            get { return m_Command; }
            set { m_Command = value; }
        }
        public SqlConnection Connection {
            get { return m_Connection; }
            set { m_Connection = value; }
        }
        public SqlDataReader DataReader {
            get { return m_DataReader; }
            set { m_DataReader = value; }
        }

        public int ExecDuration
        {
            get { return Convert.ToInt32(this.m_dblDuration); }
        }

        public string GetParamInfo(DynamicParameters DP)
        {
            try
            {
                if (DP == null)
                    return "";

                var sb = new StringBuilder();
                foreach (var name in DP.ParameterNames)
                {
                    var pValue = DP.Get<dynamic>(name);
                    sb.AppendFormat("\n@{0}=N'{1}',", name, pValue.ToString());
                }
                sb.Length--;

                string processedScript = ReplacePassword(sb.ToString());
                return processedScript;// sb.ToString();
            }
            catch (Exception ex)
            {
                // log error
                this.WriteToFile(ex);

                return "";
            }
        }

        public string GetParamInfo(IDbDataParameter[] DP)
        {
            try
            {
                if (DP == null)
                    return "";

                var sb = new StringBuilder();
                for (int i = 0; i < DP.Length; i++)
                {


                    sb.Append(DP[i]).Append("=").Append(DP[i].Value).Append(" ");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                // log error
                this.WriteToFile(ex);

                return "";
            }
        }

        public void _CloseConnection()
        {
            try
            {
                if ((m_Connection.State == ConnectionState.Open))
                {
                    m_Connection.Close();
                    m_Connection.Dispose();
                    m_Command.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.WriteToFile(ex);
            }
            finally
            {
            }
        }

        public bool _OpenConnection(string _m_ConnectionString)
        {
            bool functionReturnValue = false;

            try
            {
                if (m_Connection == null || m_Connection.State == ConnectionState.Closed) // 2015-07-16 09:33:40 ngocta2 => bug: open 2 connection, close 1
                {
                    // CHUA OPEN roi thi tao new connection
                    m_Connection = new SqlConnection(_m_ConnectionString);
                    m_Connection.Open();
                    functionReturnValue = true;
                }
                else
                {
                    if (m_Connection.State == ConnectionState.Open)
                    {
                        // OPEN roi thi ko tao connection nua
                        functionReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                functionReturnValue = false;
                this.WriteToFile(ex);
            }
            return functionReturnValue;
        }

        private string ReplacePassword(string sqlScript)
        {
            string processedScript = "";

            Match matchResults = null;
            try
            {
                Regex regexObj = new Regex("@Password=N'(.*?)'");
                matchResults = regexObj.Match(sqlScript);
                if (matchResults.Success)
                {
                    string text = matchResults.Groups[1].Value;
                    processedScript = sqlScript.Replace(text, "******");
                    return processedScript;
                }
                else
                {
                    return sqlScript;
                }
            }
            catch (Exception ex)
            {
                // log error
                this.WriteToFile(ex);

                return sqlScript;
            }
        }
        public DataSet GetDatasetFromSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString)
        {
            DataSet functionReturnValue = null;
            if ((_OpenConnection(_m_ConnectionString) == false))
                return null;
            DataSet ds = new DataSet();

            try
            {

                this.Logger.Debug("Before connect DB -> Store Procedure Name:" + SPname + " | Parameter:" + GetParamInfo(paramArr));

                m_Command.Connection = m_Connection;
                m_Command.CommandText = SPname;
                m_Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (SqlParameter param in paramArr)
                        {
                            m_Command.Parameters.Add(param);
                        }
                    }
                }
                DateTime dtBegin = DateTime.Now; // duration  
                m_DataAdapter = new SqlDataAdapter(m_Command);
                m_DataAdapter.Fill(ds);
                this.m_dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                m_Command.Parameters.Clear();
                functionReturnValue = ds;
            }
            catch (Exception ex)
            {
                functionReturnValue = null;
                this.WriteToFile(ex);

            }
            finally
            {
                m_Command.Parameters.Clear();
                _CloseConnection();     // Close Connection
            }
            
            return functionReturnValue;
        }
        public SqlDataReader GetDataReaderFromSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString)
        {

            if ((_OpenConnection(_m_ConnectionString) == false))           // Open Connection
                return null;

            try
            {

                this.Logger.Debug("Before connect DB -> Store Procedure Name:" + SPname + " | Parameter:" + GetParamInfo(paramArr));

                m_Command.Connection = m_Connection;
                m_Command.CommandText = SPname;
                m_Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (SqlParameter param in paramArr)
                        {
                            m_Command.Parameters.Add(param);
                        }
                    }
                }
                DateTime dtBegin = DateTime.Now; // duration  
                this.DataReader = m_Command.ExecuteReader();
                this.m_dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration

            }
            catch (Exception ex)
            {

                this.WriteToFile(ex);

            }
            finally
            {
                m_Command.Parameters.Clear();
            }


            return this.DataReader;
        }

        public bool ExecuteSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString)
        {
            bool ExecuteSuccess = false;
            if ((_OpenConnection(_m_ConnectionString) == false))           // Open Connection
                return false;

            try
            {

                this.Logger.Debug("Before connect DB -> Store Procedure Name:" + SPname + " | Parameter:" +
                                  GetParamInfo(paramArr));

                m_Command.Connection = m_Connection;
                m_Command.CommandText = SPname;
                m_Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (SqlParameter param in paramArr)
                        {
                            m_Command.Parameters.Add(param);
                        }
                    }
                }

                DateTime dtBegin = DateTime.Now; // duration  
                if (m_Command.ExecuteNonQuery() > 0)
                    ExecuteSuccess = true;
                this.m_dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration

            }
            catch (Exception ex)
            {

                this.WriteToFile(ex);
                ExecuteSuccess = false;

            }
            finally
            {
                m_Command.Parameters.Clear();
                _CloseConnection();     // Close Connection
            }


            return ExecuteSuccess;
        }

        public CResponseMessage GetResponseFromExecutedSP(string SPname, IDbDataParameter[] paramArr, string _m_ConnectionString)
        {
            var message = new SqlParameter()
            {
                ParameterName = "@Message",
                SqlDbType = SqlDbType.NVarChar,
                Size = 200,
                Direction = ParameterDirection.Output,
                Value = "Không thể thực hiện"
            };
            var errorCode = new SqlParameter()
            {
                ParameterName = "@ErrorCode",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
                Value = -1
            };

            var paramList = paramArr.ToList();
            paramList.Add(message);
            paramList.Add(errorCode);

            paramArr = paramList.ToArray();

            this.ExecuteSP(SPname, paramArr, _m_ConnectionString);

            var responeMessage = new CResponseMessage()
            {
                Code = errorCode.SqlValue?.ToString(),
                Message = message.SqlValue?.ToString()
            };


            return responeMessage;

        }
    }
}

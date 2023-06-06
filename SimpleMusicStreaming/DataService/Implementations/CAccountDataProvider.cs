using CommonLib.Interfaces;
using CoreLib.Config;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Implementations
{
    public class CAccountDataProvider : CBaseDataProvider, IAccountDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ISerilogProvider _serilogProvider;
        private readonly string _m_ConnectionString;

        public CAccountDataProvider(IConfiguration configuration, ISerilogProvider serilogProvider)
            : base(serilogProvider)
        {
            // conn string chi dung cho class nay
            _m_ConnectionString = configuration.GetConnectionString("DefaultConnection");
            this._configuration = configuration;
            this._serilogProvider = serilogProvider;
        }
        public CResponseMessage Delete(int id)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();
            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@Id",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = id
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Account_Delete, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Insert(CAccount account)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();
            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@Username",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = account.Username,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Password",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = account.Password,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@RetypedPassword",
                        Direction= System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = account.RetypedPassword,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@DisplayName",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = account.DisplayName,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Email",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = account.Email,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@CountryId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = account.CountryId
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Account_Insert, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Search(string username)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();

            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@Username",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = username,
                        Size = 255
                    }
                };
                cResponse.Data = objSQL.GetDatasetFromSP(SPRoute.sp_Account_Search, sp_Params, _m_ConnectionString);
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Update(CAccount account)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();

            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@Id",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = account.Id
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Password",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = account.Password,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@RetypedPassword",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = account.RetypedPassword,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@DisplayName",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = account.DisplayName,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Email",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = account.Email,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@CountryId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = account.CountryId
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Account_Update, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }
    }
}

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
    public class CRoleDataProvider : CBaseDataProvider, IRoleDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ISerilogProvider _serilogProvider;
        private readonly string _m_ConnectionString;

        public CRoleDataProvider(IConfiguration configuration, ISerilogProvider serilogProvider)
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
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Roles_Delete, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Insert(CRole role)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();
            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@RoleName",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = role.Name,
                        Size = 50
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Roles_Insert, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Search(string name)
        {
            throw new NotImplementedException();
        }

        public CResponseMessage Update(CRole role)
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
                        Value = role.Id
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@RoleName",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = role.Name,
                        Size = 50
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@IsActive",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Bit,
                        Value = role.IsActive
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Roles_Update, sp_Params, _m_ConnectionString);
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

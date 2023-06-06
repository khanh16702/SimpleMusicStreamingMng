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
    public class CCountryDataProvider : CBaseDataProvider, ICountryDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ISerilogProvider _serilogProvider;
        private readonly string _m_ConnectionString;

        public CCountryDataProvider(IConfiguration configuration, ISerilogProvider serilogProvider)
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
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Countries_Delete, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage GetDetail(int id)
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
                cResponse.Data = objSQL.GetDatasetFromSP(SPRoute.sp_Countries_GetDetail, sp_Params, _m_ConnectionString);
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Insert(CCountry country)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();

            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@Name",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = country.Name,
                        Size = 255
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Countries_Insert, sp_Params, _m_ConnectionString);
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
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();

            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@Name",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = name,
                        Size = 255
                    }
                };
                cResponse.Data = objSQL.GetDatasetFromSP(SPRoute.sp_Countries_Search, sp_Params, _m_ConnectionString);
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Update(CCountry country)
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
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = country.Id
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Name",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = country.Name,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@IsActive",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Bit,
                        Value = country.IsActive
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Countries_Update, sp_Params, _m_ConnectionString);
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

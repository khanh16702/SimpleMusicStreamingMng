using CommonLib.Interfaces;
using CoreLib.Config;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib.Implementations
{
    public class CTrackDataProvider : CBaseDataProvider, ITrackDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ISerilogProvider _serilogProvider;
        private readonly string _m_ConnectionString;

        public CTrackDataProvider(IConfiguration configuration, ISerilogProvider serilogProvider)
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
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Track_Delete, sp_Params, _m_ConnectionString);
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
                cResponse.Data = objSQL.GetDatasetFromSP(SPRoute.sp_Track_GetDetail, sp_Params, _m_ConnectionString);
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Insert(CTrack track, string artistsId, string genresId)
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
                        Value = track.Name,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Image",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = track.Image,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Duration",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Float,
                        Value = track.Duration
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@AlbumId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = track.AlbumId
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@CreatedBy",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = track.CreatedBy,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Media",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = track.Media,
                        Size = 255
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@ArtistsId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = artistsId,
                        Size = -1
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@GenresId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Value = genresId,
                        Size = -1
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Track_Insert, sp_Params, _m_ConnectionString);
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
                cResponse.Data = objSQL.GetDatasetFromSP(SPRoute.sp_Track_Search, sp_Params, _m_ConnectionString);
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Update(CTrack track)
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
                        Value = track.Id
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@IsActive",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Bit,
                        Value = track.IsActive
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Track_Update, sp_Params, _m_ConnectionString);
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

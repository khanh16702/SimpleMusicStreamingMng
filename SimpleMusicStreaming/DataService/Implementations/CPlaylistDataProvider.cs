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
    public class CPlaylistDataProvider : CBaseDataProvider, IPlaylistDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ISerilogProvider _serilogProvider;
        private readonly string _m_ConnectionString;

        public CPlaylistDataProvider(IConfiguration configuration, ISerilogProvider serilogProvider)
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
                        ParameterName = "@PlaylistId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = id
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Playlist_Delete, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Insert(CPlaylist playlist)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();
            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@AccountId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = playlist.AccountId
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Name",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Value = playlist.Name,
                        Size = 50
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Playlist_Insert, sp_Params, _m_ConnectionString);
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

        public CResponseMessage Update_AddTrack(int trackId, int playlistId)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();
            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@TrackId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = trackId
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@PlaylistId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = playlistId,
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Playlist_Update_AddTrack, sp_Params, _m_ConnectionString);
                this.Logger.Debug("After connect DB -> Message:" + cResponse.Code.ToString());
            }
            catch (Exception e)
            {
                this.WriteToFile(e);
            }
            return cResponse;
        }

        public CResponseMessage Update_RemoveTrack(int trackId, int playlistId)
        {
            CBaseDataProvider objSQL = new CBaseDataProvider(_serilogProvider);
            CResponseMessage cResponse = new CResponseMessage();
            try
            {
                var sp_Params = new[]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@TrackId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = trackId
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@PlaylistId",
                        Direction = System.Data.ParameterDirection.Input,
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = playlistId,
                    }
                };
                cResponse = objSQL.GetResponseFromExecutedSP(SPRoute.sp_Playlist_Update_RemoveTrack, sp_Params, _m_ConnectionString);
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

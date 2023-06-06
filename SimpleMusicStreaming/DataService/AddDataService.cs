using CommonLib.Implementations;
using CommonLib.Interfaces;
using DataServiceLib.Implementations;
using DataServiceLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLib
{
    public static class AddDataService
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountDataProvider, CAccountDataProvider>();
            services.AddTransient<IAlbumDataProvider, CAlbumDataProvider>();
            services.AddTransient<IArtistDataProvider, CArtistDataProvider>();
            services.AddTransient<ICountryDataProvider, CCountryDataProvider>();
            services.AddTransient<IGenreDataProvider, CGenreDataProvider>();
            services.AddTransient<IPlaylistDataProvider, CPlaylistDataProvider>();
            services.AddTransient<IRoleDataProvider, CRoleDataProvider>();
            services.AddTransient<ITrackDataProvider, CTrackDataProvider>();
            services.AddTransient<ICBaseDataProvider, CBaseDataProvider>();
            services.AddTransient<IGenreOfTrackDataProvider, CGenreOfTrackDataProvider>();
            services.AddTransient<IArtistOfTrackDataProvider, CArtistOfTrackDataProvider>();
            return services;
        }
    }
}

// <copyright file="AutoMapperConfig.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Infrastructure.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using Microsoft.Extensions.Logging.Abstractions;

    public class AutoMapperConfig
    {
        /// <summary>
        /// Gets the mapping configuration built by <see cref="Execute"/>; controllers pass it to ProjectTo.
        /// </summary>
        public static IConfigurationProvider Configuration { get; private set; }

        public static IMapper Mapper { get; private set; }

        public void Execute()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();

            var configuration = new MapperConfiguration(
                cfg =>
                {
                    LoadStandardMappings(cfg, types);
                    LoadCustomMappings(cfg, types);
                },
                NullLoggerFactory.Instance);

            Configuration = configuration;
            Mapper = configuration.CreateMapper();
        }

        private static void LoadStandardMappings(IMapperConfigurationExpression configuration, IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Source = i.GetGenericArguments()[0],
                            Destination = t,
                        }).ToArray();

            foreach (var map in maps)
            {
                configuration.CreateMap(map.Source, map.Destination);
            }
        }

        private static void LoadCustomMappings(IMapperConfigurationExpression configuration, IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(configuration);
            }
        }
    }
}

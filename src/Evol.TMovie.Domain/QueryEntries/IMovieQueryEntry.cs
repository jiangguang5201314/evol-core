﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Evol.TMovie.Domain.Models.AggregateRoots;
using Evol.TMovie.Domain.QueryEntries.Parameters;
using Evol.Common;
using Evol.Domain.Data;

namespace Evol.TMovie.Domain.QueryEntries
{
    public interface IMovieQueryEntry : IQueryEntry
    {
        Task<Movie> FetchAsync(Guid id);

        Task<List<Movie>> RetrieveAsync(MovieQueryParameter param);

        Task<IPaged<Movie>> PagedAsync(int index, int size);

        Task<IPaged<Movie>> PagedAsync(Expression<Func<Movie, bool>> predicate,  int index, int size);
    }
}

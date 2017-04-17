﻿using Evol.EntityFramework.Repository;
using Evol.TMovie.Domain.Models.AggregateRoots;
using Evol.TMovie.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evol.TMovie.Data.Repositories
{
    public class EmployeeRepository : BaseEntityFrameworkRepository<Employee, TMovieDbContext>, IEmployeeRepository
    {
        public EmployeeRepository(IEfUnitOfWorkDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}

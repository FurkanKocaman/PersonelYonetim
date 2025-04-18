﻿using GenericRepository;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class IzinTalepRepository : Repository<IzinTalep, ApplicationDbContext>, IIzinTalepRepository
{
    public IzinTalepRepository(ApplicationDbContext context) : base(context)
    {
    }
}


﻿using Microsoft.AspNetCore.Identity;

namespace PersonelYonetim.Server.Domain.Roller;

public sealed class AppUserRole : IdentityUserRole<Guid>
{
    public Guid SirketId { get; set; }
}

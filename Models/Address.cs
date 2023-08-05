using System;
using System.Collections.Generic;

namespace CoroDr.IdentityAPI.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? StreetInformation { get; set; }

    public int? WardId { get; set; }

    public int? DistrictId { get; set; }

    public int? CityId { get; set; }

    public int? Country { get; set; }

    public virtual City? City { get; set; }

    public virtual District? District { get; set; }

    public virtual Ward? Ward { get; set; }
}

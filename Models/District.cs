using System;
using System.Collections.Generic;

namespace CoroDr.IdentityAPI.Models;

public partial class District
{
    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public int? CityId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual City? City { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}

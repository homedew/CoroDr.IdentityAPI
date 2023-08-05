using System;
using System.Collections.Generic;

namespace CoroDr.IdentityAPI.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public int? CountryId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Country? Country { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}

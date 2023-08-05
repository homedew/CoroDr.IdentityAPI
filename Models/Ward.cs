using System;
using System.Collections.Generic;

namespace CoroDr.IdentityAPI.Models;

public partial class Ward
{
    public int WardId { get; set; }

    public string WardName { get; set; } = null!;

    public int? DistrictId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual District? District { get; set; }
}

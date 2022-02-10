using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public interface IMembershipTypeRepository
    {
        List<MembershipType> GetAllMembershipTypes();
    }
}

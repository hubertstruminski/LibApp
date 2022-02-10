using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public class MembershipTypeRepository : IMembershipTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MembershipType> GetAllMembershipTypes()
        {
            return _context.MembershipTypes.ToList();
        }
    }
}

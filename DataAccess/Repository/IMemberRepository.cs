using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public  interface IMemberRepository
    {
        IEnumerable<MemberObject> GetAllMembers();
        void AddMember(MemberObject member);
        void RemoveMember(int memberId);
        void UpdateMember(MemberObject member);
        int GetProperNewID();
        MemberObject GetMember(int memberId);
    }
}

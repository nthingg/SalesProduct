using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAllMembers();
        void AddMember(Member member);
        void RemoveMember(int memberId);
        void UpdateMember(Member member);
        int GetProperNewID();
        Member GetMember(int memberId);
    }
}

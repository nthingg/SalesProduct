using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public IEnumerable<Member> GetAllMembers() => MemberDAO.Instance.GetMemberList();
        public void AddMember(Member member) => MemberDAO.Instance.AddMember(member);
        public void RemoveMember(int memberId) => MemberDAO.Instance.RemoveMember(memberId);
        public void UpdateMember(Member member) => MemberDAO.Instance.UpdateMember(member);
        public int GetProperNewID() => MemberDAO.Instance.GetSeed();
        public Member GetMember(int memberID) => MemberDAO.Instance.GetMemberByID(memberID);
    }
}

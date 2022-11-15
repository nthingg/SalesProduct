using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public IEnumerable<MemberObject> GetAllMembers() => MemberDAO.Instance.GetMemberList();
        public void AddMember(MemberObject member) => MemberDAO.Instance.AddMember(member);
        public void RemoveMember(int memberId) => MemberDAO.Instance.RemoveMember(memberId);
        public void UpdateMember(MemberObject member) => MemberDAO.Instance.UpdateMember(member);
        public int GetProperNewID() => MemberDAO.Instance.GetSeed();
        public MemberObject GetMember(int memberID) => MemberDAO.Instance.GetMemberByID(memberID);
    }
}

using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses
{
    public class MemberDAO
    {
        private static MemberDAO instance = null;
        private static readonly Object instanceLock = new object();
        private MemberDAO() { }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new MemberDAO();
                }
                return instance;
            }
        }

        public IEnumerable<Member> GetMemberList()
        {
            var members = new List<Member>();
            try
            {
                using var context = new SalesProductContext();
                members = context.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public Member GetMemberByID(int id)
        {
            Member member = null;   
            try
            {
                using var context = new SalesProductContext();
                member = context.Members.SingleOrDefault(c => c.MemberId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public int GetSeed()
        {
            var members = new List<Member>();
            try
            {
                using var context = new SalesProductContext();
                members = context.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            var member = members.Last();
            return member.MemberId + 1;
        }

        public void AddMember(Member member)
        {
            try
            {
                using var context = new SalesProductContext();
                context.Members.Add(member);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateMember(Member member)
        {
            try
            {
                using var context = new SalesProductContext();
                context.Members.Update(member);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveMember(int MemberID)
        {
            try
            {
                Member member = GetMemberByID(MemberID);
                using var context = new SalesProductContext();
                context.Members.Remove(member);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

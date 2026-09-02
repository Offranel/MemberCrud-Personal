using MemberCrud.Models;
using System.Collections.Generic;

namespace MemberCrud.Services
{
    /// <summary>
    /// Abstraction for member CRUD operations used by UI and tests.
    /// </summary>
    public interface IMemberService
    {
        List<Member> GetAllMembers();

        void AddMember(Member member);

        void UpdateMember(Member member);

        void DeleteMember(Member member);
    }
}

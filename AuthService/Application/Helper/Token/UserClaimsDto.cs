using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.Token
{
    public class UserClaimsDto
    {
        private readonly IPasswordHasher _passwordHasher;
        public UserClaimsDto(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }
        public UserClaimsDto() { }
       

    }
}

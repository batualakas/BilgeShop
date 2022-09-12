using BilgeShop.Business.Services;
using BilgeShop.Business.Types;
using BilgeShop.Data.Dto;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtector _dataProtector;

        public UserManager(IRepository<UserEntity> userRepository , IDataProtectionProvider dataProtectionProvider)
        {
            _userRepository = userRepository;
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }

        public ServiceMessage AddUser(UserEntity user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower()).ToList();

            if(hasMail.Any()) // hasMail is not null
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu eposta adresi zaten kayıtlıdır."
                };
            }


            user.Password = _dataProtector.Protect(user.Password);

            _userRepository.Add(user);

            return new ServiceMessage
            {
                IsSucceed = true,
                
            };
            
        }

        public UserEntity GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public UserEntity Login(string email, string password)
        {
            var user = _userRepository.Get(x => x.Email == email);

            if(user is null)
            {
                return null;
            }

            var rawPassword = _dataProtector.Unprotect(user.Password);

            if (rawPassword != password)
            {
                return null;
            }
           
                return user;
            


        }

        public void UpdateUser(UserDto user)
        {
            var userEntity = _userRepository.GetById(user.Id);

            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.Email = user.Email;
            userEntity.Address = user.Address;

            _userRepository.Update(userEntity);
        }
    }
}

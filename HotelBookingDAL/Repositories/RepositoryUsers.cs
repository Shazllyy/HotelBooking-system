using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingDAL.Repositories
{
    public class RepositoryUsers :IRepository <user>,ILogin<user>
    {
        private readonly HotelBookingContext _DbContext;

        public RepositoryUsers(HotelBookingContext context)
        {
            _DbContext = context;
        }
        
        public async Task<user> Create(user _object)
        {
            try
            {               
                    var obj =await _DbContext.users.AddAsync(_object);
                    await _DbContext.SaveChangesAsync();
                    return obj.Entity;
               
                    
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<user> Delete(int id)
        {
            try
            {
              
                    var obj =await _DbContext.users.FindAsync(id);
                   
                        _DbContext.Remove(obj);
                        await _DbContext.SaveChangesAsync();
                        return obj;
                   

               
                

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<user>> GetAll()
        {
            try
            {
                return  await _DbContext.users.ToListAsync();
                
               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<user> GetById(int Id)
        {
            try
            {
                return await _DbContext.users.FindAsync(Id);                                                                          
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<user> Login(string userName, string passWord)
        {
            try
            {


                var obj = await _DbContext.users.Where(u => u.userName == userName && u.userPassword == passWord).FirstOrDefaultAsync();
                return obj;



            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<user> Update(int id, user _object)
        {
            try
            {
               
                
                var obj =await _DbContext.users.FindAsync(id);
                obj.userName = _object.userName;
                obj.userPassword = _object.userPassword;
                obj.role = _object.role;
                await _DbContext.SaveChangesAsync();
                return obj;
                 
                   
               
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

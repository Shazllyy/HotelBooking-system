using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingDAL.Repositories
{
    public class RepositoryBranches :IRepository <branch>
    {
        private readonly HotelBookingContext _DbContext;

        public RepositoryBranches(HotelBookingContext context)
        {
            _DbContext = context;
        }
       
        public async Task<branch> Create(branch _object)
        {            
            try
            {               
                   var obj =await _DbContext.branches.AddAsync(_object);
                   await  _DbContext.SaveChangesAsync();
                   return obj.Entity;
               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<branch> Delete(int id)
        {
            try
            {
                
                    var obj = await _DbContext.branches.FindAsync(id);                   
                    _DbContext.branches.Remove(obj);
                    await _DbContext.SaveChangesAsync();
                    return obj;                                  

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<branch>> GetAll()
        {
            try
            {

                return await _DbContext.branches.ToListAsync();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<branch>  GetById(int Id)
        {
            try
            {
               
                    
               return await _DbContext.branches.FindAsync(Id);

               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<branch> Update(int id ,branch _object)
        {
            try
            {
                var obj = await _DbContext.branches.FindAsync(id);                
                obj.branch_name = _object.branch_name;
                obj.branch_country = _object.branch_country;
                obj.branch_city = _object.branch_city;                                                                 
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

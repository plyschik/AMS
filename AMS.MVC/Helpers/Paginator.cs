using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.MVC.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Helpers
{
    public class Paginator<T>
    {
        public int CurrentPage { get; private set; }
        
        public int TotalPages { get; private set; }
        
        public List<T> Elements { get; private set; }
        
        public async Task<Paginator<T>> Create(IQueryable<T> queryable, int currentPage, int elementsPerPage)
        {
            var elementsCount = await queryable.CountAsync();

            if (elementsCount == 0)
            {
                TotalPages = 1;
            }
            else
            {
                TotalPages = (int) Math.Ceiling(elementsCount / (double) elementsPerPage);   
            }

            if (currentPage < 1 || currentPage > TotalPages)
            {
                throw new PageNumberOutOfRangeException();
            }

            CurrentPage = currentPage;
            
            Elements = await queryable
                .Skip((CurrentPage - 1) * elementsPerPage)
                .Take(elementsPerPage)
                .ToListAsync();

            return this;
        }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}

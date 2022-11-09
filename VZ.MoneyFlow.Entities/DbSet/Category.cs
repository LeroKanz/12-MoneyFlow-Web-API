using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace VZ.MoneyFlow.Entities.DbSet
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }        

        public List<Category> ChildrenCategories { get; set; } = new List<Category>();
    }
}

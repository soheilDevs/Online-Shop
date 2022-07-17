using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.BaseEntities
{
    public class BaseEntity
    {
        #region Properties
        [Key]
        public long Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateData { get; set; }=DateTime.Now;

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }= BitConverter.GetBytes(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());

        public virtual void Delete()
        {
            this.IsDeleted = true;
        }
        public virtual void UndoDelete()
        {
            this.IsDeleted = false;
        }
        public virtual void Activate()
        {
            this.IsActive = true;
        }
        public virtual void Deactivate()
        {
            this.IsActive = false;
        }
    }
}

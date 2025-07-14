using Core.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Tracker
{
    public class Tracker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string TblName { get; set; }
        public EnumTrackMethodType MethodType { get; set; }
        public string rowId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public ICollection<TrackerDetail> TrackerDetails { get; set; }

        [NotMapped]
        public PropertyValues CurrentValues { get; set; }
        [NotMapped]
        public PropertyValues OriginalValues { get; set; }

        public Tracker()
        {
            TrackerDetails = new HashSet<TrackerDetail>();
        }
    }
}

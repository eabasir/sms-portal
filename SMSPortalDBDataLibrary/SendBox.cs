//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMSPortalDBDataLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class SendBox
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SendBox()
        {
            this.SendBox_Phone = new HashSet<SendBox_Phone>();
        }
    
        public System.Guid TFId { get; set; }
        public System.Guid TFMessage_Id { get; set; }
        public System.Guid TFUser_Id { get; set; }
        public System.Guid TFQueue_Id { get; set; }
        public System.DateTime TFDateTimeCreate { get; set; }
        public string TFDateTimeCreateFA { get; set; }
    
        public virtual Message Message { get; set; }
        public virtual Queue Queue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SendBox_Phone> SendBox_Phone { get; set; }
    }
}

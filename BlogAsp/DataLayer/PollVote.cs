//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlogAsp.DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class PollVote
    {
        public int idPollVote { get; set; }
        public string ipAddress { get; set; }
        public int idAnswer { get; set; }
        public Nullable<int> idLastUserChange { get; set; }
        public Nullable<System.DateTime> LastTimeChange { get; set; }
    
        public virtual PollAnswer PollAnswer { get; set; }
    }
}
namespace TeenWise.WebSite.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RegisteredUser
    {
        public RegisteredUser()
        { }

        public RegisteredUser(long id, string email, DateTime registeredAt)
        {
            this.ID = id;
            this.EmailAddress = email;
            this.RegisteredAt = registeredAt;
        }

        public long ID { get; set; }
        [Required, EmailAddress]
        public string EmailAddress { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}

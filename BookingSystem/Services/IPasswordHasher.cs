﻿namespace BookingSystem.Services
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
        public bool Verify(string passwordHash, string inputPassword);


    }
}

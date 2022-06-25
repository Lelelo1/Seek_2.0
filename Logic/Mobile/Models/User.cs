﻿using System;
namespace Logic.Models
{
	public class User
	{
		public bool IsLoggedIn { get; }
		public string Id { get; }

		public User(bool isLoggedIn, string id)
        {
			IsLoggedIn = isLoggedIn;
			Id = id;
        }
	}
}


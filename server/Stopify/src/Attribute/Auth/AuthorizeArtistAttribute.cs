﻿namespace Stopify.Attribute.Auth;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class AuthorizeArtistAttribute : System.Attribute;
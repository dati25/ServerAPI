﻿using System.Text.RegularExpressions;

namespace SeverAPI.Commands
{
    public abstract class ICommand
    {
        protected MyContext context = new MyContext();
        protected Tester tester = new Tester();

    }
}

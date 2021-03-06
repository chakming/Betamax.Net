﻿using Castle.DynamicProxy;
using System;

namespace mmSquare.Betamax
{
    public class Player
    {
        private readonly Tape _tape;

        private readonly ProxyGenerator _generator;

        public Player()
            : this(new FileTape())
        {
        }

        public Player(Tape tape)
        {
            _tape = tape;
            _generator = new ProxyGenerator();
        }

        public T Play<T>() where T : class
        {
            return _generator.CreateInterfaceProxyWithoutTarget<T>(new MethodInterceptor(_tape));
        }

        public object Play(Type t)
        {
            return _generator.CreateInterfaceProxyWithoutTarget(t, new MethodInterceptor(_tape));
        }

        internal class MethodInterceptor : IInterceptor
        {
            private readonly Tape _tape;
            private int _count;

            public MethodInterceptor(Tape tape)
            {
                _tape = tape;
                _count = 0;
            }

            public void Intercept(IInvocation invocation)
            {
                invocation.ReturnValue = _tape.Playback(invocation.Method.ReflectedType, invocation.Method.Name, _count);
                _count++;
            }
        }
    }
}